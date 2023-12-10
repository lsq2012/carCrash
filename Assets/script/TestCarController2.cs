using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCarController2 : MonoBehaviour
{
    

    //ע����˸��������ĸ��ǻ�ȡ������ײ���ģ��ĸ��ǻ�ȡ����ģ�͵�
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

    //���Ƴ���ģ���ƶ� ת��
    void WheelsModel_Update(Transform t, WheelCollider wheel)
    {
        Vector3 pos = t.position;
        Quaternion rot = t.rotation;

        wheel.GetWorldPose(out pos, out rot);

        t.position = pos;
        t.rotation = rot;
    }

}
