using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightDetection : MonoBehaviour
{
    const int waitWidth = 16;

    // for the score;
    public TMP_Text scoreText;

    public GameObject mainMenu;
    bool ready = false;

    private Spawner spawner;

    public int width = 640;
    public int height = 360;

    public float threshhold = 0;
    public Material lightSky;
    public Material darkSky;

    public float On = 0;
    public float Off = 0;

    // code to change posters
    public GameObject posters; // Assign the cube in the Inspector
    public Material posterOn; // Assign the first material in the Inspector
    public Material posterOff; // Assign the second material in the Inspector

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
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
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

    public void GetOn(){
        cam.GetPixels32(pixels);

        // get total amount of light and print
        float total = 0;
        foreach (Color32 c in pixels)
        {
            total += c.r;
            total += c.b;
            total += c.g;
        }
        On = total;
    }

    public void GetOff(){
        cam.GetPixels32(pixels);

        // get total amount of light and print
        float total = 0;
        foreach (Color32 c in pixels)
        {
            total += c.r;
            total += c.b;
            total += c.g;
        }
        Off = total;
    }

    public void CalcThreshhold(){
        threshhold = (On + Off) / 2;
        Debug.Log("tHRESHHOD: " + threshhold);
        mainMenu.SetActive(false);
        // set the battery to be ready to be drained
        ready = true;    
        spawner.enabled = true; //start enemy spawns
        // make mouse invisible
        Cursor.visible = false;
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
            if (total > threshhold) { //light on
                Debug.Log("the light is on: " + total);
                RenderSettings.skybox = lightSky;
                setObjectsDayorNight(true);
                // code should hard reset skybox to make sure eveything is reflecting right
                RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
                RenderSettings.defaultReflectionResolution = 128;
                DynamicGI.UpdateEnvironment(); // Updates global illumination for skybox changes

                scoreText.color = Color.black;
                // change the poster material
                posters.GetComponent<Renderer>().material = posterOn;
                
                if (ready) {
                    battery.GetComponent<DrainBattery>().Drain(0.1f);
                }
            } else { //light off    
                Debug.Log("the light is off: " + total);
                RenderSettings.skybox = darkSky;
                setObjectsDayorNight(false);

                // code should hard reset skybox to make sure eveything is reflecting right
                RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
                RenderSettings.defaultReflectionResolution = 128;
                DynamicGI.UpdateEnvironment(); // Updates global illumination for skybox changes

                scoreText.color = Color.white;
                // change the poster material
                posters.GetComponent<Renderer>().material = posterOff;
            }

            tex.SetPixels32(pixels);
            tex.Apply();
        }
    }

    private void setObjectsDayorNight(bool isDay)
    {
        NightModeable[] nightmodeableObjects = FindObjectsOfType<NightModeable>();
        foreach (NightModeable script in nightmodeableObjects)
        {  
            if(isDay)
            {
                script.setDay();
            }
            else
            {
                script.setNight();
            }
            
        }
    }
}
