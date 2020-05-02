using System.Data.Entity;
using System.Diagnostics;
using Effort;

namespace CSESoftware.Repository.EntityFramework
{
    public class BaseMockDbContext : DbContext
    {
        public BaseMockDbContext(string databaseId) : base(DbConnectionFactory.CreatePersistent(databaseId), false)
        {
            Database.Log = generatedSql => Debug.WriteLine(generatedSql); // Log SQL to output window

            Configuration.LazyLoadingEnabled = false; // disable lazy loading
            Configuration.AutoDetectChangesEnabled = false; // disable tracking
        }
    }
}
