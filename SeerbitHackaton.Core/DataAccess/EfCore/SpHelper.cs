global using System.Text.RegularExpressions;

namespace SeerbitHackaton.Core.DataAccess.EfCore
{
    public static class SpHelper
    {
        public static string GetFileContentWithName(string filePath)
        {
            string sqlContent = "";
            var baseDir = $@"{AppDomain.CurrentDomain.BaseDirectory}";
            if (Directory.Exists($"{baseDir}\bin"))
            {
                sqlContent = File.ReadAllText(string.Format(@"{0}\bin\Scripts\{1}", baseDir, filePath));
                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                sqlContent = appPathMatcher.Match(sqlContent).Value;
            }
            else
            {
                sqlContent = File.ReadAllText(string.Format(@"{0}\Scripts\{1}", baseDir, filePath));
            }

            return sqlContent;
        }
    }
}
