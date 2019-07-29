using Moq;
using RentCar.Models;
using RentCar.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RentCarTests
{
    public class UnitTestAddress
    {
        [Fact]
        public void Insert()
        {
            var mock = new Mock<IAddressService>();
            mock.Setup(x => x.InsertAsync(It.IsAny<Address>()));

            var address = new Address
            {
                Id = 1,
                Street = "Abc",
                District = "Def",
                State = "Ghi",
                City = "Jkl",
                Number = 123
            };
            mock.Object.InsertAsync(address);
            mock.Verify(x => x.InsertAsync(It.IsAny<Address>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Update()
        {
            var mock = new Mock<IAddressService>();
            mock.Setup(x => x.UpdateAsync(It.IsAny<Address>()));

            var address = new Address
            {
                Id = 1,
                Street = "Abc",
                District = "Def",
                State = "Ghi",
                City = "Jkl",
                Number = 123
            };
            mock.Object.UpdateAsync(address);
            mock.Verify(x => x.UpdateAsync(It.IsAny<Address>()), Times.Once);
        }


        [Fact]
        public void Delete()
        {
            var mock = new Mock<IAddressService>();
            mock.Setup(x => x.RemoveAsync(It.IsAny<int>()));

            mock.Object.RemoveAsync(2);
            mock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void FindById()
        {
            var mock = new Mock<IAddressService>();
            mock.Setup(x => x.FindByIdAsync(It.IsAny<int>()));

            mock.Object.FindByIdAsync(1);
            mock.Object.FindByIdAsync(2);
            mock.Object.FindByIdAsync(3);

            mock.Verify(x => x.FindByIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task FindAll()
        {
            var mock = new Mock<IAddressService>();
            mock.Setup(x => x.FindAllAsync());

            var addresses1 = await mock.Object.FindAllAsync();
            List<Address> addresses2 = null;

            Assert.Equal<Address>(addresses2, addresses1);
        }
    }
}
