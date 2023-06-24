using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    public RawImage rawImage;

    public void OnButtonPress()
    {
        StartCoroutine(OpenGallery());
    }

    private IEnumerator OpenGallery()
    {
        // Request permission to access the Camera Roll
        NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Load the selected image into a Texture2D
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Apply the Texture2D to your RawImage
                rawImage.texture = texture;
                // rawImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
                rawImage.rectTransform.sizeDelta = new Vector2(600,600);
            }
        }, "Select an image", "image/*");
        // rawImage.rectTransform.sizeDelta = new Vector2(600,600);


        yield return null;
    }
}
