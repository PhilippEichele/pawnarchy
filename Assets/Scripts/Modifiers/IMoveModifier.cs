using System.Collections.Generic;   // ←  sorgt für List<T>
using UnityEngine;                  // ←  sorgt für Vector2Int

public interface IMoveModifier
{
    void ModifyMoves(
        ChessPiece piece,
        Board board,
        List<Vector2Int> moves);
}

