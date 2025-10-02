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

    void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
