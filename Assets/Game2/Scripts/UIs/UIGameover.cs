using UnityEngine.UI;

public class UIGameover : CustomCanvas
{
    public Button RetryBtn;

    private void Start()
    {
        RetryBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            Loader.Load(Loader.Scene.GameplayScene);
        });
    }

    private void OnDestroy()
    {
        RetryBtn.onClick.RemoveAllListeners();
    }
}
