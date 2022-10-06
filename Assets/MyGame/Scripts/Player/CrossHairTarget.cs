using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        
        transform.position = Physics.Raycast(ray, out hitInfo) ? hitInfo.point : ray.GetPoint(100);

        //if(Physics.Raycast(ray, out hitInfo))
        //{
        //    transform.position = hitInfo.point;
        //}
        //else
        //{
        //    transform.position = ray.GetPoint(100);
        //}
    }
}
