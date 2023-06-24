using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ReverbController : MonoBehaviour
{
    private AudioReverbFilter reverbFilter;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        reverbFilter.reverbLevel = slider.value * 100; // Assuming slider values are between 0 and 1, adjust accordingly
    }
}
