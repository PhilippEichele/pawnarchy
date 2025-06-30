
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPicker : MonoBehaviour
{
    public static PowerUpPicker Instance { get; private set; }

    [Header("UI-Referenzen")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject    buttonPrefab;

    private Action<PowerUp> onComplete;

    private void Awake()
    {
        Instance = this;
        panel.gameObject.SetActive(false);

        if (!panel.TryGetComponent(out VerticalLayoutGroup vg))
        {
            vg = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            vg.childAlignment     = TextAnchor.MiddleCenter;
            vg.spacing            = 8f;
            vg.childControlWidth  = true;
            vg.childControlHeight = true;
        }
        if (!panel.TryGetComponent(out ContentSizeFitter csf))
        {
            csf = panel.gameObject.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }

    public void Show(IEnumerable<PowerUp> choices, Player player, Action<PowerUp> onChoice)
    {
        onComplete = onChoice;

        foreach (Transform c in panel) Destroy(c.gameObject);

        foreach (var pu in choices)
        {
            var btnGO = Instantiate(buttonPrefab, panel);
            btnGO.name = "PU_" + pu.name;

            var img = btnGO.GetComponent<Image>();
            if (img != null && pu.icon != null) img.sprite = pu.icon;

            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = pu.displayName;

            var btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => Pick(pu));
        }

        panel.anchoredPosition = Vector2.zero;
        panel.gameObject.SetActive(true);
    }

    private void Pick(PowerUp pu)
    {
        panel.gameObject.SetActive(false);
        onComplete?.Invoke(pu);
    }
}

