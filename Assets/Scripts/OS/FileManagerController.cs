using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileManagerController : MonoBehaviour
{
    [SerializeField]
    private RectTransform filesParent;

    [SerializeField]
    private GameObject fileTemplate;

    [SerializeField]
    private List<ScriptStorage> scriptsStorages = new List<ScriptStorage>();

    private void OnEnable()
    {
        StartCoroutine(ShowFiles());
    }

    private IEnumerator ShowFiles()
    {

        for (int i = filesParent.childCount - 1; i >= 0; i--)
        {
            Destroy(filesParent.GetChild(i).gameObject);
        }

        foreach (var script in scriptsStorages)
        {

            GameObject go = Instantiate(fileTemplate);
            go.transform.SetParent(filesParent);
            go.GetComponentInChildren<Text>().text = script.Name;

            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                CodeEditorController.Instance.gameObject.GetComponent<WindowController>().OpenWindow();

                string id = script.ID;
                CodeEditorController.Instance.LoadScript(id);
                LinksStorage.Instance.DialogController.ShowDialog(script.Dialog_ID);
            });

            go.transform.localScale = Vector3.one;

            yield return new WaitForEndOfFrame();

        }
    }
}
