using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

    private static int framesToFire = 0;
    public GameObject bulletPrefab;

    public static void DoFire(Transform hero, Vector3 mousePoint, float maxAngle, GameObject bulletPrefab, bool mousePlayer) {
        if (framesToFire != 0) {
            framesToFire--;
            return;
        }
        if (!((mousePlayer && Input.GetMouseButton(0)) || (!mousePlayer && Input.GetAxis("Trigger") <= -.3f))) {
            return;
        }
        if (!Hero_Management.self.fireWeapon(1)) {
            return;
        }

        //Calculate rotated mouse point for bloodlust
        Vector3 destination = Vector3.zero;
        float angle = Random.Range(0f, maxAngle) * (Random.Range(0, 2) == 0 ? -1 : 1);
        Vector3 heading = mousePoint - Camera.main.transform.position;
        Vector3 rayDirection = heading / heading.magnitude;
        Plane characterPlane = new Plane(hero.up, hero.position);
        Ray mouseRay = new Ray(Camera.main.transform.position, rayDirection);
        float distance;
        Vector3 direction = Quaternion.AngleAxis(-angle, Camera.main.transform.forward) * mouseRay.direction;
        Ray negative = new Ray(Camera.main.transform.position, direction);
        
        if (characterPlane.Raycast(negative, out distance))
        {
            destination = negative.GetPoint(distance);
            destination.y = hero.position.y + .2f;
        }

        //Instantiate game object
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SendMessage("SetOrigin", hero);
        bullet.SendMessage("SetDestination", destination);
        bullet.SendMessage("SetDamage", GetDamage());

        //Hatch nearby cocoons
        GameObject[] cocoons = GameObject.FindGameObjectsWithTag("Cocoon");
        for (int i = 0; i < cocoons.Length; i++)
        {
            cocoons[i].SendMessage("CheckHatchFireBullet", Hero_Management.self.gameObject.transform.position);
        }

        //Reset fire rate
        SetWeaponFireRate(Hero_Management.self.weapon);
    }

    private static float GetDamage() {
        switch (Hero_Management.self.weapon) {
            case Constants.WEAPONS.Pistol:
                //return 30f * GetDamageModifier();
                return 35f * GetDamageModifier();
            case Constants.WEAPONS.Rifle:
                //return 60f * GetDamageModifier();
                return 75f * GetDamageModifier();
            case Constants.WEAPONS.MachineGun:
              //  return 40f * GetDamageModifier();
                return 10f * GetDamageModifier();
            default:
                return 0f; //Unimplemented weapon. Why doesn't C# have pattern matching?
        }
    }

    private static float GetDamageModifier() {
        return 0.9947f * Mathf.Exp(.011f * Hero_Management.self.getBloodlustCount());
    }

    public static void SetWeaponFireRate(Constants.WEAPONS weapon) {
        switch (Hero_Management.self.weapon) {
            case Constants.WEAPONS.Pistol:
                //Fire.framesToFire = 30;
                Fire.framesToFire = 15;
                break;
            case Constants.WEAPONS.Rifle:
                //Fire.framesToFire = 45;
                Fire.framesToFire = 25;
                break;
            case Constants.WEAPONS.MachineGun:
                //Fire.framesToFire = 10;
                Fire.framesToFire = 3;
                break;
            default:
                Fire.framesToFire = 60;
                break;
        }
    }
}