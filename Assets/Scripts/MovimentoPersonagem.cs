using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    private Rigidbody meuRigidbody;

    void Awake ()
    {
        meuRigidbody = GetComponent<Rigidbody>();
    }

    public void Movimentar (Vector3 direcao, float velocidade)
    {
        meuRigidbody.MovePosition(
                meuRigidbody.position +
                direcao.normalized * velocidade * Time.deltaTime);
    }

    public void Rotacionar (Vector3 direcao)
    {
        //Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        //meuRigidbody.MoveRotation(novaRotacao);

        var speed = 20;
        Vector3 novaDirecao = Vector3.RotateTowards(transform.forward, direcao, speed * Time.deltaTime, 0.0f);
        Quaternion novaRotacao = Quaternion.LookRotation(novaDirecao);
        meuRigidbody.MoveRotation(novaRotacao);
    }

    public void Morrer()
    {

        meuRigidbody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(ExecuteWithDelay());

    }

    private IEnumerator ExecuteWithDelay()
    {
        yield return new WaitForSeconds(15f); // Defina o tempo de espera em segundos
        meuRigidbody.constraints = RigidbodyConstraints.None;
        meuRigidbody.isKinematic = false;
    }
}
