using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    AudioSource audioSource;
    public Material imageMaterial;
    float[] samples = new float[512];
    float audioVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        UpdateImageColor();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        audioVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            audioVolume += samples[i];
        }
    }

    void UpdateImageColor()
    {
        float saturation = audioVolume * 10f; // Adjust this multiplier as needed
        saturation = Mathf.Clamp(saturation, 0f, 2f); // Clamps the saturation between 0 and 2

        float h, s, v;
        Color.RGBToHSV(imageMaterial.color, out h, out s, out v);
        Color newColor = Color.HSVToRGB(h, saturation, v);
        imageMaterial.color = newColor;
    }
}
