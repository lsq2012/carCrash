using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollOverNotify : MonoBehaviour
{
   
    
    public Transform TransToFollow;
    public Vector3 offset;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
        transform.position = TransToFollow.position + offset;
        //transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles + (new Vector3(72, 0, 0)));
        transform.rotation = Camera.main.transform.rotation;
    }
}
