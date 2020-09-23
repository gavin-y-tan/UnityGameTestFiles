using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;


public class MenuInteract : MonoBehaviour
{


    public int[,] map = 
      { { 0, 0, 1, 0, 1, 0, 0 }, 
        { 0, 1, 0, 1, 0, 1, 0 }, 
        { 1, 0, 1, 0, 1, 0, 1 }, 
        { 0, 1, 0, 1, 0, 1, 0 }, 
        { 0, 0, 1, 0, 1, 0, 0 } };

    public int currentRow = 2;
    public int currentCol = 0;

     

    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(map);
    }
}
