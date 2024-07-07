using Scrabble.Domain;

namespace Scrabble.WASM.Client.Models
{
  record BoardStatus(bool IsCurrentMoveValid, int CurrentMoveScore, string Messages, Move? CurrentMove = null);
}
