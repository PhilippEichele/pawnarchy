using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinnerScreen : MonoBehaviour
{
    public static WinnerScreen Instance { get; private set; }

    [Header("Sprites")]
    [SerializeField] private Sprite whiteWinsSprite;
    [SerializeField] private Sprite blackWinsSprite;

    private Image img;

    private void Awake()
    {
        Instance = this;
        img = GetComponent<Image>();
		GetComponent<Button>().onClick.AddListener(Restart);
        gameObject.SetActive(false);      // zunächst unsichtbar
    }

    public void Show(Player winner)
    {
        img.sprite = winner == Player.White ? whiteWinsSprite : blackWinsSprite;
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
