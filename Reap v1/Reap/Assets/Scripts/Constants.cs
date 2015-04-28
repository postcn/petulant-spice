using UnityEngine;
using System.Collections;
using System;

public class Constants {
    public const int MAP_FLOOR = -3;

    public static IEnumerator Wait(int seconds, Action method) {
        yield return new WaitForSeconds(seconds);
        if (method != null) {
            method();
        }
    }
}
