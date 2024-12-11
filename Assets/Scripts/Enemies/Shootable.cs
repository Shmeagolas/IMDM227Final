using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] private float health;

    [SerializeField] private Color hitColor = Color.white, explodeColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.1f, growFactor = 1.2f, shrinkFactor = .9f;   // How long the flash lasts.
    private int explodeFlashes = 10;

    private MeshRenderer[] renderers; // Array to store all mesh renderers.
    private Color[] originalColors;   // Array to store original colors.
    private bool isFlashing = false;

    //private Vector3 growFactor = new Vector3(1.2f, 1.2f, 1.2f), shrinkFactor = new Vector3(.9f, .9f, .9f);
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
            GetComponent<Approacher>().stopMoving();
            StartCoroutine(Explode(hitColor, explodeColor, explodeFlashes, new Vector3(1f,1f,1f)));
            return;
        }

        if (!isFlashing)
        {
            StartCoroutine(Flash(hitColor));
        }
    }

    public void Attack(){
        GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeAll;
        GetComponent<Approacher>().stopMoving();
        StartCoroutine(Explode(hitColor, explodeColor, explodeFlashes, new Vector3(1f,1f,1f)));
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

 private IEnumerator Explode(Color color1, Color color2, int times, Vector3 targetScale)
    {
        if (times <= 0)
        {
            Destroy(this.gameObject);
            yield break;
        }

        // Change to color1
        foreach (var renderer in renderers)
        {
            renderer.material.color = color1;
        }

        // Scale up gradually to the target scale over `flashDuration`

        if(times % 2 == 0f)
        {
            targetScale *= growFactor;
        }
        else
        {
            targetScale *= shrinkFactor;
        }
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / flashDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure it reaches the exact target scale at the end
        transform.localScale = targetScale;

        // Wait for the remainder of the `flashDuration`
        yield return new WaitForSeconds(flashDuration - elapsedTime);

        // Recursive call to alternate colors and scale back down
        StartCoroutine(Explode(color2, color1, times - 1, initialScale));
    }
}
