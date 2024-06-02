﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public static readonly int rowCount = R._15 - R._1 + 1;
        public static readonly int colCount = C.O - C.A + 1;

        public readonly Square[,] squares = new Square[rowCount, colCount];

        public Board()
        {
            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    squares[r, c] = new Square();

            Initialize();
        }

        public Board(Coord startFrom, List<Tile> tiles, bool inRow) : this() {
            if (inRow)
                this.PlaceTilesInARow(startFrom, tiles);
            else
                this.PlaceTilesInACol(startFrom, tiles);
        }
        
        public Board(Board other)
        {
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    squares[r, c] = other.squares[r, c].Copy();
                }
            }
        }

        public Board Copy() => new (this);
        
        public Square GetSquare(Coord loc) => 
            squares[loc.RowToValue(), loc.ColToValue()];

        public Tile GetTile(Coord loc) =>
            GetSquare(loc).Tile;

        public bool IsOccupied(Coord coord) =>
            squares[coord.RowToValue(), coord.ColToValue()].IsOccupied;
       
        public bool IsOccupied(Coord startCoord, Coord endCoord)
        {
            int startRow = startCoord.RowToValue();
            int endRow = endCoord.RowToValue();
            int startCol = startCoord.ColToValue();
            int endCol = endCoord.ColToValue();

            if (startRow == endRow)     // If checking a row
            {
                return IsOccupiedRange(startRow, startCol, endCol, true);
            }
            else if (startCol == endCol) // If checking a column
            {
                return IsOccupiedRange(startCol, startRow, endRow, false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private bool IsOccupiedRange(int fixedValue, int start, int end, bool isRow)
        {
            for (int i = start; i <= end; i++)
            {
                var squareValue = isRow ? squares[fixedValue, i] : squares[i, fixedValue];
                if (squareValue.IsOccupied)
                    return true;

            }
            return false;
        }

        public List<Square> GetRowSlice(int row)
        {
            List<Square> slice = [];
            for (int col = 0; col < colCount; col++)
            {
                slice.Add(squares[row, col]);
            }
            return slice;
        }

        public List<Square> GetColumnSlice(int col)
        {
            List<Square> slice = [];
            for (int row = 0; row < rowCount; row++)
            {
                slice.Add(squares[row, col]);
            }
            return slice;
        }

        public List<LocationEvaluator<Square>> GetCoordSquares(bool filterForOccupied = false)
        {
            List<LocationEvaluator<Square>> squareList = [];

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if (squares[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
                        squareList.Add(new LocationEvaluator<Square>(r, c, squares[r, c]));

            return squareList;
        }


        public Board PlaceTile(Coord coord, Tile tile)
        {
            var col = coord.ColToValue();
            var row = coord.RowToValue();

            if (this.IsOccupiedRange(row, col, col, true))
            {
                throw new InvalidOperationException("The specified space is already occupied.");
            }

            Board board = this.Copy();

            board.squares[row, col].Tile = tile; 

            return board;
        }

        private Board PlaceTiles(int fixedValue, int start, List<Tile> tiles, bool isRow)
        {
            Board board = this.Copy();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (isRow)
                {
                    board.squares[fixedValue, start + i].Tile = tiles[i];
                }
                else
                {
                    board.squares[start + i, fixedValue].Tile = tiles[i];
                }
            }

            return board;
        }

        public Board PlaceTilesInARow(Coord startFrom, List<Tile> tiles)
        {        
            var startCol = startFrom.ColToValue();
            var endCol = startCol + tiles.Count - 1;
            var theRow = startFrom.RowToValue();

            if (this.IsOccupiedRange(theRow, startCol, endCol, true))
            {
                throw new InvalidOperationException("The specified row is already occupied.");
            }

            return PlaceTiles(theRow, startCol, tiles, true);            
        }

        public Board PlaceTilesInACol(Coord startFrom, List<Tile> tiles)
        {
            var startRow = startFrom.RowToValue();
            var endRow = startRow + tiles.Count - 1;
            int theCol = startFrom.ColToValue();
   
            if (this.IsOccupiedRange(theCol, startRow, endRow, false))
            {
                throw new InvalidOperationException("The specified col is already occupied.");
            }

            return PlaceTiles(theCol, startRow, tiles, false);
        }

    }
}