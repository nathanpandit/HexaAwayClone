using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseScreen
{

    [SerializeField] private Button startButton;
    void Awake()
    {
        type = ScreenType.MainMenu;
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        LevelManager.Instance().StartGame();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        startButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {GameManager.level}";
    }
}
