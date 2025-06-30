using UnityEngine;
using System.Collections.Generic;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();

        int  dir  = Owner == Player.White ? 1 : -1;
        var  f    = PowerUpRules.For(Owner);
        bool dbl  = f.pawnsAlwaysDouble;
        bool back = f.pawnsBackwards;

        Vector2Int fwd = BoardPos + new Vector2Int(0, dir);
        if (board.InBounds(fwd) && board.IsEmpty(fwd))
            moves.Add(fwd);

        Vector2Int fwd2 = BoardPos + new Vector2Int(0, 2 * dir);
        bool atStart = Owner == Player.White ? BoardPos.y == 1 : BoardPos.y == 6;
        if (board.InBounds(fwd2) &&
            board.IsEmpty(fwd) && board.IsEmpty(fwd2) &&
            (dbl || atStart))
        {
            moves.Add(fwd2);
        }

        if (back)
        {
            Vector2Int backPos = BoardPos - new Vector2Int(0, dir);
            if (board.InBounds(backPos) && board.IsEmpty(backPos))
                moves.Add(backPos);
        }

        Vector2Int[] diag = { new(-1, dir), new(1, dir) };
        foreach (var d in diag)
        {
            var p = BoardPos + d;
            if (!board.InBounds(p)) continue;

            if (board.IsEnemy(p, Owner) ||
                TurnSystem.Instance.CanEnPassant(BoardPos, p, Owner))
            {
                moves.Add(p);
            }
        }

        return moves;
    }
}

