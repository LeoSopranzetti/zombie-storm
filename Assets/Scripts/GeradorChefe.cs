using System.Collections;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{

    private float tempoParaProximaGeracao = 0;
    public float tempoEntreGeracoes = 60;
    private float distanciaDeGeracao = 3;
    public GameObject chefePrefab;
    private ControlaInterface scriptControlaInterface;
    public Transform[] posicoesPossiveisDeGeracao;
    private Transform jogador;
    public float increaseBossLife = 0;
    public float increaseBossDamage = 0;
    public LayerMask LayerBoss;
    private float distanceFromPlayerToSpawn = 20f;


    private void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes;
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        jogador = GameObject.FindWithTag("Jogador").transform;
    }


    public void spawnBoss()
    {
        StartCoroutine(spawnBossCoroutine());
    }

    IEnumerator spawnBossCoroutine()
    {
        Vector3 posicaoDeCriacao = randomPosition();
        Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerBoss);

        //Checa se a posição aleatoria gerada está longe do jogador
        bool safeDistanceBewteenPlayer = Vector3.Distance(posicaoDeCriacao, jogador.transform.position) > distanceFromPlayerToSpawn;

        while (colisores.Length <= 0 && !safeDistanceBewteenPlayer)
        {
            posicaoDeCriacao = randomPosition();
            colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerBoss);
            safeDistanceBewteenPlayer = Vector3.Distance(posicaoDeCriacao, jogador.transform.position) > distanceFromPlayerToSpawn;
            yield return null;
        }
        GameObject newBoss = Instantiate(chefePrefab, posicaoDeCriacao, Quaternion.identity);
        addStatsPerSpawn(newBoss);
        scriptControlaInterface.AparecerChefeCriado();
        tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes;
    }


        Vector3 randomPosition()
    {
        int getRandomIndex = Random.Range(0, posicoesPossiveisDeGeracao.Length);

        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao;
        posicao += posicoesPossiveisDeGeracao[getRandomIndex].transform.position;
        posicao.y = 0;


        return posicao;
    }

    void addStatsPerSpawn(GameObject newBoss)
    {
        Status statusNewBoss = newBoss.GetComponent<Status>();
        statusNewBoss.VidaInicial += increaseBossLife;
        statusNewBoss.Vida += increaseBossLife;
        statusNewBoss.attack += increaseBossDamage;
    }

    public void updaateBossStatus(float updateBossLife, float updateBossDamage)
    {
        increaseBossLife = updateBossLife;
        increaseBossDamage = updateBossDamage;
    }



}
