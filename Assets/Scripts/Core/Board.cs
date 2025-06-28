using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    // ---------- Datenstrukturen ----------
    public ChessPiece[,] grid    = new ChessPiece[8, 8];   // Figuren
    public BoardSquare[,] squares = new BoardSquare[8, 8]; // Felder

    // ---------- Layout ----------
    [Header("Layout")]
    public float tileSize = 1f;
    public Vector2 origin;                     // wird im Awake berechnet
    [SerializeField] private SpriteRenderer boardImage;

    // ---------- Awake (einzige Version!) ----------
    private void Awake()
    {
        Instance = this;                       // Singleton-Zuweisung

        // Board-Grafik finden
        if (boardImage == null)
            boardImage = GetComponentInChildren<SpriteRenderer>();

        // tileSize & origin ableiten
        tileSize = boardImage.bounds.size.x / 8f;
        float half = boardImage.bounds.size.x * 0.5f;
        origin = new Vector2(-half + tileSize * 0.5f,
                             -half + tileSize * 0.5f);
    }

    // ---------- Mapping ----------
    public Vector3 BoardToWorld(Vector2Int p)
    {
        return transform.position +
               new Vector3(origin.x + p.x * tileSize,
                           origin.y + p.y * tileSize,
                           0f);
    }

    public Vector2Int WorldToBoard(Vector3 world)
    {
        Vector3 local = world - transform.position;
        int x = Mathf.RoundToInt((local.x - origin.x) / tileSize);
        int y = Mathf.RoundToInt((local.y - origin.y) / tileSize);
        return new Vector2Int(x, y);
    }

    // ---------- Square-Verwaltung ----------
    public void PlaceSquare(BoardSquare sq, Vector2Int c)
    {
        squares[c.x, c.y] = sq;
        sq.coord = c;
        sq.transform.position = BoardToWorld(c);
    }
    public BoardSquare GetSquare(Vector2Int c) => squares[c.x, c.y];

    // ---------- Figuren-Logik ----------
    public ChessPiece GetPiece(Vector2Int pos) => grid[pos.x, pos.y];

    public void PlacePiece(ChessPiece piece, Vector2Int pos)
    {
        grid[pos.x, pos.y] = piece;
        piece.BoardPos     = pos;
        piece.transform.position = BoardToWorld(pos);
    }

	public void MovePiece(Vector2Int from, Vector2Int to)
    {
        // 1) Referenzen
        ChessPiece piece  = grid[from.x, from.y];
        ChessPiece target = grid[to.x,   to.y];

        // 2) Normales Schlagen
        if (target != null && target.Owner != piece.Owner)
        {
            Destroy(target.gameObject);
        }

        // 3) En Passant
        bool isPawn      = piece is Pawn;
        bool diagonal    = Mathf.Abs(to.x - from.x) == 1 && Mathf.Abs(to.y - from.y) == 1;
        bool squareEmpty = target == null;
        if (isPawn && diagonal && squareEmpty
            && TurnSystem.Instance.CanEnPassant(from, to, piece.Owner))
        {
            Vector2Int enemyPos = TurnSystem.Instance.LastMove.to;
            var enemy = grid[enemyPos.x, enemyPos.y];
            if (enemy != null) Destroy(enemy.gameObject);
            grid[enemyPos.x, enemyPos.y] = null;
        }

        // 4) Rochade
        if (piece is King && Mathf.Abs(to.x - from.x) == 2)
        {
            int dir  = (to.x - from.x) > 0 ? 1 : -1;
            int rank = piece.Owner == Player.White ? 0 : 7;
            Vector2Int rookFrom = dir > 0 ? new Vector2Int(7, rank)
                                          : new Vector2Int(0, rank);
            Vector2Int rookTo   = new Vector2Int(to.x - dir, rank);

            if (grid[rookFrom.x, rookFrom.y] is Rook rook && !rook.HasMoved)
            {
                grid[rookFrom.x, rookFrom.y] = null;
                grid[rookTo.x,   rookTo.y]   = rook;
                rook.BoardPos           = rookTo;
                rook.transform.position = BoardToWorld(rookTo);
                rook.HasMoved           = true;
            }
        }

        // 5) Promotion?
        bool promotionRank = piece is Pawn
            && ((piece.Owner == Player.White && to.y == 7)
             || (piece.Owner == Player.Black && to.y == 0));

        if (promotionRank)
        {
            // Alten Bauern entfernen
            Destroy(piece.gameObject);
            grid[from.x, from.y] = null;

            // Promotion-Dialog anzeigen
            PromotionManager.Instance.Show(to, piece.Owner, (string selectedType) =>
			{
				// **Hier** spawnst du jetzt genau einmal den neuen Stein:
				GameObject prefab = BoardSpawner.Instance.GetPrefab(selectedType, piece.Owner);
				if (prefab == null)
				{
					Debug.LogWarning($"Kein Prefab für {selectedType} ({piece.Owner}) gefunden!");
					return;
				}

				var go       = Instantiate(prefab);
				var newPiece = go.GetComponent<ChessPiece>();
				newPiece.Owner = piece.Owner;

				Board.Instance.PlacePiece(newPiece, to);

				// Zug abschließen
				TurnSystem.Instance.RegisterMove(from, to, newPiece);
				GameManager.Instance.EndTurn();
			});            
			return; // Rest überspringen
        }

        // 6) Normaler Zug
        grid[to.x,   to.y]   = piece;
        grid[from.x, from.y] = null;
        piece.BoardPos           = to;
        piece.transform.position = BoardToWorld(to);
        piece.HasMoved           = true;

        // 7) Abschluss
        TurnSystem.Instance.RegisterMove(from, to, piece);
        GameManager.Instance.EndTurn();
    }

    public bool InBounds(Vector2Int p) => p.x >= 0 && p.x < 8 && p.y >= 0 && p.y < 8;

    public IEnumerable<T> GetAllPieces<T>(Player owner) where T : ChessPiece
    {
        var list = new List<T>();
        foreach (var piece in grid)
            if (piece is T t && t.Owner == owner) list.Add(t);
        return list;
    }

	public bool IsEmpty(Vector2Int p)       => grid[p.x, p.y] == null;
	public bool IsEnemy(Vector2Int p, Player me)
		=> !IsEmpty(p) && grid[p.x, p.y].Owner != me;

	public bool SquareIsAttacked(Vector2Int pos, Player defender)
	{
		foreach (var piece in grid)
		{
			if (piece == null || piece.Owner == defender) continue;

			// Nur Grundzüge prüfen – reicht für Rochade
			foreach (var move in piece.GetBaseMoves(this))
				if (move == pos) return true;
		}
		return false;
	}


	public bool IsKingInCheck(Player player)
	{
		// König suchen
		Vector2Int kingPos = default;
		foreach (var piece in grid)
			if (piece is King k && k.Owner == player)
				kingPos = piece.BoardPos;

		// Angriffsprüfung ähnlich wie SquareIsAttacked
		foreach (var piece in grid)
		{
			if (piece == null || piece.Owner == player) continue;
			foreach (var mv in piece.GetBaseMoves(this))
				if (mv == kingPos)
					return true;
		}
		return false;
	}
}

