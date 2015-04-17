using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	LineRenderer line;
	private float speed = 0.06f;
	private float angle = 0.0f;

	void Start() {
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = true;
		line.SetVertexCount(2);
		line.useWorldSpace = true;
	}

	// Update is called once per frame
	void Update () {
		Rotate();
		UpdatePosition();
		GenerateSight();
	}
	
	void Rotate() {
		GetMouseAngle();
		this.transform.eulerAngles = new Vector3(0, this.angle, 0);
	}

	void UpdatePosition() {
		Vector3 delta = Vector3.zero;
		
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey("w")) {
			delta.z += 1;
		}
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey("s")) {
			delta.z -= 1;
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey("a")) {
			delta.x -= 1;
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey("d")) {
			delta.x += 1;
		}
		delta.Normalize();
		delta *= speed;
		this.transform.position += delta;
	}

	void GenerateSight() {
		line.SetPosition (0, this.transform.position);
		var v3 = Input.mousePosition;
		v3.z = Camera.main.nearClipPlane;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		line.SetPosition (1, v3);
		Debug.Log ("Character: " + this.transform.position);
		Debug.Log("Mouse: " + v3);
	}

	void GetMouseAngle() {
		Vector3 characterPosition = Camera.main.WorldToScreenPoint(this.transform.position);
		Vector3 mouse = Input.mousePosition;


		Vector3 translated = Vector3.zero;
		translated.x = mouse.x - characterPosition.x;
		translated.y = mouse.y - characterPosition.y;

		this.angle = Vector3.Dot(Vector3.up, translated);
		this.angle /= (Vector3.up.magnitude * translated.magnitude);
		this.angle = Mathf.Acos(this.angle);
		this.angle = this.angle * 180 / Mathf.PI;
		if (mouse.x <= characterPosition.x) {
			this.angle = 360 - this.angle;
		}
	}
}
