using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    
    private PlayerInput playerInput;
    public Vector2 inputLook;

    private Vector3 headRotation;

    [SerializeField] private float lookXSensitivity = 0.5f;
    [SerializeField] private float lookYSensitivity = 0.5f;
    private float lookYCapMin = -70f;
    private float lookYCapMax = 80f;

    private void Awake()
    {
        playerInput = new PlayerInput();

        //adds event to update inputLook vector when mouse delta (Look) is changed
        playerInput.Player.Look.performed += e => inputLook = e.ReadValue<Vector2>();

        playerInput.Enable();

        headRotation = this.transform.localRotation.eulerAngles;
    }

    void Update()
    {
        doRotation();
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
}
