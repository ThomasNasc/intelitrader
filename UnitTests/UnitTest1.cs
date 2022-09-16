
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Registro.Controllers;
using Registro.Interfaces;
using Registro.Models;
using Registro.Services;
using Assert = Xunit.Assert;


namespace Registro.test
{

    public class RegisterControllerTest
    {
        [Fact]
        public async Task Get_Acess_Unit_One_Time_Once()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            var sut = new UsersController(mockUserService.Object);
            mockUserService.Setup(service => service.GetUsuarios())
                .ReturnsAsync(new List<User>() {        new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        firstName = "nome1",
                        surName = "nome2",
                        age = 23,
                        dateOfCreation = DateTime.Now
                    }});
            //Act
            var result = (OkObjectResult)await sut.GetAllUsuarios();



            mockUserService.Verify(service => service.GetUsuarios(), Times.Once());
        }

        [Fact]
        public async Task Get_Acess_All_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            var sut = new UsersController(mockUserService.Object);
            mockUserService.Setup(service => service.GetUsuarios())
                .ReturnsAsync(new List<User>() {        new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        firstName = "nome1",
                        surName = "nome2",
                        age = 23,
                        dateOfCreation = DateTime.Now
                    }});
            //Act
            var result = (OkObjectResult)await sut.GetAllUsuarios();


            //Assert
            result.StatusCode.Should().Be(200);



        }
        [Fact]
        public async Task Get_OnSucess_Return_ListOfUsers()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.GetUsuarios())
                .ReturnsAsync(new List<User>()
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        firstName = "nome1",
                        surName = "nome2",
                        age = 23,
                        dateOfCreation = DateTime.Now
                    }
                });

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.GetAllUsuarios();

            //Assert

            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<User>>();

        }
        [Fact]
        public async Task Get_OnNoUsersFound_Return404()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.GetUsuarios())
                .ReturnsAsync(new List<User>());

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.GetAllUsuarios();

            //Assert

            result.Should().BeOfType<NotFoundResult>();

        }

        [Fact]
        public async Task Get_Just_a_User()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.GetUser("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync((new User()
            {
                Id = "c7bfdef8-27c-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = DateTime.Now
            }));


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.GetAsyncUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            //Assert
            Assert.IsType<ActionResult<User>>(result);
        }
        [Fact]
        public async Task Get_user_doenst_exist()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.GetUser("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(new User
            {
                Id = "Usuario Invalido",
                firstName = "Invalido",
                age = 0,
                dateOfCreation = DateTime.Now
            });


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.GetAsyncUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            //Assert
            Assert.IsType<NotFoundResult>(result.Result);



        }


        [Fact]
        public async Task Post_a_user_With_Sucess()
        {
            User userForTest = new User()
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = DateTime.Now
            };
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.PostUser(userForTest)).ReturnsAsync(userForTest);
            ;

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.PostAsyncUser(userForTest);

            //Assert 
            result.Should().BeOfType<CreatedAtActionResult>();
            var objectResult = (CreatedAtActionResult)result;
            objectResult.StatusCode.Should().Be(201);


        }
        [Fact]
        public async Task Post_a_user_With_Failure()
        {
            User userForTest = new User()
            {
                //Usuario sem first name(atributo obrigatorio)
                surName = "nome2",
                age = 23,
            };
            //Arrange
            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(service => service.PostUser(userForTest)).ReturnsAsync(userForTest);
            ;

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.PostAsyncUser(userForTest);

            //Assert 
            result.Should().BeOfType<BadRequestResult>();
            var objectResult = (BadRequestResult)result;
            objectResult.StatusCode.Should().Be(400);
        }



        [Fact]
        public async Task Delete_onSucess()

        {
            User userForTest = new User()
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = DateTime.Now
            };
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.DeleteUser("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(new ObjectResult(null)
            {
                StatusCode = 204
            });


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.DeleteAsyncUser(userForTest.Id);

            //Assert 

            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_Failed()

        {
            User userForTest = new User()
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = DateTime.Now
            };
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.DeleteUser("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(new ObjectResult(null)
            {
                StatusCode = 404
            });


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.DeleteAsyncUser(userForTest.Id);

            //Assert 

            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Put_onSucess()

        {
            User userForTest = new User()
            {

                firstName = "nome1",

            };
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.PutUser("c7bfdef8-27c0-4895-99d1-a14b048025e8", userForTest)).ReturnsAsync(new ObjectResult(null)
            {
                StatusCode = 204
            });


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.PutAsyncUser("c7bfdef8-27c0-4895-99d1-a14b048025e8", userForTest);

            //Assert 

            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Put_Failed()

        {
            User userForTest = new User()
            {

                firstName = "nome1",

            };
            //Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.PutUser("c7bfdef8-27c0-4895-99d1-a14b048025e8", userForTest)).ReturnsAsync(new ObjectResult(null)
            {
                StatusCode = 404
            });


            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.PutAsyncUser("c7bfdef8-27c0-4895-99d1-a14b048025e8", userForTest);

            //Assert 

            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(404);
        }


        ///
        /// ///TESTS SERVICES
        ///
        [Fact]
        public async void POST_TEST_IF_NEW_USER_HAS_BEEN_ACEPTED_AND_THE_VALUES_ID_AND_DATE_WASNT_DEFINED_BY_USER()
        {

            var userForTest = new User()
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nomehsdfhfhtest1",
                surName = "nometest2",
                age = 23,
                dateOfCreation = new DateTime(2020, 07, 02, 22, 59, 59)
            };
            var mock = new Mock<ILogger<UserServices>>();
            ILogger<UserServices> logger = mock.Object;
            var options = new DbContextOptionsBuilder<UserContext>().Options;
            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<UserContext>(options);
            mockContext.Setup(m => m.Usuarios).Returns(mockSet.Object);
            var service = new UserServices(mockContext.Object, logger);


            var result = await service.PostUser(userForTest);

            Assert.Equal(userForTest.firstName, result.firstName);
            Assert.Equal(userForTest.surName, result.surName);
            Assert.Equal(userForTest.age, result.age);
            Assert.NotEqual(userForTest.Id, result.Id);
            Assert.NotEqual(userForTest.dateOfCreation, result.dateOfCreation);
        }
        //[Fact]
        //public async void PUT_TEST_IF_USER_HAS_CHANGED_WITH_SUCESS_()
        //{
        //    var UserExpected = new User
        //    {
        //        Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
        //        firstName = "nome1",
        //        surName = "nome2",
        //        age = 23,
        //        dateOfCreation = new DateTime(2020, 07, 02, 22, 59, 59)
        //    };


        //    var UserChange = new User()
        //    {

        //        firstName = "nome1",

        //    };

        //    var mock = new Mock<ILogger<UserServices>>();
        //    ILogger<UserServices> logger = mock.Object;
        //    var options = new DbContextOptionsBuilder<UserContext>().Options;

        //    var mockSet = new Mock<DbSet<User>>();
        //    mockSet.Setup(s => s.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(UserExpected);

        //    var mockContext = new Mock<UserContext>(options);
        //    mockContext.Setup(m => m.Usuarios).Returns(mockSet.Object);

        //    var service = new UserServices(mockContext.Object, logger);


        //    var result = await service.PutUser("c7bfdef8-27c0-4895-99d1-a14b048025e8", UserChange);


        //    result.Should().BeOfType<ObjectResult>();
        //    var objectResult = result;
        //    objectResult.StatusCode.Should().Be(204);
        //}
        [Fact]
        public async void GetUNIQUEUSER_()
        {
            var UserExpected = new User
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = new DateTime(2020, 07, 02, 22, 59, 59)
            };

            var mock = new Mock<ILogger<UserServices>>();
            ILogger<UserServices> logger = mock.Object;


            var options = new DbContextOptionsBuilder<UserContext>().Options;

            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(s => s.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(UserExpected);

            var mockContext = new Mock<UserContext>(options);
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UserServices(mockContext.Object, logger);

            var result = await service.GetUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            Assert.Equal(UserExpected, result);
            mockSet.Verify(m => m.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8"), Times.Once());

        }

        [Fact]
        public async void Get_UNIQUE_USER_FAIL()
        {


            var mock = new Mock<ILogger<UserServices>>();
            ILogger<UserServices> logger = mock.Object;


            var options = new DbContextOptionsBuilder<UserContext>().Options;

            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(s => s.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8")).Returns(null);

            var mockContext = new Mock<UserContext>(options);
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UserServices(mockContext.Object, logger);

            var result = await service.GetUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            Assert.Equal("Usuario Invalido", result.Id);
            mockSet.Verify(m => m.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8"), Times.Once());

        }


        [Fact]
        public async void DELETE_SUCESS_SERVICE()
        {
            var UserExpected = new User
            {
                Id = "c7bfdef8-27c0-4895-99d1-a14b048025e8",
                firstName = "nome1",
                surName = "nome2",
                age = 23,
                dateOfCreation = new DateTime(2020, 07, 02, 22, 59, 59)
            };

            var mock = new Mock<ILogger<UserServices>>();
            ILogger<UserServices> logger = mock.Object;


            var options = new DbContextOptionsBuilder<UserContext>().Options;

            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(s => s.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8")).ReturnsAsync(UserExpected);

            var mockContext = new Mock<UserContext>(options);
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UserServices(mockContext.Object, logger);

            var result = await service.DeleteUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            result.Should().BeOfType<ObjectResult>();

            result.StatusCode.Should().Be(204);

        }

        [Fact]
        public async void DELETE_FAILED_USER_DIDNT_EXIST_SERVICE()
        {


            var mock = new Mock<ILogger<UserServices>>();
            ILogger<UserServices> logger = mock.Object;


            var options = new DbContextOptionsBuilder<UserContext>().Options;

            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(s => s.FindAsync("c7bfdef8-27c0-4895-99d1-a14b048025e8")).Returns(null);

            var mockContext = new Mock<UserContext>(options);
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UserServices(mockContext.Object, logger);

            var result = await service.DeleteUser("c7bfdef8-27c0-4895-99d1-a14b048025e8");


            result.Should().BeOfType<ObjectResult>();

            result.StatusCode.Should().Be(404);

        }

    }




}