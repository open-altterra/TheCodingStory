using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DreamBlackoutScript : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private GameObject dreamBlackout;

    private void OnEnable()
    {
        ShowDreamBlackout();
    }

    public void ShowDreamBlackout()
    {
        dreamBlackout.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f);
    }

    public void HideDreamBlackout()
    {
        canvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            dreamBlackout.SetActive(false);
        });
    }
}
