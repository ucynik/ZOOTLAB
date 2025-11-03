using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCanvasRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update() {
        if (transform.parent == null) return;
        Vector3 parentHPScale = gameObject.transform.localScale;
        parentHPScale.x = transform.parent.transform.localScale.x;
        if (Mathf.Abs(parentHPScale.x) < 0.001f) {
            parentHPScale.x = 0.01f;
        }
        transform.localScale = new Vector3(1f / parentHPScale.x, transform.localScale.y, transform.localScale.z);
    }
    
}
