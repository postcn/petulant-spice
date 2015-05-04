using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

    private static int framesToFire = 0;
    public GameObject bulletPrefab;

    public static void DoFire(Transform hero, Vector3 mousePoint, GameObject bulletPrefab) {
        if (framesToFire != 0) {
            framesToFire--;
            return;
        }
        if (!Input.GetMouseButton(0)) {
            return;
        }
        SetWeaponFireRate(Hero_Management.self.weapon);
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SendMessage("SetOrigin", hero);
        bullet.SendMessage("SetDestination", mousePoint);
    }

    public static void SetWeaponFireRate(Constants.WEAPONS weapon) {
        switch (Hero_Management.self.weapon) {
            case Constants.WEAPONS.Pistol:
                Fire.framesToFire = 30;
                break;
            case Constants.WEAPONS.Rifle:
                Fire.framesToFire = 45;
                break;
            case Constants.WEAPONS.MachineGun:
                Fire.framesToFire = 10;
                break;
            default:
                Fire.framesToFire = 60;
                break;
        }
    }
}