using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightModeable : MonoBehaviour
{
    [SerializeField] private GameObject day, night;

    public virtual void Start()
    {
        day.SetActive(true);
        night.SetActive(false);
    }

    public virtual void setDay()
    {
        night.SetActive(false);
        day.SetActive(true);
    }

    public virtual void setNight()
    {
        day.SetActive(false);
        night.SetActive(true);
    }
}
