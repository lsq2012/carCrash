using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rgbBar : MonoBehaviour
{
    MeshRenderer[] RgbRender;
    List<MeshRenderer> MRlist = new List<MeshRenderer>();
    public float emssionStrength = 5;
    public int lightNumber;
    public float intervalDis;
    public GameObject LightPrefab;
    bool Shiny = false;
    public bool alongX = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lightNumber; i++)
        {
            Vector3 Pos;
            if (alongX)
            {
                 Pos = new Vector3(transform.position.x + i * intervalDis, transform.position.y, transform.position.z);
            }
            else
            {
                Pos = new Vector3(transform.position.x , transform.position.y, transform.position.z + i * intervalDis);
            }
           
            GameObject currentOBJ = Instantiate(LightPrefab, Pos, Quaternion.identity);
            currentOBJ.transform.parent = transform;
            MRlist.Add(currentOBJ.GetComponent<MeshRenderer>());
        }
        startFlashing();
        StartCoroutine(RGBFlash());

    }

    // Update is called once per frame
   

    void AssignColor(float EMStrength)
    {
        for (int i = 0; i < MRlist.Count; i++)
        {
            int yu = (i + 1) % 3;
            float r = 0;
            float g = 0;
            float b = 0;
            switch (yu)
            {
                case 0:
                    r = 1;
                    break;
                case 1:
                    g = 1;
                    break;
                case 2:
                    b = 1;
                    break;
            }
            //MRlist[i].material.color = new Color(r, g, b);
            MRlist[i].material.SetColor("_EmissionColor", new Color(r, g, b) * emssionStrength);
        }
    }

    private void Update()
    {
       // StartCoroutine(RGBFlash());
    }
    IEnumerator RGBFlash()
    {
        while (true)
        {
            if (Shiny)
            {
                for (int i = 0; i < MRlist.Count; i++)
                {
                    if (i == MRlist.Count - 1)
                    {
                        MRlist[i].material.SetColor("_EmissionColor", MRlist[0].material.GetColor("_EmissionColor"));
                    }
                    else
                    {
                        MRlist[i].material.SetColor("_EmissionColor", MRlist[i + 1].material.GetColor("_EmissionColor"));
                    }
                }

            }
           
        
        yield return new WaitForSeconds(0.25f);

        }
        
       
    }

    public void startFlashing()
    {
        Shiny = true;
        AssignColor(5);
        //StartCoroutine(RGBFlash());
        Invoke("stopFlashing", 5f);
    }
    void stopFlashing()
    {
        Shiny = false;
        //StopCoroutine(RGBFlash());
        print("stopped");
        AssignColor(-1f);
    }
}
