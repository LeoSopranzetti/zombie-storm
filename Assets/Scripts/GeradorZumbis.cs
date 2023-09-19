using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi;
    public float TempoGerarZumbi = 1;
    public LayerMask LayerZumbi;
    private float distanciaDeGeracao = 3;
    private int quantidadeDeZumbisVivos;
    public Transform[] posicoesPossiveisDeGeracao;
    private float increaseZombieLife = 0;
    private float increaseZombieDamage = 0;
    private GameObject player;
    private float distanceFromPlayerToSpawn = 20f;


    private void Start()
    {
        player = GameObject.FindWithTag("Jogador");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeGeracao);
    }


    public void diminuirQuantidadeDeZumbisVivos()
    {
        quantidadeDeZumbisVivos--;
    }

    public void spawnZombie()
    {
        StartCoroutine(spawnZombieCoroutine());

    }

    IEnumerator spawnZombieCoroutine()
    {
        //Baseado na quantidade de posições dentro do gerador aleatoriza uma
        Vector3 posicaoDeCriacao = AleatorizarPosicao();

        //Checa se existem outros colisores dentro a posição
        Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);

        //Checa se a posição aleatoria gerada está longe do jogador
        bool safeDistanceBewteenPlayer = Vector3.Distance(posicaoDeCriacao, player.transform.position) > distanceFromPlayerToSpawn;


        while (colisores.Length <= 0 && !safeDistanceBewteenPlayer)
        {
            posicaoDeCriacao = AleatorizarPosicao();
            colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);
            safeDistanceBewteenPlayer = Vector3.Distance(posicaoDeCriacao, player.transform.position) > distanceFromPlayerToSpawn;
            yield return null;
        }

        GameObject zombie =  Instantiate(Zumbi, posicaoDeCriacao, transform.rotation);
        Status zombieStatus = zombie.GetComponent<Status>();

        zombieStatus.Vida += increaseZombieLife;
        zombieStatus.VidaInicial += increaseZombieLife;
        zombieStatus.attack += increaseZombieDamage;

        zombie.GetComponent<NavMeshAgent>().agentTypeID = 0;
        quantidadeDeZumbisVivos++;
    }

    Vector3 AleatorizarPosicao()
    {
        int getRandomIndex = Random.Range(0, posicoesPossiveisDeGeracao.Length);

        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao;
        posicao += posicoesPossiveisDeGeracao[getRandomIndex].transform.position;
        posicao.y = 0;


        return posicao;
    }

    public void updateZombieStatus(float updateZombieLife, float updateZombieDamage)
    {
        increaseZombieLife = updateZombieLife;
        increaseZombieDamage = updateZombieDamage;
    }
}
