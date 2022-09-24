using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector]
    public CinemachineFreeLook playerCamera;
    [HideInInspector]
    public CinemachineImpulseSource cameraShake;
    [HideInInspector]
    public Animator rigController;
    public Vector2[] recoilPattern;
    public float duration;

    private float time;
    private int recoilPatternIndex;
    private float verticalRecoil;
    private float horizontalRecoil;

    void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if(time > 0)
        {
            playerCamera.m_YAxis.Value -= ((verticalRecoil / 1000) * Time.deltaTime) / duration;
            playerCamera.m_XAxis.Value -= ((horizontalRecoil / 10) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
        
    }

    private int GetNextIndex(int currentIndex)
    {
        return (recoilPatternIndex + 1) % recoilPattern.Length;
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        horizontalRecoil = recoilPattern[recoilPatternIndex].x;
        verticalRecoil = recoilPattern[recoilPatternIndex].y;
        recoilPatternIndex = GetNextIndex(recoilPatternIndex);
        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
    }

    public void Reset()
    {
        recoilPatternIndex = 0;
    }
}
