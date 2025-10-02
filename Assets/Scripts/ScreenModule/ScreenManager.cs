using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenManager : Singleton<ScreenManager>
{
    private static ScreenManager _instance;
    private BaseScreen[] screens;
    private RootScreen[] roots;

    void Awake()
    {
        screens = GetComponentsInChildren<BaseScreen>(includeInactive: true);
        roots = GetComponentsInChildren<RootScreen>(includeInactive: true);
    }

    void Start()
    {
        HideAllScreens();
        ShowScreen(ScreenType.MainMenu);
    }

    public void ShowScreen(ScreenType screenType)
    {
        var currentScreen = screens.FirstOrDefault(s => s.type == screenType)?.gameObject;
        currentScreen.SetActive(true);
    }

    public void HideScreen(ScreenType screenType)
    {
        var screen = screens.FirstOrDefault(s => s.type == screenType)?.gameObject;
        if (screen != null)
        {
            screen.SetActive(false);
        }
    }

    public void HideAllBaseScreens()
    {
        foreach (var screen in screens)
        {
            Debug.Log("Setting screen " + screen.type + " to inactive");
            screen.gameObject.SetActive(false);
        }
    }

    public void HideAllRootScreens()
    {
        foreach(var root in roots)
        {
            root.gameObject.SetActive(false);
        }
    }
    
    public void HideAllScreens()
    {
        HideAllBaseScreens();
        HideAllRootScreens();
    }
    
    
}