using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image maskImg;
    public float FillAmount;
    public Transform TransToFollow;
    public Vector3 offset;
    // Start is called before the first frame update
    private void Awake()
    {
        maskImg = transform.Find("EnergyBar/BarMask").GetComponent<Image>();
        maskImg.fillAmount = 1f;
       
        // energyBarCal = new EnergyBarCal();


    }

    // Update is called once per frame
    void Update()
    {
        maskImg.fillAmount = FillAmount;
        transform.position = TransToFollow.position + offset;
        transform.rotation = Camera.main.transform.rotation;
    }
}
