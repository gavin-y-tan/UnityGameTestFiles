using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{

    public int max=100;
    public float current=0;
    public Image mask;
    
        


    void Start()
    {
        
    }


    void Update()
    {
        
        GetCurrentFill(); 
    }

    void GetCurrentFill()
    {
        float fill = current / (float)max;
        mask.fillAmount = fill;
    }

}
