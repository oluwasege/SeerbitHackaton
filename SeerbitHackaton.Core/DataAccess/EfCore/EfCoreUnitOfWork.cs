namespace SeerbitHackaton.Core.DataAccess.EfCore
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        public readonly DbContext _context;
        private bool _disposed;

        public EfCoreUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            //Determine at runtime which db provider is being used, i.e Sqlite does not support these methods, so if it is testing, these methods should not run.
            if (!_context.Database.IsSqlite())
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;

                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                    _context.Database.OpenConnection();

                _context.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (!_context.Database.IsSqlite())
            {
                _context.ChangeTracker.DetectChanges();

                SaveChanges();
                _context.Database.CommitTransaction();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Rollback()
        {
            if (!_context.Database.IsSqlite())
            {
                _context.Database.CurrentTransaction?.Rollback();
            }
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return (TDbContext)_context;
        }
    }
}
