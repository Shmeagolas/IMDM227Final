using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    //input systemv vars
    private PlayerInput playerInput;
    private Vector2 inputLook;
    public float inputClick;

    //look vars
    private Vector3 headRotation;
    [SerializeField] private float lookSensitivity;
    private float lookXSensitivity = 0.4f,
        lookYSensitivity = 0.4f,
        lookYCapMin = -70f,
        lookYCapMax = 80f;

    //firing vars
    private float firingDelay = .2f, //seconds
        nextFireTime = -1f,
        rayCastMaxDistance = 1000f;
    int layerMask = 1 << 9;//blastable layer
    [SerializeField] private BlasterRecoil blaster;

    // Laser vars
    [SerializeField] private Laser laser; 
    [SerializeField] private LineRenderer laserRenderer; 
    [SerializeField] private Transform laserStartPoint;
    private float laserDuration = 0.1f; 

    private void Awake()
    {
        playerInput = new PlayerInput();

        //adds event to update inputLook vector when mouse delta (Look) is changed
        playerInput.Player.Look.performed += e => inputLook = e.ReadValue<Vector2>();
        
        //events update inputClick float when mouse is pressed/unpressed
        playerInput.Player.Fire.performed += e => inputClick = e.ReadValue<float>();
        playerInput.Player.Fire.canceled += e => inputClick = 0f;  


        playerInput.Enable();

        headRotation = this.transform.localRotation.eulerAngles;

        // make mouse invisible
        Cursor.visible = false;

        if(lookSensitivity != 0)
        {
            lookXSensitivity = lookSensitivity;
            lookYSensitivity = lookSensitivity;
        }

        laser.Initialize(laserRenderer);
    }

    void Update()
    {
        doRotation();
        doFire();
    }

    private void doRotation()
    {
        //no need for time delta time here bc i believe it's already factored in the input
        headRotation.x -= lookYSensitivity * inputLook.y;
        
        //keeps x rotation value between min cap and max cap
        headRotation.x = Mathf.Clamp(headRotation.x, lookYCapMin, lookYCapMax);
        
        //this is value would also typically control the whole character's rotation
        //however our character is stationary so I'm only rotating the "head"
        headRotation.y += lookXSensitivity * inputLook.x;
        

        this.transform.localRotation = Quaternion.Euler(headRotation);
    }

    private void doFire()
{

    //Debug.Log($"Current Time: {Time.time}, Next Fire Time: {nextFireTime}, Firing Delay: {firingDelay}");

    // cooldown timer has elapsed
    if (Time.time < nextFireTime)
    {
        return;
    }

    // fire input was triggered
    if (inputClick < 1f)
    {
        return;
    }

    // reset fire cooldown 
    nextFireTime = Time.time + firingDelay;

    blaster.doRecoil();

    RaycastHit blasted;

        if (Physics.Raycast(transform.position, transform.forward, out blasted, rayCastMaxDistance, layerMask))
    {
        Debug.Log($"Hit object: {blasted.collider.gameObject.name}");

        // Draw the laser to the hit point
        laser.DrawLaser(blasted.point, laserDuration);
    }
    else
    {
        // If no hit, draw laser to maximum distance
        Vector3 laserEndPoint = transform.position + transform.forward * rayCastMaxDistance;
        laser.DrawLaser(laserEndPoint, laserDuration);
    }
}

}
