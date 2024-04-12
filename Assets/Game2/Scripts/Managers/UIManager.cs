using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public UIMainmenu UIMainmenu;
    public UISettings UISettings;
    public UIWelcome UIWelcome;
    public UIAboutGame UIAboutGame;




    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        CloseAll();
        DisplayUIWelcome(true);
        DisplayMainmenu(true);
    }

    public void CloseAll()
    {
        DisplayMainmenu(false);
        DisplaySettingsMenu(false);
        DisplayUIWelcome(false);
        DisplayUIAboutGame(false);
    }


    public void DisplayMainmenu(bool isActive)
    {
        UIMainmenu.DisplayCanvas(isActive);
    }

    public void DisplaySettingsMenu(bool isActive)
    {
        UISettings.DisplayCanvas(isActive);
    }

    public void DisplayUIWelcome(bool isActive)
    {
        UIWelcome.DisplayCanvas(isActive);
    }

    public void DisplayUIAboutGame(bool isActive)
    {
        UIAboutGame.DisplayCanvas(isActive);
    }
}
