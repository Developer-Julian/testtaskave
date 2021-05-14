using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskAve.WebClient.Models;
using TestTaskAve.WebClient.Services;

namespace TestTaskAve.WebClient.Controllers.V1
{
    [Route("v1/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ITcpServerMessenger tcpServerMessenger;
        public ChatController(ITcpServerMessenger tcpServerMessenger)
        {
            this.tcpServerMessenger = tcpServerMessenger;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("connect")]
        public ActionResult Connect([FromBody] ChatMessage message)
        {
            this.tcpServerMessenger.Connect(message.UserName);
            return this.Ok();
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("disconnect")]
        public ActionResult Disconnect([FromBody] ChatMessage message)
        {
            this.tcpServerMessenger.Disconnect(message.UserName);
            return this.Ok();
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("")]
        public ActionResult SendMessage([FromBody] ChatMessage message)
        {
            this.tcpServerMessenger.SendMessage(message.UserName, message.Message);
            return this.Ok();
        }
    }
}
