using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Core.Interfaces;
using Moq;
using Xunit;
using System.Collections.Generic;
using Sat.Recruitment.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersCRUD
    {
        private readonly Mock<IUserService> _usersService;
        public UsersCRUD() 
        {
            _usersService = new Mock<IUserService>();
        }

        [Fact]
        public void GetUsers()
        {
            List<UserDetails> users = new List<UserDetails>();
            var usersFromFile = _usersService.Setup(x => x.GetUsers()).Returns(users.AsEnumerable());
            Assert.NotNull(users);
        }

        [Fact]
        public void GetUserById()
        {
            int id = 1;
            Task<UserDetails> user = Task.FromResult(new UserDetails { Id = id });
            var singleUser = _usersService.Object.GetUser(id);
            Assert.NotNull(singleUser);
            Assert.True(id == singleUser.Id, "true");
        }


        //var userController = new UsersController();

        //var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;


        //Assert.True(result.IsSuccess);
        //Assert.Equal("User Created", result.Errors);

        //[Fact]
        //public void Test2()
        //{
        //    //var userController = new UsersController();

        //    //var result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;


        //    //Assert.False(result.IsSuccess);
        //    //Assert.Equal("The user is duplicated", result.Errors);
        //}
    }
}
