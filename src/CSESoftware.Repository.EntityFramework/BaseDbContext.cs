using System.Data.Entity;
using System.Diagnostics;

namespace CSESoftware.Repository.EntityFramework
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(string connectionString) : base(connectionString)
        {
            Database.Log = generatedSql => Debug.WriteLine(generatedSql); // Log SQL to output window

            Configuration.LazyLoadingEnabled = false; // disable lazy loading
            Configuration.AutoDetectChangesEnabled = false; // disable tracking
        }
    }
}