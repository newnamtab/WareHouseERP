namespace StorageManagement.Tests
{
    public class StorageTests
    {
        [Theory]
        [InlineData(5, true)]
        [InlineData(0, false)]
        public void Storage_Happy_Path_Has_Storage_Space(int capacity, bool expectedToHaveStorageSpace)
        {
            var sut = GetSut(capacity);
            var productId = Guid.NewGuid();

            var hasStorageSpace = sut.HasStorageSpace(productId);

            Assert.Equal(expectedToHaveStorageSpace, hasStorageSpace);
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(0, false)]
        public void Storage_Happy_Path_Can_AddItem(int capacity, bool expectedToAdd)
        {
            var sut = GetSut(capacity);
            var productId = Guid.NewGuid();
            
            var addResult = sut.AddItem(productId);

            Assert.Equal(expectedToAdd, addResult);
        }

        [Fact]
        public void Storage_Happy_Path_Has_Product_Available()
        {
            var sut = GetSut();
            var productId = Guid.NewGuid();
            sut.AddItem(productId);

            var hasProductAvailable = sut.HasProductAvailable(productId);

            Assert.True(hasProductAvailable);
        }
        [Fact]
        public void Storage_Sad_Path_NOT_Have_Product_Available()
        {
            var sut = GetSut();
            
            sut.AddItem( Guid.NewGuid() );

            var hasProductAvailable = sut.HasProductAvailable(Guid.NewGuid());

            Assert.False(hasProductAvailable);
        }

        [Fact]
        public void Storage_Happy_Path_Can_Remove_Product()
        {
            var sut = GetSut();
            var productId = Guid.NewGuid();
            sut.AddItem(productId);

            var removeResult = sut.RemoveItem(productId);

            Assert.Equal(productId, removeResult.ProductId);
        }
        
        [Fact]
        public void Storage_Sad_Path_Can_NOT_Remove_Product_When_Not_Present()
        {
            var sut = GetSut();
            var someNotExistingProductId = Guid.NewGuid();
            var removeResult = sut.RemoveItem( someNotExistingProductId );

            Assert.Equal(Guid.Empty, removeResult.ProductId);
            Assert.Equal(Guid.Empty, removeResult.ItemId);
        }
        private Storage GetSut(int withCapacity = 5) => new Storage(withCapacity);
    }
}
