using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using Ecom_Onboarding.DTO;
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

namespace Ecom_Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventhubController : ControllerBase
    {
        private readonly IGameMessageSenderFactory _msgSernderFactory;
        private IMapper _mapper;
        private IConfiguration _config;
        private readonly ILogger<EventhubController> _logger;



        public EventhubController(ILogger<EventhubController> logger, IConfiguration configuration, IMapper mapper, IGameMessageSenderFactory a)
        {
            _mapper = mapper;
            _config = configuration;
            _logger = logger;
            _msgSernderFactory = a;
        }
        /// <summary>
        /// Create message to eventhub 
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
                string topic = _config.GetValue<string>("EventHub:EventHubNameTest");

                //create event hub producer
                using IGameMessageSender message = _msgSernderFactory.Create(_config, topic);

                //create batch
                await message.CreateEventBatchAsync();

                //add message, ini bisa banyak sekaligus
                message.AddMessage(game);

                //send message
                await message.SendMessage();
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BadRequestResult();
            }
        }
    }
}
