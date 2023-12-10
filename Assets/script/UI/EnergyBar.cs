using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{//UI理论上可以运行。 小车生成后实例化这个 再告诉这个 该跟随的物体以及offset。 之后小车还要获得这个类 漂移时候告诉这个类数据 让其增减、播放动画。 之后还要做关卡开始和结束的计分 和重来系统。 因此怎么标识两个小车也很重要（或者告诉两个小车自己是个啥也可以）
    Image maskImg;
    RawImage barRawImg;
    //private EnergyBarCal energyBarCal;
   public float FillAmount;
    // Start is called before the first frame update
   public Transform TransToFollow;
    public Vector3 offset;
    public bool Gaining;
    private void Awake()
    {
        maskImg = transform.Find("EnergyBar/BarMask").GetComponent<Image>();
        maskImg.fillAmount = 0.1f;
        barRawImg = transform.Find("EnergyBar/BarMask/Bar").GetComponent<RawImage>();
       // energyBarCal = new EnergyBarCal();


    }
   
    private void Update()
    {
        // energyBarCal.Update();
        maskImg.fillAmount = FillAmount;
        transform.position = TransToFollow.position + offset;
        transform.rotation = Camera.main.transform.rotation;
        if (Gaining)
        {
            Animated();
        }
        


    }
    void Animated()
    {
        Rect uvRect = barRawImg.uvRect;
        uvRect.x -= 2f * Time.deltaTime;
        barRawImg.uvRect = uvRect;
        if (maskImg.fillAmount < 0.34f)
        {
            barRawImg.color = new Color(barRawImg.color.r, barRawImg.color.g, barRawImg.color.b, 0.5f);
        }
        else
        {
            barRawImg.color = new Color(barRawImg.color.r, barRawImg.color.g, barRawImg.color.b, 1f);
        }
        
        
    }
}
/*public class EnergyBarCal
{
    public const float Energy_max = 3f;
    float energy;
    //float energyGainAmount;
    public EnergyBarCal()
    {
        energy = 0;
        //energyGainAmount = 0.5f;
    }
    public void Update()
    {
        //energy += energyGainAmount * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0f, Energy_max);
    }
    public void tryspendEnergy(int amount)
    {
        if (energy >= amount)
        {
            energy -= amount;
        }
    }
    public float GetEnergyNormalize(float Power)
    {
        return Power / Energy_max;
    }
}*/
