using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

    public float Velocidade = 20;
    private Rigidbody rigidbodyBala;
    public AudioClip SomDeMorte;

    private void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        rigidbodyBala.MovePosition
            (rigidbodyBala.position + 
            transform.forward * Velocidade * Time.deltaTime);
	}

    void OnTriggerEnter(Collider objetoDeColisao)
    {
        Quaternion rotacaoOpostoABala = Quaternion.LookRotation(-transform.forward);
        switch (objetoDeColisao.tag)
        {
            case "Inimigo":
                ControlaInimigo scriptControlaInimigo = objetoDeColisao.GetComponent<ControlaInimigo>();
                scriptControlaInimigo.TomarDano(1);
                scriptControlaInimigo.ParticulaSangue(transform.position, rotacaoOpostoABala);
                break;
            case "Chefe":
                ControlaChefe scriptControlaChefe = objetoDeColisao.GetComponent<ControlaChefe>();
                scriptControlaChefe.TomarDano(1);
                scriptControlaChefe.ParticulaSangue(transform.position, rotacaoOpostoABala);
                break;
        }

        Destroy(gameObject);
    }
}
