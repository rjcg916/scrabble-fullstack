using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
          tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        public bool IsOccupied(Coord coord) => squares[coord.RVal, coord.CVal].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));

        public (bool valid, List<PlacementError> errorList) IsMoveValid(List<TilePlacement> move)
        {
            Board board = new(this);

            board.PlaceTiles(move);

            if (!IsOccupied(STAR))
            {
                return (false, [new(Placement.Star, Board.STAR, "STAR not occupied")]);
            }

            var (areTilesContiguous, placementError) = board.TilesContiguousOnBoard(move);

            if (!areTilesContiguous)
            {
                return (false, [placementError]);
            }

            return board.BoardContainsOnlyValidWords();
        }

        public (bool valid, List<PlacementError> errorList) BoardContainsOnlyValidWords()
        {
            var invalidMessages = new List<PlacementError>();

            invalidMessages.AddRange(ValidateWordSlices(r => GetSquares(SquareByColumn, r, (0, Coord.ColCount - 1)), Coord.ColCount, Placement.Horizontal));
            invalidMessages.AddRange(ValidateWordSlices(c => GetSquares(SquareByRow, c, (0, Coord.RowCount - 1)), Coord.RowCount, Placement.Vertical));

            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

        public (bool valid, PlacementError error) TilesContiguousOnBoard(List<TilePlacement> tilePlacementList)
        {
            // make sure each of the tiles in list is contiguous with another tile
            foreach (var (coord, _) in tilePlacementList)
            {
                bool isContiguous = false;

                var adjacentCoords = coord.GetAdjacent();

                foreach (var adjCoord in adjacentCoords)
                {

                    if (squares[adjCoord.RVal, adjCoord.CVal].IsOccupied)
                    {
                        isContiguous = true;
                        break; // no need to search for another continguous tile
                    }
                }

                if (!isContiguous) // no need to examine other tiles
                {
                    return (false, new PlacementError(Placement.Vertical, coord, "Tile Not Contiguous"));
                }
            }

            return (true, new PlacementError(Placement.All, Board.STAR, "No Error"));
        }

        // apply word validity check across a slice (row or col) of the board
        internal List<PlacementError> ValidateWordSlices(
                                            Func<int, List<Square>> getSquares,
                                            int sliceCount,
                                            Placement placement)
        {
            List<PlacementError> invalidMessages = [];

            for (int index = 0; index < sliceCount; index++)
            {
                var charList = getSquares(index).ToCharList();

                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = placement == Placement.Horizontal ? new Coord((R)index, 0) : new Coord(0, (C)index);
                        invalidMessages.Add(new(placement, coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }
    }
}