

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ChessPrefabGenerator
{
    private const string SourceSpriteFolder = "Assets/Sprites/Chess";
    private const string TargetPrefabFolder = "Assets/Prefabs/Chess";
    private const float  PieceScale         = 0.8f;   // 80 %

    [MenuItem("Tools/Chess/Generate Piece Prefabs")]
    public static void GeneratePrefabs()
    {
        if (!AssetDatabase.IsValidFolder(TargetPrefabFolder))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Chess");

        // ── Sorting-Layer prüfen ───────────────────────────────
        bool sortingLayerExists = false;
        foreach (var layer in SortingLayer.layers)
            if (layer.name == "Pieces") { sortingLayerExists = true; break; }
        if (!sortingLayerExists)
            Debug.LogWarning("Sorting Layer \"Pieces\" existiert nicht – bitte im TagManager anlegen!");

        // ── Schleife über alle Figuren-Sprites ────────────────
        foreach (var guid in AssetDatabase.FindAssets("t:Sprite", new[] { SourceSpriteFolder }))
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            var    sprite     = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            // 1 · GO, Renderer, Collider
            var go       = new GameObject(sprite.name);
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = sortingLayerExists ? "Pieces" : renderer.sortingLayerName; // ★
            renderer.sortingOrder     = 0;   // optional, falls du Reihenfolge brauchst
            go.transform.localScale   = Vector3.one * PieceScale;

            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;

            // 2 · Figuren-Script (wie zuvor)
            switch (sprite.name.ToLower())
            {
                case var s when s.Contains("pawn"):   go.AddComponent<Pawn>();   break;
                case var s when s.Contains("rook"):   go.AddComponent<Rook>();   break;
                case var s when s.Contains("knight"): go.AddComponent<Knight>(); break;
                case var s when s.Contains("bishop"): go.AddComponent<Bishop>(); break;
                case var s when s.Contains("queen"):  go.AddComponent<Queen>();  break;
                case var s when s.Contains("king"):   go.AddComponent<King>();   break;
                default:
                    Debug.LogWarning($"[ChessGen] Überspringe '{sprite.name}' – kein Script gefunden");
                    Object.DestroyImmediate(go);
                    continue;
            }

            // 3 · PieceDefinition (unverändert)
            var def = ScriptableObject.CreateInstance<PieceDefinition>();
            def.displayName = sprite.name;
            def.sprite      = sprite;
            AssetDatabase.CreateAsset(def, $"{TargetPrefabFolder}/{sprite.name}_def.asset");

            go.GetComponent<ChessPiece>().data = def;

            // 4 · Prefab speichern
            PrefabUtility.SaveAsPrefabAsset(go, $"{TargetPrefabFolder}/{sprite.name}.prefab");
            Object.DestroyImmediate(go);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Chess-Prefabs (80 % + Collider + SortingLayer \"Pieces\") erfolgreich generiert!");
    }
}
#endif

