using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public interface ITileBag
    {
        int count { get; }

        List<Tile> DrawTiles(int drawCount);

    }
}
