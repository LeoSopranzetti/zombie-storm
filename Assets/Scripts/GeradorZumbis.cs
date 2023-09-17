﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi;
    private float contadorTempo = 0;
    public float TempoGerarZumbi = 1;
    public LayerMask LayerZumbi;
    private float distanciaDeGeracao = 3;
    private float DistanciaDoJogadorParaGeracao = 13;
    private GameObject jogador;
    private int quantidadeMaximaDeZumbisVivos = 2;
    private int quantidadeDeZumbisVivos;
    private float contadorAumentoSpawnRate;
    private int vidaExtraZumbis = 0;
    private float tempoProximoAumentoSpawnRate = 30;
    private float tempoProximaVidaExtraZumbis = 30;
    private float contadorVidaExtraZumbis;

    private void Start()
    {
        jogador = GameObject.FindWithTag("Jogador");
        contadorAumentoSpawnRate = tempoProximoAumentoSpawnRate;
        contadorVidaExtraZumbis = tempoProximaVidaExtraZumbis;
        for (int i = 0; i < quantidadeMaximaDeZumbisVivos; i++)
        {
            StartCoroutine(GerarNovoZumbi());
        }

    }

    // Update is called once per frame
    void Update () {
        bool possoGerarZumbisPelaDistancia = Vector3.Distance(transform.position, jogador.transform.position) > DistanciaDoJogadorParaGeracao;

        if (possoGerarZumbisPelaDistancia == true && quantidadeDeZumbisVivos < quantidadeMaximaDeZumbisVivos)
        {
            contadorTempo += Time.deltaTime;

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0;
            }
        }

        if (Time.timeSinceLevelLoad > contadorAumentoSpawnRate)
        {
            quantidadeMaximaDeZumbisVivos++;
            contadorAumentoSpawnRate = Time.timeSinceLevelLoad + contadorAumentoSpawnRate;
        }

        if (Time.timeSinceLevelLoad > contadorVidaExtraZumbis)
        {
            vidaExtraZumbis++;
            contadorVidaExtraZumbis = Time.timeSinceLevelLoad + contadorVidaExtraZumbis;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeGeracao);
    }

    IEnumerator GerarNovoZumbi ()
    {
        Vector3 posicaoDeCriacao = AleatorizarPosicao();
        Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);

        while(colisores.Length > 0)
        {
            posicaoDeCriacao = AleatorizarPosicao();
            colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);
            yield return null;
        }

        ControlaInimigo zumbi  = Instantiate(Zumbi, posicaoDeCriacao, transform.rotation).GetComponent<ControlaInimigo>();
        if (vidaExtraZumbis >= 0)
        {
            zumbi.GetComponent<Status>().VidaInicial += vidaExtraZumbis;
            zumbi.GetComponent<Status>().Vida += vidaExtraZumbis;
            zumbi.GetComponent<Status>().attack += 5;
        }
        zumbi.meuGerador = this;
        quantidadeDeZumbisVivos++;
    }

    Vector3 AleatorizarPosicao ()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao;
        posicao += transform.position;
        posicao.y = 0;

        return posicao;
    }

    public void diminuirQuantidadeDeZumbisVivos()
    {
        quantidadeDeZumbisVivos--;
    }
}
