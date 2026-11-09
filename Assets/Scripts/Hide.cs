using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hide : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject vignette0;
    [SerializeField] private Transform vignette;
    [SerializeField] private CanvasGroup darkness;
    [SerializeField] private Slider knob;
    [SerializeField] private GameObject hint;
    [SerializeField] private MindTrigger mindTrigger;

    [Header("Settings")]
    [SerializeField] private float lowScale = 0.5f;
    [SerializeField] private float returnSpeed = 1f;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> lampSounds = new List<AudioClip>();
    [SerializeField] private float soundCooldown = 10f;
    
    [SerializeField] private float scaryChance = 0.3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private Teleportation teleportation;
    private bool scaryTriggered;

    private AudioSource audioSource;
    private Vector3 startScale;
    private Coroutine returnRoutine;
    private bool soundOnCooldown;
    private bool playerInside;
    public static bool hintShown = false; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startScale = vignette.localScale;

        darkness.alpha = 0f;
        darkness.gameObject.SetActive(false);

        knob.value = knob.minValue;
        knob.gameObject.SetActive(false);

        knob.onValueChanged.AddListener(OnKnobChanged);
        
        InputManager.Instance.InteractPressed += OnExitInput;
        InputManager.Instance.BookPressed += OnExitInput;
    }

    private void OnDestroy()
    {
        knob.onValueChanged.RemoveListener(OnKnobChanged);
        InputManager.Instance.InteractPressed -= OnExitInput;
        InputManager.Instance.BookPressed -= OnExitInput;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        playerInside = true;

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }

        vignette0.SetActive(false);
        vignette.gameObject.SetActive(true);
        darkness.gameObject.SetActive(true);
        knob.gameObject.SetActive(true);
        knob.interactable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        playerInside = false;

        knob.interactable = false;

        if (returnRoutine != null)
            StopCoroutine(returnRoutine);

        returnRoutine = StartCoroutine(ReturnKnob());
    }

    private void OnKnobChanged(float value)
    {
        ApplyValue(value);

        if (!soundOnCooldown && lampSounds.Count > 0)
        {
            audioSource.PlayOneShot(
                lampSounds[Random.Range(0, lampSounds.Count)]
            );

            StartCoroutine(SoundCooldown());
        }

        if (!hintShown&&value >= knob.maxValue * 0.90f)
        {
            hintShown = true;
            StartCoroutine(mindTrigger.ShowThought(hint, 4f, 1.5f));
        }
        if (!scaryTriggered&&value >= knob.maxValue * 0.90f)
        {
            scaryTriggered = true; 
            if (Random.value <= scaryChance)
            {
                teleportation.ExternTeleport(rb,targets[Random.Range(0, targets.Count)]);
            }
        }
    }

    private IEnumerator SoundCooldown()
    {
        soundOnCooldown = true;
        yield return new WaitForSeconds(soundCooldown);
        soundOnCooldown = false;
    }

    private void ApplyValue(float value)
    {
        float normalizedValue = Mathf.InverseLerp(
            knob.minValue,
            knob.maxValue * 0.89f,
            value
        );

        float scaleMultiplier = Mathf.Lerp(
            1f,
            lowScale,
            normalizedValue
        );

        vignette.localScale = startScale * scaleMultiplier;
        darkness.alpha = normalizedValue;
    }

    private IEnumerator ReturnKnob()
    {
        while (knob.value > knob.minValue)
        {
            knob.value = Mathf.MoveTowards(
                knob.value,
                knob.minValue,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }

        knob.value = knob.minValue;

        darkness.gameObject.SetActive(false);
        knob.gameObject.SetActive(false);

        vignette.gameObject.SetActive(false);
        vignette0.SetActive(true);

        returnRoutine = null;
    }
    private void OnExitInput()
    {
        if (!playerInside)
            return;
        
        if (!knob.gameObject.activeSelf)
            return;

        if (returnRoutine != null)
            return;

        knob.interactable = false;
        returnRoutine = StartCoroutine(ReturnKnob());
    }
}