using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup blackoutCanvasGroup;

    [SerializeField]
    private Text blackoutText;

    [SerializeField]
    private bool clearPlayerPrefs = false;

    private void Update()
    {
        if (clearPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            clearPlayerPrefs = false;
        }
    }

    public void ShowBlackout(string header = "", bool animateText = true, float animateBlockoutTime = 2f, float autoHideInSeconds = 0f)
    {
        LinksStorage.Instance.CharacterController.LockRaycast = true;
        blackoutText.text = "";
        blackoutCanvasGroup.alpha = 0f;
        blackoutCanvasGroup.gameObject.SetActive(true);


        blackoutCanvasGroup.DOFade(1f, animateBlockoutTime).OnComplete(() =>
        {
            if (animateText)
            {
                if (!string.IsNullOrWhiteSpace(header))
                    StartCoroutine(StartTextAnimation(3f, header));
            }
            else
            {
                blackoutText.text = header;
            }
        });


        if (autoHideInSeconds > 0f)
        {
            StartCoroutine(AutohideTimer(autoHideInSeconds));
        }
    }

    IEnumerator AutohideTimer(float autoHideInSeconds)
    {
        yield return new WaitForSecondsRealtime(autoHideInSeconds);
        HideBlackout();

    }

    public void HideBlackout(bool animateBlackout = true, float animateBlockoutTime = 2f)
    {
        blackoutCanvasGroup.alpha = 1f;
        blackoutCanvasGroup.gameObject.SetActive(true);

        if (animateBlackout)
        {
            blackoutCanvasGroup.DOFade(0f, animateBlockoutTime).OnComplete(() =>
            {
                blackoutCanvasGroup.gameObject.SetActive(false);
                LinksStorage.Instance.CharacterController.LockRaycast = false;
            });
        }
        else
        {
            blackoutCanvasGroup.alpha = 0f;
        }
    }

    IEnumerator StartTextAnimation(float delay, string header)
    {
        yield return new WaitForSecondsRealtime(delay);

        for (int i = 0; i < header.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            blackoutText.text += header[i];
        }
    }
}
