using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    [Multiline(3)]
    private string sceneHeader = "";
    [SerializeField]
    private bool animateText = true;
    [SerializeField]
    private float animateBlockoutTime = 0f;
    [SerializeField]
    private float autohideBlockoutInSeconds = 2f;
    [SerializeField]
    private string startDialog = "GAME_START";

    private void Start()
    {
        LinksStorage.Instance.UIController.ShowBlackout(sceneHeader, animateText, animateBlockoutTime, autohideBlockoutInSeconds);
        if (!string.IsNullOrWhiteSpace(startDialog))
        {
            StartCoroutine(StartDialogWithDelay(autohideBlockoutInSeconds + 3f));
        }
    }

    IEnumerator StartDialogWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LinksStorage.Instance.DialogController.ShowDialog(startDialog);
    }

    public void ShowGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void HideGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(GoToSceneWithBlackout(1f, sceneName));
    }

    IEnumerator GoToSceneWithBlackout(float delay, string sceneName) 
    {
        LinksStorage.Instance.UIController.ShowBlackout(animateBlockoutTime: delay);
        yield return new WaitForSecondsRealtime(delay + 0.5f); 
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
