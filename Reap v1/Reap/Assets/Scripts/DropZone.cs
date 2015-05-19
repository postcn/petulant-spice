using UnityEngine;
using System.Collections;

public class DropZone : MonoBehaviour {
    private const int RADIUS = 2;
    public GameObject drop;
    private bool activated = false;

	// Use this for initialization
	void Start () {
        DropZoneManager.self.register(this);
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) {
            r.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (activated && (Hero_Management.mousePlayer != null || Hero_Management.controllerPlayer != null) ) {
			if (Hero_Management.mousePlayer != null) {
				float f = distance(drop.transform.position, Hero_Management.mousePlayer.transform.position);
				if (f < RADIUS) {
					if (Hero_Management.controllerPlayer == null) {
						DropZoneManager.self.picked_up = true;
					}
					Hero_Management.mousePlayer.pickUp();
				}
			}
			if (Hero_Management.controllerPlayer != null) {
				float f = distance(drop.transform.position, Hero_Management.controllerPlayer.transform.position);
				if (f < RADIUS) {
					if (Hero_Management.mousePlayer == null) {
						DropZoneManager.self.picked_up = true;
					}
					Hero_Management.controllerPlayer.pickUp();
				}
			}
        }
	}

    private float distance(Vector3 point1, Vector3 point2) {
        float x = Mathf.Pow(point1.x - point2.x, 2);
        float y = Mathf.Pow(point1.y - point2.y, 2);
        float z = Mathf.Pow(point1.z - point2.z, 2);
        return Mathf.Sqrt(x+y+z);
    }

    public void activate() {
        activated = true;
        var renderers = GetComponentsInChildren<Renderer>();;
        foreach (Renderer r in renderers) {
            r.enabled = true;
        }
    }
}
