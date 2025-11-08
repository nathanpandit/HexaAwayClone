using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : BaseScreen
{

    [SerializeField] private Button backButton;
    void Awake()
    {
        type = ScreenType.SettingsScreen;
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnEnable()
    {
        GameManager.PauseGame();
    }

    void OnDisable()
    {
        if (ScreenManager.Instance().GetActiveBaseScreenCount() == 0)
        {
            GameManager.ResumeGame();
        }
    }

    void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
