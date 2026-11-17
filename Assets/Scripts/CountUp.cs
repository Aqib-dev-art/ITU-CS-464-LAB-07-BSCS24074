using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// Counts a number up when the screen appears (Results score, Shop coins).
public class CountUp : MonoBehaviour
{
    public TMP_Text label;
    public int target = 12450;
    public float duration = 1.2f;
    public string format = "N0";

    void OnEnable() { StopAllCoroutines(); StartCoroutine(Run()); }

    IEnumerator Run()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float e = 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 3f); // EaseOutCubic
            label.text = Mathf.RoundToInt(target * e).ToString(format);
            yield return null;
        }
        label.text = target.ToString(format);
    }
}
