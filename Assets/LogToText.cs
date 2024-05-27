using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LogToText : MonoBehaviour
{
  public TextMeshProUGUI textMeshPro;
  public string consoleOutput;

  void Awake()
  {
    Application.logMessageReceived += HandleLog;
  }

  void OnDisable()
  {
    Application.logMessageReceived -= HandleLog;
  }

  void HandleLog(string logString, string stackTrace, LogType type)
  {
    if (type == LogType.Exception)
    {
      consoleOutput += logString + "\n " + stackTrace + "\n";
      textMeshPro.text = consoleOutput;
    }
  }
}
