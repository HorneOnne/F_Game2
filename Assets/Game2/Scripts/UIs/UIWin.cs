using UnityEngine.UI;

public class UIWin : CustomCanvas
{
    public Button NextBtn;

    private void Start()
    {
        NextBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            GameManager.Instance.RandomLevel();
            Loader.Load(Loader.Scene.GameplayScene);
        });
    }

    private void OnDestroy()
    {
        NextBtn.onClick.RemoveAllListeners();
    }
}
