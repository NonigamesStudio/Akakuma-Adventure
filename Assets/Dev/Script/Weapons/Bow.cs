using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] List<Arrow> poolArrows;
    [SerializeField] float dmg;
    [SerializeField] float coolDownTime;
    [SerializeField] float speedArrow;
    [SerializeField] public Player player;
    

    bool coolDown = true;

    private void Awake()
    {
        foreach (Arrow arrow in poolArrows)
        {
            arrow.dmg = dmg;
            arrow.bow = this;
        }
    }

    public void Attack(float bonusDmg)
    {
        player.OnBowRealese?.Invoke();
        AnimController_Player.ins.PlayAnim(AnimNamesPlayer.ReleaseBow);
        if (!coolDown) return;

        coolDown = false;
        LeanTween.delayedCall(coolDownTime, () => { coolDown = true; });

        foreach (Arrow arrow in poolArrows)
        {
            if(!arrow.gameObject.activeSelf)
            {
                arrow.gameObject.SetActive(true);
                arrow.ThrowArrow(speedArrow);
                break;
            }
        }
    }
    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }
}
