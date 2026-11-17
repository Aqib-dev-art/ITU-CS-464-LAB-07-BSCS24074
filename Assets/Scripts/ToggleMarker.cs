using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Shows a "SELECTED" tag (non-colour cue) or ON/OFF label when a Toggle changes.
public class ToggleMarker : MonoBehaviour
{
    public Toggle toggle;
    public GameObject onMarker;   // visible when ON
    public GameObject offMarker;  // visible when OFF (optional)
    bool ready;

    void Awake()
    {
        if (!toggle) toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(Apply);
    }

    void OnEnable()
    {
        ready = false;
        Apply(toggle.isOn);
        ready = true;
    }

    void Apply(bool on)
    {
        if (onMarker)
        {
            bool was = onMarker.activeSelf;
            onMarker.SetActive(on);
            if (on && !was && ready && isActiveAndEnabled)
            {
                StopAllCoroutines();
                StartCoroutine(Pop(onMarker.transform));
            }
        }
        if (offMarker) offMarker.SetActive(!on);
    }

    // Tag pops in with EaseOutBack so the selection change is noticed.
    IEnumerator Pop(Transform t)
    {
        float time = 0;
        while (time < 1f)
        {
            time += Time.unscaledDeltaTime / 0.25f;
            float x = Mathf.Clamp01(time);
            const float c1 = 1.70158f, c3 = c1 + 1f;
            float e = 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
            t.localScale = Vector3.one * Mathf.LerpUnclamped(0.6f, 1f, e);
            yield return null;
        }
        t.localScale = Vector3.one;
    }
}
