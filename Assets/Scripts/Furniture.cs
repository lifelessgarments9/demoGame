using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Furniture : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject messageRu;
    [SerializeField] GameObject messageEng;
    [SerializeField] GameObject keyImg;
    [SerializeField] private MindTrigger mindTrigger;
    [SerializeField] private float soundCooldown = 7.3f;
    [SerializeField] GameObject teapot;
    [SerializeField] private Animator teapotAnimator;
    [SerializeField] GameObject furnitureClosed;
    [SerializeField] GameObject furnitureOpened;
    [SerializeField] private Animator flyAnimator;
    [SerializeField] private AudioSource keyAudioSource;
    [SerializeField] private List<AudioClip> closedSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> keySounds = new List<AudioClip>();
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip seatSound;
    [SerializeField] private AudioClip stukSound;
    [SerializeField] private List<AudioClip> tableSounds = new List<AudioClip>();
    [SerializeField] private ExtraBook extraBook;
    [SerializeField] private GameObject furnBook;
    private AudioSource audioSource;
    private bool soundOnCooldown;
    private bool triggered;
    private bool opened;
    public static bool gotKey;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        furnitureOpened.SetActive(false);
        teapotAnimator.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (opened)
        {
            opened = false;
            extraBook.OpenBook(furnBook);
            return;
        }
        if(soundOnCooldown) return;
        StartCoroutine(SoundCooldown());
        if(seatSound!=null)audioSource.PlayOneShot(seatSound);
        if(stukSound!=null)audioSource.PlayOneShot(stukSound);
        if(tableSounds.Count > 0)audioSource.PlayOneShot(tableSounds[Random.Range(0, tableSounds.Count)]);
        if(triggered) return;
        if(teapotAnimator!=null)
        {
            teapot.SetActive(false);
            teapotAnimator.gameObject.SetActive(true);
            teapotAnimator.Play("teapot");
            keyImg.SetActive(true);
            keyAudioSource.PlayOneShot(keySounds[Random.Range(0, keySounds.Count)]);
            gotKey=true;
            triggered = true;
        }
        if(flyAnimator!=null&&!gotKey)
        {
            if(GameFlow.IsRussian)StartCoroutine(mindTrigger.ShowThought(messageRu, 4f, 1.5f));
            if(!GameFlow.IsRussian)StartCoroutine(mindTrigger.ShowThought(messageEng, 4f, 1.5f));
            audioSource.PlayOneShot(closedSounds[Random.Range(0, closedSounds.Count)]);
        }
        if(flyAnimator!=null&&gotKey)
        {
            furnitureClosed.SetActive(false);
            furnitureOpened.SetActive(true);
            flyAnimator.Play("fly");
            audioSource.PlayOneShot(openSound);
            keyImg.SetActive(false);
            triggered = true;
            opened = true;
        }
    }
    private IEnumerator SoundCooldown()
    {
        soundOnCooldown = true;
        yield return new WaitForSeconds(soundCooldown);
        soundOnCooldown = false;
    }
}
