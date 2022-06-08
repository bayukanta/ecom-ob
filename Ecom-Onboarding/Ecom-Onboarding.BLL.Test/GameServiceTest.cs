using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Ecom_Onboarding.BLL;
using Ecom_Onboarding.BLL.Services;
using Ecom_Onboarding.BLL.Redis;
using Ecom_Onboarding.BLL.Eventhub;
using Ecom_Onboarding.DAL.Models;
using Ecom_onboarding.DAL.Repository;
using Ecom_Onboarding.BLL.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ecom_Onboarding.BLL.Test
{
    public class GameServiceTest
    {
        private IEnumerable<Game> games;
        private Mock<IRedisService> redis;
        private Mock<IUnitOfWork> uow;
        private Mock<IGameMessageSenderFactory> msg;

        public GameServiceTest()
        {
            games = CommonHelper.LoadDataFromFile<IEnumerable<Game>>(@"Mock\Game.json");
            uow = MockUnitOfWork();
            redis = MockRedis();
        }

        private GameService CreateGameService()
        {
            return new GameService(uow.Object, redis.Object);
        }

        #region method mock depedencies


        private Mock<IUnitOfWork> MockUnitOfWork()
        {
            var gameQueryable = games.AsQueryable().BuildMock().Object;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GameRepository.GetAll())
                .Returns(gameQueryable);

            mockUnitOfWork
                .Setup(u => u.GameRepository.IsExist(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns((Expression<Func<Game, bool>> condition) => gameQueryable.Any(condition));

            mockUnitOfWork
               .Setup(u => u.GameRepository.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .ReturnsAsync((Expression<Func<Game, bool>> condition) => gameQueryable.FirstOrDefault(condition));

            mockUnitOfWork
               .Setup(u => u.GameRepository.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Game game, CancellationToken token) =>
               {
                   game.Id = Guid.NewGuid();
                   return game;
               });

            mockUnitOfWork
                .Setup(u => u.GameRepository.Delete(It.IsAny<Expression<Func<Game, bool>>>()))
                .Verifiable();


            mockUnitOfWork
                .Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return mockUnitOfWork;
        }


        private Mock<IRedisService> MockRedis()
        {
            var mockRedis = new Mock<IRedisService>();

            mockRedis
                .Setup(x => x.GetAsync<Game>(It.Is<string>(x => x.Equals("game_gameName:stringa"))))
                .ReturnsAsync(games.FirstOrDefault(x => x.Id == Guid.Parse("stringa")))
                .Verifiable();

            mockRedis
                .Setup(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mockRedis
              .Setup(x => x.DeleteAsync(It.IsAny<string>())).Verifiable();

            return mockRedis;
        }


        #endregion method mock depedencies

        [Fact]
        public async Task GetAllAsync_Success()
        {
            //arrange
            var expected = games;

            var svc = CreateGameService();

            // act
            var actual = await svc.GetAllGameAsync();

            // assert      
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("stringa")]
        [InlineData("stringb")]
        public async Task GetByName_Success(string name)
        {
            //arrange
            
            var expected = games.First(x => x.Name == name);

            var svc = CreateGameService();

            //act
            var actual = await svc.GetGameByNameAsync(name);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateGame_Success()
        {
            //arrange
            var expected = new Game
            {
                Name = "string",
                Genre = "string"
            };

            var svc = CreateGameService();

            //act
            Func<Task> act = async () => { await svc.CreateGameAsync(expected); };

            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("Name")]
        public async Task DeleteGame_Success(string name)
        {
            //arrange

            var svc = CreateGameService();

            //act
            Func<Task> act = async () => { await svc.DeleteGameAsync(name); };
            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);

        }

        [Theory]
        [InlineData("stringc", "stringc edit")]
        public async Task UpdateUser_Success(string name, string genre)
        {
            //arrange
            var expected = new Game
            {
                Name = name,
                Genre = genre
            };

            var svc = CreateGameService();

            //act
            Func<Task> act = async () => { await svc.UpdateGameAsync(expected); };

            //assert
            await act.Should().NotThrowAsync<Exception>();
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }



    }
}
