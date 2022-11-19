using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

public class InHouseEditor : MonoBehaviour
{
    [MenuItem("InHouseSDK/Take Screenshot", false, 1)]
    static void TakeScreenshot()
    {
        double unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        string nameFile = "Screenshot_" + Screen.width + "x" + Screen.height + "- {0}_{1}_{2}_{3}" + ".png";
        nameFile = string.Format(nameFile, DateTime.Now.Day,
            DateTime.Now.Month, DateTime.Now.Year, unixTimeStamp.ToString());
        string filePath = Application.dataPath.Remove(Application.dataPath.Length - 7) + "/Screenshot";
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            AssetDatabase.Refresh();
        }
        ScreenCapture.CaptureScreenshot(filePath + "/" + nameFile);
    }

    [MenuItem("InHouseSDK/Attach/Cheater", false, 4)]
    static void AttachCheater()
    {
        var path = "Prefabs/btnCheat";
        var ob = Resources.Load(path) as GameObject;
        if (ob == null)
            return;
        GameObject loggerOb = Instantiate(ob) as GameObject;
        loggerOb.name = "CHEATER";
        loggerOb.transform.localScale = Vector3.one;
        loggerOb.transform.SetAsLastSibling();

    }
    [MenuItem("InHouseSDK/Attach/Debug Console", false, 4)]
    static void AttachDebugConsole()
    {
        var path = "Prefabs/IngameDebugConsole";
        var ob = Resources.Load(path) as GameObject;
        if (ob == null)
            return;
        GameObject debugOb = Instantiate(ob) as GameObject;
        debugOb.name = "DEBUG_CONSOLE";
        debugOb.transform.localScale = Vector3.one;
        debugOb.transform.SetAsLastSibling();
    }

    [MenuItem("InHouseSDK/Delete/Data Folder", false, 4)]
    static void DeleteDataFolder()
    {
        foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
        {
            DirectoryInfo data_dir = new DirectoryInfo(directory);
            data_dir.Delete(true);
        }

        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            file_info.Delete();
        }

        Caching.ClearCache();
    }

    [MenuItem("InHouseSDK/Attach/Graphy FPS ", false, 4)]
    static void AttachGraphyFPS()
    {
        var path = "Prefabs/GraphyFPS";
        var ob = Resources.Load(path) as GameObject;
        if (ob == null)
            return;
        GameObject debugOb = Instantiate(ob) as GameObject;
        debugOb.name = "GRAPHY_FPS";
        debugOb.transform.localScale = Vector3.one;
        debugOb.transform.SetAsLastSibling();
    }

    [MenuItem("InHouseSDK/Delete/PlayPref", false, 1)]
    static void DeleteAllPlayPref()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log(" ======== Delete All PlayPref ======== ");
    }

    [MenuItem("InHouseSDK/Open Folder/Persistent Data ", false, 4)]
    static void OpenDataFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("InHouseSDK/Open Folder/Screenshot", false, 4)]
    static void OpenScreenShotFolder()
    {
        string filePath = Application.dataPath.Remove(Application.dataPath.Length - 7) + "/Screenshot";
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            AssetDatabase.Refresh();
        }
        EditorUtility.RevealInFinder(filePath);
    }

    [MenuItem("InHouseSDK/Open Scene/Loading &1", false, 5)]
    static void OpenSceneDebugLogin()
    {
        OpenScene("Loading");
    }

    [MenuItem("InHouseSDK/Open Scene/Main &2", false, 5)]
    static void OpenSceneHome()
    {
        OpenScene("Main");
    }

    static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/MyGame/Scenes/" + sceneName + ".unity");
        }
    }
}

#endif
