using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ImageSaturationController : MonoBehaviour
{
    public RawImage targetImage;
    public Material saturationMaterial;
    // public ImageToSound imageToSoundScript;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        targetImage.material = saturationMaterial;
    }

    

    private void Update()
    {
        targetImage.material.SetFloat("_Saturation", slider.value);
        // imageToSoundScript.UpdateTexture();
    }
}
