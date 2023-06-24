using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ImageSaturationController : MonoBehaviour
{
    public RawImage targetImage;
    public Material saturationMaterial;
    // public Slider saturationSlider;
    // public ImageToSound imageToSoundScript;
    private Slider saturationSlider;


    public void Start()
    {
        saturationSlider = GetComponent<Slider>();
        targetImage.material = saturationMaterial;
    }

    

    private void Update()
    {
        targetImage.material.SetFloat("_Saturation", saturationSlider.value);
        // imageToSoundScript.UpdateTexture();
    }
}
