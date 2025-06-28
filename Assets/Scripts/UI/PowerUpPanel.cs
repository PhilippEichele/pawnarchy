using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour
{
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    [SerializeField] private TMPro.TextMeshProUGUI leftLabel;
    [SerializeField] private TMPro.TextMeshProUGUI rightLabel;

    private GameManager gm;
    private Player currentPlayer;

    public void Show(PowerUp a, PowerUp b, Player player)
    {
        gm = GameManager.Instance;
        currentPlayer = player;

        leftLabel.text  = a.displayName;
        rightLabel.text = b.displayName;

        leftBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.RemoveAllListeners();
        leftBtn.onClick.AddListener(() => Pick(a));
        rightBtn.onClick.AddListener(() => Pick(b));

        gameObject.SetActive(true);
    }

    private void Pick(PowerUp choice)
    {
        choice.Apply(gm, currentPlayer);
        gameObject.SetActive(false);
    }
}

