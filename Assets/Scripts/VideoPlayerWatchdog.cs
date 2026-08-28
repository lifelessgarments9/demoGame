using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerWatchdog : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float checkInterval = 60f;
    [SerializeField] private int stage2Minutes = 15;
	private int minutes = 0;
	public static bool stage2;
    public static bool stage3;

    private float timer;

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer < checkInterval)
            return;

        timer = 0f;
		minutes+=1;
		if(minutes==stage2Minutes) stage2=true;

        videoPlayer.Play();
    }

}