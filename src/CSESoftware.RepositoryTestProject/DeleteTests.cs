using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CSESoftware.RepositoryTestProject.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSESoftware.RepositoryTestProject
{
    [TestClass]
    public class DeleteTests : BaseTest
    {
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


            var deleteRepository = GetRepository(options);
            deleteRepository.Delete(await deleteRepository.GetFirstAsync<Topping>());
            await deleteRepository.SaveAsync();


            var readRepository = GetRepository(options);
            Assert.AreEqual(false, await readRepository.GetExistsAsync<Topping>(x => x.Id == 1));
            Assert.AreEqual(0, await readRepository.GetCountAsync<Topping>());
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task DeleteByIdTest()
        {
            var options = GetDatabaseId();
            var createRepository = GetRepository(options);

            createRepository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            });
            await createRepository.SaveAsync();


            var deleteRepository = GetRepository(options);
            deleteRepository.Delete<Topping>(1);
            await deleteRepository.SaveAsync();


            var readRepository = GetRepository(options);
            Assert.AreEqual(false, await readRepository.GetExistsAsync<Topping>(x => x.Id == 1));
            Assert.AreEqual(0, await readRepository.GetCountAsync<Topping>());
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task DeleteByListTest()
        {
            var options = GetDatabaseId();

            var topping1 = new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            };
            var topping2 = new Topping
            {
                Name = "Bacon",
                AdditionalCost = 1
            };
            var topping3 = new Topping
            {
                Name = "Sausage",
                AdditionalCost = 2.25
            };

            var createRepository = GetRepository(options);
            createRepository.Create(new List<Topping>{topping1, topping2, topping3});
            await createRepository.SaveAsync();


            var deleteRepository = GetRepository(options);
            var toppingsToDelete = await deleteRepository.GetAllAsync<Topping>(x => x.AdditionalCost > 2);
            deleteRepository.Delete(toppingsToDelete);
            await deleteRepository.SaveAsync();


            var readRepository = GetRepository(options);
            Assert.AreEqual(topping2.Id, (await readRepository.GetFirstAsync<Topping>()).Id);
            Assert.AreEqual(1, await readRepository.GetCountAsync<Topping>());
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task DeleteByExpressionTest()
        {
            var options = GetDatabaseId();

            var topping1 = new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5
            };
            var topping2 = new Topping
            {
                Name = "Bacon",
                AdditionalCost = 1
            };
            var topping3 = new Topping
            {
                Name = "Sausage",
                AdditionalCost = 2.25
            };

            var createRepository = GetRepository(options);
            createRepository.Create(new List<Topping>{topping1, topping2, topping3});
            await createRepository.SaveAsync();


            var deleteRepository = GetRepository(options);
            deleteRepository.Delete<Topping>(x => x.AdditionalCost > 2);
            await deleteRepository.SaveAsync();


            var readRepository = GetRepository(options);
            Assert.AreEqual(topping2.Id, (await readRepository.GetFirstAsync<Topping>()).Id);
            Assert.AreEqual(1, await readRepository.GetCountAsync<Topping>());
        }
    }
}
