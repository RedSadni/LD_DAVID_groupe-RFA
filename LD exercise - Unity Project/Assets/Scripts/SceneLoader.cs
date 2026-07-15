using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] scenesToLoad;

    public void OnLoadButtonPressed()
    {
        StartCoroutine(LoadScenesAsync());
    }

    private IEnumerator LoadScenesAsync()
    {
        if (scenesToLoad == null || scenesToLoad.Length == 0)
        {
            Debug.LogWarning("SceneLoader: No scenes assigned to load.");
            yield break;
        }

        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            string sceneName = scenesToLoad[i];

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning($"SceneLoader: Scene entry at index {i} is empty, skipping.");
                continue;
            }

            // First scene loads with Single mode to clear the current scene(s),
            // subsequent scenes load additively so they stack on top.
            LoadSceneMode mode = (i == 0) ? LoadSceneMode.Single : LoadSceneMode.Additive;

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);

            if (asyncOp == null)
            {
                Debug.LogError($"SceneLoader: Failed to start loading scene '{sceneName}'. " +
                                "Make sure it's added to Build Settings.");
                continue;
            }

            // Wait until this scene finishes loading before moving to the next
            while (!asyncOp.isDone)
            {
                yield return null;
            }
        }

        Debug.Log("SceneLoader: All scenes loaded.");
    }
}