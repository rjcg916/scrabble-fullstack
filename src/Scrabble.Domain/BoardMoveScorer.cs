using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public int ScoreMove(List<TilePlacement> tilePlacementList)
        {
            PlacementSpec tileSpecs = tilePlacementList.ToPlacementSpec();

            return ScoreMove(tileSpecs.FixedLocation,
                                tileSpecs.TileLocations,
                                tileSpecs.Placement);
        }

        public int ScoreMove(int sliceLocation, List<int> tileLocations, Placement placement)
        {
            return placement switch
            {
                Placement.Horizontal => ScoreMoveDirectional(sliceLocation, tileLocations, true),
                Placement.Vertical => ScoreMoveDirectional(sliceLocation, tileLocations, false),
                _ => throw new Exception("Invalid Placement"),
            };
        }

        // score by checking for words:
        // in slice direction and words created in perpendicular direction
        internal int ScoreMoveDirectional(int sliceLocation, List<int> tileLocations, bool isHorizontal)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetEndpoints(isHorizontal, sliceLocation, tileLocations);
            var moveSlice = GetSquares(isHorizontal, sliceLocation, singleRunStart, singleRunEnd);

            if (moveSlice.Count == 0)
                throw new Exception("MoveSlice must have one or more tiles");

            score += moveSlice.ScoreRun();

            for (int perpendicularFixed = singleRunStart; perpendicularFixed <= singleRunEnd; perpendicularFixed++)
            {
                var (multiRunStart, multiRunEnd) = GetEndpoints(!isHorizontal, perpendicularFixed, [sliceLocation]);

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
    } 
}