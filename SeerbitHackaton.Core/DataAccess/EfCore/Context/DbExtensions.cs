global using Microsoft.EntityFrameworkCore.Infrastructure;
global using System.Text;
global using System.Data.Common;
global using System.Data;
global using Microsoft.Data.SqlClient;

namespace Shared.DataAccess.EfCore.Context
{
    public static class DbExtensions
    {
        public static bool IsDatabaseFkDeleteException(this DbUpdateException updateEx, out string foreignKeyErrorMessage)
        {
            foreignKeyErrorMessage = null;

            if (updateEx == null || updateEx.Entries.All(e => e.State != EntityState.Deleted))
                return false;

            var exception = (updateEx.InnerException ?? updateEx.InnerException?.InnerException) as SqlException;
            var errors = exception?.Errors.Cast<SqlError>();

            var errorMessages = new StringBuilder();

            if (errors != null)
            {
                foreach (var exceptionError in errors.Where(e => e.Number == 547))
                {
                    errorMessages.AppendLine($"Message: {exceptionError.Message}");
                    errorMessages.AppendLine($"ErrorNumber: {exceptionError.Number}");
                    errorMessages.AppendLine($"LineNumber: {exceptionError.LineNumber}");
                    errorMessages.AppendLine($"Source: {exceptionError.Source}");
                    errorMessages.AppendLine($"Procedure: {exceptionError.Procedure}");
                }
            }

            if (errorMessages.Length == 0) return false;

            foreignKeyErrorMessage = errorMessages.ToString();

            return true;
        }

        public static bool IsUpdateConcurrencyException(this DbUpdateException ex, out string properties)
        {
            properties = null;

            if (ex == null || ex.Entries.All(e => e.State != EntityState.Modified))
                return false;

            var errorMessages = new StringBuilder();

            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is IEntity)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        errorMessages.AppendLine($"Entity: {entry.Metadata.Name}\tOld Value:" +
                            $" {databaseValue}\tNew Value{proposedValue}");
                    }

                    // Refresh original values to bypass next concurrency check
                    // entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }

            if (errorMessages.Length == 0) return false;

            properties = errorMessages.ToString();
            return true;
        }

        public static async Task<IEnumerable<T>> ExecuteSqlToObject<T>(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var response = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            var conn = database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            var command = conn.CreateCommand();

            command.CommandText = sql;

            if (null != parameters
           && parameters.Length > 0)
            {
                var dbParameters = new DbParameter[parameters.Length];

                if (parameters.All(p => p is DbParameter))
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        dbParameters[i] = (DbParameter)parameters[i];
                    }
                }
                else if (!parameters.Any(p => p is DbParameter))
                {
                    var sb = new StringBuilder(sql).Append(" ");

                    var length = parameters.Length;
                    for (int i = 0; i < length; i++)
                    {
                        sb.Append("@p" + i);
                        if (i < length - 1)
                            sb.Append(", ");

                        dbParameters[i] = command.CreateParameter();
                        dbParameters[i].ParameterName = string.Format("p{0}", i);
                        dbParameters[i].Value = parameters[i] ?? DBNull.Value;
                    }

                    command.CommandText = sb.ToString();
                }
                else
                {
                    throw new InvalidOperationException("couldn't mix dbparameter and other objects");
                }

                command.Parameters.AddRange(dbParameters);
            }

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var type = reader.GetFieldType(i);

                    var val = reader.GetValue(i);
                    row.Add(reader.GetName(i), val == DBNull.Value ? null : val);
                }

                var instance = Activator.CreateInstance<T>();

                foreach (var column in row.Keys)
                {
                    foreach (var property in properties)
                    {
                        if (column == property.Name)
                            property.SetValue(instance, row[column]);
                    }
                }

                response.Add(instance);
            }
            return response;
        }
    }
}
