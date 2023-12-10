using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManger : MonoBehaviour
{
   public GameObject DriftEffect;
   public GameObject NitroEffect;
    public GameObject DashIncator;
    public GameObject weakEffect;
    public GameObject CarHeadHittenSpark;
    public GameObject HitGroundSpark;
    
    //之后可以有声音！
    // Start is called before the first frame update
    void Start()
    {
        
        DriftEffect = GameObject.Instantiate(DriftEffect, transform);
        DriftEffect.transform.parent = gameObject.transform;
        Drift(false);


        NitroEffect = GameObject.Instantiate(NitroEffect, transform);
        NitroEffect.transform.parent = gameObject.transform;
        stopNitro();

        DashIncator = GameObject.Instantiate(DashIncator, transform);
        DashIncator.transform.parent = gameObject.transform;
        DashIncator.SetActive(false);

        weakEffect = GameObject.Instantiate(weakEffect, transform);
        weakEffect.transform.parent = gameObject.transform;
        weakEffect.SetActive(false);
    }

    // Update is called once per frame
   /* public void PlayEffect(int type,bool stat)//1.drift 2.nitro
    {
        switch (type)
        {
            case 1:
                PlayPaticleEffect(DriftEffect, stat);
                break;
            case 2:
                PlayPaticleEffect(NitroEffect, stat);
                break;
            case 0:
                return;
                
        }
    }
    void PlayPaticleEffect(GameObject obj, bool stat)
    {
        if (stat)
        {
            obj.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            obj.GetComponent<ParticleSystem>().Stop();
        }
       
    }
  */

    public void ShowDashindicator()
    {
        DashIncator.SetActive(true);
    }

    public void showWeakEffect()
    {
        weakEffect.SetActive(true);
    }
    public void Nitro()
    {
        NitroEffect.GetComponent<ParticleSystem>().Play();
        NitroEffect.GetComponent<TrailRenderer>().time = 1f;
        Invoke("stopNitro", 1f);
        DashIncator.SetActive(false);
    }
    void stopNitro()
    {
        NitroEffect.GetComponent<ParticleSystem>().Stop();
        NitroEffect.GetComponent<TrailRenderer>().time = 0.1f;
    }
    public void Drift(bool stat)
    {
        if (stat)
        {
            DriftEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            DriftEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void CarHeadKnockSpark(Vector3 position)
    {
        Instantiate(CarHeadHittenSpark, position, Quaternion.identity);
    }

    public void CarHitGroundSpark(Vector3 position)
    {
        Instantiate(HitGroundSpark, position, Quaternion.identity);
    }
    void insParticleObj(GameObject obj)
    {
        if (obj != null)
        {
            obj = GameObject.Instantiate(obj, transform.position, transform.rotation);
            obj.transform.parent = this.gameObject.transform;
            obj.GetComponent<ParticleSystem>().Stop();
            
        }
    }
}
