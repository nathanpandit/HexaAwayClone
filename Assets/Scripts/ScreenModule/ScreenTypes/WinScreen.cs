using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{

    [SerializeField] public Button nextLevelButton, mainMenubutton;
    void Awake()
    {
        type = ScreenType.WinScreen;
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        mainMenubutton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    void OnNextLevelButtonClicked()
    {
        
    }

    void OnMainMenuButtonClicked()
    {
        ScreenManager.Instance().HideAllScreens();
        ScreenManager.Instance().ShowScreen(ScreenType.MainMenu);
    }
}
