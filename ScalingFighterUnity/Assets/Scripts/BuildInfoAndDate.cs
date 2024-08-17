using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildInfoAndDate
{
    private static BuildInfoAndDate _instance;
    public static BuildInfoAndDate Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuildInfoAndDate();
            }
            return _instance;
        }
    }

    public DateTime BuildTime { get; private set; }
    public String BuildDate { get; private set; }

    protected BuildInfoAndDate()
    {
        try
        {
            var txt = (UnityEngine.Resources.Load("BuildDate") as TextAsset);
            if (txt != null)
                BuildDate = txt.text.Trim();
        }
        catch (Exception e)
        {
            Debug.LogError("BuildInfoAndDate error loading + parsing: " + e.Message);
        }
    }
}