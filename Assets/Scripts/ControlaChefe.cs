using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaChefe : MonoBehaviour, IMatavel
{

    private Transform jogador;
    private NavMeshAgent agente;
    private Status statusChefe;
    private AnimacaoPersonagem animacaoChefe;
    private MovimentoPersonagem movimentoChefe;
    public GameObject kitMedicoPrefab;
    public Image imageSlider;
    public Slider sliderVidaChefe;
    public Color corVidaMaxima, corVidaMinima;
    public GameObject particulaSangueZumbi;
    private float timeToBossDespawnAfterDeath = 20f;


    private void Start()
    {
        jogador = GameObject.FindWithTag("Jogador").transform;
        agente = GetComponent<NavMeshAgent>();
        statusChefe = GetComponent<Status>();
        animacaoChefe = GetComponent<AnimacaoPersonagem>();
        movimentoChefe = GetComponent<MovimentoPersonagem>();
        agente.speed = statusChefe.Velocidade;
        sliderVidaChefe.maxValue = statusChefe.VidaInicial;
        AtualizarInterface();

    }

    private void Update()
    {



    }

    private void FixedUpdate()
    {

        agente.SetDestination(jogador.position);
        animacaoChefe.Movimentar(agente.velocity.magnitude);

        bool estouPertodoJogador = agente.remainingDistance <= agente.stoppingDistance;

        if (agente.hasPath == true)
        {
            if (estouPertodoJogador)
            {
                animacaoChefe.Atacar(true);
                Vector3 direcao = jogador.position - transform.position;
                movimentoChefe.Rotacionar(direcao);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
                animacaoChefe.Atacar(false);
            }
        }

    }

    void AtacaJogador()
    {
        int dano = Random.Range(30, 40);
        dano += (int)statusChefe.attack;
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    public void TomarDano(float dano)
    {
        statusChefe.Vida -= dano;
        AtualizarInterface();
        if (statusChefe.Vida <= 0)
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
        Destroy(gameObject, timeToBossDespawnAfterDeath);
        sliderVidaChefe.gameObject.SetActive(false);
        animacaoChefe.Morrer();
        movimentoChefe.Morrer(timeToBossDespawnAfterDeath);
        this.enabled = false;
        Instantiate(kitMedicoPrefab, transform.position, Quaternion.identity);
        agente.enabled = false;

    }

    void AtualizarInterface()
    {
        sliderVidaChefe.value = statusChefe.Vida;
        float porcentagemVida = (float) statusChefe.Vida / statusChefe.VidaInicial;
        Color corDaVida = Color.Lerp(corVidaMinima, corVidaMaxima, porcentagemVida);
        imageSlider.color = corDaVida;

    }
}