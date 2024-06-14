using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public static readonly int rowCount = R._15 - R._1 + 1;
        public static readonly int colCount = C.O - C.A + 1;

        public readonly Square[,] squares = new Square[rowCount, colCount];
        public readonly static Coord STAR = new(R._8, C.H);

        private const int LOWERBOUND = 0;
        private const int DIMENSION = 15;

        public int MovesMade = 0;

        internal readonly Func<string, bool> IsWordValid;

        public Board(Func<string, bool> IsWordValid)
        {
            this.IsWordValid = IsWordValid;

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    squares[r, c] = new Square();

            Initialize();
        }

        // create a new Board with initial move
        public Board(Func<string, bool> IsWordValid, 
                     List<TilePlacement> tileList) :
            this(IsWordValid)
        {
            PlaceTiles(tileList);      
        }


        // create a new Board with initial move
        public Board(Func<string, bool> IsWordValid,
                        Coord startFrom,
                        List<Tile> tiles,
                        Placement placement) :
            this(IsWordValid)
        {
            switch (placement)
            {
                case Placement.Star:
                case Placement.Horizontal:
                    PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.ColValue + index)), tile)
                        ).ToList());
                    break;

                case Placement.Vertical:
                    PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RowValue + index), startFrom.Col), tile)
                        ).ToList());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }
        }

        // clone a Board        
        public Board(Board other)
        {
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    squares[r, c] = other.squares[r, c].Copy();
                }
            }
            IsWordValid = other.IsWordValid;
            MovesMade = other.MovesMade;
        }

        public Board Copy() => new(this);

        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
          tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        public Tile GetTile(Coord loc) => squares[loc.RowValue, loc.ColValue]?.Tile;

        public bool IsFirstMove() => MovesMade == 0;

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));


        public (bool valid, List<PlacementError> errorList) IsMoveValid(List<TilePlacement> move)
        {
           
            Board board = new Board(this);
            
            board.PlaceTiles(move);

            if (!IsOccupied(STAR))
            {
                return (false, [new(Placement.Star, 0, "STAR not occupied")]);
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

            invalidMessages.AddRange(ValidateBoardSlices( r => GetSquares(true, r), Placement.Horizontal));            
            invalidMessages.AddRange(ValidateBoardSlices( c => GetSquares(false,c), Placement.Vertical));
            
            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

        public (bool valid, PlacementError error) TilesContiguousOnBoard(List<TilePlacement> tilePlacementList)
        {

            // make sure each of the new tiles is contiguous with another tile
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
                    return (false, new PlacementError(Placement.Vertical, coord.ColValue, ""));
                }
            }
            
            return (true, new PlacementError(Placement.All, -1 , ""));
        }

        public int ScoreMove(int sliceLocation, List<int> tileLocations, Placement placement)
        {
            var score = placement switch
            {
                Placement.Horizontal => ScoreMoveDirectional(sliceLocation, tileLocations, true),
                Placement.Vertical => ScoreMoveDirectional(sliceLocation, tileLocations, false),
                _ => throw new Exception("Invalid Placement"),
            };
            return score;
        }

        public int ScoreMove(List<TilePlacement> tilePlacementList)
        {
            PlacementSpec tileSpecs = tilePlacementList.ToPlacementSpec();

            return ScoreMove(   tileSpecs.FixedLocation, 
                                tileSpecs.TileLocations, 
                                tileSpecs.Placement);
        }

        public void PlaceTiles(List<TilePlacement> tilePlacementList)
        {
            MovesMade++;

            foreach (var (coord, tile) in tilePlacementList)
            {
                var location = squares[coord.RowValue, coord.ColValue];

                location.Tile = new Tile(tile.Letter);
                location.MoveOfOccupation = MovesMade;
            };
        }

        public Board NextBoard(List<TilePlacement> tileList)
        {
            Board board = this.Copy();

            board.MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var loc = board.squares[coord.RowValue, coord.ColValue];
                loc.Tile = tile;
                loc.MoveOfOccupation = MovesMade;
            };

            return board;
        }

        // score by checking for words in slice direction and
        // words created by slice in perpendicular direction
        internal int ScoreMoveDirectional(int sliceLocation, List<int> tileLocations, bool isHorizontal)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetRun(isHorizontal, sliceLocation, tileLocations);
            var moveSlice = GetSquares(isHorizontal, sliceLocation, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            score += moveSlice.ScoreRun();

            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetRun(!isHorizontal, perpendicularFixed, [sliceLocation]);

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

        // apply word validity check across a slice (row or col) of a board
        internal List<PlacementError> ValidateBoardSlices(Func<int, List<Square>> getSquares,
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
                    var (valid, invalidWord) = charList.IsValidSequence(IsWordValid);

                    if (!valid)
                    {
                        invalidMessages.Add(new(placement, index, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }

        // retrieve contents of occupied squares from a specified range
        internal List<Square> GetSquares(bool isHorizontal, int sliceLocation, int rangeStart = LOWERBOUND, int rangeEnd = DIMENSION -1)
        {
            List<Square> slice = [];

            if (isHorizontal)
            {
                for (int col = rangeStart; col <= rangeEnd; col++)
                {
                    var sq = squares[sliceLocation, col];
                    if (sq.IsOccupied)
                        slice.Add(sq.Copy());
                }
            }
            else
            {
                for (int row = rangeStart; row <= rangeEnd; row++)
                {
                    var sq = squares[row, sliceLocation];
                    if (sq.IsOccupied)
                        slice.Add(sq.Copy());
                }
            }

            return slice;
        }

        // determine start and end location of occupied squares contiguous with specified squares
        internal (int start, int end) GetRun(bool isHorizontal, int sliceLocation, List<int> locationList)
        {
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(isHorizontal ? squares[sliceLocation, pos].IsOccupied :
                                   squares[pos, sliceLocation].IsOccupied))
                {
                    break;
                }
                minOccupied--;
            }

            for (int pos = maxMove + 1; pos < (isHorizontal ? Board.colCount : Board.rowCount); pos++)
            {
                if (!(isHorizontal ? squares[sliceLocation, pos].IsOccupied :
                                    squares[pos, sliceLocation].IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

        public List<LocationSquare> GetLocationSquares(bool IsOccupied = false)
        {
            List<LocationSquare> locationSquareList = [];

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if ( IsOccupied ?  squares[r, c].IsOccupied : !squares[r,c].IsOccupied )
                        locationSquareList.Add(new LocationSquare(new Coord((R)r, (C)c), squares[r, c]));

            return locationSquareList;
        }
    }
}