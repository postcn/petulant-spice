using UnityEngine;
using System.Collections;

public class UpdateCharacter : MonoBehaviour {

    LineRenderer line;
    public GameObject bullet;
    bool managementLoaded = false;

    void Start() {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        line.SetVertexCount(2);
        line.useWorldSpace = true;
    }
    
    void Update() {
        //Don't fire until we've loaded hero management. Otherwise we don't know what we're shooting.
        Vector3 mousePoint = GetMousePoint();

        managementLoaded = (Hero_Management.self != null);
        if (managementLoaded) {
            Fire.DoFire(this.transform, mousePoint, bullet);
        }

        Movement.Move(this.transform, mousePoint);
        GenerateSight.Generate(line, this.transform.position, mousePoint);
        Cheat();
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

    void Cheat() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Hero_Management.self.weapon = Constants.WEAPONS.Pistol;
        }
        else if (Input.GetKey(KeyCode.Alpha2)) {
            Hero_Management.self.weapon = Constants.WEAPONS.Rifle;
        }
        else if (Input.GetKey(KeyCode.Alpha3)) {
            Hero_Management.self.weapon = Constants.WEAPONS.MachineGun;
        }
    }
}