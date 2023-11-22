using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Text clockText;
    void Update()
    {
        clockText.text = DateTime.Now.ToString("f");
    }
}
