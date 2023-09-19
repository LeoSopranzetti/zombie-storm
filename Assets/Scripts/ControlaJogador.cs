using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaJogador : MonoBehaviour, IMatavel, ICuravel
{

    private Vector3 direcao;
    public LayerMask MascaraChao;
    public GameObject TextoGameOver;
    public ControlaInterface scriptControlaInterface;
    public AudioClip SomDeDano;
    private MovimentoJogador meuMovimentoJogador;
    private AnimacaoPersonagem animacaoJogador;
    public ControlaArma controlaArma;
    public Status statusJogador;
    private string playerClass;

    private void Start()
    {
        meuMovimentoJogador = GetComponent<MovimentoJogador>();
        animacaoJogador = GetComponent<AnimacaoPersonagem>();
        statusJogador = GetComponent<Status>();
        controlaArma = GetComponent<ControlaArma>();


        if (PlayerPrefs.GetString("Class") != null)
        {
            playerClass = PlayerPrefs.GetString("Class");
        }

        setDamageAndAttackSpeedToWeapon();
        //controlaArma.changeWeapon(playerClass);
    }

    // Update is called once per frame
    void Update()
    {

        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

        animacaoJogador.Movimentar(direcao.magnitude);
    }

    void FixedUpdate()
    {
        meuMovimentoJogador.Movimentar(direcao, statusJogador.Velocidade);

        meuMovimentoJogador.RotacaoJogador(MascaraChao);
    }

    public void TomarDano (float dano)
    {
        int danoMitigado = (int)dano - statusJogador.armor;
        if (danoMitigado >= 0)
        {
            statusJogador.Vida -= dano;
            scriptControlaInterface.AtualizarSliderVidaJogador();
            ControlaAudio.instancia.PlayOneShot(SomDeDano);
        } else
        {
            --statusJogador.Vida;
            scriptControlaInterface.AtualizarSliderVidaJogador();
            ControlaAudio.instancia.PlayOneShot(SomDeDano);
        }
        if(statusJogador.Vida <= 0)
        {
            Morrer();
        }        
    }

    public void Morrer ()
    {
        scriptControlaInterface.GameOver();
    }

    public void CurarVida(int quantidadeDeCura)
    {
        statusJogador.Vida += quantidadeDeCura;
        if (statusJogador.VidaInicial < statusJogador.Vida)
        {
            statusJogador.Vida = statusJogador.VidaInicial;
        }
        scriptControlaInterface.AtualizarSliderVidaJogador();
    }

    private void setDamageAndAttackSpeedToWeapon()
    {
        controlaArma.changeWeapon(playerClass);
        if (playerClass == "Pistol")
        {
            statusJogador.attack = 2f;
            statusJogador.attackSpeed = 0.5f;
            statusJogador.VidaInicial += 50;
            statusJogador.Vida += 50;
        } else if (playerClass == "SMG")
        {
            statusJogador.attack = 0.5f;
            statusJogador.attackSpeed = 0.35f;
        }
        scriptControlaInterface.updatePlayerStatusInterface();
    }
}
