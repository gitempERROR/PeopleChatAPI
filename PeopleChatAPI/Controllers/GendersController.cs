using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleChatAPI.Models.PeopleChat;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Genders")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly PeopleChatContext _context;

        public GendersController(PeopleChatContext context)
        {
            _context = context;
        }
    }
}
