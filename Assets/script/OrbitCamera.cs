using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 1;
    private void camerarotate() 
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime); 
        
        
           
       
    }
   
    private void Update()
    {
        camerarotate();
       
    }

}
