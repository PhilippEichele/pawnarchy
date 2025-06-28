using UnityEngine;
using System.Collections.Generic;

public class Knight : ChessPiece
{
    private static readonly Vector2Int[] L = {
        new( 1,  2), new( 2,  1), new(-1,  2), new(-2,  1),
        new( 1, -2), new( 2, -1), new(-1, -2), new(-2, -1)
    };

    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();

        // Klassische L-Züge
        foreach (var v in L)
        {
            var p = BoardPos + v;
            if (!board.InBounds(p)) continue;

            var piece = board.GetPiece(p);
            if (piece == null || piece.Owner != Owner)
                moves.Add(p);
        }

        if (PowerUpRules.For(Owner).knightFullCircle)
        {
            int          dir = Owner == Player.White ? 1 : -1;
            Vector2Int   p   = BoardPos + new Vector2Int(0, 2 * dir);

            if (board.InBounds(p))
            {
                var piece = board.GetPiece(p);
                if (piece == null || piece.Owner != Owner)
                    moves.Add(p);
            }
        }

        return moves;
    }
}

