using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Answer Storage", fileName = "Answer Storage")]
public class AnswersStorage : ScriptableObject
{
    [System.Serializable]
    public class AnswerID
    {
        public string ID;
        public string Answer;
    }

    public List<AnswerID> Answers = new List<AnswerID>();
}
