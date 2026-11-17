using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// Optional: pause the game on the Pause screen. Add to the Pause panel.
public class PauseTimeScale : MonoBehaviour
{
    void OnEnable()  { Time.timeScale = 0f; }
    void OnDisable() { Time.timeScale = 1f; }
}
