using System.Collections;
using System.IO;
using UnityEngine;

/// Sits on the UIManager object.
/// - Tick "Capture On Play": walks all 10 screens, saves colour + greyscale PNGs to <project>/Screenshots
/// - Tick "Tour On Play": walks all screens hands-free so you can screen-record the demo video
/// Use ONE of them at a time. Untick both when finished.
public class AutoCapture : MonoBehaviour
{
    public ScreenManager manager;
    public bool captureOnPlay = false;
    public bool tourOnPlay = false;
    public float waitPerScreen = 3f;

    static readonly string[] captureIds =
        { "Title", "MainMenu", "Mode", "Level", "Loadout", "Shop", "Settings", "Pause", "Results", "Credits" };

    // "<Back" = Back(), "^X" = GoToRoot(X), otherwise GoTo(X)
    static readonly string[] tourSteps =
        { "MainMenu", "Mode", "Level", "Loadout", "Gameplay", "Pause", "<Back", "Results",
          "^MainMenu", "Shop", "<Back", "Settings", "<Back", "Credits", "<Back" };

    void Start()
    {
        if (!manager) manager = GetComponent<ScreenManager>();
        if (captureOnPlay) StartCoroutine(Capture());
        else if (tourOnPlay) StartCoroutine(Tour());
    }

    IEnumerator Capture()
    {
        string dir = Path.GetFullPath(Path.Combine(Application.dataPath, "../Screenshots"));
        Directory.CreateDirectory(dir);
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < captureIds.Length; i++)
        {
            string id = captureIds[i];
            if (i > 0) manager.GoTo(id);
            yield return new WaitForSecondsRealtime(waitPerScreen + (id == "Results" ? 1.5f : 0f));
            yield return new WaitForEndOfFrame();

            Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
            string n = (i + 1).ToString("00") + "-" + id.ToLower();
            File.WriteAllBytes(Path.Combine(dir, n + "-colour.png"), tex.EncodeToPNG());

            // true greyscale copy (luminance)
            Color32[] px = tex.GetPixels32();
            for (int p = 0; p < px.Length; p++)
            {
                byte g = (byte)(0.299f * px[p].r + 0.587f * px[p].g + 0.114f * px[p].b);
                px[p] = new Color32(g, g, g, 255);
            }
            Texture2D gt = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
            gt.SetPixels32(px);
            gt.Apply();
            File.WriteAllBytes(Path.Combine(dir, n + "-greyscale.png"), gt.EncodeToPNG());

            Destroy(tex);
            Destroy(gt);
        }
        Debug.Log("AutoCapture done. Files saved in: " + dir);
    }

    IEnumerator Tour()
    {
        yield return new WaitForSecondsRealtime(3f);
        foreach (string step in tourSteps)
        {
            if (step == "<Back") manager.Back();
            else if (step.StartsWith("^")) manager.GoToRoot(step.Substring(1));
            else manager.GoTo(step);
            yield return new WaitForSecondsRealtime(step == "Results" ? waitPerScreen + 1.5f : waitPerScreen);
        }
        Debug.Log("Tour finished. Stop your recording.");
    }
}
