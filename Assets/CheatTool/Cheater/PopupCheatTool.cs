using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class PopupCheatTool : MonoBehaviour
{
    public GameObject pfItemMethod, pMethodContain, pDetail, pfInputItem,
    pInputContain, pRecentContain, pShowMessage, pInputMethod;
    public Text lblName, lblDetail, lblMessage;

    [SerializeField]
    protected Button buttonClear, buttonRun, buttonQuit;

    [SerializeField]
    protected InputField ifMethod;

    private MethodInfo[] myMethods;
    private MethodInfo curMethod;
    private List<MethodInfo> recentMethods = new List<MethodInfo>();

    void Start()
    {
        //for test
        this.gameObject.SetActive(true);
        if (buttonClear != null) buttonClear.onClick.AddListener(OnClearButton);
        if (buttonQuit != null) buttonQuit.onClick.AddListener(OnQuitButton);
        if (buttonRun != null) buttonRun.onClick.AddListener(OnRunButton);
        if (ifMethod != null) ifMethod.onValueChanged.AddListener(OnInputFieldMethodValueChanged);

        this.myMethods = CheatTool.Instance.GetMethods();
        HideAllMethodItem();
        OnInputFieldMethodValueChanged(ifMethod.text);
        pDetail.SetActive(false);
        pShowMessage.SetActive(false);
        pInputMethod.SetActive(true);
#if UNITY_EDITOR
        ifMethod.Select();
        ifMethod.ActivateInputField();
#endif

    }

    void OnDestroy()
    {
        if (buttonClear != null) buttonClear.onClick.RemoveAllListeners();
        if (buttonQuit != null) buttonQuit.onClick.RemoveAllListeners();
        if (buttonRun != null) buttonRun.onClick.RemoveAllListeners();

        if (ifMethod != null) ifMethod.onValueChanged.RemoveAllListeners();
        this.myMethods = null;
    }


    #region ===== Events =====
    private void OnClearButton()
    {
        ifMethod.text = "";
        pDetail.SetActive(false);
    }

    private void OnRunButton()
    {
        if (RunMethod())
        {
            //OnDeactivate();
            this.gameObject.SetActive(false);
        }
        if (this.curMethod.ReturnType == typeof(void))
        {
            ifMethod.text = "";
            pDetail.SetActive(false);
        }
       
        this.curMethod = null;
    }

    private void OnQuitButton()
    {
        // OnDeactivate();
        this.gameObject.SetActive(false);
    }

    private void OnInputFieldMethodValueChanged(string text)
    {
        HideAllMethodItem();
        FilterMethod(this.ifMethod.text.Trim());
    }

    private void OnMethodItemClick(string method)
    {
        ShowDetail(method);
    }

    #endregion

    #region ===== Methods =====
    private GameObject GetMethodItem(string methodName)
    {
        foreach (Transform item in this.pMethodContain.transform)
        {
            if (item.gameObject.activeSelf)
                continue;
            if (item.name == methodName)
            {
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }

        GameObject result = Instantiate(this.pfItemMethod) as GameObject;
        result.transform.SetParent(this.pMethodContain.transform);
        result.transform.localScale = Vector3.one;
        result.transform.localPosition = Vector3.zero;
        result.name = methodName.Trim();
        result.transform.GetChild(0).GetComponent<Text>().text = methodName.Trim();
        result.SetActive(true);
        if (result.GetComponent<Button>())
        {
            result.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnMethodItemClick(methodName.Trim());
            });
        }
        return result;
    }

    private GameObject GetInputItem(ParameterInfo parameter)
    {
        foreach (Transform item in this.pInputContain.transform)
        {
            if (item.gameObject.activeSelf)
                continue;
            if (item.name == parameter.ParameterType.Name)
            {
                if (item.GetComponent<CheatInputParaItem>())
                {
                    item.GetComponent<CheatInputParaItem>().Show(parameter);
                }
                return item.gameObject;
            }
        }

        GameObject result = Instantiate(this.pfInputItem) as GameObject;
        result.transform.SetParent(this.pInputContain.transform);
        result.transform.localScale = Vector3.one;
        result.transform.localPosition = Vector3.zero;
        result.name = parameter.ParameterType.Name;
        if (result.GetComponent<CheatInputParaItem>())
        {
            result.GetComponent<CheatInputParaItem>().Show(parameter);
        }
        return result;
    }

    private void HideAllMethodItem()
    {
        foreach (Transform item in this.pMethodContain.transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void HideAllInputItem()
    {
        foreach (Transform item in this.pInputContain.transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void HideAllRecentItem()
    {
        foreach (Transform item in this.pRecentContain.transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void FilterMethod(string textName)
    {
        List<MethodInfo> methods = GetMethods(textName);

        for (int i = 0; i < methods.Count; i++)
        {
            GetMethodItem(methods[i].Name);
        }
    }

    private List<MethodInfo> GetMethods(string methodName)
    {
        if (methodName.Trim() == "")
        {
            if (this.myMethods != null && this.myMethods.Length > 0)
            {
                return new List<MethodInfo>(this.myMethods);
            }
            else
            {
                return new List<MethodInfo>();
            }
        }

        List<MethodInfo> result = new List<MethodInfo>();
        for (int i = 0; i < this.myMethods.Length; i++)
        {
            if (this.myMethods[i].Name.ToUpper().Contains(methodName.ToUpper().Trim()))
            {
                result.Add(this.myMethods[i]);
            }
        }

        return result;
    }

    private MethodInfo GetMethodInfo(string method)
    {
        for (int i = 0; i < this.myMethods.Length; i++)
        {
            if (this.myMethods[i].Name == method)
                return this.myMethods[i];
        }
        return null;
    }

    private List<object> GetAllParameter()
    {
        List<object> result = new List<object>();
        foreach (Transform item in this.pInputContain.transform)
        {
            if (!item.gameObject.activeSelf
               || !item.GetComponent<CheatInputParaItem>())
                continue;
            result.Add(item.GetComponent<CheatInputParaItem>().GetParaValue());
        }
        return result;
    }

    private void ShowDetail(string method)
    {
        pDetail.SetActive(true);
        pInputMethod.SetActive(true);
        pShowMessage.SetActive(false);
        this.curMethod = GetMethodInfo(method);
        this.lblName.text = this.curMethod.Name;

        var detailName = string.Join
                                          (", ", this.curMethod.GetParameters()
                         .Select(x => x.ParameterType + " " + x.Name)
                         .ToArray());
        detailName = string.Format("{0} {1} ({2})",
                                              this.curMethod.ReturnType,
                                              this.curMethod.Name,
                                            detailName);
        detailName = detailName.Replace("System.", "");
        detailName = detailName.Replace("Boolean", "bool");
        detailName = detailName.Replace("Int32", "int");
        detailName = detailName.Replace("Single", "float");
        detailName = detailName.Replace("String", "string");
        detailName = detailName.Replace("Void", "void");

        this.lblDetail.text = detailName;

        HideAllInputItem();
        ParameterInfo[] parameters = this.curMethod.GetParameters();
        GameObject inputItem = null;
        for (int i = 0; i < parameters.Length; i++)
        {
            inputItem = GetInputItem(parameters[i]);
#if UNITY_EDITOR
            if (inputItem != null && i == 0 && inputItem.transform.GetChild(1).gameObject.activeSelf)
            {
                inputItem.transform.GetChild(1).GetComponent<InputField>().Select();
                inputItem.transform.GetChild(1).GetComponent<InputField>().ActivateInputField();

            }
#endif
        }
    }

    private bool RunMethod()
    {
        if (this.curMethod == null)
            return false;
        object[] objs = GetAllParameter().ToArray();
        if (objs.Length != this.curMethod.GetParameters().Length)
            return false;
        UpdateRecentMethod(this.curMethod);
        object classInstance = Activator.CreateInstance(CheatTool.Instance.GetType(), null);

        if (this.curMethod.ReturnType == typeof(System.Boolean))
        {
            var boolReturn = (Boolean)this.curMethod.Invoke(classInstance, objs);
            if (!boolReturn)
            {
                return false;
            }
        }
        else if (this.curMethod.ReturnType == typeof(System.String))
        {
            pShowMessage.SetActive(true);
            pInputMethod.SetActive(false);
            var stringReturn = (String)this.curMethod.Invoke(classInstance, objs);
            this.lblMessage.text = stringReturn;
            return false;
        }
        else
        {
            this.curMethod.Invoke(classInstance, objs);
        }

        return true;
    }

    private void UpdateRecentMethod(MethodInfo method)
    {
        // check exist
        for (int i = 0; i < this.recentMethods.Count; i++)
        {
            if (this.recentMethods[i].Name == method.Name)
                return;
        }

        this.recentMethods.Add(method);
        if (this.recentMethods.Count > 4)
        {
            this.recentMethods.RemoveAt(0);
        }
        HideAllRecentItem();

        Transform item = null;
        for (int i = 0; i < this.recentMethods.Count; i++)
        {
            item = this.pRecentContain.transform.GetChild(i);
            if (item == null || item.gameObject.activeSelf)
                continue;
            string methodName = this.recentMethods[i].Name;
            item.gameObject.SetActive(true);
            item.transform.GetChild(0).GetComponent<Text>().text = methodName;
            if (item.GetComponent<Button>())
            {

                item.GetComponent<Button>().onClick.RemoveAllListeners();
                item.GetComponent<Button>().onClick.AddListener(delegate
                {
                    ShowDetail(methodName);
                });
            }
        }

    }

    #endregion
}
