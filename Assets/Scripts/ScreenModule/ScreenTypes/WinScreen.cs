using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{

    [SerializeField] public Button nextLevelButton, mainMenubutton;
    [SerializeField] public TextMeshProUGUI congratsText;
    void Awake()
    {
        type = ScreenType.WinScreen;
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        mainMenubutton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnMainMenuButtonClicked()
    {
        ScreenManager.Instance().HideAllScreens();
        ScreenManager.Instance().ShowScreen(ScreenType.MainMenu);
    }

    void OnEnable()
    {
        congratsText.text = $"Cleared Level {GameManager.level-1}!";
        nextLevelButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {GameManager.level}";
    }
}
