using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shootable : MonoBehaviour
{

    [SerializeField] private float health;

    [SerializeField] Renderer renderer;
    private Color hitColor = Color.white; //the color turned when hit
    private float flashDuration = 0.1f; //how long 

    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        originalColor = renderer.material.color;
    }

    public void OnHit(float damage)
    {
        health -= damage;

        if(health <= 0f)
        {
            Destroy(this.gameObject);
        }

        if (!isFlashing)
        {
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        renderer.material.color = hitColor; // Change to hit color.
        yield return new WaitForSeconds(flashDuration); // Wait for the flash duration.
        renderer.material.color = originalColor; // Revert to the original color.
        isFlashing = false;
    }

}
