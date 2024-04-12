using UnityEngine.UI;

public class UIAboutGame : CustomCanvas
{
    public Button BackBtn;

    private void Start()
    {
        BackBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIManager.Instance.CloseAll();
            UIManager.Instance.DisplayMainmenu(true);
        });
    }

    private void OnDestroy()
    {
        BackBtn.onClick.RemoveAllListeners();
    }
}
