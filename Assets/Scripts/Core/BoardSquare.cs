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
        hl.enabled  = false;
        col.enabled = false;
    }

    public void SetHighlight(bool on)
    {
        hl.enabled  = on;
        col.enabled = on;
    }

    private void OnMouseDown() =>
        InputController.Instance.ClickSquare(this);
}

