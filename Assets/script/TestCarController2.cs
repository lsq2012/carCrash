using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCarController2 : MonoBehaviour
{
    

    //注意这八个变量，四个是获取车轮碰撞器的，四个是获取车轮模型的
    public WheelCollider leftF;
    public WheelCollider leftB;
    public WheelCollider rightF;
    public WheelCollider rightB;

    public Transform model_leftF;
    public Transform model_leftB;
    public Transform model_rightF;
    public Transform model_rightB;
    


    void Update()
    {
        WheelsControl_Update();
    }

   
    void WheelsControl_Update()
    {
        
        WheelsModel_Update(model_leftF, leftF);
        WheelsModel_Update(model_leftB, leftB);
        WheelsModel_Update(model_rightF, rightF);
        WheelsModel_Update(model_rightB, rightB);
    }

    //控制车轮模型移动 转向
    void WheelsModel_Update(Transform t, WheelCollider wheel)
    {
        Vector3 pos = t.position;
        Quaternion rot = t.rotation;

        wheel.GetWorldPose(out pos, out rot);

        t.position = pos;
        t.rotation = rot;
    }

}
