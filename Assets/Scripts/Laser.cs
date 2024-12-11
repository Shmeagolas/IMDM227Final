using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laserRenderer;
    [SerializeField] Light laserGlow;

    // Initialize the LaserHandler
    public void Initialize(LineRenderer lineRenderer)
    {
        laserRenderer = lineRenderer;
        laserRenderer.positionCount = 2;  // The laser needs 2 points
        laserRenderer.enabled = false;    // Initially hidden
        laserGlow.enabled = false;
    }

    // Method to draw the laser from start point to the target
    public void DrawLaser(Vector3 targetPoint, float duration)
    {
        StartCoroutine(ShowLaser(targetPoint, duration));
    }

    private IEnumerator ShowLaser(Vector3 targetPoint, float duration)
    {
        if (laserRenderer == null) yield break;

        // Get the current world position of the laser start point (moving object)
        Vector3 laserStartWorldPosition = transform.localPosition;

        laserGlow.enabled = true;

        // Enable the laser and set the start and end points in world space
        laserRenderer.enabled = true;
        laserRenderer.SetPosition(0, laserStartWorldPosition);  // Set start position in world space
        laserRenderer.SetPosition(1, transform.InverseTransformPoint(targetPoint));  // The target point in world space

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Disable the laser
        laserRenderer.enabled = false;
        laserGlow.enabled = false;
    }
}
