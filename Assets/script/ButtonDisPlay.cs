using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDisPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask GroundLayerMask;
   // Vector3 OriLocPos;
    // Update is called once per frame

    private void Start()
    {
        //OriLocPos = transform.localPosition;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        RaycastHit GroundCheck;
        if (Physics.Raycast(transform.parent.position,new Vector3(0,-1,0), out GroundCheck, 100f, GroundLayerMask))
        {
            transform.position = new Vector3(transform.parent.position.x, GroundCheck.point.y+0.1f, transform.parent.position.z-0.9f);
            //transform.localPosition = new Vector3(OriLocPos.x, transform.localPosition.y, OriLocPos.z);
        }
       
    }
}
