using UnityEngine;
using UnityEngine.SceneManagement;

public class UIVictoryWindow : MonoBehaviour
{
    public static UIVictoryWindow instance;
    public GameObject panel;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void Open()
    {
        this.panel.SetActive(true);
    }

    public void Continue()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
