﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private const float FUDGE_FACTOR = 0.05f;
	
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
        if (this.transform.position.y < Constants.MOVEMENT_FLOOR) {
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
		this.transform.position += delta;
	}
	
	void GenerateSight(Vector3 mousePoint) {
        var linePos = this.transform.position;
        linePos.y += FUDGE_FACTOR;
        mousePoint.y += FUDGE_FACTOR;
		line.SetPosition(0, linePos);
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