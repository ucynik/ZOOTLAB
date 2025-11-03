using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEditor;
using System.Security.Cryptography;

[ExecuteInEditMode]

public class PathLines : MonoBehaviour
{
    List<GameObject> checkpoints = new List<GameObject>();
    public float thickness;

    void OnDrawGizmos() {

        checkpoints.Clear(); foreach (Transform child in this.transform) checkpoints.Add(child.gameObject);

        float lineHue = 1f / checkpoints.Count * 2;

        Color lineColor = Color.HSVToRGB(lineHue, 1f, 1f);

        for (int i = 0; i < checkpoints.Count - 1; i++) {
            Vector3 p1 = checkpoints[i].transform.position;
            Vector3 p2 = checkpoints[i + 1].transform.position;
            Gizmos.DrawLine(p1, p2);
            Gizmos.color = lineColor;}
    }
}
