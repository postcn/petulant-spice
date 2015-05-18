using UnityEngine;
using System.Collections;

public class GenerateSight : MonoBehaviour {

    private static float FUDGE_FACTOR = 0.05f;

    public static void Generate(LineRenderer line, Transform hero, Vector3 mousePoint, float angle) {
        line.SetVertexCount(3);
        Vector3 heading = mousePoint - Camera.main.transform.position;
        Vector3 rayDirection = heading / heading.magnitude;
        Ray mouseRay = new Ray(Camera.main.transform.position, rayDirection);
        Plane characterPlane = new Plane(hero.up, hero.position);
        
        float distance;
        Vector3 r;
        Vector3 direction = Quaternion.AngleAxis(-angle, Camera.main.transform.forward) * mouseRay.direction;
        Ray negative = new Ray(Camera.main.transform.position, direction);
        
        if (characterPlane.Raycast(negative, out distance))
        {
            r = negative.GetPoint(distance);
            r.y = hero.position.y + FUDGE_FACTOR;
            line.SetPosition(2, r);
        }
        
        r = hero.position;
        r.y += FUDGE_FACTOR;
        line.SetPosition(1, r);
        
        direction = Quaternion.AngleAxis(angle, Camera.main.transform.forward) * mouseRay.direction;
        Ray positive = new Ray(Camera.main.transform.position, direction);
        
        if (characterPlane.Raycast(positive, out distance)) {
            r = positive.GetPoint(distance);
            r.y = hero.position.y + FUDGE_FACTOR;
            line.SetPosition(0, r);
        }
    }
}