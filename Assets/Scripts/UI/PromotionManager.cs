using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromotionManager : MonoBehaviour
{
    public static PromotionManager Instance { get; private set; }

    [Header("UI-Referenzen")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject buttonPrefab;

    private Vector2Int promotePos;
    private Player     promoteOwner;
    private Action<string> onPromotionComplete;

    private readonly string[] options = { "Queen", "Rook", "Bishop", "Knight" };

    private void Awake()
    {
        Instance = this;
        panel.gameObject.SetActive(false);

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

    public void Show(Vector2Int boardPos, Player owner, Action<string> onComplete)
    {
        promotePos            = boardPos;
        promoteOwner          = owner;
        onPromotionComplete   = onComplete;

        panel.anchoredPosition = Vector2.zero;

        foreach (Transform child in panel) Destroy(child.gameObject);

        foreach (var type in options)
        {
            var btnGO = Instantiate(buttonPrefab, panel);
            btnGO.name = "Promote_" + type;

            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = type;
            else
            {
                var uiText = btnGO.GetComponentInChildren<Text>();
                if (uiText != null) uiText.text = type;
            }

            var btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            string captured = type;
            btn.onClick.AddListener(() => OnPromote(captured));
        }

        panel.gameObject.SetActive(true);
    }

	private void OnPromote(string type)
	{
		var oldPawn = Board.Instance.GetPiece(promotePos);
		if (oldPawn != null)
			Destroy(oldPawn.gameObject);

		panel.gameObject.SetActive(false);

		onPromotionComplete?.Invoke(type);
	}
}

