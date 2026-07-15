using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] scenesToLoad;

    public void OnLoadButtonPressed()
    {
        ScenesManager scenes_manager = FindAnyObjectByType<ScenesManager>();
        scenes_manager.LoadScenes(scenesToLoad);
    }

}