using Moq;
using Persistence;
using Persistence.ReadModels;
using Persistence.Repositories;

namespace StorageManagement.Tests
{
    public class StorageManagerTests
    {
        [Theory]
        [MemberData(nameof(ProductInData))]
        public async Task Storage_Happy_Path_Can_Put_Product_In(Mock<IStorageRepository> storageRepositoryMock,
                                                                Mock<IStorageItemWriteRepository> storageItemWriteRepositoryMock,
                                                                Mock<IProductStorageReservationRepository> productStorageReservationRepositoryMock,
                                                                bool expectedResult)
        {
            var sut = GetSut(storageRepositoryMock, storageItemWriteRepositoryMock, productStorageReservationRepositoryMock);

            var addItem = new AddItemInformation(Guid.NewGuid(), "DummySku", "DummyDescription", 10);
            var result = await sut.ProductIn(addItem);

            Assert.Equal(expectedResult, result);
        }
        public static IEnumerable<object[]> ProductInData()
        {
            yield return new object[] {
                                        GivenStorageRepository( ReturnsStorageMockWithSpace(Guid.NewGuid()),
                                                                ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                                ReturnsStorageMockById(Guid.NewGuid()) ),
                                        GivenStorageItemCan(true, Guid.NewGuid() ),
                                        GivenProductStorageReservationManager(true, Guid.NewGuid() ),
                                        true
                                      };
            yield return new object[] {
                                        GivenStorageRepository( ReturnsNullStorageMock(),
                                                                ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                                ReturnsStorageMockById(Guid.NewGuid()) ),
                                        GivenStorageItemCan(false, Guid.NewGuid() ),
                                        GivenProductStorageReservationManager(true, Guid.NewGuid() ),
                                        false
                                      };
        }


        [Theory]
        [MemberData(nameof(ReserveProductData))]
        public async Task Storage_Happy_Path_Can_Reserve_Product(Mock<IStorageRepository> storageRepositoryMock,
                                                                 Mock<IStorageItemWriteRepository> storageItemWriteRepositoryMock,
                                                                 Mock<IProductStorageReservationRepository> productStorageReservationRepositoryMock,
                                                                 bool expectedResult)
        {
            var sut = GetSut(storageRepositoryMock, storageItemWriteRepositoryMock, productStorageReservationRepositoryMock);

            var someExternaleReference = "externalReferenceId";
            var productId = Guid.NewGuid();
            var result = await sut.ReserveProduct(someExternaleReference, productId);

            Assert.Equal(expectedResult, result);
        }
        public static IEnumerable<object[]> ReserveProductData()
        {
            yield return new object[] {
                                        GivenStorageRepository( ReturnsStorageMockWithSpace(Guid.NewGuid()),
                                                                ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                                ReturnsStorageMockById(Guid.NewGuid()) ),
                                        GivenStorageItemCan(false, Guid.NewGuid() ),
                                        GivenProductStorageReservationManager(true, Guid.NewGuid() ),
                                        true
                                      };
            yield return new object[] {
                                        GivenStorageRepository( ReturnsStorageMockWithSpace(Guid.NewGuid()),
                                                                ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                                ReturnsStorageMockById(Guid.NewGuid()) ),
                                        GivenStorageItemCan(false, Guid.NewGuid()),
                                        GivenProductStorageReservationManager(false,  Guid.NewGuid() ),
                                        false
                                      };
        }

        [Theory]
        [MemberData(nameof(ProductOutData))]
        public async Task Storage_Happy_Path_Can_Take_Product_Out(Mock<IStorageRepository> storageRepositoryMock,
                                                                  Mock<IStorageItemWriteRepository> storageItemWriteRepositoryMock,
                                                                  Mock<IProductStorageReservationRepository> productStorageReservationRepositoryMock,
                                                                  bool expectedToProductOut)
        {
            var sut = GetSut(storageRepositoryMock, storageItemWriteRepositoryMock, productStorageReservationRepositoryMock);

            var someExternaleReference = "externalReferenceId";
            var productId = Guid.NewGuid();
            var result = await  sut.ProductOut(someExternaleReference, productId);

            Assert.True(expectedToProductOut
                                ? result != Guid.Empty
                                : result == Guid.Empty);
        }
        public static IEnumerable<object[]> ProductOutData()
        {
           yield return new object[] {
                                       GivenStorageRepository( ReturnsStorageMockWithSpace(Guid.NewGuid()),
                                                               ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                               ReturnsStorageMockById(Guid.NewGuid()) ),
                                       GivenStorageItemCan(false, Guid.NewGuid() ),
                                       GivenProductStorageReservationManager(true, Guid.NewGuid() ),
                                       true
                                     };
           
           yield return new object[] {
                                       GivenStorageRepository( ReturnsStorageMockWithSpace(Guid.NewGuid()),
                                                               ReturnsStorageMockWithProduct(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                                                               ReturnsNullStorageMock() ),
                                       GivenStorageItemCan(false, Guid.NewGuid() ),
                                       GivenProductStorageReservationManager(true, Guid.NewGuid() ),
                                       false
                                     };
        }

        private StorageReservationManager GetSut(Mock<IStorageRepository> storageRepositoryMock,
                                                 Mock<IStorageItemWriteRepository> storageItemWriteRepositoryMock,
                                                 Mock<IProductStorageReservationRepository> productStorageReservationRepositoryMock)
        {
            return new StorageReservationManager(storageRepositoryMock.Object, storageItemWriteRepositoryMock.Object, productStorageReservationRepositoryMock.Object);
        }
        private static Mock<IStorageRepository> GivenStorageRepository(StorageRead? returnsStorageWithSpace, StorageRead? returnsStorageWithProduct, StorageRead? returnsStorageById)
        {
            var storageRepositoryMock = new Mock<IStorageRepository>();

            storageRepositoryMock.Setup(repo => repo.GetStorageWithSpace())
                                 .ReturnsAsync(() => returnsStorageWithSpace);

            storageRepositoryMock.Setup(repo => repo.GetStorageWithProduct(It.IsAny<Guid>()))
                                  .ReturnsAsync((Guid productId) => returnsStorageWithProduct);

            storageRepositoryMock.Setup(repo => repo.GetStorageById(It.IsAny<Guid>()))
                                  .ReturnsAsync((Guid storageId) => returnsStorageById );
            return storageRepositoryMock;
        }
        private static Mock<IStorageItemWriteRepository> GivenStorageItemCan(bool beAdded, Guid pickItemId = default )
        {
            var storageItemRepositoryMock = new Mock<IStorageItemWriteRepository>();
            
            storageItemRepositoryMock.Setup(repo => repo.AddItemToStorage(It.IsAny<IAddItemInformation>(), It.IsAny<Guid>()))
                                        .ReturnsAsync(beAdded);

            storageItemRepositoryMock.Setup(repo => repo.PickItemFromStorage(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                        .ReturnsAsync(pickItemId);

            return storageItemRepositoryMock;
        }
        private static Mock<IProductStorageReservationRepository> GivenProductStorageReservationManager(bool recordStorageReservation, Guid returnsProductReservationStorageId)
        {
            var productStorageReservationManagerMock = new Mock<IProductStorageReservationRepository>();

            productStorageReservationManagerMock.Setup(manager => manager.RecordStorageReservation(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                                               .ReturnsAsync(recordStorageReservation);

            productStorageReservationManagerMock.Setup(manager => manager.RetrieveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()))
                                              .ReturnsAsync(returnsProductReservationStorageId);

            productStorageReservationManagerMock.Setup(manager => manager.RemoveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()));

            return productStorageReservationManagerMock;
        }
        private static StorageRead ReturnsStorageMockWithSpace(Guid storageId) => new StorageRead(storageId, "DummyStorageWithSpaceDescription", 100, Enumerable.Empty<StorageItemRead>());
        private static StorageRead ReturnsStorageMockWithProduct(Guid storageId, Guid productId, Guid itemId) => new StorageRead(storageId, "DummyStorageWithProductDescription", 100, new [] { StorageItemMock(productId,itemId ) } );
        private static StorageRead ReturnsStorageMockById(Guid storageId) => new StorageRead(storageId, "DummyStorageByIdDescription", 100, Enumerable.Empty<StorageItemRead>());
        private static StorageRead? ReturnsNullStorageMock() => null;

        private static StorageItemRead StorageItemMock(Guid withProductId, Guid andItemId) => new StorageItemRead(withProductId, andItemId, "DummySku", "DummyItemDescription", 10);
    }
}