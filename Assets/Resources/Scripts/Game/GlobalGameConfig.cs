using UnityEngine.SceneManagement;

public static class GlobalGameConfig
{
    public static int GetTotalLevelCount()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int levelCount = 0;

        for (int i = 0; i < sceneCount; i++)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex(i);

            if (sceneName.Contains("Level"))
            {
                levelCount++;
            }
        }

        return levelCount;
    }
}
