using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField]
    private string DialogID;
    [SerializeField]
    private bool OnlyOnce;

    private bool complete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (complete) return;

        if(other.tag == "MainCharacter")
        {
            LinksStorage.Instance.DialogController.ShowDialog(DialogID);

            if (OnlyOnce) complete = true;
        }
    }
}
