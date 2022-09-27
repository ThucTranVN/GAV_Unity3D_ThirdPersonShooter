using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TMP_Text ammoUI;

    public void Refresh(int ammoCount)
    {
        ammoUI.text = "Ammo: " + ammoCount;
    }
}
