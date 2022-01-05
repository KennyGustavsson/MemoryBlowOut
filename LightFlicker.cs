using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Range(1f,10f)][SerializeField] private float minValue = 2.38f;
    [Range(1f,10f)][SerializeField] private float maxValue = 2.88f;
    [Range(0.0001f, 0.2f)] [SerializeField] private float updateTime = 0.0001f;

    private Light light = null;
    private float timer = 0f;
    
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        if (timer > updateTime)
        {
            timer = 0f;
            light.intensity = (Random.Range(minValue, maxValue));
        }

        timer += Time.deltaTime;
    }
}
