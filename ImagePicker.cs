// using System.Runtime.InteropServices;
// using UnityEngine;

// public class ImagePicker : MonoBehaviour {

//     [DllImport("__Internal")]
//     private static extern void _PickImage(string gameObjectName, string methodName);

//     public void PickImage() {
//         _PickImage(gameObject.name, "OnImagePicked");
//     }

//     void OnImagePicked(string base64Image) {
//         byte[] imageData = System.Convert.FromBase64String(base64Image);
//         Texture2D tex = new Texture2D(2, 2);
//         tex.LoadImage(imageData);

//         // You now have a texture with the selected image, you can apply it to a material, use it as a sprite, etc.
//     }
// }
