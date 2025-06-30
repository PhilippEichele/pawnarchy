using UnityEngine;
using System.Collections.Generic;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();
        Vector2Int[] dirs = { new(1,0), new(-1,0), new(0,1), new(0,-1) };

        foreach (var d in dirs)
        {
            var pos = BoardPos + d;
            while (board.InBounds(pos))
            {
                var piece = board.GetPiece(pos);
                if (piece == null)
                {
                    moves.Add(pos);
                }
                else
                {
                    if (piece.Owner != Owner) moves.Add(pos);
                    break;
                }
                pos += d;
            }
        }
        return moves;
    }
}
