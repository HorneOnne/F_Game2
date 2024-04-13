using UnityEngine;
public class UIGameplayManager : MonoBehaviour
{
    public static UIGameplayManager Instance { get; private set; }


    public UIGameplay UIGameplay;
    public UIWin UIWin;
    public UIGameover UIGameover;
    public UIGameplaySettings UIGameplaySettings;





    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        CloseAll();
    }


    public void CloseAll()
    {
        DisplayUIWin(false);
        DisplayUIGameover(false);
        DisplayUIGameplaySettings(false);
    }

    public void DisplayUIWin(bool isActive)
    {
        UIWin.DisplayCanvas(isActive);
    }

    public void DisplayUIGameover(bool isActive)
    {
        UIGameover.DisplayCanvas(isActive);
    }


    public void DisplayUIGameplaySettings(bool isActive)
    {
        UIGameplaySettings.DisplayCanvas(isActive);
    }

}
