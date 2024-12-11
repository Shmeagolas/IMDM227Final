using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] private float health;

    [SerializeField] private Color hitColor = Color.white, explodeColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.1f;   // How long the flash lasts.
    private int explodeFlashes = 15;

    private MeshRenderer[] renderers; // Array to store all mesh renderers.
    private Color[] originalColors;   // Array to store original colors.
    private bool isFlashing = false;

    void Start()
    {
        // Get all MeshRenderers in this GameObject and its children.
        renderers = GetComponentsInChildren<MeshRenderer>();

        // Store the original colors of each renderer's material.
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void OnHit(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {   
            GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeAll;
            StartCoroutine(Explode(hitColor, explodeColor, explodeFlashes));
            return;
        }

        if (!isFlashing)
        {
            StartCoroutine(Flash(hitColor));
        }
    }

    public void Attack(){
        GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeAll;
        StartCoroutine(Explode(hitColor, explodeColor, explodeFlashes));
        //deal battery damage
    }

    private IEnumerator Flash(Color color)
    {
        isFlashing = true;

        // Change each mesh renderer's material to the hit color.
        foreach (var renderer in renderers)
        {
            renderer.material.color = color;
        }

        // Wait for the flash duration.
        yield return new WaitForSeconds(flashDuration);

        // Revert each material to its original color.
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

        isFlashing = false;
    }

    private IEnumerator Explode(Color color1, Color color2, int times)
    {
        if(times <= 0)
        {
            Destroy(this.gameObject);
        }

        foreach (var renderer in renderers)
        {
            renderer.material.color = color1;
        }

        yield return new WaitForSeconds(flashDuration);


        StartCoroutine(Explode(color2, color1, times - 1));
    }
}
