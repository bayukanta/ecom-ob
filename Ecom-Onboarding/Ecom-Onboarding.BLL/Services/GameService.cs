using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.DAL.Models;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs;
using Ecom_onboarding.DAL.Repository;
using Ecom_Onboarding.BLL.Redis;
using Ecom_Onboarding.BLL.Eventhub;


namespace Ecom_Onboarding.BLL.Services
{
    public class GameService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IConfiguration _config;
        private IRedisService _redis;
        //private readonly IGameMessageSenderFactory _msgSernderFactory;


        public GameService(IUnitOfWork unitOfWork, IRedisService redis) //,IGameMessageSenderFactory msgSernderFactory)
        {
            _unitOfWork = unitOfWork;
            //_config = config;
            _redis = redis;
            //_msgSernderFactory = msgSernderFactory;

        }

        public async Task<List<Game>> GetAllGameAsync()
        {
            return await _unitOfWork.GameRepository.GetAll().ToListAsync();
        }

        public async Task<Game> GetGameByNameAsync(string Name)
        {
            var game = await _redis.GetAsync<Game>($"game_gameName:{Name}");
            if (game == null)
            {
                game = await _unitOfWork.GameRepository
                .GetAll()
                .FirstOrDefaultAsync(b => b.Name == Name);
                await _redis.SaveAsync($"game_gameName:{Name}", game);
            }
            return game;
        }

        public async Task CreateGameAsync(Game game)
        {
            bool isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == game.Name).Any();
            if (!isExist)
            {
                await _unitOfWork.GameRepository.AddAsync(game);
                await _unitOfWork.SaveAsync();

                //await SendGameToEventHub(game);
            }
            else
            {
                throw new Exception($"Game with {game.Name} already exist");
            }
        }

        //private async Task SendGameToEventHub(Game game)
        //{
        //    string topic = _config.GetValue<string>("EventHub:EventHubNameTest");

        //    //create event hub producer
        //    using IGameMessageSender message = _msgSernderFactory.Create(_config, topic);

        //    //create batch
        //    await message.CreateEventBatchAsync();

        //    //add message, ini bisa banyak sekaligus
        //    message.AddMessage(game);

        //    //send message
        //    await message.SendMessage();
        //}

        public async Task DeleteGameAsync(string Name)
        {
            bool isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == Name).Any();
            if (isExist)
            {
                _unitOfWork.GameRepository.Delete(x => x.Name == Name);
                await _unitOfWork.SaveAsync();
            }
           
        }

        public async Task UpdateGameAsync(Game game)
        {
            bool isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == game.Name).Any();
            if(isExist)
            {
                _unitOfWork.GameRepository.Edit(game);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception($"Game with name {game.Name} not exist");
            }
        }

    }
}
