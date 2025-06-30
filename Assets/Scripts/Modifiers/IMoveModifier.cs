using System.Collections.Generic;
using UnityEngine;

public interface IMoveModifier
{
    void ModifyMoves(
        ChessPiece piece,
        Board board,
        List<Vector2Int> moves);
}

