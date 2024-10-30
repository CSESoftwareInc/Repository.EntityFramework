using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CSESoftware.RepositoryTestProject.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSESoftware.RepositoryTestProject
{
    [TestClass]
    public class CreateTests : BaseTest
    {
        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task CreateOneTest()
        {
            var options = GetDatabaseId();
            var createRepository = GetRepository(options);

            createRepository.Create(new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 2.5,
                IsActive = false
            });
            await createRepository.SaveAsync();

            var readRepository = GetRepository(options);
            var result = (await readRepository.GetAllAsync<Topping>()).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Canadian Bacon", result.FirstOrDefault()?.Name);
            Assert.AreEqual(2.5, result.FirstOrDefault()?.AdditionalCost);
            Assert.IsTrue(result.FirstOrDefault()?.IsActive ?? false);
            Assert.IsTrue(result.FirstOrDefault()?.CreatedDate > DateTime.UtcNow.AddSeconds(-5));
            Assert.IsTrue(result.FirstOrDefault()?.ModifiedDate > DateTime.UtcNow.AddSeconds(-5));
            Assert.IsTrue(result.First().IsActive);
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task CreateOneNoActiveOrDateChangeTest()
        {
            var options = GetDatabaseId();
            var createRepository = GetRepository(options);

            createRepository.Create(new Crust
            {
                Name = "Pan",
                AdditionalCost = 2.5,
            });
            await createRepository.SaveAsync();

            var readRepository = GetRepository(options);
            var result = (await readRepository.GetAllAsync<Crust>()).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Pan", result.FirstOrDefault()?.Name);
            Assert.AreEqual(2.5, result.FirstOrDefault()?.AdditionalCost);
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task CreateManyTest()
        {
            var options = GetDatabaseId();

            var topping1 = new Topping
            {
                Name = "Sausage",
                AdditionalCost = 0.5,
                IsActive = false
            };
            var topping2 = new Topping
            {
                Name = "Bacon",
                AdditionalCost = 0.75,
                IsActive = false
            };
            var topping3 = new Topping
            {
                Name = "Canadian Bacon",
                AdditionalCost = 0.75
            };
            var topping4 = new Topping
            {
                Name = "Olives",
                AdditionalCost = 1.25
            };

            var toppings = new List<Topping> {topping1, topping2, topping3, topping4};

            var createRepository = GetRepository(options);
            createRepository.Create(toppings);
            await createRepository.SaveAsync();

            var readRepository = GetRepository(options);
            var result = (await readRepository.GetAllAsync<Topping>()).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(2, result.Count(x => Math.Abs(x.AdditionalCost - 0.75) < 0.001));
            Assert.IsFalse(result.Any(x => !x.IsActive));
            Assert.IsFalse(result.Any(x => x.CreatedDate < DateTime.UtcNow.AddSeconds(-5)));
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public async Task CreateManyNoActiveOrDateChangeTest()
        {
            var options = GetDatabaseId();

            var crust1 = new Crust
            {
                Name = "Thin",
                AdditionalCost = 0.5
            };
            var crust2 = new Crust
            {
                Name = "Crispy",
                AdditionalCost = 0.75
            };
            var crust3 = new Crust
            {
                Name = "Thin & Crispy",
                AdditionalCost = 0.75
            };
            var crust4 = new Crust
            {
                Name = "Pan",
                AdditionalCost = 1.25
            };

            var crusts = new List<Crust> {crust1, crust2, crust3, crust4};

            var createRepository = GetRepository(options);
            createRepository.Create(crusts);
            await createRepository.SaveAsync();

            var readRepository = GetRepository(options);
            var result = (await readRepository.GetAllAsync<Crust>()).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(2, result.Count(x => Math.Abs(x.AdditionalCost - 0.75) < 0.001));
        }
    }
}
