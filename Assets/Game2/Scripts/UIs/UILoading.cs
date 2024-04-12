using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : CustomCanvas
{
    public Image LoadingIcon;
    [SerializeField] private List<Sprite> _loadingProgressSprites;
    void Start()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            Loader.Load(Loader.Scene.MenuScene);
        }));
    }

    private IEnumerator LoadingCoroutine(System.Action onFinished)
    {
        // Swap sprites
        for (int i = 0; i < _loadingProgressSprites.Count; i++)
        {
            LoadingIcon.sprite = _loadingProgressSprites[i];
            yield return new WaitForSeconds(0.1f); // Wait for swapInterval before swapping the next sprite
        }

        onFinished?.Invoke();
    }

}
