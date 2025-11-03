using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtility : MonoBehaviour {
    public static bool isCapturing => Time.captureFramerate > 0;
    public static float unscaledDeltaTime => isCapturing ? Time.captureDeltaTime : Time.unscaledDeltaTime;
}
