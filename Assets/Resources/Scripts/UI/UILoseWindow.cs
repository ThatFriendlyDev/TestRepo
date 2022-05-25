using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoseWindow : MonoBehaviour
{
    public static UILoseWindow instance;
    public GameObject panel;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        this.panel.SetActive(false);
    }

    public void Open()
    {
        this.panel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
