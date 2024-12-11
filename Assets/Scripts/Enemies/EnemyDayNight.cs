using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDayNight : NightModeable
{

    [SerializeField] Approacher approacher;



    public override void setDay()
    {
        approacher.setDay(true);
        base.setDay();
    }

    public override void setNight()
    {
        approacher.setDay(false);
        base.setNight();

    }
}
