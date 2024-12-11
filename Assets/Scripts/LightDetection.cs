using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    
    // public Transform ball;
    const int waitWidth = 16;

    public int width = 640;
    public int height = 360;

    public float threshhold = 9E+07f;
    public Material lightSky;
    public Material darkSky;

    Color32[] pixels;

    WebCamTexture cam;
    Texture2D tex;

    public GameObject battery;

    Action update = () => { };
    void Start()
    {
        update = WaitingForCam;

        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
        int w = cam.width;
        Debug.Log($"Width = {width}, Height = {height}");
    }
    private void Update()
    {
        update();
    }
    void WaitingForCam()
    {
        if (cam.width > waitWidth)
        {
            width = cam.width;
            height = cam.height;
            Debug.Log($"Width = {width}, Height = {height}");
            pixels = new Color32[cam.width * cam.height];
            tex = new Texture2D(cam.width, cam.height, TextureFormat.RGBA32, false);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = tex;
            update = CamIsOn;
        }
        else
        {
            Debug.Log("Not Yet");
        }
    }
    void CamIsOn()
    {
        if (cam.didUpdateThisFrame)
        {
            cam.GetPixels32(pixels);

            // get total amount of light and print
            float total = 0;
            foreach (Color32 c in pixels)
            {
                total += c.r;
                total += c.b;
                total += c.g;
            }
            if (total > threshhold) {
                Debug.Log("the light is on: " + total);
                RenderSettings.skybox = lightSky;
                battery.GetComponent<DrainBattery>().Drain(0.05f);
            } else {
                Debug.Log("the light is off: " + total);
                RenderSettings.skybox = darkSky;
            }

            tex.SetPixels32(pixels);
            tex.Apply();
        }
    }
}
