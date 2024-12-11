using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] private float health;

    [SerializeField] private Color hitColor = Color.white, explodeColor = Color.yellow, dieColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f, growFactor = 1.2f, shrinkFactor = .9f, explodeDamage = 5f;   
    [SerializeField] private int explodeFlashes = 10, dieFlashes = 4, scoreValue = 100;

    private MeshRenderer[] renderers; 
    private Color[] originalColors;   
    private bool isFlashing = false;

    private DrainBattery battery;
    private ScoreCounter scoreCounter;

    void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        battery = GameObject.Find("Battery").GetComponent<DrainBattery>();
        scoreCounter = GameObject.Find("score").GetComponent<ScoreCounter>();
    }

    public void OnHit(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {   
            GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeAll;
            GetComponent<Approacher>().stopMoving();
            StartCoroutine(Explode(hitColor, dieColor, dieFlashes, new Vector3(1f,1f,1f)));
            scoreCounter.AddScore(scoreValue);
            scoreValue = 0;
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
    }

    private IEnumerator Flash(Color color)
    {
        isFlashing = true;


        foreach (var renderer in renderers)
        {
            renderer.material.color = color;
        }

        yield return new WaitForSeconds(flashDuration);

        
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

        isFlashing = false;
    }

 private IEnumerator Explode(Color color1, Color color2, int times, Vector3 targetScale)
    {
        isFlashing = true;
        if (times <= 0)
        {
            if(color2 == explodeColor || color1 == explodeColor)
            {
                battery.Drain(explodeDamage);
            }
            Destroy(this.gameObject);
            yield break;
        }

        foreach (var renderer in renderers)
        {
            renderer.material.color = color1;
        }


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



        yield return new WaitForSeconds(flashDuration - elapsedTime);

        StartCoroutine(Explode(color2, color1, times - 1, initialScale));
    }
}
