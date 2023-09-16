using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaArma : MonoBehaviour {

    public GameObject Bala;
    public GameObject CanoDaArma;
    public AudioClip SomDoTiro;
    private bool podeAtirar = true;
    private float taxaDeDisparo = 0.3f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") && podeAtirar)
        {
            //Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
            //ControlaAudio.instancia.PlayOneShot(SomDoTiro);

            StartCoroutine(Atirar());


        }
	}

    private IEnumerator Atirar()
    {
        podeAtirar = false; // Impede novos disparos temporariamente

        // Cria uma bala
        Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        ControlaAudio.instancia.PlayOneShot(SomDoTiro);

        // Aguarda o intervalo de tempo antes de poder atirar novamente
        float milissegundos = taxaDeDisparo * 1000f;
        yield return new WaitForSeconds(milissegundos / 1000f);

        podeAtirar = true; // Permite atirar novamente
    }
}
