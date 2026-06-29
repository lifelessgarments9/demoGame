using UnityEngine;
public class TimeEvents : MonoBehaviour
{
    [SerializeField] private GameFlow gameFlow;

    private float timer;
    private AudioSource speech;
    private bool speechPlayed;

    void Awake()
    {
        speech = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameFlow.CurState != GameFlow.GameState.Game)
            return;

        timer += Time.deltaTime;
        if (!speechPlayed && timer >= 5f)
        {
            speechPlayed = true;
            speech.Play();
        }
        
        if (timer >= 300f)
        {
            timer = 0f;

            // событие
        }
    }
}