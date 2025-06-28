using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Promote Random Pawn",
                 fileName = "PU_PromoteRandomPawn")]
public class PromoteRandomPawn : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        var pawns = Board.Instance.GetAllPieces<Pawn>(owner).ToList();
        if (pawns.Count == 0) return;

        Pawn       target = pawns[Random.Range(0, pawns.Count)];
        Vector2Int pos    = target.BoardPos;

        Object.Destroy(target.gameObject);
        Board.Instance.grid[pos.x, pos.y] = null;

        GameObject prefab = BoardSpawner.Instance.GetPrefab("Queen", owner);
        if (prefab == null) { Debug.LogWarning("Queen-Prefab fehlt"); return; }

        var go    = Object.Instantiate(prefab);
        var queen = go.GetComponent<ChessPiece>();
        queen.Owner = owner;

        Board.Instance.PlacePiece(queen, pos);
    }
}

