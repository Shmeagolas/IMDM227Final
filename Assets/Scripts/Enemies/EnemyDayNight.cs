using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDayNight : NightModeable
{

    [SerializeField] private GameObject day, night;

    void Start()
    {
        day.SetActive(true);
        night.SetActive(false);
    }

    public override void setDay()
    {
        base.setDay();
        night.SetActive(false);
        day.SetActive(true);
    }

    public override void setNight()
    {
        base.setNight();
        day.SetActive(false);
        night.SetActive(true);
    }
}
