using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour {

    private AudioSource sourec;
    //private bool is_Playing;
	// Use this for initialization
	void Start () {
        sourec = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!sourec.isPlaying)
        {
            sourec.Play();
        }
	}
}
