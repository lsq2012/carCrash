using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMangement_FixedParts :HealthMangemnet
{
    Vector3 Pos;
    Quaternion Rot;
  Transform Trans;
    Vector3 Offset;

    protected override void Start()
    {
        
        
        base.Start();
        Pos = transform.localPosition;
        Rot = transform.localRotation;
        
        
        
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       /* transform.localPosition = Pos;
        transform.localRotation = Rot;
       /* if (!Broken)
        {
            transform.position = ParentTrans.position - Offset;
            transform.rotation = ParentTrans.rotation;
        }*/
    }


    protected override void Crashed()
    {
        base.Crashed();
        if (GetComponent<FixedJoint>() != null)
        {
            GetComponent<FixedJoint>().breakForce = 0;
        }
        
    }
   

}
