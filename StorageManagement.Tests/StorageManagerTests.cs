using Moq;

namespace StorageManagement.Tests
{
    public class StorageManagerTests
    {
        [Theory]
        [MemberData(nameof(ProductInData))]
        public void Storage_Happy_Path_Can_Put_Product_In(Mock<IStorageManager> storageManagerMock, Mock<IProductStorageReservationManager> productStorageReservationManagerMock, bool expectedResult)
        {

            var sut = GetSut(storageManagerMock, productStorageReservationManagerMock);

            var productId = Guid.NewGuid();
            var result = sut.ProductIn(productId);

            Assert.Equal(expectedResult, result);
        }
        public static IEnumerable<object[]> ProductInData()
        {
            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ) ),
                                        GivenProcuctStorageReservationManager(true, Guid.NewGuid() ),
                                        true
                                      };
            yield return new object[] {
                                        GivenStorageManager( NullStorageMock() ,
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ) ),
                                        GivenProcuctStorageReservationManager(true, Guid.Parse("408F6B2D-6FC5-4AAC-89B7-2531E732B030") ),
                                        false
                                      };
        }


        [Theory]
        [MemberData(nameof(ReserveProductData))]
        public void Storage_Happy_Path_Can_Reserve_Product(Mock<IStorageManager> storageManagerMock, Mock<IProductStorageReservationManager> productStorageReservationManagerMock, bool expectedResult)
        {
            var sut = GetSut(storageManagerMock, productStorageReservationManagerMock);

            var someExternaleReference = "externalReferenceId";
            var productId = Guid.NewGuid();
            var result = sut.ReserveProduct(someExternaleReference, productId);

            Assert.Equal(expectedResult, result);
        }
        public static IEnumerable<object[]> ReserveProductData()
        {
            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ) ),
                                        GivenProcuctStorageReservationManager(true, Guid.NewGuid() ),
                                        true
                                      };
            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid() ) ),
                                        GivenProcuctStorageReservationManager(false,  Guid.NewGuid() ),
                                        false
                                      };
        }

        [Theory]
        [MemberData(nameof(ProductOutData))]
        public void Storage_Happy_Path_Can_Take_Product_Out(Mock<IStorageManager> storageManagerMock, Mock<IProductStorageReservationManager> productStorageReservationManagerMock, bool expectedToProductOut)
        {
            var sut = GetSut(storageManagerMock, productStorageReservationManagerMock);

            var someExternaleReference = "externalReferenceId";
            var productId = Guid.NewGuid();
            var result = sut.ProductOut(someExternaleReference, productId);

            Assert.True(expectedToProductOut
                                ? result != Guid.Empty
                                : result == Guid.Empty);
        }
        public static IEnumerable<object[]> ProductOutData()
        {
            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()) ),
                                        GivenProcuctStorageReservationManager(true, Guid.NewGuid() ),
                                        true
                                      };

            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             NullStorageMock() ),
                                        GivenProcuctStorageReservationManager(true, Guid.NewGuid() ),
                                        false
                                      };
            
            yield return new object[] {
                                        GivenStorageManager( StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, Guid.NewGuid()),
                                                             StorageMockWith(Guid.NewGuid(), true, true, true, (Guid?)null) ),
                                        GivenProcuctStorageReservationManager(true, Guid.NewGuid() ),
                                        false
                                      };
        }

        private StorageReservationManager GetSut(Mock<IStorageManager> storageManagerMock, Mock<IProductStorageReservationManager> productStorageReservationManagerMock)
        {
            return new StorageReservationManager(storageManagerMock.Object, productStorageReservationManagerMock.Object);
        }
        private static Mock<IStorageManager> GivenStorageManager(IStorage? returnsStorageWithSpace, IStorage? returnsStorageWithProduct, IStorage? returnsStorageById)
        {
            var storageManagerMock = new Mock<IStorageManager>();

            storageManagerMock.Setup(manager => manager.GetStorageWithSpace())
                                 .Returns(() => returnsStorageWithSpace);

            storageManagerMock.Setup(manager => manager.GetStorageWithProduct(It.IsAny<Guid>()))
                                  .Returns((Guid productId) => returnsStorageWithProduct);

            storageManagerMock.Setup(manager => manager.GetStorageById(It.IsAny<Guid>()))
                                  .Returns((Guid storageId) => returnsStorageById);
            return storageManagerMock;
        }
        private static Mock<IProductStorageReservationManager> GivenProcuctStorageReservationManager(bool recordStorageReservation, Guid returnsStorageReservationProductId)
        {
            var productStorageReservationManagerMock = new Mock<IProductStorageReservationManager>();

            productStorageReservationManagerMock.Setup(manager => manager.RecordStorageReservation(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                                               .Returns(recordStorageReservation);

            productStorageReservationManagerMock.Setup(manager => manager.RetrieveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()))
                                              .Returns(returnsStorageReservationProductId);

            productStorageReservationManagerMock.Setup(manager => manager.RemoveStorageReservation(It.IsAny<string>(), It.IsAny<Guid>()));

            return productStorageReservationManagerMock;
        }
        private static IStorage StorageMockWith(Guid id, bool productAvailable, bool storageSpace, bool canAddItem, Guid? storageItemProductId)
        {
            var storageMock = new Mock<IStorage>();
            storageMock.SetupGet(s => s.Id).Returns(id);
            storageMock.Setup(s => s.HasProductAvailable(It.IsAny<Guid>())).Returns(productAvailable);
            storageMock.Setup(s => s.HasStorageSpace()).Returns(storageSpace);
            storageMock.Setup(s => s.AddItem(It.IsAny<Guid>())).Returns(canAddItem);
            storageMock.Setup(s => s.PickItem(It.IsAny<Guid>())).Returns(storageItemProductId.HasValue
                                                                                             ? StorageItem.NewItem(storageItemProductId.Value)
                                                                                             : StorageItem.Empty() );

            return storageMock.Object;
        }
        private static IStorage? NullStorageMock() => null;
    }
}