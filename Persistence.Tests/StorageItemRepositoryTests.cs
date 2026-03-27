using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Repositories;

namespace Persistence.Tests
{
    public class StorageItemRepositoryTests
    {
        [Theory]
        [ClassData(typeof(StorageItemQueryTestRows))]
        internal async Task StorageItems_Query_Tests(Mock<IApplicationDbContext> givenContextMock, StorageItemQuery query, IEnumerable<Guid> expectedStorageItemsIds)
        {
            var sut = GetSut(givenContextMock);

            var result = await sut.QueryStorageItems(query);

            Assert.NotNull(result);

            Assert.Contains(result, item => expectedStorageItemsIds.Contains(item.ItemId));
        }

        private StorageItemQueryProvider GetSut(Mock<IApplicationDbContext> contextMock)
        {
            return new StorageItemQueryProvider(contextMock.Object);
        }
    }
    internal class StorageItemQueryTestRows : TheoryData<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>
    {
        public StorageItemQueryTestRows()
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
            ).WithTestDisplayName("Minimum Price Test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                       StorageItemWith(Guid.Parse("F293DDC3-5693-4D46-98AD-783A4A901F8B"), Guid.NewGuid(), 10),
                       StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"), Guid.NewGuid(), 20),
                       StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"), Guid.NewGuid(), 30)
                     ),
                StorageItemQuery.Where()
                                     .MaximumPriceIs(20),
                 ExpectItemIds("F293DDC3-5693-4D46-98AD-783A4A901F8B","896A0DC4-51FF-46B8-9289-D1E9110C0A1A")                                 
            ).WithTestDisplayName("Maximum Price Test"));

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
            ).WithTestDisplayName("Interval Price Test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 10,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7")),
                      
                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"),
                                      Guid.NewGuid(), 20,
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),
                      
                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 30,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7")),
                      
                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 40,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7"))
                     ),
                StorageItemQuery.Where()
                                    .CategoriesIncludes(Guid.Parse("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21") )
                                ,
                ExpectItemIds("896A0DC4-51FF-46B8-9289-D1E9110C0A1A")
            ).WithTestDisplayName("Single Item Category Query Test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 10,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7")),

                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"),
                                      Guid.NewGuid(), 20,
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"),
                                      Guid.NewGuid(), 30,
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 40,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7"))
                     ),
                StorageItemQuery.Where()
                                    .CategoriesIncludes(Guid.Parse("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21"))
                                ,
                ExpectItemIds("896A0DC4-51FF-46B8-9289-D1E9110C0A1A", "D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7")
            ).WithTestDisplayName("Double Item Category Query Test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"),
                                      Guid.NewGuid(), 10,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7")),

                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 20,
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.NewGuid(),
                                      Guid.NewGuid(), 30,
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"),
                                      Guid.NewGuid(), 40,
                                      StorageItemCategoryWith("21747AAA-D3D6-43AE-99FA-8B674A0489CD"))
                     ),
                StorageItemQuery.Where()
                                    .CategoriesIncludes(Guid.Parse("9D3A5577-4F43-4E38-B377-21C51F9588A7"),
                                                        Guid.Parse("21747AAA-D3D6-43AE-99FA-8B674A0489CD"))
                                ,
                ExpectItemIds("896A0DC4-51FF-46B8-9289-D1E9110C0A1A", "D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7")
            ).WithTestDisplayName("Multiple Item Category Query Test"));

            Add(new TheoryDataRow<Mock<IApplicationDbContext>, StorageItemQuery, IEnumerable<Guid>>(
                Given(
                      StorageItemWith(Guid.Parse("896A0DC4-51FF-46B8-9289-D1E9110C0A1A"),
                                      Guid.NewGuid(), 10,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7"),
                                      StorageItemCategoryWith("21747AAA-D3D6-43AE-99FA-8B674A0489CD")),

                      StorageItemWith(Guid.Parse("21FC1697-0EE7-4A0C-B68C-7423249E18A0"),
                                      Guid.NewGuid(), 20,
                                      StorageItemCategoryWith("9D3A5577-4F43-4E38-B377-21C51F9588A7"),
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.Parse("D9ECB2AE-8A4D-4589-AD86-4CE5C20D9FE7"),
                                      Guid.NewGuid(), 30,
                                      StorageItemCategoryWith("21747AAA-D3D6-43AE-99FA-8B674A0489CD"),
                                      StorageItemCategoryWith("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21")),

                      StorageItemWith(Guid.Parse("04035EF3-49FC-4F74-A946-EDEE9F98DBA7"),
                                      Guid.NewGuid(), 40,
                                      StorageItemCategoryWith("5C4C95D2-37FF-4083-A96A-771F879E506C"),
                                      StorageItemCategoryWith("B08E0FBF-22EF-458A-A85D-07C9DC71068E"))
                     ),
                StorageItemQuery.Where()
                                    .CategoriesIncludes(Guid.Parse("4454CB2B-2CD9-4FFC-AC6F-1F07A66A4E21"),
                                                        Guid.Parse("9D3A5577-4F43-4E38-B377-21C51F9588A7")),
                
                ExpectItemIds("21FC1697-0EE7-4A0C-B68C-7423249E18A0", "896A0DC4-51FF-46B8-9289-D1E9110C0A1A")
            ).WithTestDisplayName("Mixed Multiple Item Category Query Test"));

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

        private StorageItemCategory StorageItemCategoryWith(string id) =>
            new StorageItemCategory
            {
                Id = id.IsNullOrEmpty() ? Guid.Empty : Guid.Parse(id),
                Name = $"Category-{( id.IsNullOrEmpty() ? Guid.Empty.ToString() : id )}"
            };
        private IEnumerable<Guid> ExpectItemIds(params string[] itemIds)=> itemIds.Select(ii=> Guid.Parse(ii));
    }
}