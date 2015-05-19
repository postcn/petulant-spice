using UnityEngine;
using System.Collections;

public class GenerateSight : MonoBehaviour {

    private static float FUDGE_FACTOR = 0.05f;

    public static void Generate(LineRenderer line, Hero_Management hero, Vector3 mousePoint, float angle) {
        line.SetVertexCount(3);
        Vector3 heading = mousePoint - hero.followingCamera.transform.position;
        Vector3 rayDirection = heading / heading.magnitude;
        Ray mouseRay = new Ray(hero.followingCamera.transform.position, rayDirection);
        Plane characterPlane = new Plane(hero.gameObject.transform.up, hero.gameObject.transform.position);
        
        float distance;
        Vector3 r;
		Vector3 direction = Quaternion.AngleAxis(-angle, hero.followingCamera.transform.forward) * mouseRay.direction;
		Ray negative = new Ray(hero.followingCamera.transform.position, direction);
        
        if (characterPlane.Raycast(negative, out distance))
        {
            r = negative.GetPoint(distance);
            r.y = hero.gameObject.transform.position.y + FUDGE_FACTOR;
            line.SetPosition(2, r);
        }
        
        r = hero.gameObject.transform.position;
        r.y += FUDGE_FACTOR;
        line.SetPosition(1, r);
        
		direction = Quaternion.AngleAxis(angle, hero.followingCamera.transform.forward) * mouseRay.direction;
		Ray positive = new Ray(hero.followingCamera.transform.position, direction);
        
        if (characterPlane.Raycast(positive, out distance)) {
            r = positive.GetPoint(distance);
            r.y = hero.gameObject.transform.position.y + FUDGE_FACTOR;
            line.SetPosition(0, r);
        }
    }
}