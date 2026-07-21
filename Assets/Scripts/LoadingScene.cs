using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private string gameScene = "SampleScene";
    [SerializeField] private VideoPlayer videoPlayer;

    IEnumerator Start()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(gameScene);
        loading.allowSceneActivation = false;

        videoPlayer.Play();

        while (loading.progress < 0.9f)
            yield return null;

        loading.allowSceneActivation = true;
    }
}