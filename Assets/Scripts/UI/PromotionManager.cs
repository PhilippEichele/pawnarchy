using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // falls ihr TextMeshPro nutzt

public class PromotionManager : MonoBehaviour
{
    public static PromotionManager Instance { get; private set; }

    [Header("UI-Referenzen")]
    [SerializeField] private RectTransform panel;         // Euer Panel, als Container
    [SerializeField] private GameObject buttonPrefab;     // Prefab: Button mit Image+Sprite und Text child

    private Vector2Int promotePos;
    private Player     promoteOwner;
    private Action<string> onPromotionComplete;

    private readonly string[] options = { "Queen", "Rook", "Bishop", "Knight" };

    private void Awake()
    {
        Instance = this;
        panel.gameObject.SetActive(false);

        // Panel als Vertical-Container einrichten (einmalig)
        if (panel.GetComponent<VerticalLayoutGroup>() == null)
        {
            var vg = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            vg.childControlWidth     = true;
            vg.childControlHeight    = true;
            vg.childForceExpandWidth = false;
            vg.childForceExpandHeight= false;
            vg.spacing               = 5f;
            vg.childAlignment        = TextAnchor.MiddleCenter;
        }
        if (panel.GetComponent<ContentSizeFitter>() == null)
        {
            var csf = panel.gameObject.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit   = ContentSizeFitter.FitMode.PreferredSize;
        }
    }

    /// <summary>
    /// Zeigt das Panel mit den Promotion-Buttons an.
    /// </summary>
    public void Show(Vector2Int boardPos, Player owner, Action<string> onComplete)
    {
        promotePos            = boardPos;
        promoteOwner          = owner;
        onPromotionComplete   = onComplete;

        // Panel zentrieren (Panel-Anchors sollten auf Mitte stehen)
        panel.anchoredPosition = Vector2.zero;

        // Alte Buttons löschen
        foreach (Transform child in panel) Destroy(child.gameObject);

        // Für jede Option: Prefab klonen und konfigurieren
        foreach (var type in options)
        {
            var btnGO = Instantiate(buttonPrefab, panel);
            btnGO.name = "Promote_" + type;

            // Der Image-Component im Prefab behält automatisch seinen Sprite bei,
            // Ihr müsst also hier **kein** image.sprite = … setzen!

            // Text füllen (hier TextMeshProUGUI, alternativ UnityEngine.UI.Text)
            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = type;
            else
            {
                var uiText = btnGO.GetComponentInChildren<Text>();
                if (uiText != null) uiText.text = type;
            }

            // Klick-Listener
            var btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            string captured = type;
            btn.onClick.AddListener(() => OnPromote(captured));
        }

        panel.gameObject.SetActive(true);
    }

	private void OnPromote(string type)
	{
		// Alten Bauern entfernen
		var oldPawn = Board.Instance.GetPiece(promotePos);
		if (oldPawn != null)
			Destroy(oldPawn.gameObject);

		// UI schließen
		panel.gameObject.SetActive(false);

		// Nur Callback aufrufen – keine Instanzierung hier!
		onPromotionComplete?.Invoke(type);
	}
}

