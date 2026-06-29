using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HerbariumController : MonoBehaviour
{
    [SerializeField] GameObject bookRoot;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Animator pawAnimator;
    [SerializeField] float showbookdelay=0.7f;
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
    private int pawIndex;
    private Coroutine quitRoutine;

    private bool[] pageState = new bool[5];

    void Start()
    {
        bookRoot.SetActive(false);

        leftButton.onClick.AddListener(PrevPage);
        rightButton.onClick.AddListener(NextPage);
    }
    
    void Update()
    {
        if (!bookRoot.activeInHierarchy) return;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            NextPage();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            PrevPage();
        }
    }

    public void TryQuitFromClick()
    {
        if (!isAnimating 
            && currentPage == pawIndex 
            && quitRoutine == null 
            && !pageState[pawIndex]
           )
        {
            TryQuitBook();
        }
    }

    public void OpenBook(int i)
    {
        pawIndex = i;
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing");
        StartCoroutine(ShowBook(showbookdelay,i));
    }
    
    IEnumerator ShowBook(float delay,int i)
    {
        yield return new WaitForSeconds(delay);
        bookRoot.SetActive(true);
        PlayPage(GetPageClip(currentPage));
        pawAnimator.gameObject.SetActive(true);
        SetPawState(i);
    }
    void TryQuitBook()
    {
        if (quitRoutine != null) return;
        pawAnimator.Play("paw" + pawIndex + "end");
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing2");
        quitRoutine = StartCoroutine(QuitBook(showbookdelay));
    }
    IEnumerator QuitBook(float delay)
    {
        isAnimating = true;
        SetButtons(false);
        yield return new WaitForSeconds(delay);
        videoPlayer.Stop();
        videoPlayer.clip = null;
        bookRoot.SetActive(false);
        pawAnimator.gameObject.SetActive(false);
        isAnimating = false;
        quitRoutine = null;
        pageState[pawIndex] = true;
        player.inputLocked = false;
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


/*
private VideoClip GetTurnClip(Func<int, TurnSet> getTurnSet, int i)
{
    var t = getTurnSet(i);
    bool current = pageState[i];
    bool next = (i + 1 < 5) && pageState[i + 1];

    return (!current && !next) ? t.normal :
           (current && !next) ? t.done :
           (!current && next) ? t.next : t.both;
}

VideoClip GetTurnClip(int i) => GetTurnClip(idx => turns[idx], i);
VideoClip GetRevTurnClip(int i) => GetTurnClip(idx => revturns[idx], i);






[System.Serializable]
public class TurnSet
{
    public VideoClip normal;
    public VideoClip done;
    public VideoClip next;
    public VideoClip both;
}

[SerializeField] TurnSet[] turns;
[SerializeField] TurnSet[] revturns;

----------------------------------------------------
VideoClip GetTurnClip(TurnSet[] set, int i)
{
    var t = set[i];
    bool current = pageState[i];
    bool next = (i + 1 < pageState.Length) && pageState[i + 1];

    if (!current && !next) return t.normal;
    if (current && !next)  return t.done;
    if (!current && next)  return t.next;
    return t.both;
}
----------------------------------------------------
yield return PlayClip(GetTurnClip(turns, i));      // вперед
yield return PlayClip(GetTurnClip(revturns, i));   // назад
----------------------------------------------------
public void NextPage()
{
    if (isAnimating || currentPage >= 4) return;
    StartCoroutine(TurnRoutine(1));
}

public void PrevPage()
{
    if (isAnimating || currentPage <= 0) return;
    StartCoroutine(TurnRoutine(-1));
}
----------------------------------------------------
IEnumerator TurnRoutine(int direction)
{
    isAnimating = true;
    SetButtons(false);

    int i = direction > 0 ? currentPage : currentPage - 1;
    var set = direction > 0 ? turns : revturns;

    yield return PlayClip(GetTurnClip(set, i));

    currentPage += direction;

    yield return PlayClip(GetPageClip(currentPage));

    SetButtons(true);
    isAnimating = false;
}
*/