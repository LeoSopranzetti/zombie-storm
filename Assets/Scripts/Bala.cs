using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

    public float Velocidade = 20;
    private Rigidbody rigidbodyBala;
    public AudioClip SomDeMorte;
    private GameObject jogador;

    private void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>();
        jogador = GameObject.FindWithTag("Jogador");
    }

    // Update is called once per frame
    void FixedUpdate () {
        rigidbodyBala.MovePosition
            (rigidbodyBala.position + 
            transform.forward * Velocidade * Time.deltaTime);
	}

    void OnTriggerEnter(Collider objetoDeColisao)
    {
        float playerAttack = jogador.GetComponent<Status>().attack;
        Quaternion rotacaoOpostoABala = Quaternion.LookRotation(-transform.forward);
        switch (objetoDeColisao.tag)
        {
            case "Inimigo":
                ControlaInimigo scriptControlaInimigo = objetoDeColisao.GetComponent<ControlaInimigo>();
                scriptControlaInimigo.TomarDano(playerAttack);
                scriptControlaInimigo.ParticulaSangue(transform.position, rotacaoOpostoABala);
                break;
            case "Chefe":
                ControlaChefe scriptControlaChefe = objetoDeColisao.GetComponent<ControlaChefe>();
                scriptControlaChefe.TomarDano(playerAttack);
                scriptControlaChefe.ParticulaSangue(transform.position, rotacaoOpostoABala);
                break;
        }

        Destroy(gameObject);
    }
}
