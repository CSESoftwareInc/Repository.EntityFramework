using System;
using CSESoftware.Repository.EntityFramework;

namespace CSESoftware.RepositoryTestProject.Setup
{
    public class BaseTest
    {
        public static string GetDatabaseId()
        {
            return Guid.NewGuid().ToString();
        }

        public static Repository<DbContext> GetRepository(string databaseId)
        {
            return new Repository<DbContext>(new DbContext(databaseId));
        }
    }
}
