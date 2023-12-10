using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSuicide : MonoBehaviour
{
    public ParticleSystem PS;
  public void suicide()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (PS != null)
        {
            if (PS.isStopped)
            {
                suicide();
            }
        }
    }
}
