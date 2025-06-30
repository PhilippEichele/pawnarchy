using UnityEngine;
using System.Collections.Generic;

public abstract class ChessPiece : MonoBehaviour
{
    public PieceDefinition data;
    public Player Owner { get; set; }
    public Vector2Int BoardPos { get; set; }

	public bool HasMoved { get; set; }

    private readonly List<IMoveModifier> modifiers = new();

    public abstract List<Vector2Int> GetBaseMoves(Board board);

    public List<Vector2Int> GetLegalMoves(Board board)
	{
		var legal = new List<Vector2Int>();
		foreach (var mv in GetBaseMoves(board))
		{
			var from = BoardPos;
			var to   = mv;
			var captured = board.GetPiece(to);

			board.grid[to.x, to.y] = this;
			board.grid[from.x, from.y] = null;
			Board.Instance.PlacePiece(this, to);

			bool kingStillInCheck = board.IsKingInCheck(Owner);

			board.grid[from.x, from.y] = this;
			board.grid[to.x, to.y] = captured;
			Board.Instance.PlacePiece(this, from);
			if (captured != null)
				captured.transform.position = board.BoardToWorld(to);

			if (!kingStillInCheck) legal.Add(mv);
		}
		return legal;
	}

    public void AddModifier(IMoveModifier mod) => modifiers.Add(mod);


	private void OnMouseDown()
	{
		if (InputController.Instance.HasActiveSelection &&
			InputController.Instance.IsHighlighted(BoardPos))
		{
			var sq = Board.Instance.GetSquare(BoardPos);
			InputController.Instance.ClickSquare(sq);
			return;
		}

		InputController.Instance.ClickPiece(this);
	}
}
