using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraRenderer : MonoBehaviour
{
    private Camera camera;

    public Material material;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material)
        {
            Graphics.Blit(source, destination, material);
            //Debug.Log("a");
            //return;
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
