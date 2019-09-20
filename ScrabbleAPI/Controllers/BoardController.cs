using System;
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
    public class BoardController : ControllerBase
    {


        // GET: api/Board


        // GET: api/Board/5
        //        public string Get(int id)
        //       {
        //           return "value";
        //       }

        [HttpGet]
        public ActionResult<IEnumerable<CoordSquare>> GetSquares(int id)
        {
            return new Board().GetCoordSquares();
        }

        [HttpGet]
        [Route("Tiles")]
        public ActionResult<IEnumerable<CoordSquare>> GetTiles(int id)
        {
            // return new Board().GetCoordSquares(filterForOccupied:true);
            var board = new Board();
            board.PlaceTile(new Coord(R._11, C._E), new Tile("Z", 5));

            return board.GetCoordSquares(filterForOccupied: true);

        }

        // POST: api/Board
        [HttpPost]
        [Route("Tiles")]
        public void PlaceTile([FromForm] Coord coord, [FromForm]Tile tile)
        {
            var board = new Board();
            board.PlaceTile(coord, tile);
        }

        // PUT: api/Board/5
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
