using UnityEngine;
using System.Collections;

public class GenerateSight : MonoBehaviour {

    private static float FUDGE_FACTOR = 0.05f;
    private static Vector3 FUDGE_VECTOR = new Vector3(0, FUDGE_FACTOR, 0);

    public static void Generate(LineRenderer line, Vector3 hero, Vector3 mousePoint) {
        line.SetPosition(0, hero);
        line.SetPosition(1, mousePoint + FUDGE_VECTOR);
    }
}