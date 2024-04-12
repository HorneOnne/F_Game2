using UnityEngine;
public class UIGameplayManager : MonoBehaviour
{
    public static UIGameplayManager Instance { get; private set; }

    //public UIBackground UIBackground;
    //public UIGameplay UIGameplay;
    //public UILevelSelection UILevelSelection;
    //public UIDiceRoll UIDiceRoll;
    //public UIInformation UIInformation;
    //public UIWin UIWin;
    //public UIGameover UIGameover;
    //public UIMiniGame UIMiniGame;




    //private void Awake()
    //{
    //    Instance = this;
    //}


    //private void Start()
    //{
    //    CloseAll();
    //    DisplayUILevelSelection(true);
    //}


    //public void CloseAll()
    //{
    //    //DisplayGameplayMenu(false);
    //    DisplayUILevelSelection(false);
    //    DisplayUIDiceRoll(false);
    //    DisplayUIInformation(false);
    //    DisplayUIWin(false);
    //    DisplayUIGameover(false);
    //    DisplayUIMiniGame(false);
    //}


    //public void DisplayGameplayMenu(bool isActive)
    //{
    //    UIGameplay.DisplayCanvas(isActive);
    //}

    //public void DisplayUILevelSelection(bool isActive)
    //{
    //    UILevelSelection.DisplayCanvas(isActive);
    //}

    //public void DisplayUIDiceRoll(bool isActive)
    //{
    //    UIDiceRoll.UpdateTurnText();
    //    UIDiceRoll.DiceNumberText.text = "-";
    //    UIDiceRoll.DisplayCanvas(isActive);
    //}

    //public void DisplayUIInformation(bool isActive)
    //{
    //    UIInformation.DisplayCanvas(isActive);
    //}

    //public void DisplayUIWin(bool isActive)
    //{
    //    UIWin.DisplayCanvas(isActive);
    //}

    //public void DisplayUIGameover(bool isActive)
    //{
    //    UIGameover.DisplayCanvas(isActive);
    //}
    //public void DisplayUIMiniGame(bool isActive)
    //{
    //    UIMiniGame.DisplayCanvas(isActive);
    //}
}
