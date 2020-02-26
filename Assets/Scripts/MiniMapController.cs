using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void MakeSquarePngFromOurVirtualThingy()
    {
        // capture the virtuCam and save it as a square PNG.

        int sqr = 1024;

        GetComponent<Camera>().aspect = 1.0f;
        // recall that the height is now the "actual" size from now on

        RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
        // the 24 can be 0,16,24, formats like
        // RenderTextureFormat.Default, ARGB32 etc.

        GetComponent<Camera>().targetTexture = tempRT;
        GetComponent<Camera>().Render();

        RenderTexture.active = tempRT;
        Texture2D virtualPhoto = new Texture2D(sqr, sqr, TextureFormat.RGB24, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        GetComponent<Camera>().targetTexture = null;
        // consider ... Destroy(tempRT);

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }
}
