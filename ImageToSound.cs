using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Networking;


public class ImageToSound : MonoBehaviour
{
    public RawImage uiImage1; // Assign your UI Image in the Unity Inspector
    public int sampleRate = 44100; // Standard audio sample rate
    private float duration1 = 0.1f; // Duration for each sound
    private float delayBetweenPixels1 = 0.1f; // Delay between each pixel's sound
    public Slider speedSlider;
    public Slider sliderPitch;
    public Slider waveformSlider;
    private float waveformValue;
    public Slider volumeSlider;
    public Toggle toggle1;
    public Image pixel1;
    public Toggle playStay;
    private bool playTog = true;
    private Color currentPlayingColor1 = new Color(1f,1f,1f,1f);

    private int lastPlayedPixelIndex = 0;

    private float baseF = 108f;

    private Texture2D textureD1;
    private Color pixelColor1;

    private bool textureReady1 = false;
    private float sample1;

    public Slider echoSlider;
    private AudioEchoFilter echoFilter;

    public Slider distortionSlider;
    private AudioDistortionFilter distortionFilter;

    public Slider chorusSlider;
    private AudioChorusFilter chorusFilter;

    public Slider reverbSlider;
    private AudioReverbFilter reverbFilter;
    


    public void Start()
    {
        if (echoSlider != null)
        {
            echoFilter = gameObject.AddComponent<AudioEchoFilter>();   
            echoFilter.enabled = true;
            echoSlider.onValueChanged.AddListener(UpdateEcho);
            echoFilter.delay = 0;
            echoFilter.decayRatio = 0;
            // UpdateEcho(echoSlider.value);
        }

        if (reverbSlider != null)
        {
            reverbFilter = gameObject.AddComponent<AudioReverbFilter>();
            reverbFilter.enabled = true;
            reverbFilter.reverbLevel = 0;
            reverbFilter.reverbDelay = 0;
            reverbFilter.reflectionsLevel = 0;
        }

        if (distortionSlider != null)
        {
            distortionFilter = gameObject.AddComponent<AudioDistortionFilter>();
            distortionFilter.enabled = true;
            distortionFilter.distortionLevel = 0;
        }

        if (waveformSlider != null)
        {
            waveformSlider.onValueChanged.AddListener(UpdateWaveform);
            UpdateWaveform(waveformSlider.value);
        }

        if (chorusSlider != null)
        {
            chorusFilter = gameObject.AddComponent<AudioChorusFilter>();
            chorusFilter.enabled = true;
            chorusFilter.depth = 0f;
            chorusFilter.rate = 0f;
            chorusSlider.onValueChanged.AddListener(UpdateChorus);
        }

        if (speedSlider != null)
        {
            speedSlider.onValueChanged.AddListener(UpdateSpeed);
            UpdateSpeed(speedSlider.value);
        }

        if (sliderPitch != null)
        {
            sliderPitch.onValueChanged.AddListener(UpdatePitch);
            UpdatePitch(sliderPitch.value);
        }
        
        if (toggle1 != null)
        {
            toggle1.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(toggle1.isOn);
        }

        if (playStay != null)
        {
            playStay.onValueChanged.AddListener(togglePlay);
            // togglePlay(playStay.isOn);
        }
    }

    private void UpdateSpeed(float value)
    {
        if (playTog)
        {
            duration1 = Mathf.Lerp(1f, 0.005f, value);
            delayBetweenPixels1 = Mathf.Lerp(.5f, 0.05f, value);
        }
        else
        {
            duration1 = 3000;
            delayBetweenPixels1 = 3000;
        }
        
    }

    private void UpdatePitch(float value)
    {
        baseF = Mathf.Lerp(108f, 936f, value);
    }

    private void UpdateWaveform(float value)
    {
        waveformValue = value;
    }

    private void UpdateEcho(float value)
    {
        echoFilter.delay = Mathf.Lerp(0f, 4000f, value);
        echoFilter.decayRatio = value;
    }

    private void UpdateChorus(float value)
    {
        chorusFilter.rate = Mathf.Lerp(0f, 20f, value);
        chorusFilter.depth = value/3;
    }

    public void togglePlay(bool isOn)
    {
        // playTog = !playTog;
        if (isOn)
        {
            duration1 = Mathf.Lerp(.8f, 0.01f, speedSlider.value);
            delayBetweenPixels1 = Mathf.Lerp(.1f, 0.001f, speedSlider.value);
            playTog = true;
        }
        else if (!isOn)
        {
            // duration1 = 3000;
            // delayBetweenPixels1 = 3000;
            playTog = false;
        }
    }

///////////////////////////////////////// HOW TO STOP THE FOR LOOP? PUT A CHECK ON PLAYTOG. BREAK IF !PLAYTOG

    void Update()
    {
        // Update the UI image color with the currently playing color
        pixel1.color = currentPlayingColor1;
        // echoFilter.delay = Mathf.Lerp(0f, 4000f, echoSlider.value);
        // echoFilter.decayRatio = echoSlider.value;
        UpdateEcho(echoSlider.value);
        reverbFilter.reverbLevel = Mathf.Lerp(0f, 1000f, reverbSlider.value);
        reverbFilter.reverbDelay = reverbSlider.value/10;
        reverbFilter.reflectionsLevel = Mathf.Lerp(0f, 100f, reverbSlider.value);
        distortionFilter.distortionLevel = distortionSlider.value;
        // chorusFilter.rate = Mathf.Lerp(0f, 20f, chorusSlider.value);
        
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            textureD1 = TextureToTexture2D(uiImage1.texture);
            Color32[] pixels = textureD1.GetPixels32();
            StartPlayingSounds(pixels);
        }
        else
        {
            StopAllCoroutines();
        }
    }

    

    

    Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    void StartPlayingSounds(Color32[] pixels)
    {
        StartCoroutine(PlayImageSound(pixels));
    }


    ////////////////////////////// Get stop/go code working (need two separate PlayImageSound() methods toggled by playStay toggle)
    ////////////////////////////// Connect pixel select by touch, show pixel with pixel selector/marker
    ////////////////////////////// Loop/record/save
    ////////////////////////////// refine, refine, refine!!
    ////////////////////////////// what sliders could be turned into knobs like my spacetime sliderknobs?? volume? saturation?
    /////////////////////////////// HOW TO SHARE?   share image with soundcreation? Facebook, email, etc?


    IEnumerator PlayImageSound(Color32[] pixels)
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            if (!toggle1.isOn)
            {
                yield break;
            }

            Color32 pixelColor1 = pixels[i];
            
            if (pixelColor1.a != 0 && 80 < (pixelColor1.r + pixelColor1.g + pixelColor1.b))
            {
                currentPlayingColor1 = pixelColor1;

                AudioClip audioClip = MakeAudioClipFromPixel(currentPlayingColor1, i);
                AudioSource audioSource = gameObject.AddComponent<AudioSource>(); // create a new AudioSource for each pixel
                audioSource.clip = audioClip;
                audioSource.volume = volumeSlider.value;    
                echoFilter.delay = Mathf.Lerp(0f, 4000f, echoSlider.value);
                echoFilter.decayRatio = echoSlider.value;            
                audioSource.Play();
                lastPlayedPixelIndex = i;

                StartCoroutine(DestroyAudioSourceWhenFinished(audioSource)); // destroy the AudioSource when it's done playing
            }

            yield return new WaitForSeconds(delayBetweenPixels1/2);
        }
    }

    IEnumerator DestroyAudioSourceWhenFinished(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(audioSource);
    }








    // COLOR TO SOUND BY RGB CHORD
    AudioClip MakeAudioClipFromPixel(Color pixelColor1, int i)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        int sampleCount = (int)(sampleRate * duration1);
        float[] samples = new float[sampleCount];
        Color pixelC = pixelColor1;
        int N = i;

        float baseFrequency = baseF; 
        float[] pentatonicScale = new float[5] {1.0f, Mathf.Pow(2, 2.0f / 12), Mathf.Pow(2, 4.0f / 12), Mathf.Pow(2, 7.0f / 12), Mathf.Pow(2, 9.0f / 12)};

        int redNote = Mathf.FloorToInt(pixelC.r * 5) % 5;
        int greenNote = Mathf.FloorToInt(pixelC.g * 5) % 5;
        int blueNote = Mathf.FloorToInt(pixelC.b * 5) % 5;

        float redBaseFrequency = baseFrequency * pentatonicScale[redNote];
        float greenBaseFrequency = baseFrequency * pentatonicScale[greenNote];
        float blueBaseFrequency = baseFrequency * pentatonicScale[blueNote];     

        float redBaseFrequencyFFTH = 3*redBaseFrequency/2; 
        float greenBaseFrequencyFFTH = 3*greenBaseFrequency/2;
        float blueBaseFrequencyFFTH = 3*blueBaseFrequency/2;


        // Generate a chord of sound for this pixel
        for (int k = 0; k < sampleCount; k++)
        {
            float time = (float)k / sampleRate;

            float redBaseSample = Mathf.Sin(2 * Mathf.PI * redBaseFrequency * time);
            float greenBaseSample = Mathf.Sin(2 * Mathf.PI * greenBaseFrequency * time);
            float blueBaseSample = Mathf.Sin(2 * Mathf.PI * blueBaseFrequency * time);
            float redSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * redBaseFrequency * time));
            float greenSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * greenBaseFrequency * time));
            float blueSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * blueBaseFrequency * time));
            // Interpolate between the sine wave and square wave based on the waveformValue
            float redSample = Mathf.Lerp(redBaseSample, redSquareSample, waveformValue);
            float greenSample = Mathf.Lerp(greenBaseSample, greenSquareSample, waveformValue);
            float blueSample = Mathf.Lerp(blueBaseSample, blueSquareSample, waveformValue);

            float redFFTHBaseSample = Mathf.Sin(2 * Mathf.PI * redBaseFrequencyFFTH * time);
            float greenFFTHBaseSample = Mathf.Sin(2 * Mathf.PI * greenBaseFrequencyFFTH * time);
            float blueFFTHBaseSample = Mathf.Sin(2 * Mathf.PI * blueBaseFrequencyFFTH * time);
            float redFFTHSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * redBaseFrequencyFFTH * time));
            float greenFFTHSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * greenBaseFrequencyFFTH * time));
            float blueFFTHSquareSample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * blueBaseFrequencyFFTH * time));
            // Interpolate between the sine wave and square wave based on the waveformValue
            float redFFTHSample = Mathf.Lerp(redFFTHBaseSample, redFFTHSquareSample, waveformValue);
            float greenFFTHSample = Mathf.Lerp(greenFFTHBaseSample, greenFFTHSquareSample, waveformValue);
            float blueFFTHSample = Mathf.Lerp(blueFFTHBaseSample, blueFFTHSquareSample, waveformValue);
      
            // sample1 = .167f*(redSample + redFFTHSample + greenSample + greenFFTHSample + blueSample + blueFFTHSample) / 6;
            sample1 = .618f*(redSample + greenSample + blueSample) / 3;
            
            // Debug.Log($"The pan is set to: {audioSource.panStereo}");
            

            // Apply a fade in/out
            float fadeLength = 0.001f; // Length of the fade in seconds
            int fadeSamples = (int)(fadeLength * sampleRate); // Length of the fade in samples
            if (k < fadeSamples) // If we're in the fade in
            {
                sample1 *= (float)k / fadeSamples; // Linear fade in
            }
            else if (k > sampleCount - fadeSamples) // If we're in the fade out
            {
                sample1 *= (float)(sampleCount - k) / fadeSamples; // Linear fade out
            }

            
            // samples[k] = sample1 * ((pixelColor1.r + pixelColor1.g + pixelColor1.b) / (3f)); // Adjust the volume of the sample based on color intensity
            // samples[k] = sample1;
            samples[k] = 0.8f*sample1 * (pixelColor1.r + pixelColor1.g + pixelColor1.b) ;
        }

        AudioClip audioClip = AudioClip.Create("PixelSound", samples.Length, 1, sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }



}

