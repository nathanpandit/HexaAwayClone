using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseScreen
{

    [SerializeField] private Button startButton, settingsButton;
    void Awake()
    {
        type = ScreenType.MainMenu;
        startButton.onClick.AddListener(OnStartButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    void OnStartButtonClick()
    {
        GameManager.ResumeGame();
        LevelManager.Instance().StartGame();
        gameObject.SetActive(false);
    }

    void OnSettingsButtonClick()
    {
        ScreenManager.Instance().ShowScreen(ScreenType.SettingsScreen);
    }

    void OnEnable()
    {
        /*
        if(GameManager.level == 0) startButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Level 1";
        else startButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {GameManager.level}";
        */

        GameManager.PauseGame();
    }
}
