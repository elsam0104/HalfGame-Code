using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class ImageConverter
{
    public static string TextureToBase64(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        string base64String = Convert.ToBase64String(bytes);
        return base64String;
    }

    public static Texture2D Base64ToTexture(string base64String)
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }
}
