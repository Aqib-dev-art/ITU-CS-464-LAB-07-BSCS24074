using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// Gentle looping pulse: use on "TAP TO START" so the title screen tells you what to do.
public class Pulse : MonoBehaviour
{
    public float amount = 0.06f, speed = 2f;
    void Update()
    {
        float s = 1f + Mathf.Sin(Time.unscaledTime * speed) * amount; // sine = ease in-out loop
        transform.localScale = Vector3.one * s;
    }
}
