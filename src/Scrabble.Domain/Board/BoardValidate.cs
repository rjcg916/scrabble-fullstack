﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {

        public (bool valid, List<PlacementError> errorList) IsMoveValid(Move move)
        {
            Board board = new(this);

            try
            {
                board = board.MakeMove(move);
            }
            catch (Exception e)
            {
                return (false, [new(move.TilePlacements.First().Coord, e.Message)]);
            }

            if (!board.IsOccupied(STAR))
            {
                return (false, [new(Board.STAR, "STAR not occupied")]);
            }

            var (areTilesContiguous, placementError) = board.TilesContiguous(move.TilePlacements);

            if (!areTilesContiguous)
            {
                return (false, [placementError]);
            }

            // validate vertical and horizontal slices

            var invalidMessages = new List<PlacementError>();

            invalidMessages.AddRange(ValidateWordSlices(r => GetSquares(SquareByColumn, r, (0, Coord.ColCount - 1)), Coord.ColCount, true));
            invalidMessages.AddRange(ValidateWordSlices(c => GetSquares(SquareByRow, c, (0, Coord.RowCount - 1)), Coord.RowCount, false));

            return invalidMessages.Count > 0    ? (false, invalidMessages) 
                                                : (true, new List<PlacementError>());
        }

        public (bool valid, PlacementError) TilesContiguous(List<TilePlacement> tilePlacementList)
        {

            // get currently occupied squares
            var occupiedList = new List<(int, int)>();
            for (int r = 0; r < Coord.RowCount; r++)
                for (int c = 0; c < Coord.ColCount; c++)
                    if (squares[r, c].IsOccupied)
                        occupiedList.Add((r, c));

            // get proposed tiles for squares   
            var proposedList = new List<(int row, int col)>();
            foreach (var (coord, _) in tilePlacementList)
            {
                proposedList.Add((coord.RVal, coord.CVal));
            }


            // for display purposes, sort tiles in board placement order
            tilePlacementList = [.. tilePlacementList.OrderBy(tp => tp.Coord.RVal).OrderBy(tp => tp.Coord.CVal)];

            // report results
            var (row, col) = proposedList.FirstOrDefault();
            var letters = tilePlacementList.Select(tp => tp.Tile).ToList().TilesToLetters();

            var isContiguous = Placement.IsContiguous(occupiedList, proposedList);
            var msg = isContiguous ? letters : $"Not Contiguous :: {letters}";
            var placementError = new PlacementError(new Coord(row, col), msg);
            return (isContiguous, placementError);
        }

        /// <summary>
        /// apply word validity check across a slice (row or col) of the board
        /// </summary>
        internal List<PlacementError> ValidateWordSlices(
                                            Func<int, List<Square>> getSquares,
                                            int sliceCount,
                                            bool isHorizontal)
        {
            List<PlacementError> invalidMessages = [];

            for (int index = 0; index < sliceCount; index++)
            {
                var sqList = getSquares(index);
                var charList = sqList.ToCharList();

                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = isHorizontal
                                ? new Coord((R)index, 0) : new Coord(0, (C)index);

                        invalidMessages.Add(new(coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }
    }
}
