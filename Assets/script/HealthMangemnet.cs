using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthMangemnet : MonoBehaviour,DamageInterface
{
    public float Orihealth = 100f;
    public float HurtLevel = 3000f;
    public float DamamgeDecreaser = 0.01f;
    float health;
    Material Mat;
    bool Hurted=false;
    Rigidbody rb;
    public bool PrintImpluse;
   public bool Broken = false;
    Vector3 OriLocalPos;
    Quaternion OriLocalRot;
    Color oriColor;

  /* public Vector3 impluseDir
    {
        get
        {
            
        }
    }*/
   // public GameObject wheelCollider;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
       // Mat = GetComponent<MeshRenderer>().material;
        //oriColor = Mat.color;
        health = Orihealth;
       
       OriLocalPos = transform.localPosition;
        OriLocalRot = transform.localRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        float Impluse = collision.impulse.magnitude;
        
        if (!Hurted) 
        {
            if (Impluse > HurtLevel)
            {
                Hurted = true;
                health -= collision.impulse.magnitude * DamamgeDecreaser;
                Invoke("canBeHurt", 0.5f);
                //Mat.color = Color.red;

                transform.parent.parent.GetComponent<CarController>().GetHitten(collision.impulse.magnitude * DamamgeDecreaser,collision.impulse.normalized,0f,transform.position,"ground");
                if (health < 0)
                {
                    //health = 0.01f;
                    Crashed();
                    
              
                }
               
            }
            if (PrintImpluse)
            {
                print(Impluse + gameObject.name);
            }
        }
      /*  var damageI = collision.gameObject.GetComponent<DamageInterface>();
        if(damageI != null)
        {
            damageI.CalculateDamage(rb.velocity.normalized, 6f);
        }*/
        
        

        
       
        
    }
   protected virtual  void FixedUpdate()
    {
       
    }
    void canBeHurt()
    {
        Hurted = false;
       // Mat.color = new Color(1, health / Orihealth, health / Orihealth, 1);
    }
   protected virtual void Crashed()
    {
        /*rb.constraints = RigidbodyConstraints.None;
        
     
        Hurted = true;
        Broken = true;*/
    }

   public void Recover()
    {
        transform.localPosition = OriLocalPos;
        transform.rotation = OriLocalRot;
        health = Orihealth;
        Broken = false;
        customRecover();//子类的回复
    }
    protected virtual void customRecover()
    {

    }

    public void CalculateDamage(float damage,Vector3 impluseDir, float Force,GameObject WhoDidThis,Vector3 HappenPos)
    {
        if (!Hurted)
        {
            Hurted = true;
            //Mat.color = Color.red;
            Invoke("canBeHurt", 1f);
            transform.parent.parent.GetComponent<CarController>().GetHitten(damage, impluseDir, Force,HappenPos,"carhead");
            WhoDidThis.GetComponent<CarHead>().KnockBack();
            //print(WhoDidThis +"launching" + transform.parent.parent.gameObject.name);
        }
        
        //rb.AddForce(impluseDir*100f*-1);

    }
}
