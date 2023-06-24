// THIS IS FOR WHEN THERE ARE TWO IMAGES TO USE!!

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class ImageToSound : MonoBehaviour
// {
//     public RawImage uiImage1, uiImage2;
//     public int sampleRate = 44100;
//     private float duration1 = 0.01f;
//     private float duration2 = 0.01f;
//     private float delayBetweenPixels1 = 0.001f;
//     private float delayBetweenPixels2 = 0.001f;
//     public Slider slider1;
//     public Slider slider2;
//     public Toggle toggle1;
//     public Toggle toggle2;
//     public Image pixel1;
//     public Image pixel2;

//     private Coroutine coroutine1, coroutine2;
//     private AudioSource audioSource1;

//     private Color currentPlayingColor1, currentPlayingColor2;

//     private float baseF = 196f;
//     private float scale = 7f;
 

//     private Texture2D textureD1, textureD2;
//     private Color pixelColor1, pixelColor2;

//     private bool textureReady1 = false, textureReady2 = false;
//     private float sample1, sample2;

//     public void Start()
//     {
       
//         // if (slider1 != null)
//         // {
//         //     slider1.onValueChanged.AddListener(UpdateImage1);
//         //     // UpdateValues1(slider1.value);
//         // }
    

//         // if (slider2 != null)
//         // {
//         //     slider2.onValueChanged.AddListener(UpdateImage2);
//         //     // UpdateValues2(slider2.value);
//         // }
      

//         if (toggle1 != null)
//         {
//             toggle1.onValueChanged.AddListener(OnToggleChanged1);
//             OnToggleChanged1(toggle1.isOn);
//         }
//         else
//         {
//             Debug.LogError("Toggle1 not assigned in the inspector");
//         }

//         if (toggle2 != null)
//         {
//             toggle2.onValueChanged.AddListener(OnToggleChanged2);
//             OnToggleChanged2(toggle2.isOn);
//         }
//         else
//         {
//             Debug.LogError("Toggle2 not assigned in the inspector");
//         }
//     }

  

//     void Update()
//     {
//         pixel1.color = currentPlayingColor1;
//         pixel2.color = currentPlayingColor2;
//         if (slider1 != null)
//         {
//             duration1 = Mathf.Lerp(0.01f, 0.1f, slider1.value);
//             delayBetweenPixels1 = Mathf.Lerp(0.001f, 0.01f, slider1.value);
//             // slider1.onValueChanged.AddListener(UpdateImage1);
//             // // UpdateValues1(slider1.value);
//         }
    

//         if (slider2 != null)
//         {
//             duration2 = Mathf.Lerp(0.01f, 0.1f, slider2.value);
//             delayBetweenPixels2 = Mathf.Lerp(0.001f, 0.01f, slider2.value);
//             // slider2.onValueChanged.AddListener(UpdateImage2);
//             // // UpdateValues2(slider2.value);
//         }
//     }


//     // private void UpdateValues1(float value1)
//     // {
//     //     duration1 = Mathf.Lerp(0.01f, 0.1f, value1);
//     //     delayBetweenPixels1 = Mathf.Lerp(0.001f, 0.01f, value1);
//     // }

//     // private void UpdateValues2(float value2)
//     // {
//     //     duration2 = Mathf.Lerp(0.01f, 0.1f, value2);
//     //     delayBetweenPixels2 = Mathf.Lerp(0.001f, 0.01f, value2);
//     // }


//     void OnToggleChanged1(bool isOn1)
//     {
//         if (isOn1)
//         {
//             textureD1 = TextureToTexture2D(uiImage1.texture);
//             Color32[] pixels = textureD1.GetPixels32();
//             coroutine1 = StartCoroutine(PlaySounds(pixels, 1));
//         }
//         else
//         {
//             if (coroutine1 != null) StopCoroutine(coroutine1);
//         }
//     }

//     void OnToggleChanged2(bool isOn2)
//     {
//         if (isOn2)
//         {
//             textureD2 = TextureToTexture2D(uiImage2.texture);
//             Color32[] pixels2 = textureD2.GetPixels32();
//             coroutine2 = StartCoroutine(PlaySounds(pixels2, 2));
//         }
//         else
//         {
//             if (coroutine2 != null) StopCoroutine(coroutine2);
//         }
//     }


//     Texture2D TextureToTexture2D(Texture texture)
//     {
//         Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
//         RenderTexture currentRT = RenderTexture.active;
//         RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
//         Graphics.Blit(texture, renderTexture);
//         RenderTexture.active = renderTexture;
//         texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
//         texture2D.Apply();
//         RenderTexture.active = currentRT;
//         RenderTexture.ReleaseTemporary(renderTexture);
//         return texture2D;
//     }

//     void StartPlayingSounds(Color32[] pixels, int imageIndex)
//     {
//         switch (imageIndex)
//         {
//             case 1:
//                 StartCoroutine(PlaySounds(pixels, 1));
//                 break;
//             case 2:
//                 StartCoroutine(PlaySounds(pixels, 2));
//                 break;
//             default:
//                 Debug.LogError("Invalid imageIndex");
//                 break;
//         }
//     }

//     IEnumerator PlaySounds(Color32[] pixels, int imageIndex)
//     {
//         AudioSource[] audioSources = GetComponents<AudioSource>();
//         AudioSource audioSource1 = audioSources[0];
//         AudioSource audioSource2 = audioSources[1];

//         for (int i = 0; i < pixels.Length; i++)
//         {
//             Color32 pixel = pixels[i];
//             switch (imageIndex)
//             {
//                 case 1:
//                     currentPlayingColor1 = pixel;
//                     AudioClip clip1 = MakeAudioClipFromPixel(currentPlayingColor1, i, audioSource1);
//                     audioSource1.PlayOneShot(clip1);
//                     yield return new WaitForSeconds(clip1.length + delayBetweenPixels1);
//                     break;
//                 case 2:
//                     currentPlayingColor2 = pixel;
//                     AudioClip clip2 = MakeAudioClipFromPixel(currentPlayingColor2, i, audioSource2);
//                     audioSource2.PlayOneShot(clip2);
//                     yield return new WaitForSeconds(clip2.length + delayBetweenPixels2);
//                     break;
//                 default:
//                     Debug.LogError("Invalid imageIndex");
//                     break;
//             }
//         }
//     }


   



//     AudioClip MakeAudioClipFromPixel(Color pixelColor1, int i, AudioSource audioSource)
//     {
     
//         int sampleCount = audioSource == audioSource1 ? (int)(sampleRate * duration1) : (int)(sampleRate * duration2);

//         // int sampleCount = (int)(sampleRate * duration1);
//         float[] samples = new float[sampleCount];
//         Color pixelC = pixelColor1;
//         int N = i;

//         float baseFrequency = baseF; 

//         float[] pentatonicScale = new float[5] {1.0f, Mathf.Pow(2, 2.0f / 12), Mathf.Pow(2, 4.0f / 12), Mathf.Pow(2, 7.0f / 12), Mathf.Pow(2, 9.0f / 12)};

//         // Decide which note in the pentatonic scale each color channel should correspond to
//         int redNote = Mathf.FloorToInt(pixelC.r * 5) % 5;
//         int greenNote = Mathf.FloorToInt(pixelC.g * 5) % 5;
//         int blueNote = Mathf.FloorToInt(pixelC.b * 5) % 5;


//         // Calculate the frequencies for each color channel
//         float redBaseFrequency = baseFrequency * pentatonicScale[redNote];
//         float greenBaseFrequency = baseFrequency * pentatonicScale[greenNote];
//         float blueBaseFrequency = baseFrequency * pentatonicScale[blueNote];     


//         // float redBaseFrequency = baseFrequency * (1 + scale * pixelC.r); // Frequency for the red component
//         float redBaseFrequencyTHRD = 5*redBaseFrequency/4; // Frequency for the red component
//         float redBaseFrequencyFFTH = 3*redBaseFrequency/2; // Frequency for the red component


//         // float greenBaseFrequency = baseFrequency * (1 + scale * pixelC.g); // Frequency for the green component
//         float greenBaseFrequencyTHRD = 6*greenBaseFrequency/5; // Frequency for the red component
//         float greenBaseFrequencyFFTH = 3*greenBaseFrequency/2;

//         // float blueBaseFrequency = baseFrequency * (1 + scale * pixelC.b); // Frequency for the blue component
//         float blueBaseFrequencyTHRD = 6*blueBaseFrequency/5; // Frequency for the red component
//         float blueBaseFrequencyFFTH = 1024*blueBaseFrequency/729;



//         // The pentatonic scale ratios
      


//         // Generate a chord of sound for this pixel
//         for (int k = 0; k < sampleCount; k++)
//         {
//             float time = (float)k / sampleRate;

//             // Generate a sine wave for each color component
//             float redBaseSample = Mathf.Sin(2 * Mathf.PI * redBaseFrequency * time);
//             float redTHRDSample = Mathf.Sin(2 * Mathf.PI * redBaseFrequencyTHRD * time);
//             float redFFTHSample = Mathf.Sin(2 * Mathf.PI * redBaseFrequencyFFTH * time);
            
            
//             float greenBaseSample = Mathf.Sin(2 * Mathf.PI * greenBaseFrequency * time);
//             float greenTHRDSample = Mathf.Sin(2 * Mathf.PI * greenBaseFrequencyTHRD * time);
//             float greenFFTHSample = Mathf.Sin(2 * Mathf.PI * greenBaseFrequencyFFTH * time);


//             float blueBaseSample = Mathf.Sin(2 * Mathf.PI * blueBaseFrequency * time);
//             float blueTHRDSample = Mathf.Sin(2 * Mathf.PI * blueBaseFrequencyTHRD * time);
//             float blueFFTHSample = Mathf.Sin(2 * Mathf.PI * blueBaseFrequencyFFTH * time);

//             // Combine the samples into a chord
//             // float sample1 = (redSample + greenSample + blueSample) / 3;
//             // float sample1 = (redBaseSample + redTHRDSample + redFFTHSample
//             //                 + greenBaseSample + greenTHRDSample + greenFFTHSample
//             //                 + blueBaseSample + blueTHRDSample + blueFFTHSample) / 9;

//             if (N%4 == 1)
//             {
//                 sample1 = (redBaseSample + redTHRDSample + redFFTHSample) / 3;
//                 audioSource.panStereo = -1;
//             }
//             else if (N%4 == 3)
//             {
//                 sample1 = (greenBaseSample + greenTHRDSample + greenFFTHSample) / 3;
//                 audioSource.panStereo = 1;
//             }
//             else
//             {
//                 sample1 = (blueBaseSample + blueTHRDSample + blueFFTHSample) / 3;
//                 audioSource.panStereo = 0;
//             }
            
//             // Debug.Log($"The pan is set to: {audioSource.panStereo}");
            

//             // Apply a fade in/out
//             float fadeLength = 0.01f; // Length of the fade in seconds
//             int fadeSamples = (int)(fadeLength * sampleRate); // Length of the fade in samples
//             if (k < fadeSamples) // If we're in the fade in
//             {
//                 sample1 *= (float)k / fadeSamples; // Linear fade in
//             }
//             else if (k > sampleCount - fadeSamples) // If we're in the fade out
//             {
//                 sample1 *= (float)(sampleCount - k) / fadeSamples; // Linear fade out
//             }

//             // samples[k] = sample1;
//             samples[k] = sample1 * ((pixelColor1.r + pixelColor1.g + pixelColor1.b) / (3f)); // Adjust the volume of the sample based on color intensity

//         }

//         AudioClip audioClip = AudioClip.Create("PixelSound", samples.Length, 1, sampleRate, false);
//         audioClip.SetData(samples, 0);
//         audioClip.LoadAudioData();

//         return audioClip;
//     }
// }