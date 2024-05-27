using System.Collections.Generic;

namespace Scrabble.Domain
{
    public interface ITileBag
    {
        int Count { get; }

        List<Tile> DrawTiles(int drawCount);

    }
}
