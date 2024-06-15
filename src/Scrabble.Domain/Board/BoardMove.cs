using System;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

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
                loc.MoveOfOccupation = board.MovesMade;
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
        internal void PlaceTiles(List<TilePlacement> tilePlacementList)
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

        //public int ScoreMove0(int sliceLocation, List<int> tileLocations, Placement placement)
        //{
        //    return placement switch
        //    {
        //        Placement.Horizontal => ScoreMoveDirectional(sliceLocation, tileLocations, true),
        //        Placement.Vertical => ScoreMoveDirectional(sliceLocation, tileLocations, false),
        //        _ => throw new Exception("Invalid Placement"),
        //    };
        //}
        public int ScoreMove(int sliceLocation, List<int> tileLocations, Placement placement)
        {
            return placement switch
            {
                Placement.Horizontal => ScoreMoveHorizontal(sliceLocation, tileLocations),
                Placement.Vertical   => ScoreMoveVertical(sliceLocation, tileLocations),
                _ => throw new Exception("Invalid Placement"),
            };
        }
        //public int ScoreMoveHorizontal(int sliceLocation, List<int> tileLocations)
        //{
        //    return ScoreMoveDirectionalHorizontal(sliceLocation, tileLocations);
        //}
        //public int ScoreMoveVertical(int sliceLocation, List<int> tileLocations)
        //{
        //    return ScoreMoveDirectionalVertical(sliceLocation, tileLocations);
        //}

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        //internal int ScoreMoveDirectional(int sliceLocation, List<int> tileLocations, bool isHorizontal)
        //{
        //    int score = 0;

        //    var (singleRunStart, singleRunEnd) = GetEndpoints(isHorizontal, sliceLocation, tileLocations);
        //    var moveSlice = GetSquares(isHorizontal, sliceLocation, singleRunStart, singleRunEnd);

        //    if (moveSlice.Count == 0)
        //        throw new Exception("MoveSlice must have one or more tiles");

        //    score += moveSlice.ScoreRun();

        //    for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
        //    {
        //        var (multiRunStart, multiRunEnd) = GetEndpoints(!isHorizontal, perpendicularFixed, [sliceLocation]);

        //        if (multiRunStart < multiRunEnd)
        //        {
        //            var perpendicularSlice = GetSquares(!isHorizontal, perpendicularFixed, multiRunStart, multiRunEnd);
        //            var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
        //            if ((perpendicularSlice.Count > 0) && anyNewTiles)
        //                score += perpendicularSlice.ScoreRun();
        //        }
        //    }

        //    return score;
        //}

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        //internal int ScoreMoveDirectionalHorizontal(int sliceLocation, List<int> tileLocations)
        //{
        //    int score = 0;

        //    var (singleRunStart, singleRunEnd) = GetEndpointsHorizontal(sliceLocation, tileLocations);
        //    var moveSlice = GetSquaresHorizontal(sliceLocation, singleRunStart, singleRunEnd);

        //    if (moveSlice.Count == 0)
        //        throw new Exception("MoveSlice must have one or more tiles");

        //    score += moveSlice.ScoreRun();

        //    for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
        //    {
        //        var (multiRunStart, multiRunEnd) = GetEndpointsVertical(perpendicularFixed, [sliceLocation]);

        //        if (multiRunStart < multiRunEnd)
        //        {
        //            var perpendicularSlice = GetSquaresVertical(perpendicularFixed, multiRunStart, multiRunEnd);
        //            var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
        //            if ((perpendicularSlice.Count > 0) && anyNewTiles)
        //                score += perpendicularSlice.ScoreRun();
        //        }
        //    }

        //    return score;
        //}
        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        //internal int ScoreMoveDirectionalVertical(int sliceLocation, List<int> tileLocations)
        //{
        //    int score = 0;

        //    var (singleRunStart, singleRunEnd) = GetEndpointsVertical(sliceLocation, tileLocations);
        //    var moveSlice = GetSquaresVertical(sliceLocation, singleRunStart, singleRunEnd);

        //    if (moveSlice.Count == 0)
        //        throw new Exception("MoveSlice must have one or more tiles");

        //    score += moveSlice.ScoreRun();

        //    for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
        //    {
        //        var (multiRunStart, multiRunEnd) = GetEndpointsHorizontal(perpendicularFixed, [sliceLocation]);

        //        if (multiRunStart < multiRunEnd)
        //        {
        //            var perpendicularSlice = GetSquaresHorizontal(perpendicularFixed, multiRunStart, multiRunEnd);
        //            var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
        //            if ((perpendicularSlice.Count > 0) && anyNewTiles)
        //                score += perpendicularSlice.ScoreRun();
        //        }
        //    }

        //    return score;
        //}


        static internal int ScoreMoveSlice(int location, 
                                        int singleRunStart, int singleRunEnd,
                                        Func<int, int, int, List<Square>> GetSquares)
        {  
            var moveSlice = GetSquares(location, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            return moveSlice.ScoreRun();
        }

        static internal int ScorePerpendicularSlices(int singleRunStart, int singleRunEnd, int sliceLocation, int MovesMade,
                                        Func<int, List<int>, (int, int) > GetEndpoints,
                                        Func<int, int, int, List<Square>> GetSquares)
        {
            int score = 0;
            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetEndpoints(perpendicularFixed, [sliceLocation]);

                if (multiRunStart < multiRunEnd)
                {
                    var perpendicularSlice = GetSquares(perpendicularFixed, multiRunStart, multiRunEnd);
                    var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
                    if ((perpendicularSlice.Count > 0) && anyNewTiles)
                        score += perpendicularSlice.ScoreRun();
                }
            }

            return score;
        }


        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveHorizontal(int sliceLocation, List<int> tileLocations)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetEndpointsHorizontal(sliceLocation, tileLocations);
            var moveSlice = GetSquaresHorizontal(sliceLocation, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            score += moveSlice.ScoreRun();

            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetEndpointsVertical(perpendicularFixed, [sliceLocation]);

                if (multiRunStart < multiRunEnd)
                {
                    var perpendicularSlice = GetSquaresVertical(perpendicularFixed, multiRunStart, multiRunEnd);
                    var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
                    if ((perpendicularSlice.Count > 0) && anyNewTiles)
                        score += perpendicularSlice.ScoreRun();
                }
            }

            return score;
        }

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveHorizontal1(int sliceLocation, List<int> tileLocations, int MovesMade)
        {

            var (singleRunStart, singleRunEnd) = GetEndpointsHorizontal(sliceLocation, tileLocations);

            int score = Board.ScoreMoveSlice(sliceLocation, singleRunStart, singleRunEnd, GetSquaresHorizontal);

            score += Board.ScorePerpendicularSlices(singleRunStart, singleRunEnd, sliceLocation, MovesMade, GetEndpointsVertical, GetSquaresVertical);

            return score;
        }

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveVertical(int sliceLocation, List<int> tileLocations)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetEndpointsVertical(sliceLocation, tileLocations);
            var moveSlice = GetSquaresVertical(sliceLocation, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            score += moveSlice.ScoreRun();

            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetEndpointsHorizontal(perpendicularFixed, [sliceLocation]);

                if (multiRunStart < multiRunEnd)
                {
                    var perpendicularSlice = GetSquaresHorizontal(perpendicularFixed, multiRunStart, multiRunEnd);
                    var anyNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == MovesMade);
                    if ((perpendicularSlice.Count > 0) && anyNewTiles)
                        score += perpendicularSlice.ScoreRun();
                }
            }

            return score;
        }
        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveVertical1(int sliceLocation, List<int> tileLocations, int MovesMade)
        {

            var (singleRunStart, singleRunEnd) = GetEndpointsVertical(sliceLocation, tileLocations);

            int score = Board.ScoreMoveSlice(sliceLocation, singleRunStart, singleRunEnd, GetSquaresVertical);

            score += Board.ScorePerpendicularSlices(singleRunStart, singleRunEnd, sliceLocation, MovesMade, GetEndpointsHorizontal, GetSquaresHorizontal);

            return score;
        }
    }
}