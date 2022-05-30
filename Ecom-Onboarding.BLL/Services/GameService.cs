using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Interface;
using Microsoft.Extensions.Configuration;

namespace Ecom_Onboarding.BLL.Services
{
    public class GameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public GameService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<List<Game>> GetAllGameAsync()
        {
            return await _unitOfWork.GameRepository.GetAll().ToListAsync();
        }

        public async Task<Game> GetGameByNameAsync(string Name)
        {
            return await _unitOfWork.GameRepository
                .GetAll()
                .FirstOrDefaultAsync(b => b.Name == Name);
        }

        public async Task CreateGameAsync(Game game)
        {
            bool isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == game.Name).Any();
            if (!isExist)
            {
                _unitOfWork.GameRepository.Add(game);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception($"Game with {game.Name} already exist");
            }
        }

        public void DeleteGame(string Name)
        {
            _unitOfWork.GameRepository.Delete(x => x.Name == Name);
        }

        public void UpdateGame(Game game)
        {
            bool isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == game.Name).Any();
            if(isExist)
            {
                _unitOfWork.GameRepository.Edit(game);
                _unitOfWork.Save();
            }
        }

    }
}
