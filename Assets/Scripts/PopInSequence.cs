using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// Stars / reward icons pop in one by one when Results opens (EaseOutBack).
public class PopInSequence : MonoBehaviour
{
    public Transform[] items;
    public float delay = 0.15f, time = 0.35f;

    void OnEnable() { StopAllCoroutines(); StartCoroutine(Run()); }

    IEnumerator Run()
    {
        foreach (var i in items) i.localScale = Vector3.zero;
        yield return new WaitForSecondsRealtime(0.4f);
        foreach (var i in items)
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / time;
                float x = Mathf.Clamp01(t);
                const float c1 = 1.70158f, c3 = c1 + 1f;
                float e = 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
                i.localScale = Vector3.one * e;
                yield return null;
            }
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
