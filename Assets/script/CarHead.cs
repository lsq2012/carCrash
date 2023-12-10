using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHead : MonoBehaviour
{
    public float Force;
    public float Damage;
    public float knockBackForce;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageI = collision.gameObject.GetComponent<DamageInterface>();
        //Vector3 dir = transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.up)*-1;
        Vector3 dir =  rb.velocity.normalized*0.2f + Vector3.up;
        //Vector3 dir =   Vector3.up;
        if (damageI != null)
        {
            damageI.CalculateDamage(Damage,dir.normalized, Force, gameObject,transform.position);
        }
    }
    // Update is called once per frame
    public void KnockBack()
    {
        transform.parent.parent.GetComponent<Rigidbody>().velocity = transform.parent.parent.GetComponent<Rigidbody>().velocity.normalized * -knockBackForce;
    }
}
