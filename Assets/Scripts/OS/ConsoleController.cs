using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleController : MonoBehaviour
{
    [SerializeField]
    private Text console;
    public static ConsoleController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public static void Print(object data)
    {
        Instance.console.text += $"[{DateTime.Now.ToString("HH:mm:ss")}] {data}\n";
    }

    public static void Clear()
    {
        Instance.console.text = "";
    }
}

