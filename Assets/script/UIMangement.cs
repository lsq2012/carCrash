using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMangement : MonoBehaviour
{
    public Text RolloverNotify;
    // Start is called before the first frame update
    public void SetRollOverNotifyVisbility( bool vis)
    {
        RolloverNotify.enabled = vis;
    }
   
}
