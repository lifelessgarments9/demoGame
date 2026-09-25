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
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
        yield return null;
        AsyncOperation loading = SceneManager.LoadSceneAsync(gameScene);
        loading.allowSceneActivation = false;

        while (loading.progress < 0.9f)
            yield return null;

        loading.allowSceneActivation = true;
        
    }
}