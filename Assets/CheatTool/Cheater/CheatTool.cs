using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public partial class CheatTool : BaseManager<CheatTool>
{
    public GameObject BtnCheat, BtnLogger, gGraphyFPS;

    private string[] excludeMethods = new string[] { "GetMethods", "OnInited", "Update", "OnGUI",
        "CreateCheatButton" , "CreateDebugButton" , "ShowCheatButton", "Awake", "OnDestroy" , "GetAllFields"};

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnInited()
    {
        base.OnInited();
        CreateCheatButton();
    }

    public void HideCheatButton()
    {
        if (CheatTool.Instance.gameObject == null)
            return;
        CheatTool.Instance.gameObject.SetActive(false);
    }

    public void ShowCheatButton()
    {
        CheatTool.Instance.BtnCheat.SetActive(true);
    }

    public void CreateCheatButton()
    {
        if (CheatTool.Instance.BtnCheat != null)
            return;

        var ob = Resources.Load("Prefabs/btnCheat") as GameObject;
        if (ob == null)
            return;
        CheatTool.Instance.BtnCheat = Instantiate(ob) as GameObject;
        CheatTool.Instance.BtnCheat.transform.localScale = Vector3.one;
        CheatTool.Instance.BtnCheat.transform.SetAsLastSibling();
        CheatTool.Instance.BtnCheat.name = "CHEAT_BUTTON";
        CheatTool.Instance.BtnCheat.SetActive(true);
    }

    public void ShowIngameDebug()
    {
        if (CheatTool.Instance.BtnLogger != null)
        {
            CheatTool.Instance.BtnLogger.SetActive(true);
            return;
        }

        var ob = Resources.Load("Prefabs/IngameDebugConsole") as GameObject;
        if (ob == null)
            return;
        CheatTool.Instance.BtnLogger = Instantiate(ob) as GameObject;
        CheatTool.Instance.BtnLogger.transform.localScale = Vector3.one;
        CheatTool.Instance.BtnLogger.transform.SetAsLastSibling();
        CheatTool.Instance.BtnLogger.name = "IngameDebugConsole";
        CheatTool.Instance.BtnLogger.SetActive(true);
    }

    public void HideDebugButton()
    {
        if (CheatTool.Instance.BtnLogger != null) CheatTool.Instance.BtnLogger.SetActive(false);
    }

    public string ShowCurrentFrameRate()
    {
        return " QualitySettings.vSyncCount = " + QualitySettings.vSyncCount + " \n Application.targetFrameRate = " + Application.targetFrameRate;
    }

    public string ShowSystemLanguage()
    {
        return Application.systemLanguage.ToString() + " --- " + Application.targetFrameRate;
    }

    public void SettingFps(int fps, int vSyncCount)
    {
        QualitySettings.vSyncCount = vSyncCount;
        Application.targetFrameRate = fps;
    }

    public void ChangeFps30()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    public void ChangeFps60()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void ShowGraphyFPSDisplay()
    {
        if (CheatTool.Instance.gGraphyFPS != null)
        {
            CheatTool.Instance.gGraphyFPS.SetActive(true);
            return;
        }

        var ob = Resources.Load("Prefabs/GraphyFPS") as GameObject;
        if (ob == null)
            return;
        CheatTool.Instance.gGraphyFPS = Instantiate(ob) as GameObject;
        CheatTool.Instance.gGraphyFPS.transform.localScale = Vector3.one;
        CheatTool.Instance.gGraphyFPS.transform.SetAsLastSibling();
        CheatTool.Instance.gGraphyFPS.name = "GraphyFPS";
        CheatTool.Instance.gGraphyFPS.SetActive(true);
    }

    public void HideGraphyFPSDisplay()
    {
        if (CheatTool.Instance.gGraphyFPS != null) CheatTool.Instance.gGraphyFPS.SetActive(false);
    }

    public void SetScreenResolutionPC(int width, int height)
    {
#if UNITY_STANDALONE
        Screen.SetResolution(width, height, false);
#endif
    }

    public void ClearCache()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log(" ======== Delete All PlayPref ======== ");
    }

    public void TakeScreenshot()
    {
        double unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        string nameFile = "Screenshot_" + Screen.width + "x" + Screen.height + "- {0}_{1}_{2}_{3}" + ".png";
        nameFile = string.Format(nameFile, DateTime.Now.Day,
            DateTime.Now.Month, DateTime.Now.Year, unixTimeStamp.ToString());
        string filePath = Application.dataPath.Remove(Application.dataPath.Length - 7) + "/Screenshot";
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        ScreenCapture.CaptureScreenshot(filePath + "/" + nameFile);
    }

    public void ExampleMethodA(string para1, int para2 = 123, float para3 = 0.4f, string para4 = "abc")
    {
        Debug.Log("ExampleMethodA " + para1 + " + para2:  " + para2 + " : " + para3 + " : " + para4);
    }

    public MethodInfo[] GetMethods()
    {
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        MethodInfo[] methods = CheatTool.Instance.GetType().GetMethods(flags);

        List<MethodInfo> result = new List<MethodInfo>();
        bool check = false;
        for (int i = 0; i < methods.Length; i++)
        {
            check = false;
            for (int j = 0; j < this.excludeMethods.Length; j++)
            {
                if (this.excludeMethods[j] == methods[i].Name)
                {
                    check = true;
                    break;
                }
            }
            if (check)
                continue;
            result.Add(methods[i]);

        }
        return result.ToArray();
    }

    public List<FieldInfo> GetAllFields()
    {
        List<FieldInfo> result = new List<FieldInfo>();
        BindingFlags bindingFlags = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance |
                              BindingFlags.Static;
        var fieldValues = CheatTool.Instance.GetType()
                     .GetFields(bindingFlags);
        foreach (FieldInfo field in fieldValues)
        {
            result.Add(field);
        }
        return result;
    }
}
