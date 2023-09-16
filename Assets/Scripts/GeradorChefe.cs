using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{

    private float tempoParaProximaGeracao = 0;
    public float tempoEntreGeracoes = 60;
    public GameObject chefePrefab;
    private ControlaInterface scriptControlaInterface;
    public Transform[] posicoesPossiveisDeGeracao;
    private Transform jogador;
    public AudioClip somDeGeracao;

    private void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes;
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        jogador = GameObject.FindWithTag("Jogador").transform;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > tempoParaProximaGeracao)
        {
            Vector3 posicaoDeCriacao = calcularPosicaoMaisDistanteDoJogador(); 
            Instantiate(chefePrefab, posicaoDeCriacao, Quaternion.identity);
            ControlaAudio.instancia.PlayOneShot(somDeGeracao);
            scriptControlaInterface.AparecerChefeCriado();
            tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes;
        }
    }

    Vector3 calcularPosicaoMaisDistanteDoJogador()
    {
        Vector3 posicaoMaiorDistancia = Vector3.zero;
        float maiorDistancia = 0;
        foreach (Transform posicao in posicoesPossiveisDeGeracao)
        {
            float distanciaEntreJogador = Vector3.Distance(posicao.position, jogador.position);
            if (distanciaEntreJogador > maiorDistancia)
            {
                maiorDistancia = distanciaEntreJogador;
                posicaoMaiorDistancia = posicao.position;
            }
        }


        return posicaoMaiorDistancia;
    }

}
