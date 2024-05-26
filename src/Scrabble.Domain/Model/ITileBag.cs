using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Domain.Model
{
    public interface ITileBag
    {
        int count { get; }

        List<Tile> DrawTiles(int drawCount);

    }
}
