using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Vector3 offset;
    
    public float smoothSpeed = 0.2f;
    float OriginalCenterHeight;
   public bool Wining = false;
   public Transform WinManTrans;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(CameraStartDelay());
        OriginalCenterHeight = (GameRules.instance.players[0].transform.position.y + GameRules.instance.players[1].transform.position.y) / 2f;
    }
    
    void TwoManFocus()
    {
        
            Vector3 player1Pos = GameRules.instance.players[0].transform.position;
            Vector3 player2Pos = GameRules.instance.players[1].transform.position;
            Vector3 PlayerCenter = (player1Pos + player2Pos) / 2f;
            Vector3 DesiredPlayerCenter = new Vector3(PlayerCenter.x, Mathf.Clamp(PlayerCenter.y, OriginalCenterHeight - 10, OriginalCenterHeight + 80), PlayerCenter.z);

            Vector3 player1PlanePos = new Vector3(player1Pos.x, 0f, player1Pos.z);
            Vector3 player2PlanePos = new Vector3(player2Pos.x, 0f, player2Pos.z);
            float planeDis = (player1PlanePos - player2PlanePos).magnitude;
            float HighOffset = Mathf.Clamp(planeDis, 0, 300);
            Vector3 hieghtOffset = new Vector3(0, HighOffset, 0);//玩家距离扩大的话 摄像头移动得远

            Vector3 smoothedPos = Vector3.Lerp(transform.position, DesiredPlayerCenter + offset + hieghtOffset, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPos;
            Vector3 camerLookAtRot = new Vector3(transform.position.x, PlayerCenter.y, PlayerCenter.z);
            transform.LookAt(camerLookAtRot);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!Wining)
        {
            TwoManFocus();
        }
        else
        {
            Vector3 smoothedPos = Vector3.Lerp(transform.position, WinManTrans.position + new Vector3(20,20,20) , smoothSpeed * Time.deltaTime/2);
            transform.position = smoothedPos;
            transform.LookAt(WinManTrans);

        }
       
       
    }
    
    
}
