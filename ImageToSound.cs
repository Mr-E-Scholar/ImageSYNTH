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
    private float duration1; // Duration for each sound
    private float delayBetweenPixels1;// Delay between each pixel's sound
    public Slider speedSlider;
    public Slider sliderPitch;
    public Slider waveformSlider;
    private float waveformValue;
    public Slider volumeSlider;
    public Toggle toggle1;
    public Image pixel1;
    public Button playStay;
    private bool playTog = true;
    private bool powerTog = false;
    private Color currentPlayingColor1 = new Color(1f,1f,1f,1f);
    private int sampleCount;
    private float[] samples;
    private AudioSource audioSource;

    public Sprite playSprite;
    public Sprite stopSprite;

    public Button imageLoad;

    private int lastPlayedPixelIndex = 0;

    private float baseF = 108f;

    private Texture2D textureD1;
    private Color pixelColor1;

    private bool textureReady1 = false;
    private float sample1;

    public Slider echoSlider;
    private AudioEchoFilter echoFilter;


    private AudioDistortionFilter distortionFilter;

    public Slider chorusSlider;
    private AudioChorusFilter chorusFilter;

    public Slider reverbSlider;
    private AudioReverbFilter reverbFilter;
    


    public void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        if (echoSlider != null)
        {
            echoFilter = audioSource.gameObject.AddComponent<AudioEchoFilter>();   
            echoFilter.enabled = true;
            echoSlider.onValueChanged.AddListener(UpdateEcho);
            echoFilter.delay = 0f;
            echoFilter.decayRatio = 0f;
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


        if (waveformSlider != null)
        {
            waveformSlider.onValueChanged.AddListener(UpdateWaveform);
            distortionFilter = gameObject.AddComponent<AudioDistortionFilter>();
            distortionFilter.enabled = true;
            distortionFilter.distortionLevel = 0;
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
            playStay.onClick.AddListener(togglePlay);
            // togglePlay(playStay.isOn);
        }
    }

    private void UpdateSpeed(float value)
    {
        if (playTog)
        {
            duration1 = Mathf.Lerp(3f, 0.01f, value);
            delayBetweenPixels1 = Mathf.Lerp(.01f, 0.001f, value);
        }
        if (!playTog)
        {
            duration1 = 3000;
            delayBetweenPixels1 = 3000;
        }
        
    }

    private void UpdatePitch(float value)
    {
        baseF = Mathf.Lerp(88f, 1008f, value);
    }

    private void UpdateWaveform(float value)
    {
        waveformValue = value;
    }

    private void UpdateEcho(float value)
    {
        if (!echoFilter.enabled)
        {
            echoFilter.enabled = true;
        }
        echoFilter.delay = Mathf.Lerp(0f, 6000f, value);
        echoFilter.decayRatio = value;
    }

    private void UpdateChorus(float value)
    {
        chorusFilter.rate = Mathf.Lerp(0f, 20f, value);
        chorusFilter.depth = value/3;
    }

    

    public void togglePlay()
    {
        playTog=!playTog;

        if (playTog)
        {
            // ColorBlock colorVar = playStay.colors;
            // colorVar.normalColor = Color.green;
            // playStay.colors = colorVar;
            playStay.GetComponent<Image>().color = Color.green;
            playStay.GetComponent<Image>().sprite = playSprite;

        }
        else
        {
            // ColorBlock colorVar = playStay.colors;
            // colorVar.normalColor = Color.red;
            // playStay.colors = colorVar;
            playStay.GetComponent<Image>().color = Color.red;
            playStay.GetComponent<Image>().sprite = stopSprite;
        }
    }
    

///////////////////////////////////////// HOW TO STOP THE FOR LOOP? PUT A CHECK ON PLAYTOG. BREAK IF !PLAYTOG

    public void Update()
    {
        // Update the UI image color with the currently playing color
        pixel1.color = currentPlayingColor1;
        // echoFilter.delay = Mathf.Lerp(0f, 2000f, echoSlider.value);
        // echoFilter.decayRatio = echoSlider.value;
        // duration1 = Mathf.Lerp(2f, 0.05f, speedSlider.value);
        // delayBetweenPixels1 = Mathf.Lerp(.1f, 0.001f, speedSlider.value);
        // UpdateEcho(echoSlider.value);
        reverbFilter.reverbLevel = Mathf.Lerp(0f, 1000f, reverbSlider.value);
        reverbFilter.reverbDelay = reverbSlider.value/10;
        reverbFilter.reflectionsLevel = Mathf.Lerp(0f, 100f, reverbSlider.value);
        distortionFilter.distortionLevel = .9f*waveformSlider.value;

        // if (toggle)
        // {
        //     // Code for playing a single pixel when the user touches
        //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //     {
        //         Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        //         Color color = GetPixelColorAtWorldPosition(touchPosition);
        //         PlaySoundFromColor(color);
        //     }
        // }
        // else
        // {
        //     // Your existing code for looping through the pixels goes here
        // }
        
        
    }


    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            playStay.interactable = false;
            imageLoad.interactable = false;
            // playStay.onValueChanged.RemoveListener(togglePlay);
            if (playTog)
            {
                textureD1 = TextureToTexture2D(uiImage1.texture);
                Color32[] allPixels = textureD1.GetPixels32();

                // Filter out pixels that do not meet the requirement
                List<Color32> filteredPixels = new List<Color32>();
                for (int i = 0; i < allPixels.Length; i++)
                {
                    if (80 < (allPixels[i].r + allPixels[i].g + allPixels[i].b))
                    {
                        filteredPixels.Add(allPixels[i]);
                    }
                }

                // Convert the List of filtered pixels back to an array
                Color32[] filteredPixelArray = filteredPixels.ToArray();

                StartPlayingSoundsPLAY(filteredPixelArray);
                // StartPlayingSounds(pixels);
            }
            else
            {
                
                textureD1 = TextureToTexture2D(uiImage1.texture);
                Color32[] pixels = textureD1.GetPixels32();
                StartPlayingSoundsSTAY(pixels);
                
            }
            
        }
        else
        {
            playStay.interactable = true;
            imageLoad.interactable = true;
            
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

    void StartPlayingSoundsPLAY(Color32[] pixels)
    {
        StartCoroutine(PlayImageSound(pixels));
    }

    void StartPlayingSoundsSTAY(Color32[] pixels)
    {
        StartCoroutine(PlaySinglePixelSound(pixels[1423]));
    }


    ////////////////////////////// Get touch-pixel working!!!
    ////////////////////////////// Connect pixel select by touch, show pixel with pixel selector/marker
    ////////////////////////////// Loop/record/save
    ////////////////////////////// refine, refine, refine!!
    ////////////////////////////// what sliders could be turned into knobs like my spacetime sliderknobs?? volume? saturation? (Turn speed into BPM knob!!!!!)
    ///////////////////////////////  REMOVE DISTORTION SLIDER AND TURN THAT INTO A KNOB/TOGGLE FOR SELECTING THE SCALE STRUCTURE (PENTATONIC, OCTAVE, ETC)
    /////////////////////////////// HOW TO SHARE?   share image with soundcreation? Facebook, email, etc?

    IEnumerator PlayImageSound(Color32[] pixels)
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            
            Color32 pixelColor1 = pixels[i];
            if (pixelColor1.a != 0 && 80 < (pixelColor1.r + pixelColor1.g + pixelColor1.b))
            {
                currentPlayingColor1 = pixelColor1;
                AudioClip audioClip = MakeAudioClipFromPixel(currentPlayingColor1, i, duration1);
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                echoFilter.enabled = true;
                echoFilter.delay = Mathf.Lerp(0f, 6000f, echoSlider.value);
                echoFilter.decayRatio = echoSlider.value;
                
                audioSource.clip = audioClip;
                audioSource.volume = volumeSlider.value;         
                audioSource.Play();
                lastPlayedPixelIndex = i;
                StartCoroutine(DestroyAudioSourceWhenFinished(audioSource)); 
            }

            if (!playTog)
            {
                audioSource.Stop();
                yield break;
            }

            yield return new WaitForSeconds(duration1/10);
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


    IEnumerator PlaySinglePixelSound(Color32 pixel)
    {
        

        // if (pixel.a != 0 && 80 < (pixel.r + pixel.g + pixel.b))
        // {
            // AudioClip audioClip = MakeAudioClipFromPixel(pixel, 0, 30); // Assuming 0 as index, change if needed
            // AudioSource audioSource = gameObject.AddComponent<AudioSource>(); 
            // audioSource.clip = audioClip;
            // audioSource.volume = volumeSlider.value;    
            // audioSource.Play();

            // Commented out as per your script above.
            // StartCoroutine(DestroyAudioSourceWhenFinished(audioSource)); 
            // if (!playTog && audioSource != null)
            // {
            //     audioSource.Stop();
            //     yield break;
            // }
        // }
        return null;

        
    }






    // COLOR TO SOUND BY RGB CHORD
    AudioClip MakeAudioClipFromPixel(Color pixelColor1, int i, float duration)
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

        float attackTime = duration1/20; // Time in seconds for attack stage
        float decayTime = duration1/20; // Time in seconds for decay stage
        float sustainLevel = 0.05f; // Level for sustain stage (0.7 means 70% of max volume)
        float releaseTime = duration1/10; // Time in seconds for release stage

        // Calculate number of samples for each stage
        int attackSamples = (int)(attackTime * sampleRate);
        int decaySamples = (int)(decayTime * sampleRate);
        int sustainSamples = sampleCount - attackSamples - decaySamples; // Sustain for remaining time
        int releaseSamples = (int)(releaseTime * sampleRate);


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


      
            sample1 = (redSample + greenSample + blueSample) / 3;
            
            float envelope = 0.0f;

            if (k < attackSamples) // If we're in the attack stage
            {
                // Exponential ramp from 0 to 1
                float t = (float)k / attackSamples; // Normalized time (0 to 1)
                envelope = MathF.Exp(5.0f * (t - 1)); // Exponential curve with base e
            }
            else if (k < attackSamples + decaySamples) // If we're in the decay stage
            {
                // Exponential ramp from 1 to sustain level
                float t = (float)(k - attackSamples) / decaySamples; // Normalized time (0 to 1)
                envelope = 1.0f - (1.0f - sustainLevel) * MathF.Exp(5.0f * (t - 1)); // Inverted and offset exponential curve
            }
            else if (k < attackSamples + decaySamples + sustainSamples) // If we're in the sustain stage
            {
                envelope = sustainLevel; // Constant sustain level
            }
            else // If we're in the release stage
            {
                // Exponential ramp from sustain level to 0
                float t = (float)(k - attackSamples - decaySamples - sustainSamples) / releaseSamples; // Normalized time (0 to 1)
                envelope = sustainLevel * MathF.Exp(5.0f * (1 - t)); // Exponential curve with base e, inverted
            }
            samples[k] = 0.23f*sample1 * (pixelColor1.r + pixelColor1.g + pixelColor1.b) ;
            // samples[k] = sample1;
            samples[k] *= envelope; // Apply envelope to sample


        }

        AudioClip audioClip = AudioClip.Create("PixelSound", samples.Length, 1, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }



}




// Color GetPixelColorAtWorldPosition(Vector2 worldPosition)
// {
//     // Translate the world position to a pixel in the image
//     // This will depend on how your image is positioned and scaled in the world
//     // The following is just an example and may need to be adjusted
//     Vector2 imagePosition = worldPosition - imageBottomLeftCorner;
//     int x = (int)(imagePosition.x / imageWidth * imagePixelsWidth);
//     int y = (int)(imagePosition.y / imageHeight * imagePixelsHeight);
//     return image.GetPixel(x, y);
// }

// void PlaySoundFromColor(Color color)
// {
//     // Your existing code for playing a sound from a color goes here
// }
