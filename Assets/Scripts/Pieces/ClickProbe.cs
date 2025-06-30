using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickProbe : MonoBehaviour
{
    private Camera cam;
    private Collider2D myCol;

    private void Awake()
    {
        cam   = Camera.main;
        myCol = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 wp   = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 p2d  = new Vector2(wp.x, wp.y);

        Collider2D hit = Physics2D.OverlapPoint(p2d);
        if (hit == myCol)
            Debug.Log($"[ClickProbe] Genau dieses Stück wurde getroffen: {name}");
        else if (hit != null)
            Debug.Log($"[ClickProbe] Irgendein anderes Objekt wurde getroffen: {hit.name}");
        else
            Debug.Log("[ClickProbe] Raycast traf überhaupt kein 2D-Collider.");
    }
}
