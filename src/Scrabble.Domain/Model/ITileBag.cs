using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public interface ITileBag
    {
        int Count { get; }

        List<Tile> DrawTiles(int drawCount);

    }
}
