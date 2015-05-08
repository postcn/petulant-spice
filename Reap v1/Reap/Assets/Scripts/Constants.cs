using UnityEngine;
using System.Collections;
using System;

public class Constants {
    public const int MOVEMENT_FLOOR = -1;
    public const int MAP_FLOOR = -30;
    public const float COLLISION_DISTANCE = 0.5f;
    public const String HERO_TAG = "Hero";
    public const String ENEMY_TAG = "Enemy";

    public enum DEATH_REASONS {Fire, Fighting, Bloodlust, DropShip};

    public enum WEAPONS {Pistol, Rifle, MachineGun};

    public static IEnumerator Wait(int seconds, Func<DEATH_REASONS, Void> method, DEATH_REASONS reason) {
        yield return new WaitForSeconds(seconds);
        if (method != null) {
            method(reason);
        }
    }

    public static void playRandomAudio(AudioClip[] clips) {
        System.Random rnd = new System.Random();
        int index = rnd.Next(clips.Length);
        AudioSource.PlayClipAtPoint(clips[index], Camera.main.transform.position);
    }

    //Accurate only to hours. Breaks after that.
    public static String formatSecondsToMinute(int seconds) {
        int step = 60;
        int current = 60;
        bool shortened = false;
        int val = seconds % current;
        if (val < 10) {
            shortened = true;
        }
        String ret = "" + val;
        while (seconds / current > 0) {
            String lastPiece = shortened ? "0" + ret : ret;
            val = (seconds / current) % step;
            if (val < 10) {
                shortened = true;
            }
            ret = val + ":" + lastPiece;
            current *= step;
        }

        return ret;
    }
}
