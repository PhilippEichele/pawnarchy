using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    private ChessPiece selected;
    private readonly List<BoardSquare> hilites = new();

    private void Awake() => Instance = this;

    // -------- Figur geklickt --------
    public void ClickPiece(ChessPiece p)
    {
        if (p.Owner != GameManager.Instance.CurrentPlayer) return;
        Clear();
        selected = p;

        foreach (var pos in p.GetLegalMoves(Board.Instance))
        {
            var sq = Board.Instance.GetSquare(pos);
            sq.SetHighlight(true);
            hilites.Add(sq);
        }
    }

    // -------- Feld geklickt --------
    
	public void ClickSquare(BoardSquare sq)
	{
		if (selected == null) return;
		if (!hilites.Contains(sq)) { Clear(); return; }

		Board.Instance.MovePiece(selected.BoardPos, sq.coord);
		Clear();                     // ← deaktiviert alle HL-Sprites
	}


    public void Clear()
    {
        foreach (var s in hilites) s.SetHighlight(false);
        hilites.Clear();
        selected = null;
    }

	public bool HasActiveSelection => selected != null;

    public bool IsHighlighted(Vector2Int pos) =>
        hilites.Exists(sq => sq.coord == pos);
}
