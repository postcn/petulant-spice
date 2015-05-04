using UnityEngine;
using System.Collections;
using System;

public class Constants {
    public const int MOVEMENT_FLOOR = -1;
    public const int MAP_FLOOR = -30;
    public const float COLLISION_DISTANCE = 0.5f;
    public const String HERO_TAG = "Hero";
    public const String ENEMY_TAG = "Enemy";

    public enum DEATH_REASONS {Fire, Fighting, Bloodlust};

    public enum WEAPONS {Pistol, Rifle, MachineGun};

    public static IEnumerator Wait(int seconds, Func<DEATH_REASONS, Void> method, DEATH_REASONS reason) {
        yield return new WaitForSeconds(seconds);
        if (method != null) {
            method(reason);
        }
    }
}
