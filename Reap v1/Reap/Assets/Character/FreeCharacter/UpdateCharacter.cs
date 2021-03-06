using UnityEngine;
using System.Collections;

public class UpdateCharacter : MonoBehaviour {

    LineRenderer line;
    public GameObject bullet;
    bool managementLoaded = false;
	public Hero_Management hero;
    float lastAngle = 90f;

    void Start() {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        line.SetVertexCount(2);
        line.useWorldSpace = true;
    }
    
    void Update() {
        Vector3 mousePoint = hero.isMousePlayer ? GetMousePoint() : GetJoystickPoint();
        float maxAngle = MaxAngle(hero);

        //Don't fire until we've loaded hero management. Otherwise we don't know what we're shooting.
        managementLoaded = (hero != null);
        if (managementLoaded) {
            Fire.DoFire(hero, mousePoint, maxAngle, bullet, hero.isMousePlayer);  
        }

        Movement.Move(hero, mousePoint, hero.isMousePlayer);
        mousePoint = hero.isMousePlayer ? GetMousePoint() : GetJoystickPoint(); //Update the mouse point in the new, shifted world position
        GenerateSight.Generate(line, hero, mousePoint, maxAngle);
        Cheat(hero);
    }
    
    Vector3 GetMousePoint() {
		Ray ray = hero.followingCamera.ScreenPointToRay(Input.mousePosition);
        Plane characterPlane = new Plane (this.transform.up, this.transform.position);
        float distance = 0; 
        if (characterPlane.Raycast(ray, out distance)) { //This should always occur, because the plane runs parallel through the character
            var mousePoint = ray.GetPoint(distance); //This point is always *really* close to the plane
            mousePoint.y = this.transform.position.y; //But we can ensure that it lies on the plane so we don't cause any weird rotations in Rotate(mousePoint)
            return mousePoint;
        }
        return this.transform.position;
    }

    Vector3 GetJoystickPoint() {
        float vertical = Input.GetAxis("GuideVertical");
        float horizontal = Input.GetAxis("GuideHorizontal");

        float angle;
        if (vertical * vertical + horizontal * horizontal <= 0.2f) {
            angle = lastAngle;
        }
        else {
            angle = Mathf.Atan2(vertical, horizontal) * 180f / Mathf.PI;
            lastAngle = angle;
        }

        Vector3 joystickPoint = this.transform.position;
        joystickPoint.x += 4f;
		Vector3 heading = joystickPoint - hero.followingCamera.transform.position;
        Vector3 rayDirection = heading / heading.magnitude;
        Ray joystickRay = new Ray(hero.followingCamera.transform.position, rayDirection);
        Plane characterPlane = new Plane(this.transform.up, this.transform.position);
        
        float distance;
        Vector3 r;
		Vector3 direction = Quaternion.AngleAxis(angle, hero.followingCamera.transform.forward) * joystickRay.direction;
		Ray negative = new Ray(hero.followingCamera.transform.position, direction);
        
        if (characterPlane.Raycast(negative, out distance))
        {
            r = negative.GetPoint(distance);
            r.y = this.transform.position.y;
            return r;
        }
        
        return this.transform.position;
    }

    float MaxAngle(Hero_Management hero) {
        int count = hero.getBloodlustCount();
        return (0.0084f * count * count + 0.0564f * count - 0.1818f);
    }

    void Cheat(Hero_Management hero) {
        if (hero.isMousePlayer) {
            if (Input.GetKey(KeyCode.Alpha1)) {
                hero.weapon = Constants.WEAPONS.Pistol;
            }
            else if (Input.GetKey(KeyCode.Alpha2)) {
                hero.weapon = Constants.WEAPONS.Rifle;
            }
            else if (Input.GetKey(KeyCode.Alpha3)) {
                hero.weapon = Constants.WEAPONS.MachineGun;
            }
        }
        else {
            if (Input.GetButton("JoystickX")) {
                hero.weapon = Constants.WEAPONS.Pistol;
            }
            else if (Input.GetButton("JoystickY")) {
                hero.weapon = Constants.WEAPONS.Rifle;
            }
            else if (Input.GetButton("JoystickB")) {
                hero.weapon = Constants.WEAPONS.MachineGun;
            }
        }
    }
}