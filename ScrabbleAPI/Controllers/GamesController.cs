﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrabbleLib.Model;

namespace ScrabbleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGames games;
        public GamesController(IGames games) => this.games = games; 

        // GET: api/Games
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Games/5
        [HttpGet("{id}", Name = "Get")]
        public Game Get(int id)
        {
            return this.games.GetGame(id);
        }

        // POST: api/Games
        [HttpPost]
        public int Post([FromForm] byte numberOfPlayers)
        {
            var i = this.games.CreateGame(numberOfPlayers);

            return i;

        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
