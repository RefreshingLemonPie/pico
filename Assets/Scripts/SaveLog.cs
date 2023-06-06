using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveLog : MonoBehaviour
{
    private static string _LogPath = Application.persistentDataPath + "/Log" + string.Format("_{0:yyyy_MM_dd_HH_mm}.txt", DateTime.Now);

    

    public static void Init()
    {
        Application.logMessageReceived += LogCallback;
    }

    public static void LogCallback(string condition, string stackTrace, LogType type)
    {
        File.AppendAllText(_LogPath, condition + "\r\n", Encoding.UTF8);
    }

}
