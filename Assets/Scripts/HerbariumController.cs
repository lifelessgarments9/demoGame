using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.InputSystem;
using System.Linq;

public class HerbariumController : MonoBehaviour
{
    [SerializeField] GameObject bookRoot;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoPlayer bookVideoPlayer;
    [SerializeField] Animator pawAnimator;
    [SerializeField] Animator darkAnimator;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float animation1Chance = 0.4f; 
    [SerializeField] private float animation2Chance = 0.7f;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private VideoClip failClip1;
    [SerializeField] private VideoClip failClip2;
    [SerializeField] private VideoClip endingClip;
    [SerializeField] private GameObject endingImage;
    [SerializeField] GameObject vignetteAnimator;
    [SerializeField] private PlayerVisual playerVisual;
    public bool darkAnimationFinished;
    private bool failAnimation1Played = false;
    private bool failAnimation2Played = false;
    
    private bool failCooldown;
    [SerializeField] private float failCooldownTime = 10f;

    [SerializeField] VideoClip[] pageClips;     // page0, page1...
    [SerializeField] VideoClip[] donePageClips; // donepage0...
    [System.Serializable]
    public class TurnSet
    {
        public VideoClip normal;    // turn{i}a
        public VideoClip done;      // turn{i}{i}
        public VideoClip next;      // turn{i}{i+1}
        public VideoClip both;      // turn{i}{i}{i+1}
    }
    [SerializeField] TurnSet[] turns; // 4
    
    [System.Serializable]
    public class ReverseTurnSet
    {
        public VideoClip normal;    // revturn{i}a
        public VideoClip done;      // revturn{i}{i}
        public VideoClip next;      // revturn{i}{i+1}
        public VideoClip both;      // revturn{i}{i}{i+1}
    }
    [SerializeField] ReverseTurnSet[] revturns; // 4
    
    private int currentPage = 2;
    private bool isAnimating;
    private int pawIndex; //текущее растение

    public bool[] pageState = new bool[5];
    
    private bool viewMode;

    void Start()
    {
        videoPlayer.gameObject.SetActive(false);
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        
        bookRoot.SetActive(false);
        leftButton.onClick.AddListener(PrevPage);
        rightButton.onClick.AddListener(NextPage);
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.isLooping = false;
        
        InputManager.Instance.BookPressed += OnBookPressed;
        InputManager.Instance.LeftPressed += NextPage;
        InputManager.Instance.RightPressed += PrevPage;
    }
    
    void OnBookPressed()
    {
        if (isAnimating)
            return;

        if (!bookRoot.activeSelf)
        {
            StartCoroutine(OpenViewer());
        }
        else if (viewMode)
        {
            StartCoroutine(CloseViewer());
        }
    }
    
    IEnumerator OpenViewer()
    {
        isAnimating = true;
        viewMode = true;
        player.inputLocked = true;
        darkAnimator.gameObject.SetActive(true);
        darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing");
        
        yield return new WaitUntil(() => darkAnimationFinished);

        vignetteAnimator.SetActive(false);
        bookRoot.SetActive(true);
        pawAnimator.gameObject.SetActive(false);
        SetButtons(true);
        currentPage = 2;
        PlayPage(GetPageClip(currentPage));
        isAnimating = false;
    }
    
    IEnumerator CloseViewer()
    {
        isAnimating = true;
        SetButtons(false);
        darkAnimator.gameObject.SetActive(true);
        darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing2");
        
        yield return new WaitUntil(() => darkAnimationFinished);
        
        vignetteAnimator.SetActive(true);
        bookVideoPlayer.Stop();
        bookVideoPlayer.clip = null;
        bookRoot.SetActive(false);
        player.inputLocked = false;
        viewMode = false;
        isAnimating = false;
        UpdatePlayerAnimationStage();
    }
    
    public void OpenBook(int i)
    {
        if (isAnimating) return;
        pawIndex = i;
        StartCoroutine(OpenBookCoroutine());
    }

    IEnumerator OpenBookCoroutine()
    {
        isAnimating = true;
        darkAnimator.gameObject.SetActive(true);
        darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing");
        
        yield return new WaitUntil(() => darkAnimationFinished);
        
        vignetteAnimator.SetActive(false);
        bookRoot.SetActive(true);
        SetButtons(true);
        currentPage = 2;
        PlayPage(GetPageClip(currentPage));
        pawAnimator.gameObject.SetActive(true);
        SetPawState();
        isAnimating = false;
    }
    
    public void TryQuitFromClick()
    {
        if (viewMode) return;
        if (!isAnimating 
            && currentPage == pawIndex 
            && !pageState[pawIndex]
           )
        {
            StartCoroutine(QuitBook());
        }
        else
        {
            if (failCooldown)
                return;
            StartCoroutine(FailCooldown());
            audioSource.PlayOneShot(failSound);
            if (!failAnimation1Played && Random.value < animation1Chance)
            {
                PlayVideo(failClip1);
                failAnimation1Played = true;
            }
            else if (failAnimation1Played && !failAnimation2Played && Random.value < animation2Chance)
            {
                PlayVideo(failClip2);
                failAnimation2Played = true;
            }
        }
    }
    IEnumerator QuitBook()
    {
        isAnimating = true;
        SetButtons(false);
        pawAnimator.Play("paw" + pawIndex + "end");
        darkAnimator.gameObject.SetActive(true);
        darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing2");
        
        yield return new WaitUntil(() => darkAnimationFinished);
        
        vignetteAnimator.SetActive(true);
        bookVideoPlayer.Stop();
        bookVideoPlayer.clip = null;
        
        bookRoot.SetActive(false);
        pawAnimator.gameObject.SetActive(false);
        pageState[pawIndex] = true;
        isAnimating = false;
        UpdatePlayerAnimationStage();

        bool allHerbsCompleted = pageState.All(x => x);
        if (allHerbsCompleted)
        {
            PlayVideo(endingClip);
            yield break;
        }

        player.inputLocked = false;
    }
    public void OnDark()
    {
        darkAnimationFinished = true;
    }
    IEnumerator FailCooldown()
    {
        failCooldown = true;
        yield return new WaitForSeconds(failCooldownTime);
        failCooldown = false;
    }
    
    void PlayVideo(VideoClip clip)
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        if (vp.clip == endingClip) endingImage.SetActive(true);

        vp.Stop();
        vp.clip = null;
        vp.gameObject.SetActive(false);
    }
    
    void SetPawState()
    {
        pawAnimator.Play("paw" + pawIndex + "idle");
    }

    VideoClip GetPageClip(int i)
    {
        return pageState[i] ? donePageClips[i] : pageClips[i];
    }
    
    VideoClip GetTurnClip(int i)
    {
        var t = turns[i];
        bool current = pageState[i];
        bool next = (i + 1 < 5) && pageState[i + 1];

        if (!current && !next)
            return t.normal;   // turn{i}
        if (current && !next)
            return t.done;     // turn{i}{i}
        if (!current && next)
            return t.next;     // turn{i}{i+1}
        return t.both;         // turn{i}{i}{i+1}
    }
    VideoClip GetRevTurnClip(int i)
    {
        var t = revturns[i];
        bool current = pageState[i];
        bool next = (i + 1 < 5) && pageState[i + 1];

        if (!current && !next)
            return t.normal;   // обратный turn{i}a
        if (current && !next)
            return t.done;     // обратный turn{i}{i}
        if (!current && next)
            return t.next;     // обратный turn{i}{i+1}
        return t.both;         // обратный turn{i}{i}{i+1}
    }

    IEnumerator PlayTurnClip(VideoClip clip)
    {
        if (clip == null) yield break;

        bookVideoPlayer.isLooping = false;
        
        bookVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        bookVideoPlayer.EnableAudioTrack(0, true);
        bookVideoPlayer.SetTargetAudioSource(0, audioSource);
        
        bookVideoPlayer.Stop();
        bookVideoPlayer.clip = clip;
        bookVideoPlayer.Prepare();

        yield return new WaitUntil(() => bookVideoPlayer.isPrepared);

        bookVideoPlayer.Play();

        yield return new WaitUntil(() => bookVideoPlayer.isPlaying);
        yield return new WaitUntil(() => !bookVideoPlayer.isPlaying);
    }

    void PlayPage(VideoClip clip)
    {
        
        bookVideoPlayer.isLooping = true;

        bookVideoPlayer.Stop();
        bookVideoPlayer.clip = clip;
        bookVideoPlayer.Play();
    }


    public void NextPage()
    {
        if (!bookRoot.activeSelf || isAnimating) return;
        if (currentPage >= 4) return;

        StartCoroutine(NextRoutine());
    }

    IEnumerator NextRoutine()
    {
        isAnimating = true;
        SetButtons(false);

        int i = currentPage;

        yield return PlayTurnClip(GetTurnClip(i));

        currentPage++;

        // сразу включаем кнопки
        SetButtons(true);
        isAnimating = false;

        // запускаем loop без ожидания
        PlayPage(GetPageClip(currentPage));
    }

    public void PrevPage()
    {
        if (!bookRoot.activeSelf || isAnimating) return;
        if (currentPage <= 0) return;

        StartCoroutine(PrevRoutine());
    }

    IEnumerator PrevRoutine()
    {
        isAnimating = true;
        SetButtons(false);

        int i = currentPage - 1;

        yield return PlayTurnClip(GetRevTurnClip(i));

        currentPage--;

        SetButtons(true);
        isAnimating = false;

        PlayPage(GetPageClip(currentPage));
    }

    void SetButtons(bool state)
    {
        leftButton.interactable = state;
        rightButton.interactable = state;
    }
    
    public void UpdatePlayerAnimationStage()
    {
        if (VideoPlayerWatchdog.stage2)
        {
            playerVisual.SetSecondAnimationSet();

            VideoPlayerWatchdog.stage2 = false;
            VideoPlayerWatchdog.stage3 = true;

            return;
        }
        if (VideoPlayerWatchdog.stage3)
        {
            if (Random.value < 0.2f)
                playerVisual.SetSecondAnimationSet();
            else
                playerVisual.SetFirstAnimationSet();
        }
    }
}