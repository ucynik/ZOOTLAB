using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTime : MonoBehaviour
{   
    [Range(1f, 10f)]
    public float timeScale = 1f;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
    }
}
