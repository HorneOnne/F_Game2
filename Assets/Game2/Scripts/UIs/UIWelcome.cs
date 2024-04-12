using UnityEngine.UI;

public class UIWelcome : CustomCanvas
{
    public Button ContinueBtn;

    private void Start()
    {
        ContinueBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIManager.Instance.CloseAll();
            UIManager.Instance.DisplayMainmenu(true);
        });
    }

    private void OnDestroy()
    {
        ContinueBtn.onClick.RemoveAllListeners();
    }
}
