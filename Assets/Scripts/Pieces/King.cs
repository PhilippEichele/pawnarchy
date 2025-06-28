using UnityEngine;
using System.Collections.Generic;

public class King : ChessPiece
{
    private static readonly Vector2Int[] DELTA = {
        new( 1,  0), new(-1,  0), new( 0,  1), new( 0, -1),
        new( 1,  1), new( 1, -1), new(-1,  1), new(-1, -1)
    };

    public override List<Vector2Int> GetBaseMoves(Board board)
    {
        var moves = new List<Vector2Int>();

        // 1) normale Ein-Feld-Moves
        foreach (var d in DELTA)
        {
            var p = BoardPos + d;
            if (!board.InBounds(p))       continue;
            if (!board.IsEmpty(p) &&
                board.GetPiece(p).Owner == Owner) continue;   // eigenes Piece blockiert
            moves.Add(p);
        }

        // 2) Rochade, falls König & zugehöriger Turm nicht gezogen haben
        if (!HasMoved)
        {
            TryAddCastle(+2);   // kurze (g-Linie)
            TryAddCastle(-2);   // lange  (c-Linie)
        }
        return moves;

        // ── Lokale Hilfsfunktion ───────────────────────────
        void TryAddCastle(int dx)
        {
            int dir  = dx > 0 ? 1 : -1;
            int rank = Owner == Player.White ? 0 : 7;

            Vector2Int rookPos = dir > 0 ? new Vector2Int(7, rank)
                                         : new Vector2Int(0, rank);
            if (board.GetPiece(rookPos) is not Rook rook || rook.HasMoved) return;

            // Felder zwischen König und Turm leer?
            for (int x = BoardPos.x + dir; x != rookPos.x; x += dir)
                if (!board.IsEmpty(new Vector2Int(x, rank))) return;

            // (Optionale Schach-Prüfung kann hier rein)
            moves.Add(BoardPos + new Vector2Int(dx, 0));
        }
    }
}

