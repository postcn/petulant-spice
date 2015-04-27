using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public static CameraFollow S;

	public Transform targetTransform;
	public float camEasing = 0.1f;
	Vector3 followOffset = new Vector3(0,10,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake() {
		S = this;
	}

	void FixedUpdate() {
        if (targetTransform == null) {
            return;
        }
		Vector3 pos = targetTransform.position+followOffset;
		transform.position = Vector3.Lerp (transform.position, pos, camEasing);
	}
}
