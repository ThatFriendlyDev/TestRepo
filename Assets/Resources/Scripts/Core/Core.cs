using UnityEngine;

public class Core : MonoBehaviour
{
    public static Core singleton;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
