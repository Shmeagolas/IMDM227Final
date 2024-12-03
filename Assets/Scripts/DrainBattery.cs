using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainBattery : MonoBehaviour
{
    public Slider battery; // Reference to the Slider
    float maxCharge = 100f; // Maximum charge (100%)
    private float currentCharge;
    
    // Start is called before the first frame update
    void Start()
    {
        currentCharge = maxCharge; // start with battery at full
        Update();
    }

    // when this is called it will drain the battery by damage amount. Maybe change based on how late in the game?
    public void Drain(float damage)
    {
        currentCharge -= damage; // subtract damage
        currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge); // clamp value between range
        Debug.Log("battery is being drained");
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        // REMOVE
        currentCharge -= 0.01f; //test that it works
        // ^^^^^^
        battery.value = currentCharge;
    }
}
