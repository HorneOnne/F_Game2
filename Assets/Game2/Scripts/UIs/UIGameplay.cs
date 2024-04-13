using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIGameplay : CustomCanvas
{
    public Button SettingsBtn;
    public Button RestartBtn;
    public TextMeshProUGUI TimeText;

    public GameObject FindParent;
    public Image RequestMahjongImage;
    public TextMeshProUGUI EventTimerText;


    public GameObject MahjongCollectedPrefab;
    public Transform CollectedParent;


    private void Start()
    {
        FindParent.SetActive(false);
        SettingsBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIGameplayManager.Instance.DisplayUIGameplaySettings(true);
        });

        RestartBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            Loader.Load(Loader.Scene.GameplayScene);
        });

        TimerManager.OnEventTimeReached += SetEventUI;
        GameControllers.OnCompleteRequest += OncompleteRequestEvent;
        GameControllers.OnMatched += AddMahjong;
    }

    private void OnDestroy()
    {
        SettingsBtn.onClick.RemoveAllListeners();
        RestartBtn.onClick.RemoveAllListeners();

        TimerManager.OnEventTimeReached -= SetEventUI;
        GameControllers.OnMatched -= AddMahjong;
        GameControllers.OnCompleteRequest -= OncompleteRequestEvent;
    }


    private void Update()
    {
        TimeText.text = TimerManager.Instance.TimeToText();

        if(GameControllers.Instance.CountDown)
        {
            EventTimerText.text = TimeToText(GameControllers.Instance.EventTimer);
        }
    }



    private void SetEventUI()
    {
        StartCoroutine(Utilities.WaitAfter(0.2f, () =>
        {
            FindParent.SetActive(true);
            RequestMahjongImage.sprite = GameControllers.Instance.RequestMahjong.Icon;
        }));
    }

    public void AddMahjong(MahjongSO data)
    {
        var image = Instantiate(MahjongCollectedPrefab, CollectedParent).GetComponent<Image>();
        image.sprite = data.Icon;
    }

    public string TimeToText(float time)
    {
        if (time < 0) time = 0;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void OncompleteRequestEvent()
    {
        FindParent.SetActive(false);
    }
}
