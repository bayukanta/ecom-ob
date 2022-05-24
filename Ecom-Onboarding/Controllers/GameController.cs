using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.DTO;

namespace Ecom_Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private static Dictionary<int, GameDTO> gameDict = new Dictionary<int, GameDTO>();

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all list game
        /// </summary>
        /// <response code="200">Request ok.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<GameDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult GetAll()
        {
            return new OkObjectResult(gameDict.Values.ToList());
        }

        /// <summary>
        /// Get game by id
        /// </summary>
        /// <param id="Id">user Model.</param>
        /// <response code="200">Request ok.</response>
        /// <response code="405">Request not found.</response>
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult GetByTitle([FromRoute] int Id)
        {
            var game = gameDict.GetValueOrDefault(Id, null);
            if (game != null)
            {
                return new OkObjectResult(game);
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
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Create([FromBody] GameDTO game)
        {
            if (gameDict.TryAdd(game.Id, game))
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// Create game 
        /// </summary>
        /// <param id="Id">game data.</param>
        /// <response code="200">Request ok.</response>
        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        public ActionResult Delete([FromRoute] int Id)
        {
            gameDict.Remove(Id);
            return new OkResult();
        }
    }
}
