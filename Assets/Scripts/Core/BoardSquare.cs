using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoardSquare : MonoBehaviour
{
    public Vector2Int coord;
    [SerializeField] private SpriteRenderer hl;

    private Collider2D col;

    private void Awake()
    {
        if (hl == null) hl = GetComponentInChildren<SpriteRenderer>(true);
        col = GetComponent<Collider2D>();
        hl.enabled  = false;   // kein Licht
        col.enabled = false;   //  ➜ fängt keine Klicks ab
    }

    public void SetHighlight(bool on)
    {
        hl.enabled  = on;
        col.enabled = on;      // nur leuchtende Felder sind klickbar
    }

    private void OnMouseDown() =>
        InputController.Instance.ClickSquare(this);
}

