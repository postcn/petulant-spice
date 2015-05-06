using UnityEngine;
using System.Collections;

public class AmmoPack : MonoBehaviour {

    public const int AMMO_AMOUNT = 50;
    
    public GameObject replacementObject;
    public GameObject thisAmmopack;
    
    private bool resupplying = false;
    private AudioSource resupply_audio;
    private Light halo;
    private float step = -0.01f;
    
    // Use this for initialization
    void Start () {
        resupply_audio = thisAmmopack.GetComponent<AudioSource>();
        halo = this.thisAmmopack.GetComponent<Light>();
    }
    
    void OnCollisionEnter(Collision collision) {
        if (resupplying) {
            return;
        }
        if (collision.gameObject.tag == Constants.HERO_TAG) {
            resupplying = true;
            resupply_audio.Play();
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (resupplying && !resupply_audio.isPlaying) {
            Hero_Management.self.heal(AMMO_AMOUNT);
            Destroy(thisAmmopack);
            GameObject o = Instantiate(replacementObject);
            o.transform.position = Hero_Management.self.transform.position;
        }
        halo.intensity += step;
        if (halo.intensity <= 0 || halo.intensity >= 1) {
            step *= -1;
        }
    }
}
