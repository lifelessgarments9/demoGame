using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoPlayerWatchdog : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float checkInterval = 120f;
    [SerializeField] private int stage2Minutes = 8;
    [SerializeField] private int entityMinutes = 3;
    [SerializeField] private int stage1Minutes = 1;
    [SerializeField] GameObject hideObjects;
    [SerializeField] GameObject entityObjects;
	private int minutes = 0;
    public static bool stage2; 
    public static bool stage3;

    private float timer;

    void Start()
    {
        if (GameFlow.firstGame)
        {
            stage1Minutes= Random.Range(2, 3);
            entityMinutes= Random.Range(3, 5);
            stage2Minutes= Random.Range(6, 12);
            //4-6 6-10 12-24
        }
        else
        {
            stage1Minutes= 1;
            entityMinutes= 1;
            stage2Minutes= 1;
        }
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer < checkInterval)
            return;

        timer = 0f;
		minutes+=1;
        
        if(minutes==stage1Minutes) hideObjects.SetActive(true);
        if(minutes==entityMinutes) entityObjects.SetActive(true);
		if(minutes==stage2Minutes) stage2=true;
        
        StartCoroutine(RestartVideoRoutine());
    }

    IEnumerator RestartVideoRoutine()
    {
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        yield return null; 
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
    }

}