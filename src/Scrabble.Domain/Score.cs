﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Score
    {
        readonly Placement Placement;
        readonly Board  Board;
        readonly int SliceLocation;
        readonly List<int> TileLocations;

        public Score(Board board, List<TilePlacement> tilePlacements)
        {
            Board = board;
            var tileSpecs = tilePlacements.ToPlacementSpec();
            Placement = tileSpecs.Placement;
            TileLocations = tileSpecs.TileLocations;    
            SliceLocation = tileSpecs.SliceLocation;
        }

        public Score(Board board, int sliceLocation, List<int> tileLocations, Placement placement)
        {
            Board = board;
            Placement = placement;
            TileLocations = tileLocations;
            SliceLocation = sliceLocation;
        }

        public int Calculate()
        {
            return Placement switch
            {
                Placement.Horizontal => Calculate(Board, Board.SquareByColumn, Board.SquareByRow, SliceLocation, TileLocations),
                Placement.Vertical => Calculate(Board, Board.SquareByRow, Board.SquareByColumn, SliceLocation, TileLocations),
                _ => throw new Exception("Invalid Placement")
            };
        }

        private static int Calculate(Board board, Func<int, int, Square> primaryDirection,
                                     Func<int, int, Square> secondaryDirection,
                                     int sliceLocation, List<int> tileLocations)
        {
            var (singleRunStart, singleRunEnd) = GetEndpoints(primaryDirection, sliceLocation, tileLocations);

            int score = CalculateMoveSlice((sl, s, e) => Board.GetSquares(primaryDirection, sl, (s, e)), sliceLocation,
                                            (singleRunStart, singleRunEnd));

            score += CalculatePerpendicularSlices((sl, tls) => GetSquares(secondaryDirection, sl, tls), (singleRunStart, singleRunEnd),
                                                  sliceLocation, board.MoveNumber);

            return score;
        }

        private static int CalculateMoveSlice(Func<int, int, int, List<Square>> getSquares,
                                      int location, (int start, int end) singleRun)
        {
            var moveSlice = getSquares(location, singleRun.start, singleRun.end);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            return moveSlice.ScoreRun();
        }

        private static int CalculatePerpendicularSlices(Func<int, List<int>, List<Square>> getSquares,
                                                (int start, int end) singleRun, int sliceLocation, int movesMade)
        {
            int score = 0;

            for (int perpendicularFixed = singleRun.start; perpendicularFixed <= singleRun.end; perpendicularFixed++)
            {
                var perpendicularSlice = getSquares(perpendicularFixed, new List<int> { sliceLocation });
                var hasNewTiles = perpendicularSlice.Any(sq => sq.MoveOfOccupation == movesMade);
                if ((perpendicularSlice.Count > 1) && hasNewTiles)
                    score += perpendicularSlice.ScoreRun();
            }

            return score;
        }


        // fetch all specified squares in a slice (horizontal or vertical)
        internal static List<Square> GetSquares(
                                            Func<int, int, Square> GetSquare,
                                            int sliceLocation,
                                            List<int> locationList,
                                            int maxIndex = Coord.RowCount - 1)
        {
            var (start, end) = GetEndpoints(GetSquare, sliceLocation, locationList, maxIndex);
            return Board.GetSquares(GetSquare, sliceLocation, (start, end));
        }


        // determine start and end location of occupied squares contiguous with specified squares
        internal static (int start, int end) GetEndpoints(
                                        Func<int, int, Square> GetSquare,
                                        int sliceLocation,
                                        List<int> locationList,
                                        int maxIndex = Coord.RowCount - 1)
        {

            var minMove = locationList.Min();
            var minOccupied = minMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(GetSquare(pos, sliceLocation).IsOccupied))
                {
                    break;
                }
                minOccupied--;
            }

            var maxMove = locationList.Max();
            var maxOccupied = maxMove;

            for (int pos = maxMove + 1; pos <= maxIndex; pos++)
            {
                if (!(GetSquare(pos, sliceLocation).IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

    }
}