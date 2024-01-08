using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] List<GameObject> weaponsObj;
    List<IWeapon> weapons = new List<IWeapon>();
    private void Start()
    {
        foreach (GameObject weapon in weaponsObj)
        {
            weapons.Add(weapon.GetComponent<IWeapon>());
        }
    }

    public IWeapon GetWeaponSelected(int index)
    {
        foreach (IWeapon weapon in weapons)
        {
            weapon.TurnOnOffWeapon(false);
        }

        weapons[index].TurnOnOffWeapon(true);

        return weapons[index];
    }
}
