using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightPulse : MonoBehaviour
{
    float baseRadius;
    [SerializeField] float amp = 1f;
    [SerializeField] float freq = 1f;
    private Light2D light;
    void Start()
    {
        light=GetComponent<Light2D>();
        baseRadius = light.pointLightOuterRadius;
    }

    void Update()
    {
        float pulse=Mathf.Sin(Time.time * freq);
        light.pointLightOuterRadius = baseRadius + pulse * amp;
    }
}
