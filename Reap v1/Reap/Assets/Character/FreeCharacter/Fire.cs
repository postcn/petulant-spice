using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

    private static int framesToFire = 0;
    public GameObject bulletPrefab;

    public static void DoFire(Hero_Management hero, Vector3 mousePoint, float maxAngle, GameObject bulletPrefab, bool mousePlayer) {
        if (framesToFire != 0) {
            framesToFire--;
            return;
        }
        if (!((mousePlayer && Input.GetMouseButton(0)) || (!mousePlayer && Input.GetAxis("Trigger") <= -.3f))) {
            return;
        }
        if (!hero.fireWeapon(1)) {
            return;
        }

        //Calculate rotated mouse point for bloodlust
        Vector3 destination = Vector3.zero;
        float angle = Random.Range(0f, maxAngle) * (Random.Range(0, 2) == 0 ? -1 : 1);
		Vector3 heading = mousePoint - hero.followingCamera.transform.position;
        Vector3 rayDirection = heading / heading.magnitude;
        Plane characterPlane = new Plane(hero.gameObject.transform.up, hero.gameObject.transform.position);
		Ray mouseRay = new Ray(hero.followingCamera.transform.position, rayDirection);
        float distance;
		Vector3 direction = Quaternion.AngleAxis(-angle, hero.followingCamera.transform.forward) * mouseRay.direction;
		Ray negative = new Ray(hero.followingCamera.transform.position, direction);
        
        if (characterPlane.Raycast(negative, out distance))
        {
            destination = negative.GetPoint(distance);
            destination.y = hero.gameObject.transform.position.y + .2f;
        }

        //Instantiate game object
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SendMessage("SetOrigin", hero.gameObject.transform);
        bullet.SendMessage("SetDestination", destination);
        bullet.SendMessage("SetDamage", GetDamage(hero));

        //Hatch nearby cocoons
        GameObject[] cocoons = GameObject.FindGameObjectsWithTag("Cocoon");
        for (int i = 0; i < cocoons.Length; i++)
        {
            cocoons[i].SendMessage("CheckHatchFireBullet", hero.gameObject.transform.position);
        }

        //Reset fire rate
        SetWeaponFireRate(hero.weapon);
    }

    private static float GetDamage(Hero_Management hero) {
        switch (hero.weapon) {
            case Constants.WEAPONS.Pistol:
                return 30f * GetDamageModifier(hero);
            case Constants.WEAPONS.Rifle:
                return 75f * GetDamageModifier(hero);
            case Constants.WEAPONS.MachineGun:
                return 40f * GetDamageModifier(hero);
            default:
                return 0f; //Unimplemented weapon. Why doesn't C# have pattern matching?
        }
    }

    private static float GetDamageModifier(Hero_Management hero) {
        return 0.9947f * Mathf.Exp(.011f * hero.getBloodlustCount());
    }

    public static void SetWeaponFireRate(Constants.WEAPONS weapon) {
        switch (weapon) {
            case Constants.WEAPONS.Pistol:
                Fire.framesToFire = 25;
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