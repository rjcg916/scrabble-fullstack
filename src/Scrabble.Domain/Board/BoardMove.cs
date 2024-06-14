using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        // create a new board with list of tiles
        public Board MakeMove(List<TilePlacement> tileList)
        {
            Board board = new(this); // this.Copy();

            board.MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var loc = board.squares[coord.RowValue, coord.ColValue];
                loc.Tile = tile;
                loc.MoveOfOccupation = MovesMade;
            };

            return board;
        }

        // create a new board with list of tiles
        public Board MakeMove(Coord startFrom,
                                List<Tile> tiles,
                                Placement placement)
        {
            Board board = new(this); // this.Copy();

            board.MovesMade++;

            switch (placement)
            {
                case Placement.Star:
                case Placement.Horizontal:
                    board.PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.ColValue + index)), tile)
                        ).ToList());
                    break;

                case Placement.Vertical:
                    board.PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RowValue + index), startFrom.Col), tile)
                        ).ToList());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }

            return board;
        }
        // place tiles on board
        public void PlaceTiles(List<TilePlacement> tilePlacementList)
        {

            foreach (var (coord, tile) in tilePlacementList)
            {
                var location = squares[coord.RowValue, coord.ColValue];

                location.Tile = new Tile(tile.Letter);
                location.MoveOfOccupation = MovesMade;
            };
        }

        public int ScoreMove(List<TilePlacement> tilePlacementList)
        {
            PlacementSpec tileSpecs = tilePlacementList.ToPlacementSpec();

            return ScoreMove(tileSpecs.SliceLocation,
                                tileSpecs.TileLocations,
                                tileSpecs.Placement);
        }

        public int ScoreMove(int sliceLocation, List<int> tileLocations, Placement placement)
        {
            return placement switch
            {
                Placement.Horizontal => ScoreMoveDirectional(sliceLocation, tileLocations, true),
                Placement.Vertical => ScoreMoveDirectional(sliceLocation, tileLocations, false),
                _ => throw new Exception("Invalid Placement"),
            };
        }

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveDirectional(int sliceLocation, List<int> tileLocations, bool isHorizontal)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetEndpoints(isHorizontal, sliceLocation, tileLocations);
            var moveSlice = GetSquares(isHorizontal, sliceLocation, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            score += moveSlice.ScoreRun();

            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetEndpoints(!isHorizontal, perpendicularFixed, [sliceLocation]);

                if (multiRunStart < multiRunEnd)
                {
                    var perpendicularSlice = GetSquares(!isHorizontal, perpendicularFixed, multiRunStart, multiRunEnd);
                    var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
                    if ((perpendicularSlice.Count > 0) && anyNewTiles)
                        score += perpendicularSlice.ScoreRun();
                }
            }

            return score;
        }
    } 
}