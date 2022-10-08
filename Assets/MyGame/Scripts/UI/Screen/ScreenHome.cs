using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenHome : BaseScreen
{
    public TMP_Text userName;
    public TMP_Text passWord;

    public override void Show(object data)
    {
        base.Show(data);

        if(data != null)
        {
            if(data is UserInfo userInfo)
            {
                userName.text = userInfo.UserName;
                passWord.text = userInfo.Password;
            }
        }
    }

    public void OnClickPopupSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }
}
