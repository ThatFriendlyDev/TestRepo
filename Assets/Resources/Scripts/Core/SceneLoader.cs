using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        this.LoadLevel(Profile.instance.level, true);
    }

    public void LoadLevel(int id, bool async = false)
    {
        Profile.instance.levelCache.id = id;

        if (async)
        {
            this.StartCoroutine(this.LoadLevelAsync(id));
            return;
        }

        SceneManager.LoadScene("Level " + id, LoadSceneMode.Single);
    }

    private IEnumerator LoadLevelAsync(int id)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level " + id);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
