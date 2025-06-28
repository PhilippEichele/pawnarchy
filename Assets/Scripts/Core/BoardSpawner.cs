using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [Header("Logik-Klassen")]
    [SerializeField] private Board       board;       // dein Board-MonoBehaviour
    [SerializeField] private Transform   pieceRoot;   // leeres GameObject „Pieces“
    [SerializeField] private BoardSquare squarePrefab;

    [Header("Weiße Figuren-Prefabs")]
    [SerializeField] private GameObject whitePawn;
    [SerializeField] private GameObject whiteRook;
    [SerializeField] private GameObject whiteKnight;
    [SerializeField] private GameObject whiteBishop;
    [SerializeField] private GameObject whiteQueen;
    [SerializeField] private GameObject whiteKing;

    [Header("Schwarze Figuren-Prefabs")]
    [SerializeField] private GameObject blackPawn;
    [SerializeField] private GameObject blackRook;
    [SerializeField] private GameObject blackKnight;
    [SerializeField] private GameObject blackBishop;
    [SerializeField] private GameObject blackQueen;
    [SerializeField] private GameObject blackKing;

    // Singleton
    public static BoardSpawner Instance { get; private set; }

    // Lookup-Tabellen
    private Dictionary<string, GameObject> whitePrefabs;
    private Dictionary<string, GameObject> blackPrefabs;

    private void Awake()
    {
        Instance = this;

        // Dictionaries mit den Inspector-Werten füllen
        whitePrefabs = new Dictionary<string, GameObject>
        {
            ["Pawn"]   = whitePawn,
            ["Rook"]   = whiteRook,
            ["Knight"] = whiteKnight,
            ["Bishop"] = whiteBishop,
            ["Queen"]  = whiteQueen,
            ["King"]   = whiteKing
        };

        blackPrefabs = new Dictionary<string, GameObject>
        {
            ["Pawn"]   = blackPawn,
            ["Rook"]   = blackRook,
            ["Knight"] = blackKnight,
            ["Bishop"] = blackBishop,
            ["Queen"]  = blackQueen,
            ["King"]   = blackKing
        };
    }

    private void Start()
    {
        // Board-Squares generieren
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                var sq = Instantiate(squarePrefab, pieceRoot);
                board.PlaceSquare(sq, new Vector2Int(x, y));
            }

        // Startaufstellung
        SpawnBackRank(Player.White, 0);
        SpawnPawns   (Player.White, 1);
        SpawnBackRank(Player.Black, 7);
        SpawnPawns   (Player.Black, 6);
    }

    private void SpawnPawns(Player owner, int rank)
    {
        var prefab = owner == Player.White ? whitePawn : blackPawn;
        for (int file = 0; file < 8; file++)
            Add(prefab, new Vector2Int(file, rank), owner);
    }

    private void SpawnBackRank(Player owner, int rank)
    {
        bool white = owner == Player.White;
        Add(white ? whiteRook   : blackRook,   new Vector2Int(0, rank), owner);
        Add(white ? whiteKnight : blackKnight, new Vector2Int(1, rank), owner);
        Add(white ? whiteBishop : blackBishop, new Vector2Int(2, rank), owner);
        Add(white ? whiteQueen  : blackQueen,  new Vector2Int(3, rank), owner);
        Add(white ? whiteKing   : blackKing,   new Vector2Int(4, rank), owner);
        Add(white ? whiteBishop : blackBishop, new Vector2Int(5, rank), owner);
        Add(white ? whiteKnight : blackKnight, new Vector2Int(6, rank), owner);
        Add(white ? whiteRook   : blackRook,   new Vector2Int(7, rank), owner);
    }

    private void Add(GameObject prefab, Vector2Int pos, Player owner)
    {
        var piece = Instantiate(prefab, pieceRoot).GetComponent<ChessPiece>();
        piece.Owner = owner;
        board.PlacePiece(piece, pos);
    }

    /// <summary>
    /// Liefert das Prefab für den gegebenen Typ (z.B. "Queen") und Spieler.
    /// </summary>
    public GameObject GetPrefab(string pieceKey, Player owner)
    {
        var dict = owner == Player.White ? whitePrefabs : blackPrefabs;
        if (dict.TryGetValue(pieceKey, out var prefab))
            return prefab;

        Debug.LogWarning($"Kein Prefab für '{pieceKey}' ({owner}) gefunden!");
        return null;
    }
}

