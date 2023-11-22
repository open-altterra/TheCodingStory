using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script Storage", fileName = "Script Storage")]
public class ScriptStorage : ScriptableObject
{
    public string ID = "";
    public string Dialog_ID = "";
    public string Name = "";
    public List<string> AnswersIDRequired = new List<string>();

    [Multiline(10)]
    public List<string> Code = new List<string>();
}
