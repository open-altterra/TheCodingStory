using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoslynCSharp;
using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

public class CodeEditorController : Singleton<CodeEditorController>
{
    [SerializeField]
    private RectTransform codeParent;
    [SerializeField]
    private GameObject codeTemplate;
    [SerializeField]
    private GameObject inputCodeTemplate;
    [SerializeField]
    private GameObject fileTemplate;
    [SerializeField]
    private RectTransform fileParent;

    [SerializeField]
    private List<AssemblyReferenceAsset> referenceAssets = new List<AssemblyReferenceAsset>();

    [SerializeField]
    private AnswersStorage answerStorage;

    [SerializeField]
    private List<ScriptStorage> scriptsStorages = new List<ScriptStorage>();

    private ScriptDomain domain = null;

    public override void Awake()
    {
        base.Awake();
        Application.logMessageReceived += OnLogReceived;
    }

    private void OnLogReceived(string condition, string stackTrace, LogType type)
    {
        if(type == LogType.Error)
            ConsoleController.Print($"{condition}");
    }

    private void Start()
    {
        domain = ScriptDomain.CreateDomain("Domain", true);

        foreach (var refer in referenceAssets)
        {
            domain.RoslynCompilerService.ReferenceAssemblies.Add(refer);
        }

    }

    private void OnEnable()
    {
        StartCoroutine(LoadFiles());
    }

    public IEnumerator LoadFiles()
    {
        yield return new WaitForEndOfFrame();
        for (int i = fileParent.childCount - 1; i >= 0; i--)
        {
            Destroy(fileParent.GetChild(i).gameObject);
        }

        bool allComplete = true;


        foreach (var script in scriptsStorages)
        {
            bool complete = true;
            foreach (var answer in script.AnswersIDRequired)
            {
                if (PlayerPrefs.GetString(answer, "") != "Complete")
                {
                    complete = false;
                    allComplete = false;
                }
            }

            GameObject go = Instantiate(fileTemplate);
            go.transform.SetParent(fileParent);
            go.GetComponent<Text>().text = script.Name;
            if (complete)
            {
                go.GetComponent<Text>().color = Color.green;
            }


            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                string id = script.ID;
                LoadScript(id);
                LinksStorage.Instance.DialogController.ShowDialog(script.Dialog_ID);
            });
            go.transform.localScale = Vector3.one;
        }

        if (allComplete) LinksStorage.Instance.DialogController.ShowDialog("ALL_COMPLETE");
    }

    public void Compile()
    {
        //ConsoleController.Clear();

        var codeArray = codeParent.GetComponentsInChildren<Text>();
        string code = "";

        if(codeArray.Length == 0)
        {
            ConsoleController.Print($"Nothing to compile!");
            return;
        }

        for (int i = 0; i < codeArray.Length; i++)
        {
            code += codeArray[i].text + "\n";
        }

        try
        {
            ScriptType type = domain.CompileAndLoadMainSource(code);
            ScriptProxy proxy = type.CreateInstance();
            proxy.Call("Main");
            ConsoleController.Print($"Script \"{type.Name}\" compiled successfully!");
        }
        catch (Exception ex)
        {
            ConsoleController.Print($"Compile error!");
        }

        StartCoroutine(LoadFiles());
    }

    public void LoadScript(string id)
    {
        for(int i = codeParent.childCount - 1; i >= 0; i--)
        {
            Destroy(codeParent.GetChild(i).gameObject);
        }

        var script = scriptsStorages.FirstOrDefault(x => x.ID == id);
        
        foreach(var code in script.Code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                GameObject go = Instantiate(inputCodeTemplate);
                go.transform.SetParent(codeParent);
                go.transform.localScale = Vector3.one;
            } else
            {
                GameObject go = Instantiate(codeTemplate);
                go.transform.SetParent(codeParent);
                go.GetComponent<Text>().text = code;
                go.transform.localScale = Vector3.one;
            }
        }
    }

    public bool CheckResult(string id, object data)
    {
        var answer = answerStorage.Answers.FirstOrDefault(x => x.ID == id);
        if (answer == null) return false;

        bool result = answer.Answer == data.ToString();

        if (result)
            PlayerPrefs.SetString(id, "Complete");
        else
            PlayerPrefs.DeleteKey(id);

        return result;
    }
}
