using Moq;

namespace StorageManagement.Tests
{
    public class StorageManagerTests
    {
        [Theory]
        [MemberData(nameof(ProductInData))]
        public async Task Storage_Happy_Path_Can_Put_Product_In(Mock<IStorageProvider> storageRepositoryMock,
                                                                Mock<IStorageItemProvider> storageItemWriteRepositoryMock,
                                                                Mock<IProductStorageReservationProvider> productStorageReservationRepositoryMock,
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
        public async Task Storage_Happy_Path_Can_Reserve_Product(Mock<IStorageProvider> storageRepositoryMock,
                                                                 Mock<IStorageItemProvider> storageItemWriteRepositoryMock,
                                                                 Mock<IProductStorageReservationProvider> productStorageReservationRepositoryMock,
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
        public async Task Storage_Happy_Path_Can_Take_Product_Out(Mock<IStorageProvider> storageRepositoryMock,
                                                                  Mock<IStorageItemProvider> storageItemWriteRepositoryMock,
                                                                  Mock<IProductStorageReservationProvider> productStorageReservationRepositoryMock,
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

        private StorageReservationManager GetSut(Mock<IStorageProvider> storageRepositoryMock,
                                                 Mock<IStorageItemProvider> storageItemWriteRepositoryMock,
                                                 Mock<IProductStorageReservationProvider> productStorageReservationRepositoryMock)
        {
            return new StorageReservationManager(storageRepositoryMock.Object, storageItemWriteRepositoryMock.Object, productStorageReservationRepositoryMock.Object);
        }
        private static Mock<IStorageProvider> GivenStorageRepository(IStorage? returnsStorageWithSpace, IStorage? returnsStorageWithProduct, IStorage? returnsStorageById)
        {
            var storageRepositoryMock = new Mock<IStorageProvider>();

            storageRepositoryMock.Setup(repo => repo.GetStorageWithSpace())
                                 .ReturnsAsync(() => returnsStorageWithSpace);

            storageRepositoryMock.Setup(repo => repo.GetStorageWithProduct(It.IsAny<Guid>()))
                                  .ReturnsAsync((Guid productId) => returnsStorageWithProduct);

            storageRepositoryMock.Setup(repo => repo.GetStorageById(It.IsAny<Guid>()))
                                  .ReturnsAsync((Guid storageId) => returnsStorageById );
            return storageRepositoryMock;
        }
        private static Mock<IStorageItemProvider> GivenStorageItemCan(bool beAdded, Guid pickItemId = default )
        {
            var storageItemRepositoryMock = new Mock<IStorageItemProvider>();
            
            storageItemRepositoryMock.Setup(repo => repo.AddItemToStorage(It.IsAny<IAddItemInformation>(), It.IsAny<Guid>()))
                                        .ReturnsAsync(beAdded);

            storageItemRepositoryMock.Setup(repo => repo.PickItemFromStorage(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                        .ReturnsAsync(pickItemId);

            return storageItemRepositoryMock;
        }
        private static Mock<IProductStorageReservationProvider> GivenProductStorageReservationManager(bool recordStorageReservation, Guid returnsProductReservationStorageId)
        {
            var productStorageReservationManagerMock = new Mock<IProductStorageReservationProvider>();

            productStorageReservationManagerMock.Setup(manager => manager.RecordStorageReservation(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                                               .ReturnsAsync(recordStorageReservation);

            productStorageReservationManagerMock.Setup(manager => manager.RetrieveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()))
                                              .ReturnsAsync(returnsProductReservationStorageId);

            productStorageReservationManagerMock.Setup(manager => manager.RemoveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()));

            return productStorageReservationManagerMock;
        }
        private static IStorage ReturnsStorageMockWithSpace(Guid storageId)
        {
            var storageMock = new Mock<IStorage>();
            storageMock.SetupGet(s => s.Id).Returns(storageId);
            storageMock.SetupGet(s => s.Description).Returns("DummyStorageWithSpaceDescription");
            storageMock.SetupGet(s => s.Capacity).Returns(20);
            storageMock.SetupGet(s => s.StorageItems).Returns( Enumerable.Empty<IStorageItem> );
            return storageMock.Object;
        }
        private static IStorage ReturnsStorageMockWithProduct(Guid storageId, Guid productId, Guid itemId)
        {
            var storageMock = new Mock<IStorage>();
            storageMock.SetupGet(s => s.Id).Returns(storageId);
            storageMock.SetupGet(s => s.Description).Returns("DummyStorageWithProductDescription");
            storageMock.SetupGet(s => s.Capacity).Returns(20);
            storageMock.SetupGet(s => s.StorageItems).Returns(new[] { StorageItemMock(productId, itemId) });
            return storageMock.Object;
        }
        private static IStorage ReturnsStorageMockById(Guid storageId)
        {
            var storageMock = new Mock<IStorage>();
            storageMock.SetupGet(s => s.Id).Returns(storageId);
            storageMock.SetupGet(s => s.Description).Returns("DummyStorageByIdDescription");
            storageMock.SetupGet(s => s.Capacity).Returns(20);
            storageMock.SetupGet(s => s.StorageItems).Returns(Enumerable.Empty<IStorageItem>);
            return storageMock.Object;
        }
        private static IStorage? ReturnsNullStorageMock() => null;

        private static IStorageItem StorageItemMock(Guid withProductId, Guid andItemId)
        {
            var storageItemMock = new Mock<IStorageItem>();
            storageItemMock.SetupGet(s => s.ProductId).Returns(withProductId);
            storageItemMock.SetupGet(s => s.ItemId).Returns(andItemId);
            storageItemMock.SetupGet(s => s.SKU).Returns("DummySku");
            storageItemMock.SetupGet(s => s.Description).Returns("DummyItemDescription");
            storageItemMock.SetupGet(s => s.Price).Returns(20);
            return storageItemMock.Object;
        }
    }
}