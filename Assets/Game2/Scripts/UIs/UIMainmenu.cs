using UnityEngine;
using UnityEngine.UI;

public class UIMainmenu : CustomCanvas
{
    public Button PlayBtn;
    public Button SettingsBtn;
    public Button AboutGameBtn;
    public Button QuitBtn;

    private void Start()
    {
        PlayBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            Loader.Load(Loader.Scene.GameplayScene);
        });

        SettingsBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIManager.Instance.CloseAll();
            UIManager.Instance.DisplaySettingsMenu(true);
        });

        AboutGameBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIManager.Instance.CloseAll();
            UIManager.Instance.DisplayUIAboutGame(true);
        });

        QuitBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            Application.Quit();

            // For the Unity Editor, this will not quit the application. It will stop the editor's play mode.
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }

    private void OnDestroy()
    {
        PlayBtn.onClick.RemoveAllListeners();
        SettingsBtn.onClick.RemoveAllListeners();
        AboutGameBtn.onClick.RemoveAllListeners();
        QuitBtn.onClick.RemoveAllListeners();
    }
}
