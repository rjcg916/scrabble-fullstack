using Scrabble.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));

        public bool IsOccupied(Coord coord) => squares[coord.RVal, coord.CVal].IsOccupied;

        public (bool valid, List<PlacementMsg> errorList) IsMoveValid(Move move)
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

            var placementMsgs = new List<PlacementMsg>();

            placementMsgs.AddRange(
                ValidateWords(
                Coord.ColCount, isHorizontal: true,
                r => GetSquares(board.SquareByColumn, r, (0, Coord.ColCount - 1)), IsWordValid));

            placementMsgs.AddRange(
                ValidateWords(
                Coord.RowCount, isHorizontal: false,
                c => GetSquares(board.SquareByRow, c, (0, Coord.RowCount - 1)), IsWordValid));

            return placementMsgs.Count > 0 ?
                                (false, placementMsgs) :
                                (true, new List<PlacementMsg>());
        }

        public (bool valid, PlacementMsg msg) TilesContiguous(List<TilePlacement> tilePlacementList)
        {

            var occupiedList = GetOccupiedSquares().Select(s => (s.Coord.RVal, s.Coord.CVal)).ToList();

            var proposedList = tilePlacementList.Select(s => (s.Coord.RVal, s.Coord.CVal)).ToList();

            var isContiguous = Placement.IsContiguous(occupiedList, proposedList);

            var letters = tilePlacementList.ToLetters();
            var (row, col) = proposedList.FirstOrDefault();

            return isContiguous ?
                (true, new PlacementMsg(new Coord(row, col), letters)) :
                (false, new PlacementMsg(new Coord(row, col), $"Not Contiguous :: {letters}"));

        }

        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
             tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        internal static List<PlacementMsg> ValidateWords(
                        int sliceCount,
                        bool isHorizontal,
                        Func<int, List<Square>> getSquares,
                        Func<string, bool> IsWordValid)
        {
            List<PlacementMsg> placementMsgs = [];

            for (int index = 0; index < sliceCount; index++)
            {
                var squareList = getSquares(index);
                var charList = squareList.ToCharList();

                if (charList != null)
                {
                    var words = charList.ToWords();
                    var (valid, invalidWord) = words.ValidateWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = isHorizontal ?
                                    new Coord((R)index, 0) :
                                    new Coord(0, (C)index);

                        placementMsgs.Add(new(coord, invalidWord));
                    }
                }
            }

            return placementMsgs;
        }
    }

}
