using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake() => Instance = this;

    private Player currentPlayer = Player.White;
    public Player CurrentPlayer => currentPlayer;

	public void EndTurn()
	{
		currentPlayer = currentPlayer == Player.White ? Player.Black : Player.White;

		if (IsCheckmate())
			Debug.Log($"Schachmatt! {currentPlayer} gewinnt.");
		else if (Board.Instance.IsKingInCheck(currentPlayer))
			Debug.Log($"Schach auf {currentPlayer}!");
	}

	public bool IsCheckmate()
	{
		Player p = CurrentPlayer;
		if (!Board.Instance.IsKingInCheck(p)) return false;
		// Prüfen: Hat irgendeine Figur einen legalen Zug?
		foreach (var piece in Board.Instance.GetAllPieces<ChessPiece>(p))
			if (piece.GetLegalMoves(Board.Instance).Count > 0)
				return false;
		return true;
	}
}

