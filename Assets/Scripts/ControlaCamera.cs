using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaCamera : MonoBehaviour {

    public GameObject Jogador;
    private Vector3 distCompensar;
    public AudioClip cjHereWeGoAgain;

    // Use this for initialization
    void Start () {
        distCompensar = transform.position - Jogador.transform.position;
        ControlaAudio.instancia.PlayOneShot(cjHereWeGoAgain);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Jogador.transform.position + distCompensar;
	}
}
