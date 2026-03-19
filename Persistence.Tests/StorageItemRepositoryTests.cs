using Moq;
using Moq.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Repositories;

namespace Persistence.Tests
{
    public class StorageItemRepositoryTests
    {
        [Theory]
        [ClassData(typeof(StorageItemQueryTestRow))]
        internal async Task StorageItems_Query_Tests(Mock<IApplicationDbContext> givenContextMock, StorageItemQuery query, IEnumerable<Guid> expectedStorageItemsIds)
        {
            var sut = GetSut(givenContextMock);

            var result = await sut.GetStorageItems(query);

            Assert.NotNull(result);

            Assert.Contains(result, item => expectedStorageItemsIds.Contains(item.ItemId));
        }

        private StorageItemRepository GetSut(Mock<IApplicationDbContext> contextMock)
        {
            return new StorageItemRepository(contextMock.Object);
        }
    }
    internal class StorageItemQueryTestRow : TheoryData<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>
    {
        public StorageItemQueryTestRow()
        {
            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.Parse("F293DDC3-5693-4D46-98AD-783A4A901F8B"), Guid.NewGuid(), 10),
                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"), Guid.NewGuid(), 20),
                      StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"), Guid.NewGuid(), 30)
                     ),
                    StorageItemQuery.Where()
                                .MinimumPriceIs(20),
                    ExpectItemIds("896A0DC4-51FF-46B8-9289-D1E9110C0A1A", "D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7")
                ).WithTestDisplayName("MinimumPrice test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                       StorageItemWith(Guid.Parse("F293DDC3-5693-4D46-98AD-783A4A901F8B"), Guid.NewGuid(), 10),
                       StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"), Guid.NewGuid(), 20),
                       StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"), Guid.NewGuid(), 30)
                     ),
                     StorageItemQuery.Where()
                                     .MaximumPriceIs(20),
                      ExpectItemIds("F293DDC3-5693-4D46-98AD-783A4A901F8B","896A0DC4-51FF-46B8-9289-D1E9110C0A1A")                                 
                ).WithTestDisplayName("Maximum price test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.Parse("F293DDC3-5693-4D46-98AD-783A4A901F8B"), Guid.NewGuid(), 10),
                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"), Guid.NewGuid(), 20),
                      StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"), Guid.NewGuid(), 30),
                      StorageItemWith(Guid.Parse("1E910798-0F72-4060-AFB8-9CC5E50E4BEA"), Guid.NewGuid(), 40)
                     ),
                    StorageItemQuery.Where()
                                .MinimumPriceIs(20)
                                .MaximumPriceIs(30),
                    ExpectItemIds("896A0DC4-51FF-46B8-9289-D1E9110C0A1A", "D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7")
                ).WithTestDisplayName("Interval price test"));

        }
        private Mock<IApplicationDbContext> Given(params StorageItem[] storageItems)
        {
            var contextMock = new Mock<IApplicationDbContext>();

            contextMock.SetupGet(c => c.StorageItems).ReturnsDbSet(storageItems);

            return contextMock;
        }
        private StorageItem StorageItemWith(Guid itemId, Guid productId, decimal price, params StorageItemCategory[] categories) =>
            new StorageItem
            {
                ItemId = itemId,
                ProductId = productId,
                Sku = $"SKU-{productId}",
                Description = $"{itemId}-Description",
                Price = price,
                StorageItemCategories = categories,
            };

        private StorageItemCategory StorageItemCategoryWith(Guid id) =>
            new StorageItemCategory
            {
                Id = id,
                Name = $"Category-{id}"
            };
        private IEnumerable<Guid> ExpectItemIds(params string[] itemIds)=> itemIds.Select(ii=> Guid.Parse(ii));
    }
}