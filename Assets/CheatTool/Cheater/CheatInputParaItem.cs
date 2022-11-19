using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CheatInputParaItem : MonoBehaviour
{

    public Text lblName;
    public InputField ifPara;
    public Toggle tggBool;
    public Dropdown ddInput;

    private ParameterInfo info;
    private Type enumType;

    void Awake()
    {
        lblName.text = "";
        ifPara.text = "";
        HideAllInput();
    }

    public void Show(ParameterInfo parameterInfo)
    {
        if (parameterInfo == null)
            return;
        this.gameObject.SetActive(true);
        this.transform.SetAsLastSibling();
        lblName.text = parameterInfo.Name;
        ifPara.text = "";
        this.info = parameterInfo;

        HideAllInput();

        // show input
        if (this.info.ParameterType == typeof(System.Boolean))
        {
            tggBool.gameObject.SetActive(true);
            if (this.info.DefaultValue.GetType() == typeof(System.Boolean))
            {
                tggBool.isOn = (System.Boolean)this.info.DefaultValue;
            }
        }
        else if (this.info.ParameterType.IsEnum)
        {
            ddInput.gameObject.SetActive(true);
            ddInput.ClearOptions();
            var enumName = info.ParameterType.Name;
            enumType = Type.GetType(enumName);
            ddInput.AddOptions(Enum.GetNames(enumType).ToList());
            ddInput.value = 0;
            if (this.info.DefaultValue.GetType() == enumType)
            {
                ddInput.value = (int)this.info.DefaultValue;
            }
        }
        //else if (this.info.ParameterType.IsArray)
        //{
        //    ddInput.gameObject.SetActive(true);
        //    ddInput.ClearOptions();

        //    CheatTool classInstance = Activator.CreateInstance(CheatTool.Instance.GetType()) as CheatTool;
        //    var listFields = classInstance.GetAllFields();
        //    for (int i = 0; i < listFields.Count; i++)
        //    {
        //        Debug.Log(listFields[i].Name + " --- " + this.info.Name);
        //        if (listFields[i].Name.Equals(this.info.Name))
        //        {
        //            if (listFields[i].FieldType == typeof(System.String[]))
        //            {
        //                var val = listFields[i].GetValue(listFields[i]) as System.String[];
        //                for (int k = 0; k < val.Length; k++)
        //                {
        //                    Debug.Log(" ---------------------- " + val[k] );

        //                }
        //            } 
        //            break;
        //        }
        //    }
        //}
        else
        {
            ifPara.gameObject.SetActive(true);
        }

        // set type input field
        if (this.info.ParameterType == typeof(System.String))
        {
            ifPara.contentType = InputField.ContentType.Alphanumeric;
            if (this.info.DefaultValue.GetType() == typeof(System.String))
            {
                ifPara.text = (System.String)this.info.DefaultValue;
            }
        }
        else if (this.info.ParameterType == typeof(System.Single))
        {
            ifPara.contentType = InputField.ContentType.DecimalNumber;
            if (this.info.DefaultValue.GetType() == typeof(System.Single))
            {
                ifPara.text = ((System.Single)this.info.DefaultValue).ToString();
            }
        }
        else if (this.info.ParameterType == typeof(System.Int32))
        {
            ifPara.contentType = InputField.ContentType.IntegerNumber;
            if (this.info.DefaultValue.GetType() == typeof(System.Int32))
            {
                ifPara.text = ((System.Int32)this.info.DefaultValue).ToString();
            }
        }
        else
        {
            ifPara.contentType = InputField.ContentType.Alphanumeric;
        }
    }

    public object GetParaValue()
    {
        if (this.info.ParameterType.IsEnum)
        {
            var index = ddInput.value;
            return (Enum)Enum.ToObject(enumType, index);
        }
        else if (this.info.ParameterType == typeof(System.Boolean))
        {
            return tggBool.isOn;
        }
        else if (this.info.ParameterType == typeof(System.String))
        {
            return ifPara.text;
        }
        else if (this.info.ParameterType == typeof(System.Single))
        {
            try
            {
                return float.Parse(ifPara.text.Trim());
            }
            catch (Exception)
            {
                return -1;
            }

        }
        else if (this.info.ParameterType == typeof(System.Int32))
        {
            try
            {
                return int.Parse(ifPara.text.Trim());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        try
        {
            return int.Parse(ifPara.text.Trim());
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    private void HideAllInput()
    {
        ifPara.gameObject.SetActive(false);
        tggBool.gameObject.SetActive(false);
        ddInput.gameObject.SetActive(false);
    }

}
