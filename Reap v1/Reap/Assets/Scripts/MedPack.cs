using UnityEngine;
using System.Collections;

public class MedPack : MonoBehaviour {

    public const int MEDPACK_AMOUNT = 50;

    public GameObject replacementObject;
    public GameObject thisMedpack;

    private bool healing = false;
    private AudioSource heal_audio;
    private Light halo;
    private float step = -0.01f;

	// Use this for initialization
	void Start () {
        heal_audio = thisMedpack.GetComponent<AudioSource>();
        halo = this.thisMedpack.GetComponent<Light>();
	}

    void OnCollisionEnter(Collision collision) {
        if (healing) {
            return;
        }
        if (collision.gameObject.tag == Constants.HERO_TAG) {
            healing = true;
            heal_audio.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (healing && !heal_audio.isPlaying) {
            Hero_Management.self.heal(MEDPACK_AMOUNT);
            Destroy(thisMedpack);
            GameObject o = Instantiate(replacementObject);
            o.transform.position = Hero_Management.self.transform.position;
        }

        halo.intensity += step;
        if (halo.intensity <= 0 || halo.intensity >= 1) {
            step *= -1;
        }
	}
   
}
