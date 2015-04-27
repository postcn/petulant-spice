using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	LineRenderer line;
	private float speed = 0.06f;
	
	void Start() {
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = true;
		line.SetVertexCount(2);
		line.useWorldSpace = true;
	}
	
	void Update () {
		var mousePoint = GetMousePoint();
		Rotate(mousePoint);
		UpdatePosition();
		GenerateSight(mousePoint);
	}
	
	void Rotate(Vector3	mousePoint) {
		this.transform.LookAt(mousePoint);
	}
	
	void UpdatePosition() {
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
		this.transform.position += delta;
	}
	
	void GenerateSight(Vector3 mousePoint) {
		line.SetPosition(0, this.transform.position);
		line.SetPosition(1, mousePoint);
	}
	
	Vector3 GetMousePoint() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane characterPlane = new Plane (this.transform.up, this.transform.position);
		float distance = 0; 
		if (characterPlane.Raycast(ray, out distance)) { //This should always occur, because the plane runs parallel through the character
			var mousePoint = ray.GetPoint(distance); //This point is always *really* close to the plane
			mousePoint.y = this.transform.position.y; //But we can ensure that it lies on the plane so we don't cause any weird rotations in Rotate(mousePoint)
			return mousePoint;
		}
		return this.transform.position;
	}
}