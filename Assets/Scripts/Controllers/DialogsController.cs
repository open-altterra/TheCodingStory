using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogsController : MonoBehaviour
{


    [Header("Textures")]
    [SerializeField]
    private RenderTexture botRenderTexture;
    [SerializeField]
    private RenderTexture guyRenderTexture;

    [Header("Components")]
    [SerializeField]
    private RawImage characterIcon;
    [SerializeField]
    private Text textDialog;
    [SerializeField]
    private Text textNextButton;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private GameObject dialogGameObject;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        dialogGameObject.SetActive(false);
    }


    public void ShowDialog(string id)
    {
        var dialog = LinksStorage.Instance.DialogsStorage.Dialogs.FirstOrDefault(x => x.ID == id);

        if (dialog != null)
        {
            if (dialog.Messages.Count > 0)
            {
                ShowMessages(dialog, 0);
                LinksStorage.Instance.CharacterController.StopCharacter();
            }
            else
            {
                Debug.LogError($"In dialog '{id}' messages doesn't exists!");
            }
        }
        else
        {
            Debug.LogError($"Dialog '{id}' not found!");
        }
    }

    private void ShowMessages(DialogsStorage.Dialog dialog, int currentIndex)
    {
        DialogsStorage.Message message = dialog.Messages[currentIndex];

        characterIcon.gameObject.SetActive(true);
        dialogGameObject.SetActive(false);

        LinksStorage.Instance.CharacterController.LockRaycast = message.LockRaycast;

        switch (message.Author)
        {
            case DialogsStorage.Message.Authors.Guy:
                characterIcon.texture = guyRenderTexture;

                break;
            case DialogsStorage.Message.Authors.Bot:
                characterIcon.texture = botRenderTexture;
                break;
            case DialogsStorage.Message.Authors.Empty:
                characterIcon.gameObject.SetActive(false);
                break;
            case DialogsStorage.Message.Authors.SYSTEM:
                Debug.Log($"Invoke: {message.Content}");

                LinksStorage.Instance.EventsController.InvokeEvent(message.Content);
                if (dialog.Messages.Count > currentIndex + 1)
                {
                    int newIndex = ++currentIndex;
                    ShowMessages(dialog, newIndex);
                } else
                {
                    CloseDialog();
                }
                return;
        }

        if (dialog.Messages.Count > currentIndex + 1)
        {
            textNextButton.text = "Далее";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() =>
            {
                int newIndex = ++currentIndex;
                ShowMessages(dialog, newIndex);
            });
        }
        else
        {
            textNextButton.text = "Закрыть";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() =>
            {
                CloseDialog();
            });
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(StartAnimationDialog(message.Content));
    }

    System.Random random = new System.Random();
    IEnumerator StartAnimationDialog(string message)
    {
        dialogGameObject.SetActive(true);
        textDialog.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f + random.Next(0, 50) / 1000f);
            textDialog.text += message[i];
        }

        currentCoroutine = null;
    }

    private void CloseDialog()
    {
        dialogGameObject.SetActive(false);
        LinksStorage.Instance.CharacterController.LockRaycast = false;
    }
}
