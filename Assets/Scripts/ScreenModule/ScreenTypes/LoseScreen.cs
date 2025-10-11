using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : BaseScreen
{

    [SerializeField] public Button retryButton, mainMenuButton;
    void Awake()
    {
        type = ScreenType.LoseScreen;
        retryButton.onClick.AddListener(OnRetryButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnMainMenuButtonClick()
    {
        ScreenManager.Instance().HideAllScreens();
        ScreenManager.Instance().ShowScreen(ScreenType.MainMenu);
    }
}
