using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaInimigo : MonoBehaviour, IMatavel
{

    private GameObject jogador;
    private MovimentoPersonagem movimentaInimigo;
    private AnimacaoPersonagem animacaoInimigo;
    private Status statusInimigo;
    public AudioClip SomDeMorte;
    public float porcentagemGerarKitMedico = 0.1f;
    public GameObject kitMedicoPrefab;
    private ControlaInterface scriptControlaInterface;
    [HideInInspector]
    public GeradorZumbis meuGerador;
    public GameObject particulaSangueZumbi;
    private float timeToZombieDespawnAfterDeath = 20f;
    public Image imageSlider;
    public Slider sliderVidaZumbi;
    public Color corVidaMaxima, corVidaMinima;
    private NavMeshAgent agent;


    private void Awake()
    {
        statusInimigo = GetComponent<Status>();
    }

    // Use this for initialization
    void Start () {
        jogador = GameObject.FindWithTag("Jogador");
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        agent = GetComponent<NavMeshAgent>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
        AleatorizarZumbi();
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        sliderVidaZumbi.maxValue = statusInimigo.VidaInicial;
        agent.speed = statusInimigo.Velocidade;
        AtualizarInterface();
    }

    private void Update()
    {
        changeSpeedBasedOnDistance();
    }



    void FixedUpdate()
    {

            agent.SetDestination(jogador.transform.position);
            animacaoInimigo.Movimentar(agent.velocity.magnitude);
        
             bool estouPertodoJogador = agent.remainingDistance <= agent.stoppingDistance;
        
            if (agent.hasPath == true)
            {
                if (estouPertodoJogador)
                {
                    animacaoInimigo.Atacar(true);
                    Vector3 direcao = jogador.transform.position - jogador.transform.position;
                    movimentaInimigo.Rotacionar(direcao);
                }
                else
                {
                    if (agent.velocity != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
                    }

                animacaoInimigo.Atacar(false);
                }
            }
    }

    private void changeSpeedBasedOnDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, jogador.transform.position);
        float distanceToChangeSpeed = 50f;

        // Verifique se a distância para o jogador é menor ou igual a 30.
        if (distanceToPlayer >= distanceToChangeSpeed)
        {
            agent.speed = 20f;
        }
        else
        {
            // Gradualmente diminua a velocidade para 8 quando estiver mais longe.
            float novaVelocidade = Mathf.Lerp(agent.speed, 8f, 0.1f);
            agent.speed = novaVelocidade;
        }

        // Defina a posição de destino do agente para o jogador.
        agent.SetDestination(jogador.transform.position);
    }



    void AtacaJogador ()
    {
        int dano = Random.Range(20, 30);
        dano += (int)statusInimigo.attack;
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    void AleatorizarZumbi ()
    {
        List<GameObject> childList = new List<GameObject>();


        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Zumbi"))
            {
                childList.Add(child.gameObject);
            }

        }

        int geraTipoZumbi = Random.Range(1, childList.Count);
        childList[geraTipoZumbi].gameObject.SetActive(true);

        //int geraTipoZumbi = Random.Range(1, transform.childCount);
        //transform.GetChild(geraTipoZumbi).gameObject.SetActive(true);
    }

    public void TomarDano(float dano)
    {
        statusInimigo.Vida -= dano;
        StopCoroutine(sliderFadeOut());
        AtualizarInterface();
        if (statusInimigo.Vida <= 0)
        {
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(particulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer()
    {
        animacaoInimigo.Morrer();
        movimentaInimigo.Morrer(timeToZombieDespawnAfterDeath);
        sliderVidaZumbi.gameObject.SetActive(false);
        this.enabled = false;
        ControlaAudio.instancia.PlayOneShot(SomDeMorte);
        checkDrop();
        Destroy(gameObject, timeToZombieDespawnAfterDeath);
        agent.enabled = false;
        scriptControlaInterface.atualizarQuantidadeDeZumbisMortos();
        WaveManager.instance.ZombieDefeated();
    }

    void checkDrop()
    {

        if (Random.value <= porcentagemGerarKitMedico)
        {
            Instantiate(kitMedicoPrefab, transform.position, Quaternion.identity);
            timeToZombieDespawnAfterDeath = 1f;
            return;
        }
        //if (Random.value <= porcentagemDropUpgradePlayer)
        //{
         //   Instantiate(playerUpgradeItem, transform.position, Quaternion.identity);
          //  timeToZombieDespawnAfterDeath = 1f;
         //   return;
        //}
    }

    void AtualizarInterface()
    {
        if (statusInimigo.VidaInicial <= 0.5)
        {
            sliderVidaZumbi.gameObject.SetActive(false);
        } else
        {
            sliderVidaZumbi.gameObject.SetActive(true);
            sliderVidaZumbi.value = statusInimigo.Vida;
            float porcentagemVida = (float)statusInimigo.Vida / statusInimigo.VidaInicial;
            Color corDaVida = Color.Lerp(corVidaMinima, corVidaMaxima, porcentagemVida);
            imageSlider.color = corDaVida;
            StartCoroutine(sliderFadeOut());
        }


    }

    private IEnumerator sliderFadeOut()
    {
        yield return new WaitForSeconds(4f);
        sliderVidaZumbi.gameObject.SetActive(false);
    }
}
