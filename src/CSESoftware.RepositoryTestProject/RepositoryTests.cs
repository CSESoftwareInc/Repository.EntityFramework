using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CSESoftware.Repository.EntityFramework;
using CSESoftware.RepositoryTestProject.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbContext = CSESoftware.RepositoryTestProject.Setup.DbContext;

namespace CSESoftware.RepositoryTestProject
{
    [TestClass]
    public class RepositoryTests
    {
        #region Repository Setup

        private static string GetDatabaseId()
        {
            return Guid.NewGuid().ToString();
        }

        private static Repository<DbContext> GetRepository(string databaseId)
        {
            return new Repository<DbContext>(new DbContext(databaseId));
        }

        #endregion

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task CreateTest()
        {
            var options = GetDatabaseId();
            var repository = GetRepository(options);

            repository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            });
            await repository.SaveAsync();

            var repository2 = GetRepository(options);
            var result = (await repository2.GetAllAsync<Topping>()).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Canadian Bacon", result.FirstOrDefault()?.Name);
            Assert.AreEqual(2.5, result.FirstOrDefault()?.AdditionalCost);
            Assert.IsTrue(result.FirstOrDefault()?.CreatedDate > DateTime.UtcNow.AddSeconds(-3));
            Assert.IsTrue(result.FirstOrDefault()?.ModifiedDate > DateTime.UtcNow.AddSeconds(-3));
            Assert.IsTrue(result.First().IsActive);
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task UpdateTest()
        {
            var options = GetDatabaseId();
            var repository = GetRepository(options);

            repository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            });
            await repository.SaveAsync();


            var repository2 = GetRepository(options);
            var topping = await repository2.GetFirstAsync<Topping>();
            topping.Name = "Super Canadian Bacon";
            repository2.Update(topping);
            await repository2.SaveAsync();


            var repository3 = GetRepository(options);
            var updatedTopping = await repository3.GetFirstAsync<Topping>();

            Assert.AreEqual("Super Canadian Bacon", updatedTopping.Name);
            Assert.AreEqual(2.5, updatedTopping.AdditionalCost);
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task DeleteByEntityTest()
        {
            var options = GetDatabaseId();
            var repository = GetRepository(options);

            repository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            });
            await repository.SaveAsync();


            var repository2 = GetRepository(options);
            repository2.Delete(await repository2.GetFirstAsync<Topping>());
            await repository2.SaveAsync();


            var repository3 = GetRepository(options);
            Assert.AreEqual(false, await repository3.GetExistsAsync<Topping>(x => x.Id == 1));
            Assert.AreEqual(0, await repository3.GetCountAsync<Topping>());
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task DeleteByIdTest()
        {
            var options = GetDatabaseId();
            var repository = GetRepository(options);

            repository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            });
            await repository.SaveAsync();


            var repository2 = GetRepository(options);
            repository2.Delete<Topping>(1);
            await repository2.SaveAsync();


            var repository3 = GetRepository(options);
            Assert.AreEqual(false, await repository3.GetExistsAsync<Topping>(x => x.Id == 1));
            Assert.AreEqual(0, await repository3.GetCountAsync<Topping>());
        }
    }
}