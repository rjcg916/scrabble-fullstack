using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
          tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));

        public (bool valid, List<PlacementError> errorList) IsMoveValid(List<TilePlacement> move)
        {    
            Board board = new(this);
            
            board.PlaceTiles(move);

            if (!IsOccupied(STAR))
            {
                return (false, [new(Placement.Star, Board.STAR, "STAR not occupied")]);
            }

            var (areTilesContiguous,placementError) = board.TilesContiguousOnBoard(move);

            if (!areTilesContiguous)
            {
                return (false, [placementError]);
            }
            
            return board.BoardContainsOnlyValidWords();
        }

        public (bool valid, List<PlacementError> errorList) BoardContainsOnlyValidWords()
        {
            var invalidMessages = new List<PlacementError>();

            invalidMessages.AddRange(ValidateWordSlices( r => GetSquares(true, r), Placement.Horizontal));            
            invalidMessages.AddRange(ValidateWordSlices( c => GetSquares(false, c), Placement.Vertical));
            
            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

        public (bool valid, PlacementError error) TilesContiguousOnBoard(List<TilePlacement> tilePlacementList)
        {
            // make sure each of the tiles in list is contiguous with another tile
            foreach (var (coord, _) in tilePlacementList)
            {
                bool isContiguous = false;

                // Check the four adjacent squares (up, down, left, right)
                var adjacentCoords = new List<Coord>
                    {
                        new((R)( Math.Max( coord.RowValue - 1, 0)), coord.Col), // Up
                        new((R)( Math.Min( coord.RowValue + 1, Board.rowCount - 1) ), coord.Col), // Down
                        new(coord.Row, (C)( Math.Max(coord.ColValue - 1, 0))), // Left
                        new(coord.Row, (C)( Math.Min(coord.ColValue + 1, Board.colCount - 1)))  // Right
                    };

                foreach (var adjCoord in adjacentCoords)
                {
                    if (adjCoord.RowValue >= 0 && adjCoord.RowValue < rowCount &&
                        adjCoord.ColValue >= 0 && adjCoord.ColValue < colCount)
                    {
                        if (squares[adjCoord.RowValue, adjCoord.ColValue].IsOccupied)
                        {
                            isContiguous = true;
                            break; // no need to search for another continguous tile
                        }
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
        internal List<PlacementError> ValidateHorizontalWordSlices(Func<int, List<Square>> getSquares)
        {
            List<PlacementError> invalidMessages = [];

            int sliceCount = Board.rowCount;

            for (int index = 0; index < sliceCount; index++)
            {
                var charList = getSquares(index).ToCharList();

                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = new Coord((R)index, 0);
                        invalidMessages.Add(new(Placement.Horizontal, coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }

        // apply word validity check across a slice (row or col) of the board
        internal List<PlacementError> ValidateVerticalWordSlices(Func<int, List<Square>> getSquares)
        {
            List<PlacementError> invalidMessages = [];

            int sliceCount = Board.colCount;

            for (int index = 0; index < sliceCount; index++)
            {
                var charList = getSquares(index).ToCharList();

                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = new Coord(0, (C)index);
                        invalidMessages.Add(new(Placement.Vertical, coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }

        // apply word validity check across a slice (row or col) of the board
        internal List<PlacementError> ValidateWordSlices(Func<int, List<Square>> getSquares,
                                             Placement placement)
        {
            List<PlacementError> invalidMessages = [];

            int sliceCount = Placement.Horizontal == placement ? 
                                Board.rowCount : Board.colCount;
  
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