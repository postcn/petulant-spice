using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private const float FUDGE_FACTOR = 0.05f;
	private static float speed = 0.06f;
	
    public static void Move(Hero_Management hero, Vector3 mousePoint, bool mousePlayer) {
        Rotate(hero.gameObject.transform, mousePoint);
        if (mousePlayer) {
            UpdatePosition(hero);
        }
        else {
            UpdatePositionWithJoystick(hero);
        }

        GameObject[] cocoons = GameObject.FindGameObjectsWithTag("Cocoon");
        for (int i = 0; i < cocoons.Length; i++)
        {
            cocoons[i].SendMessage("CheckHatchMovement", hero.gameObject.transform.position);
        }
    }

	private static void Rotate(Transform hero, Vector3	mousePoint) {
		hero.LookAt(mousePoint);
	}
	
	private static void UpdatePosition(Hero_Management hero) {
        if (hero.gameObject.transform.position.y < Constants.MOVEMENT_FLOOR) {
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
        if (hero != null && hero.getBloodlustCount() > Hero_Management.BREATHING_THRESHOLD) {
            float lust = (float) hero.getBloodlustCount();
            delta *= (1.0f -  (lust - Hero_Management.BREATHING_THRESHOLD) / Hero_Management.MAX_BLOODLUST);
        }
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) {
			delta *= 2;
		}
		hero.gameObject.transform.position += delta;
	}

    private static void UpdatePositionWithJoystick(Hero_Management hero) {
        Vector3 delta = Vector3.zero;
        delta.x = Input.GetAxis("Horizontal");
        delta.z = Input.GetAxis("Vertical");
        //delta.Normalize();
        delta *= speed;
        //Make the hero slower based upon
        if (hero != null && hero.getBloodlustCount() > Hero_Management.BREATHING_THRESHOLD) {
            float lust = (float) hero.getBloodlustCount();
            delta *= (1.0f -  (lust - Hero_Management.BREATHING_THRESHOLD) / Hero_Management.MAX_BLOODLUST);
        }
        if (Input.GetButton("LeftJoyButton")) {
            delta *= 2;
        }
        hero.gameObject.transform.position += delta;
    }
}