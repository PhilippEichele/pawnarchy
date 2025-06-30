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
		{
			Player winner = currentPlayer == Player.White ? Player.Black : Player.White;
			WinnerScreen.Instance.Show(winner);
			return;
		}

		if (Board.Instance.IsKingInCheck(currentPlayer))
			Debug.Log($"Schach auf {currentPlayer}!");
	}

	public bool IsCheckmate()
	{
		Player p = CurrentPlayer;
		if (!Board.Instance.IsKingInCheck(p)) return false;
		foreach (var piece in Board.Instance.GetAllPieces<ChessPiece>(p))
			if (piece.GetLegalMoves(Board.Instance).Count > 0)
				return false;
		return true;
	}
}

