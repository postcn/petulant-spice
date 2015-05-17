using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private const float FUDGE_FACTOR = 0.05f;
	private static float speed = 0.06f;
	
    public static void Move(Transform hero, Vector3 mousePoint, bool mousePlayer) {
        Rotate(hero, mousePoint);
        if (mousePlayer) {
            UpdatePosition(hero);
        }
        else {
            UpdatePositionWithJoystick(hero);
        }
    }

	private static void Rotate(Transform hero, Vector3	mousePoint) {
		hero.LookAt(mousePoint);
	}
	
	private static void UpdatePosition(Transform hero) {
        if (hero.position.y < Constants.MOVEMENT_FLOOR) {
            return;
        }
		Vector3 delta = Vector3.zero;
		
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")) {
			delta.z += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s")) {
			delta.z -= 1;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a")) {
			delta.x -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d")) {
			delta.x += 1;
		}
		delta.Normalize();
		delta *= speed;
        //Make the hero slower based upon
        if (Hero_Management.self != null && Hero_Management.self.getBloodlustCount() > Hero_Management.BREATHING_THRESHOLD) {
            float lust = (float) Hero_Management.self.getBloodlustCount();
            delta *= (1.0f -  (lust - Hero_Management.BREATHING_THRESHOLD) / Hero_Management.MAX_BLOODLUST);
        }
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) {
			delta *= 2;
		}
		hero.position += delta;
	}

    private static void UpdatePositionWithJoystick(Transform hero) {
        Vector3 delta = Vector3.zero;
        delta.x = Input.GetAxis("Horizontal");
        delta.z = Input.GetAxis("Vertical");
        //delta.Normalize();
        delta *= speed;
        //Make the hero slower based upon
        if (Hero_Management.self != null && Hero_Management.self.getBloodlustCount() > Hero_Management.BREATHING_THRESHOLD) {
            float lust = (float) Hero_Management.self.getBloodlustCount();
            delta *= (1.0f -  (lust - Hero_Management.BREATHING_THRESHOLD) / Hero_Management.MAX_BLOODLUST);
        }
        if (Input.GetAxis("Trigger") >= .3f) {
            delta *= 2;
        }
        hero.position += delta;
    }
}