using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.DTO;
using Ecom_Onboarding.BLL.Services;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Repository;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Model = Ecom_Onboarding.DAL.Models;
using Ecom_onboarding.DAL.Repository;
using Ecom_Onboarding.BLL.Redis;
using Ecom_Onboarding.BLL.Eventhub;
using Microsoft.AspNetCore.Authorization;


namespace Ecom_Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private IRedisService _redis;
        private ILogger<RedisController> _logger;
        private IMapper _mapper;

        public RedisController(ILogger<RedisController> logger, IMapper mapper, IConfiguration configuration, IRedisService redis)
        {
            _mapper = mapper;
            _logger = logger;
            _redis = redis;
        }

        /// <summary>
        /// Get game by name in redis
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
            var game = await _redis.GetAsync<Game>($"game_gameName:{Name}");
            if (game != null)
            {
                GameDTO mappedResult = _mapper.Map<GameDTO>(game);
                return new OkObjectResult(mappedResult);
            }
            return new NotFoundResult();
        }

        /// <summary>
        /// Create game in redis 
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
            
            //var game = await _redis.GetAsync<Game>($"game_gameName:{gameDTO.Name}");
            //if (game == null)
            //{
            //    await _redis.SaveAsync($"game_gameName:{gameDTO.Name}", game);
            //    return new OkResult();
            //}
            //else return new BadRequestResult();

            try
            {
                Game game = _mapper.Map<Game>(gameDTO);
                await _redis.SaveAsync($"game_gameName:{gameDTO.Name}", game);
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BadRequestResult();
            }

        }

        /// <summary>
        /// Delete game from redis 
        /// </summary>
        /// <param Name="Name">game data.</param>
        /// <response code="200">Request ok.</response>
        [HttpDelete]
        [Route("{Name}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        public ActionResult Delete([FromRoute] string Name)
        {
            _redis.DeleteAsync($"game_gameName:{Name}");

            return new OkResult();
        }


    }
}
