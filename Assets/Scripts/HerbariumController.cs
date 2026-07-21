using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.InputSystem;

public class HerbariumController : MonoBehaviour
{
    [SerializeField] GameObject bookRoot;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Animator pawAnimator;
    [SerializeField] float showbookdelay=0.7f;
    [SerializeField] float closebookdelay=0.7f;
    [SerializeField] Animator darkAnimator;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource audioSource;

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
    private bool isAnimating = false;
    private int pawIndex; //текущее растение

    private bool[] pageState = new bool[5];
    
    private bool viewMode;

    void Start()
    {
        bookRoot.SetActive(false);

        leftButton.onClick.AddListener(PrevPage);
        rightButton.onClick.AddListener(NextPage);
    }
    
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame && !isAnimating)
        {
            if (!bookRoot.activeSelf)
                StartCoroutine(OpenViewer());
            else if (viewMode)
                StartCoroutine(CloseViewer());
            return;
        }

        if (!bookRoot.activeSelf) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            NextPage();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            PrevPage();
    }
    
    IEnumerator OpenViewer()
    {
        isAnimating = true;
        viewMode = true;
        player.inputLocked = true;
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing");

        yield return new WaitForSeconds(showbookdelay);

        bookRoot.SetActive(true);
        SetButtons(true); 
        PlayPage(GetPageClip(currentPage));
        isAnimating = false;
    }
    
    IEnumerator CloseViewer()
    {
        isAnimating = true;
        SetButtons(false);
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing2");

        yield return new WaitForSeconds(closebookdelay);

        videoPlayer.Stop();
        videoPlayer.clip = null;
        bookRoot.SetActive(false);
        player.inputLocked = false;
        viewMode = false;
        isAnimating = false;
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
        darkAnimator.SetTrigger("Closing");

        yield return new WaitForSeconds(showbookdelay);

        bookRoot.SetActive(true);
        SetButtons(true);
        PlayPage(GetPageClip(currentPage));
        pawAnimator.gameObject.SetActive(true);
        SetPawState(pawIndex);
        isAnimating = false;
    }
    
    public void TryQuitFromClick()
    {
        if (!isAnimating 
            && currentPage == pawIndex 
            && !pageState[pawIndex]
           )
        {
            StartCoroutine(QuitBook());
        }
    }
    IEnumerator QuitBook()
    {
        isAnimating = true;
        SetButtons(false);
        pawAnimator.Play("paw" + pawIndex + "end");
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing2");
        yield return new WaitForSeconds(closebookdelay);
        videoPlayer.Stop();
        videoPlayer.clip = null;
        bookRoot.SetActive(false);
        pawAnimator.gameObject.SetActive(false);
        pageState[pawIndex] = true;
        player.inputLocked = false;
        isAnimating = false;
    }
    
    void SetPawState(int i)
    {
        pawAnimator.Play("paw" + i + "idle");
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

        videoPlayer.isLooping = false;
        
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        
        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Prepare();

        yield return new WaitUntil(() => videoPlayer.isPrepared);

        videoPlayer.Play();

        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
    }

    void PlayPage(VideoClip clip)
    {
        
        videoPlayer.isLooping = true;

        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }


    public void NextPage()
    {
        if (isAnimating) return;
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
        if (isAnimating) return;
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
}