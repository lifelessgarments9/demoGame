using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RevealTarget : MonoBehaviour
{
    [SerializeField] private Material vignetteMaterial;

    private Camera cam;

    [SerializeField] private float holeRadius = 0.15f;
    [SerializeField] private float feather = 0.05f;

    void Awake()
    {
        cam = Camera.main;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector3 viewportPos =
            cam.WorldToViewportPoint(transform.position);

        vignetteMaterial.SetVector(
            "_HolePosition",
            new Vector4(viewportPos.x, viewportPos.y, 0f, 0f));

        vignetteMaterial.SetFloat(
            "_HoleRadius",
            holeRadius);

        vignetteMaterial.SetFloat(
            "_Feather",
            feather);

        vignetteMaterial.SetFloat(
            "_HoleEnabled",
            1f);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        vignetteMaterial.SetFloat("_HoleEnabled", 0f);
    }
}