using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs Storage", fileName = "Dialogs Storage")]
public class DialogsStorage : ScriptableObject
{
    [System.Serializable]
    public class Dialog
    {
        public string ID;
        public List<Message> Messages = new List<Message>();
    }

    [System.Serializable]
    public class Message
    {
        public enum Authors { Guy, Bot, Empty, SYSTEM }
        public Authors Author;
        [Multiline(3)]
        public string Content;
        public bool LockRaycast = true;
    }

    public List<Dialog> Dialogs = new List<Dialog>();
}
