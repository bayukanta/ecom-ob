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
using Ecom_Onboarding.Models;

namespace Ecom_Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger, UnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var result = await _unitOfWork.GameRepository.GetAll().ProjectTo<GameDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new OkObjectResult(result);
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
        public async Task<ActionResult> GetByTitleAsync([FromRoute] string Name)
        {
            var game = await _unitOfWork.GameRepository.GetAll().Where(b => b.Name == Name).FirstOrDefaultAsync();
            if (game != null)
            {
                var gameDTO = _mapper.Map<GameDTO>(game);
                return new OkObjectResult(gameDTO);
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
            var isExist = _unitOfWork.GameRepository.GetAll().Where(x => x.Name == gameDTO.Name).Any();
            if (!isExist)
            {
                var game = _mapper.Map<Game>(gameDTO);
                await _unitOfWork.GameRepository.AddAsync(game);
                await _unitOfWork.SaveAsync();
                return new OkResult();
            }
            return new BadRequestResult();
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
            _unitOfWork.GameRepository.Delete(x => x.Name == Name);
            return new OkResult();
        }
    }
}
