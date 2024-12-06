using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterRecoil : MonoBehaviour
{
    private Vector3 basePosition;
    private Quaternion baseRotation;
    private Vector3 midAnimationPosition;
    private Quaternion midAnimationRotation;
    [SerializeField] private float maxAngle, recoilAngle, maxBack, recoilBack, recoilSpeed, resetSpeed;
    private float angleRange = .5f, backRange = .5f;


    private Coroutine currentRecoilCoroutine;

    void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
    }


    public void doRecoil()
    {
         if (currentRecoilCoroutine != null)
        {
            StopCoroutine(currentRecoilCoroutine);
            SaveMidAnimationValues(); // Save state when stopped
        }
        currentRecoilCoroutine = StartCoroutine(Recoil()); //starts recoil animation
    }
    private IEnumerator Recoil()
    {
        
        float backAmount = Random.Range(recoilBack - recoilBack * backRange, recoilBack + recoilBack * backRange);
        Vector3 newPosition = basePosition + new Vector3(0, 0, -backAmount);

        float angleAmount = Random.Range(recoilAngle - recoilAngle * angleRange, recoilAngle + recoilAngle * angleRange);
        Quaternion newRotation = baseRotation * Quaternion.Euler(0, 0, -angleAmount);

        //move to recoil position and angle
        while (Vector3.Distance(transform.localPosition, newPosition) > 0.01f || Quaternion.Angle(transform.localRotation, newRotation) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * recoilSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, Time.deltaTime * recoilSpeed);
            yield return null;
        }

        //move to original position and angle
        while (Vector3.Distance(transform.localPosition, basePosition) > 0.01f || 
               Quaternion.Angle(transform.localRotation, baseRotation) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, basePosition, Time.deltaTime * resetSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, baseRotation, Time.deltaTime * resetSpeed);
            yield return null;
        }

        //make sure its exactly on original postion
        transform.localPosition = basePosition;
        transform.localRotation = baseRotation;

        //animation ends so set mid animation values to null
    }

    private void SaveMidAnimationValues()
    {
        midAnimationPosition = transform.localPosition;
        midAnimationRotation = transform.localRotation;

    }
}
