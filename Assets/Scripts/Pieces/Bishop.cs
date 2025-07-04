using UnityEngine;
using System.Collections.Generic;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();
        bool phaseOwn = PowerUpRules.For(Owner).bishopPhaseAllies;

        Vector2Int[] dirs = {
            new( 1,  1), new( 1, -1),
            new(-1,  1), new(-1, -1)
        };

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
                else if (piece.Owner != Owner)
                {
                    moves.Add(pos);
                    break;
                }
                else
                {
                    if (!phaseOwn) break;
                }

                pos += d;
            }
        }
        return moves;
    }
}

