using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : BasePopup
{
    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickCloseButton()
    {
        this.Hide();
    }
}
