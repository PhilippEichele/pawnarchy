using UnityEngine;
using System.Collections.Generic;

public class Queen : ChessPiece
{
    private static readonly Vector2Int[] L = {
        new( 1,  2), new( 2,  1), new(-1,  2), new(-2,  1),
        new( 1, -2), new( 2, -1), new(-1, -2), new(-2, -1)
    };

    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();

        Vector2Int[] dirs = {
            new( 1,  0), new(-1,  0), new( 0,  1), new( 0, -1),
            new( 1,  1), new( 1, -1), new(-1,  1), new(-1, -1)
        };

        foreach (var d in dirs)
        {
            var p = BoardPos + d;
            while (board.InBounds(p))
            {
                var piece = board.GetPiece(p);
                if (piece == null)
                {
                    moves.Add(p);
                }
                else
                {
                    if (piece.Owner != Owner) moves.Add(p);
                    break;
                }
                p += d;
            }
        }

        if (PowerUpRules.For(Owner).queenKnightMove)
        {
            foreach (var v in L)
            {
                var p = BoardPos + v;
                if (!board.InBounds(p)) continue;

                var piece = board.GetPiece(p);
                if (piece == null || piece.Owner != Owner)
                    moves.Add(p);
            }
        }

        return moves;
    }
}

