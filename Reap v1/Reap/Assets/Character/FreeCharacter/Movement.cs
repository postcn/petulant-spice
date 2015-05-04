using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private const float FUDGE_FACTOR = 0.05f;
	private static float speed = 0.06f;
	
    public static void Move(Transform hero, Vector3 mousePoint) {
        Rotate(hero, mousePoint);
        UpdatePosition(hero);
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
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) {
			delta *= 2;
		}
		hero.position += delta;
	}
}