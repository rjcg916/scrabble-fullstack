﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Scrabble.Domain
{
    public class Score
    {
        readonly Board Board;
        readonly bool IsHorizontal;
        readonly int SliceLocation;
        readonly List<int> TileLocations;

        public Score(Board board, Move move)
        {
            Board = board;
            var tileSpecs = move.TilePlacements.ToPlacementSpec();
            IsHorizontal = tileSpecs.IsHorizontal;
            TileLocations = tileSpecs.TileLocations;
            SliceLocation = tileSpecs.SliceLocation;
        }

        public int Calculate() =>
                    IsHorizontal ? 
                        Calculate(Board, Board.SquareByColumn, Board.SquareByRow, SliceLocation, TileLocations):
                        Calculate(Board, Board.SquareByRow, Board.SquareByColumn, SliceLocation, TileLocations);
     
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
                var perpendicularSlice = getSquares(perpendicularFixed, [sliceLocation]);
                var hasNewTiles = perpendicularSlice.Any(sq => sq.MoveNumber == movesMade);
                if ((perpendicularSlice.Count > 1) && hasNewTiles)
                    score += perpendicularSlice.ScoreRun();
            }

            return score;
        }

        
        /// <summary>
        ///  fetch all specified squares in a slice (horizontal or vertical)
        /// </summary>
        internal static List<Square> GetSquares(
                                            Func<int, int, Square> GetSquare,
                                            int sliceLocation,
                                            List<int> locationList,
                                            int maxIndex = Coord.RowCount - 1)
        {
            var (start, end) = GetEndpoints(GetSquare, sliceLocation, locationList, maxIndex);
            return Board.GetSquares(GetSquare, sliceLocation, (start, end));
        }

        /// <summary>
        /// determine start and end location of occupied squares contiguous with specified squares
        /// </summary>
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