using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager singleton;

    private void Awake()
    {
        this.InvokeRepeating(nameof(this.Run), 1f, 1f);
    }

    private void OnDestroy()
    {
        this.CancelInvoke();
    }

    private void Run()
    {
        Profile.instance.Save();
    }
}
