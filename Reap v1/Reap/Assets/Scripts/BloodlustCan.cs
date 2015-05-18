﻿using UnityEngine;
using System.Collections;

public class BloodlustCan : MonoBehaviour {
    
    public GameObject replacementObject;
    public GameObject thisCan;
    
    private bool resupplying = false;
    private AudioSource resupply_audio;
    private Light halo;
    private float step = -0.01f;
    
    // Use this for initialization
    void Start () {
        resupply_audio = thisCan.GetComponent<AudioSource>();
        halo = thisCan.GetComponent<Light>();
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
            Hero_Management.self.clearBloodlust();
            Destroy(thisCan);
            GameObject o = Instantiate(replacementObject);
            o.transform.position = Hero_Management.self.transform.position;
        }
        halo.intensity += step;
        if (halo.intensity <= 0 || halo.intensity >= 1) {
            step *= -1;
        }
    }
}