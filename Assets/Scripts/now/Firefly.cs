using UnityEngine;

public class Firefly : MonoBehaviour
{
    public enum State
    {
        FlyTo,
        Orbit
    }

    [Header("Movement")]
    [SerializeField] private float flySpeed = 2f;
    [SerializeField] private float orbitDistance = 1.5f;

    [Header("Orbit")]
    [SerializeField] private float ellipseX = 1.5f;
    [SerializeField] private float ellipseY = 0.7f;

    [Header("Random")]
    [SerializeField] private float moveNoise = 0.3f;
    [SerializeField] private float orbitNoise = 0.2f;

    private Transform player;
    private State state;
    private bool active;
    private bool returningHome;
    private Vector3 homePosition;

    private float angle;
    private float seed;
    private float speedMultiplier;
    private float orbitSpeed;
    private float orbitDirection;
    private float orbitRotation;
    private float orbitX;
    private float orbitY;
    private Quaternion orbitRotationQuat;

    void Awake()
    {
        homePosition = transform.position;
    }

    void Update()
    {
        if (!active && !returningHome)
            return;

        if (state == State.FlyTo)
            UpdateFlyTo();
        else
            UpdateOrbit();
    }

    public void Activate(Transform target)
    {
        seed = Random.Range(0f, 1000f);
        speedMultiplier = Random.Range(0.8f, 1.2f);
        angle = Random.Range(0f, Mathf.PI * 2f);
        orbitSpeed = Random.Range(0.6f, 1.6f);
        orbitRotation = Random.Range(0f,360f);
        orbitDirection = Random.value > 0.5f ? 1f : -1f;
        orbitX = ellipseX * Random.Range(0.7f, 1.4f);
        orbitY = ellipseY * Random.Range(0.7f, 1.4f);
        orbitRotationQuat = Quaternion.Euler(0,0,orbitRotation);
        
        player = target;
        active = true;
        returningHome = false;
        state = State.FlyTo;
    }

    public void Deactivate()
    {
        active = false;
        returningHome = true;
        state = State.FlyTo;
    }

    void UpdateFlyTo()
    {
        Vector3 targetPos = returningHome ? homePosition : player.position;

        Vector3 dir = (targetPos - transform.position).normalized;

        dir += new Vector3(
            Mathf.PerlinNoise(Time.time * 2f, seed) - 0.5f,
            Mathf.PerlinNoise(seed, Time.time * 2f) - 0.5f,
            0f
        ) * moveNoise;

        transform.position += dir.normalized * flySpeed * speedMultiplier * Time.deltaTime;

        float dist =
            Vector2.Distance(transform.position, targetPos);

        if (returningHome)
        {
            if (dist <= 0.1f)
            {
                transform.position = homePosition;
                returningHome = false;
            }

            return;
        }

        if (dist <= orbitDistance)
        {
            state = State.Orbit;
        }
    }

    void UpdateOrbit()
    {
        angle += orbitDirection*orbitSpeed*Time.deltaTime;
        float currentX = orbitX * (1f + Mathf.Sin(Time.time * 0.7f + seed) * 0.15f);

        float currentY = orbitY * (1f + Mathf.Cos(Time.time * 0.5f + seed) * 0.15f);

        Vector3 offset = new(
            Mathf.Cos(angle) * currentX,
            Mathf.Sin(angle) * currentY,
            0f
        );

        offset += new Vector3(
            (Mathf.PerlinNoise(Time.time, seed) - 0.5f) * orbitNoise,
            (Mathf.PerlinNoise(seed, Time.time) - 0.5f) * orbitNoise,
            0f
        );
        offset = orbitRotationQuat * offset;

        Vector3 targetPos =
            player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            flySpeed * Time.deltaTime * speedMultiplier
        );
    }
}