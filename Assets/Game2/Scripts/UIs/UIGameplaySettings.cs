using UnityEngine.UI;
using UnityEngine;

public class UIGameplaySettings : CustomCanvas
{
    public Button CloseBtn;
    public Button RestartBtn;

    public Button SoundBtn;
    public Button MusicBtn;

    [Header("Images")]
    [SerializeField] private Sprite _activeIcon;
    [SerializeField] private Sprite _deactiveIcon;

    private void Start()
    {
        UpdateSoundFXUI();
        UpdateMusicUI();
        CloseBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.Button, false);
            UIGameplayManager.Instance.DisplayUIGameplaySettings(false);
        });

        RestartBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.isSoundFXActive = true;
            SoundManager.Instance.isMusicActive = true;
            SoundManager.Instance.MuteSoundFX(false);
            SoundManager.Instance.MuteBackground(false);

            UpdateSoundFXUI();
            UpdateMusicUI();

            SoundManager.Instance.PlaySound(SoundType.Button, false);
        });


        SoundBtn.onClick.AddListener(() =>
        {
            ToggleSFX();
            SoundManager.Instance.PlaySound(SoundType.Button, false);
        });

        MusicBtn.onClick.AddListener(() =>
        {
            ToggleMusic();
            SoundManager.Instance.PlaySound(SoundType.Button, false);
        });
    }

    private void OnDestroy()
    {
        CloseBtn.onClick.RemoveAllListeners();
        RestartBtn.onClick.RemoveAllListeners();

        SoundBtn.onClick.RemoveAllListeners();
        MusicBtn.onClick.RemoveAllListeners();
    }


    private void ToggleSFX()
    {
        SoundManager.Instance.MuteSoundFX(SoundManager.Instance.isSoundFXActive);
        SoundManager.Instance.isSoundFXActive = !SoundManager.Instance.isSoundFXActive;

        UpdateSoundFXUI();
    }


    private void UpdateSoundFXUI()
    {
        if (SoundManager.Instance.isSoundFXActive)
        {
            SoundBtn.image.sprite = _activeIcon;
        }
        else
        {
            SoundBtn.image.sprite = _deactiveIcon;
        }
    }

    private void ToggleMusic()
    {
        SoundManager.Instance.MuteBackground(SoundManager.Instance.isMusicActive);
        SoundManager.Instance.isMusicActive = !SoundManager.Instance.isMusicActive;

        UpdateMusicUI();
    }

    private void UpdateMusicUI()
    {
        if (SoundManager.Instance.isMusicActive)
        {
            MusicBtn.image.sprite = _activeIcon;
        }
        else
        {
            MusicBtn.image.sprite = _deactiveIcon;
        }
    }

}