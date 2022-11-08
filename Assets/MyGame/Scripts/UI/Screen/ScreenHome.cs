using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickPopupSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }

    public void StartGame()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingGame>();
        }
        this.Hide();
    }
}
