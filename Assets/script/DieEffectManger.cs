using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffectManger : MonoBehaviour
{
   public GameObject[] GameObjToDestory;
   public GameObject[] addRigAndColliderObj;
    public AudioSource Scream;
    public AudioSource Boom;
    
    public GameObject DieEffect;
    // Start is called before the first frame update
    void Start()
    {
       // DieEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        for(int i = 0; i < GameObjToDestory.Length; i++)
        {
            GameObjToDestory[i].SetActive(false);
            //Destroy(GameObjToDestory[i]);
        }
        
        for (int i = 0; i < addRigAndColliderObj.Length; i++)
        {
            addRigAndColliderObj[i].AddComponent<CapsuleCollider>();
            addRigAndColliderObj[i].AddComponent<Rigidbody>().mass = 2;
        }
        /* carhead.GetComponent<CarHead>().enabled = false;
         carhead.GetComponent<FixedJoint>().breakForce = 0;
         carTail.GetComponent<FixedJoint>().breakForce = 0;
         carTail.GetComponent<HealthMangement_FixedParts>().enabled = false;*/

        Instantiate(DieEffect, transform.position, Quaternion.identity);
        Scream.Play();
        Boom.Play();
        Invoke("DestoryGameobj", 1f);
    }

    void DestoryGameobj()
    {
        for (int i = 0; i < GameObjToDestory.Length; i++)
        {
            
            Destroy(GameObjToDestory[i]);
        }

    }
}
