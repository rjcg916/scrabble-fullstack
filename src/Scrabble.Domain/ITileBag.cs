using System.Collections.Generic;

namespace Scrabble.Domain
{
    public interface ITileBag
    {
        int Count { get; }

        (List<Tile> drawnTiles, TileBag newTileBag) DrawTiles(TileDrawCount count);

    }
}
