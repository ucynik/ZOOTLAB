using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Flip : MonoBehaviour
{
    private Vector3 prev;
    private Vector3 scale;
    private float flipTime = 0.12f;
    private float elapasedTime;
    private Transform headIcon;
    [HideInInspector]
    public bool pause = false;

    private bool isFlip = false;

    private float flipTo;
    
    // Start is called before the first frame update
    void Start()
    {
        prev = transform.position;
        scale = transform.localScale;

        if (this.transform.Find("Icon") != false) {
            headIcon = this.transform.Find("Icon");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pause) return;
        Vector3 currentPosition = transform.position;

        if (isFlip) {
            elapasedTime += Time.deltaTime;
            float temp = elapasedTime / flipTime;
            float newScale = Mathf.Lerp(scale.x, -scale.x, temp);
            transform.localScale = new Vector3(newScale, scale.y, scale.z);

            if (temp >= 1f) {
                isFlip = false;
                scale.x = flipTo;
            }

            if (headIcon != null) {
                headIcon.localScale = new Vector3(0.15f * -1, -1, 0.15f);
            }

        } else {
            if ((currentPosition.x < prev.x && transform.localScale.x > 0) || (currentPosition.x > prev.x && transform.localScale.x < 0)) {
                isFlip = true;
                elapasedTime = 0f;
                flipTo = -scale.x;
            }
        }
        prev = currentPosition;
    }
}