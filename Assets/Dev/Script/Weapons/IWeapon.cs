using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IWeapon
{
    public void Attack(float bonusDmg);
    public void TurnOnOffWeapon(bool turnOnOff);

}
