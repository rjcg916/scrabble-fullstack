using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {

        // return a new board with tiles placed in the move
        public Board MakeMove(List<TilePlacement> tileList)
        {
            Board board = new(this);

            board.MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var loc = board.squares[coord.RVal, coord.CVal];
                loc.Tile = tile;
                loc.MoveOfOccupation = board.MovesMade;
            };

            return board;
        }

        // return a new board with tiles placed in the move
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
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)
                        ).ToList());
                    break;

                case Placement.Vertical:
                    board.PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)
                        ).ToList());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }

            return board;
        }

        // place tiles on board
        internal void PlaceTiles(List<TilePlacement> tilePlacementList)
        {
            foreach (var (coord, tile) in tilePlacementList)
            {
                var location = squares[coord.RVal, coord.CVal];

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
               Placement.Horizontal => ScoreMove(SquareByColumn, SquareByRow, sliceLocation, tileLocations, MovesMade),
               Placement.Vertical => ScoreMove(SquareByRow, SquareByColumn, sliceLocation, tileLocations, MovesMade),
                _ => throw new Exception("Invalid Placement")
            };
        }

        internal int ScoreMove(Func<int, int, Square> primaryDirection, 
                               Func<int, int, Square> secondaryDirection, 
                               int sliceLocation, List<int> tileLocations, int MovesMade)
        {
            var (singleRunStart, singleRunEnd) = GetEndpoints(primaryDirection, sliceLocation, tileLocations);

            int score = ScoreMoveSlice(sliceLocation,
                                        (singleRunStart, singleRunEnd),
                                        (sl, s, e) => GetSquares(primaryDirection, sl, (s, e)));

            score += ScorePerpendicularSlices((singleRunStart, singleRunEnd),
                                                sliceLocation,
                                                MovesMade,
                                                (sl, tls) => GetSquares(secondaryDirection, sl, tls));

            return score;
        }

        static internal int ScoreMoveSlice(
                                    int location,
                                    (int start, int end) singleRun,
                                    Func<int, int, int, List<Square>> GetSquares)
        {
            var moveSlice = GetSquares(location, singleRun.start, singleRun.end);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            return moveSlice.ScoreRun();
        }

        static internal int ScorePerpendicularSlices((int start, int end) singleRun, int sliceLocation, int MovesMade,
                                                    Func<int, List<int>, List<Square>> GetSquares)
        {
            int score = 0;

            for (int perpendicularFixed = singleRun.start; perpendicularFixed <= singleRun.end; perpendicularFixed++)
            {
                var perpendicularSlice = GetSquares(perpendicularFixed, [sliceLocation]);
                var hasNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
                if ((perpendicularSlice.Count > 1) && hasNewTiles)
                    score += perpendicularSlice.ScoreRun();
            }

            return score;
        }

    }
}