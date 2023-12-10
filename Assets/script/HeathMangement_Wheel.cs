using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathMangement_Wheel : HealthMangemnet
{
    public GameObject wheelCollider;
    public GameObject WheelModel;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        wheelCollider.GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);

        transform.position = pos;
        transform.rotation = rot;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!Broken)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            wheelCollider.GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);

            WheelModel.transform.position = pos;
            WheelModel.transform.rotation = rot;
        }
        else
        {
            WheelModel.transform.position = transform.position;
            WheelModel.transform.rotation = transform.rotation;
        }
    }
    protected override void Crashed()
    {
       base.Crashed();
        
            wheelCollider.SetActive(false);
        
    }
}
