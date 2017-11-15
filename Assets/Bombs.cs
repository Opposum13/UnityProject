using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : Collectable
{

    protected override void OnRabitHit(HeroRabit rabit)
    {
        this.CollectedHide();
        if (rabit.isBig == true)
        {
            rabit.becomeSmall();
        }
        else
        {
            rabit.dead = true;
            
        }
    }
}
