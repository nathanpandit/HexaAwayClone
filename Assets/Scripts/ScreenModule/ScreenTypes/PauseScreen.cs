using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : BaseScreen
{

    [SerializeField] private Button resumeButton, settingsButton, quitButton;
    void Awake()
    {
        type = ScreenType.PauseScreen;
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    void OnResumeButtonClicked()
    {
        gameObject.SetActive(false);
    }

    void OnSettingsButtonClicked()
    {
        ScreenManager.Instance().ShowScreen(ScreenType.SettingsScreen);
    }

    void OnQuitButtonClicked()
    {
        GameManager.ResetLevel();
        ScreenManager.Instance().ShowScreen(ScreenType.MainMenu);
        gameObject.SetActive(false);
    }
}
