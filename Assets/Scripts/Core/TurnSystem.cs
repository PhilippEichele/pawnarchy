using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    private void Awake() => Instance = this;

    private readonly Dictionary<Player, int> moves = new()
    {
        { Player.White, 0 },
        { Player.Black, 0 }
    };

    public (Vector2Int from, Vector2Int to, ChessPiece piece) LastMove { get; private set; }

    public event Action<Player, int> OnFiveMovesPerPlayer;

	public void RegisterMove(Vector2Int from, Vector2Int to, ChessPiece piece)
	{
		LastMove = (from, to, piece);

		Player owner = piece.Owner;
		moves[owner]++;

		int count = moves[owner];
		Debug.Log($"[TurnSystem] {owner}-Zug #{count}");

		if (count % 5 == 0)
		{
			Debug.Log($"[TurnSystem] --> OnFiveMovesPerPlayer ({owner}, batch {count/5})");
			OnFiveMovesPerPlayer?.Invoke(owner, count / 5);
		}
	}


    public bool CanEnPassant(Vector2Int myFrom, Vector2Int myTo, Player me)
    {
        if (LastMove.piece is not Pawn) return false;

        bool onCorrectRank = me == Player.White ? myFrom.y == 4 : myFrom.y == 3;

        return onCorrectRank &&
               Mathf.Abs(LastMove.from.y - LastMove.to.y) == 2 &&
               LastMove.to.y == myFrom.y &&
               LastMove.to.x == myTo.x &&
               LastMove.piece.Owner != me;
    }
}

