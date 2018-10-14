using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaboem : MonoBehaviour {
    public AudioSource myAudio;

    private void OnEnable() {
        myAudio.Play();
    }
}
