using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.DTO;
using Ecom_Onboarding.BLL.Services;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Interface;
using Ecom_Onboarding.DAL.Repository;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Model = Ecom_Onboarding.DAL.Models;

namespace Ecom_Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private IMapper _mapper;
        private readonly ILogger<GameController> _logger;


        public GameController(ILogger<GameController> logger, IUnitOfWork uow, IConfiguration configuration, IMapper mapper, IRedisService redis)
        {
            _logger = logger;
            _mapper = mapper;
            _gameService ??= new GameService(uow, configuration, redis);
        }

        /// <summary>
        /// Get all list game
        /// </summary>
        /// <response code="200">Request ok.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<GameDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetAllAsync()
        {

            List<Game> result = await _gameService.GetAllGameAsync();
            List<GameDTO> mappedResult = _mapper.Map<List<GameDTO>>(result);
            return new OkObjectResult(mappedResult);
        }

        /// <summary>
        /// Get game by id
        /// </summary>
        /// <param Name="Name">user Model.</param>
        /// <response code="200">Request ok.</response>
        /// <response code="405">Request not found.</response>
        [HttpGet]
        [Route("{Name}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetByNameAsync([FromRoute] string Name)
        {
            Game result = await _gameService.GetGameByNameAsync(Name);
            if (result != null)
            {
                GameDTO mappedResult = _mapper.Map<GameDTO>(result);
                return new OkObjectResult(mappedResult);
            }
            return new NotFoundResult();
        }

        /// <summary>
        /// Create game 
        /// </summary>
        /// <param game="game">game data.</param>
        /// <response code="200">Request ok.</response>
        /// <response code="400">Request failed because of an exception.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateAsync([FromBody] GameDTO gameDTO)
        {
            try
            {
                Game game = _mapper.Map<Game>(gameDTO);
                await _gameService.CreateGameAsync(game);
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Create game 
        /// </summary>
        /// <param Name="Name">game data.</param>
        /// <response code="200">Request ok.</response>
        [HttpDelete]
        [Route("{Name}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        public ActionResult Delete([FromRoute] string Name)
        {
            _gameService.DeleteGame(Name);
            return new OkResult();
        }

        /// <summary>
        /// Update game 
        /// </summary>
        /// <param Name="Name">game data.</param>
        /// <response code="200">Request ok.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        public ActionResult Update([FromBody] GameDTO gameDTO)
        {
            Model.Game game = _mapper.Map<Model.Game>(gameDTO);
            _gameService.UpdateGame(game);
            return new OkResult();
        }

    }
}
