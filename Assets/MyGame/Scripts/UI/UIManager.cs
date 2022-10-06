using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public GameObject cScreen, cPopup, cNotify;

    private Dictionary<string, BaseScreen> screens = new Dictionary<string, BaseScreen>();
    private Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();
    private Dictionary<string, BaseNotify> notifies = new Dictionary<string, BaseNotify>();

    private List<string> rmScreens = new List<string>();
    private List<string> rmPopups = new List<string>();
    private List<string> rmNotifies = new List<string>();

    private const string SCREEN_RESOURCES_PATH = "Prefabs/UI/Screen/";
    private const string POPUP_RESOURCES_PATH = "Prefabs/UI/Popup/";
    private const string NOTIFY_RESOURCES_PATH = "Prefabs/UI/Notify/";

    private BaseScreen curScreen;
    private BasePopup curPopup;
    private BaseNotify curNotify;

    public Dictionary<string, BaseScreen> Screens => screens;
    public Dictionary<string, BasePopup> Popups => popups;
    public Dictionary<string, BaseNotify> Notifies => notifies;
    public BaseScreen CurScreen => curScreen;
    public BasePopup CurPopup => curPopup;
    public BaseNotify CurNotify => curNotify;


    #region Screen
    public void ShowScreen<T>(object data = null, bool forceShow = false) where T: BaseScreen
    {

    }

    private BaseScreen GetNewScreen<T>() where T : BaseScreen
    {
        string nameScreen = typeof(T).Name;
        GameObject pfScreen = GetUIPrefab(UIType.Screen, nameScreen);
        if(pfScreen == null || !pfScreen.GetComponent<BaseScreen>())
        {
            throw new MissingReferenceException("Cant found " + nameScreen + "screen. !!!");
        }
        GameObject ob = Instantiate(pfScreen) as GameObject;
        ob.transform.SetParent(this.cScreen.transform);
        ob.transform.localScale = Vector3.one;
        ob.transform.localPosition = Vector3.zero;
#if UNITY_EDITOR
        ob.name = "SCREEN_" + nameScreen;
#endif
        BaseScreen sceneScr = ob.GetComponent<BaseScreen>();
        sceneScr.Init();
        return sceneScr;
    }

    public void HideAllScreens()
    {

    }

    public T GetExistScreen<T>() where T : BaseScreen
    {
        return null;
    }

    private void RemoveScreen(string v)
    {

    }


    #endregion

    private GameObject GetUIPrefab(UIType t, string uiName)
    {
        GameObject result = null;
        var defaultPath = "";
        if(result == null)
        {
            switch (t)
            {
                case UIType.Screen:
                    {
                        defaultPath = SCREEN_RESOURCES_PATH + uiName;
                    }
                    break;
                case UIType.Popup:
                    {
                        defaultPath = POPUP_RESOURCES_PATH + uiName;
                    }
                    break;
                case UIType.Notify:
                    {
                        defaultPath = NOTIFY_RESOURCES_PATH + uiName;
                    }
                    break;
            }

            result = Resources.Load(defaultPath) as GameObject;
        }

        return result;
    }
}
