using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaInimigo : MonoBehaviour, IMatavel
{

    private GameObject Jogador;
    private MovimentoPersonagem movimentaInimigo;
    private AnimacaoPersonagem animacaoInimigo;
    private Status statusInimigo;
    public AudioClip SomDeMorte;
    private Vector3 posicaoAleatoria;
    private Vector3 direcao;
    private float contadorVagar;
    private float tempoEntrePosicoesAleatorias = 4;
    public float porcentagemGerarKitMedico = 0.1f;
    public float porcentagemGerarSMG= 0.04f;
    public GameObject kitMedicoPrefab;
    public GameObject smgWeapon;
    private ControlaInterface scriptControlaInterface;
    [HideInInspector]
    public GeradorZumbis meuGerador;
    public GameObject particulaSangueZumbi;
    private float timeToZombieDespawnAfterDeath = 20f;
    public Image imageSlider;
    public Slider sliderVidaZumbi;
    public Color corVidaMaxima, corVidaMinima;


    // Use this for initialization
    void Start () {
        Jogador = GameObject.FindWithTag("Jogador");
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
        AleatorizarZumbi();
        statusInimigo = GetComponent<Status>();
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        sliderVidaZumbi.maxValue = statusInimigo.VidaInicial;
        AtualizarInterface();
    }

    void FixedUpdate()
    {
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);

        movimentaInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude);

        if(distancia > 15)
        {
            Vagar ();
        }
        else if (distancia > 2.5)
        {
            direcao = Jogador.transform.position - transform.position;

            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);

            animacaoInimigo.Atacar(false);
        }
        else
        {
            direcao = Jogador.transform.position - transform.position;

            animacaoInimigo.Atacar(true);
        }
    }

    void Vagar ()
    {
        contadorVagar -= Time.deltaTime;
        if(contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += tempoEntrePosicoesAleatorias + Random.Range(-1f, 1f);
        }

        bool ficouPertoOSuficiente = Vector3.Distance(transform.position, posicaoAleatoria) <= 0.05;
        if (ficouPertoOSuficiente == false)
        {
            direcao = posicaoAleatoria - transform.position;
            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);
        }           
    }

    Vector3 AleatorizarPosicao ()
    {
        Vector3 posicao = Random.insideUnitSphere * 10;
        posicao += transform.position;
        posicao.y = transform.position.y;

        return posicao;
    }

    void AtacaJogador ()
    {
        int dano = Random.Range(20, 30);
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano);
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

    public void TomarDano(int dano)
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
        scriptControlaInterface.atualizarQuantidadeDeZumbisMortos();
        meuGerador.diminuirQuantidadeDeZumbisVivos();
    }

    void checkDrop()
    {
        if (Random.value <= porcentagemGerarKitMedico)
        {
            Instantiate(kitMedicoPrefab, transform.position, Quaternion.identity);
            timeToZombieDespawnAfterDeath = 1f;
            return;
        }
        if (Random.value <= porcentagemGerarSMG)
        {
            Instantiate(smgWeapon, transform.position, Quaternion.identity);
            timeToZombieDespawnAfterDeath = 1f;
            return;
        }
    }

    void AtualizarInterface()
    {
        if (statusInimigo.VidaInicial == 1)
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
