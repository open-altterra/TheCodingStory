using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotRecoveryScript : MonoBehaviour
{
    [SerializeField]
    private Text interfaceText;

    [SerializeField]
    private InputField interfaceInputField;

    private string defaultText = "";
    private Coroutine currentCroutine;
    private void Awake()
    {
        defaultText = interfaceText.text;
    }

    private void OnEnable()
    {
        interfaceInputField.ActivateInputField();
    }

    public void TryToSendCommand()
    {
        if (string.IsNullOrWhiteSpace(interfaceInputField.text)) return;

        if(interfaceInputField.text != "recovery")
        {
            if (currentCroutine != null)
                StopCoroutine(currentCroutine);

            currentCroutine = StartCoroutine(StartTextAnimation($"> {interfaceInputField.text}\n- Unknown command. Try to start system...\n{defaultText}"));
        } else
        {
            interfaceInputField.interactable = false;
            currentCroutine = StartCoroutine(StartTextAnimation($"> recovery\n- Start recovery...\n\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n... ... ... ... ... ... ... ... ...\n\n- Recovery complete! Try to start system...\n- System successfully started!", true));
        }

        interfaceInputField.text = "";
    }

    IEnumerator StartTextAnimation(string text, bool isCorrect = false)
    {
        interfaceText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            interfaceText.text += text[i];
        }

        currentCroutine = null;

        if (isCorrect)
        {
            LinksStorage.Instance.DialogController.ShowDialog("BOT_RECOVERY_COMPLETE");
        }
    }
}
