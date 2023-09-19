using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class ControlaInterface : MonoBehaviour {

    private ControlaJogador scriptControlaJogador;
    public Slider SliderVidaJogador;
    public GameObject PainelDeGameOver;
    public Text TextoTempoDeSobrevivencia;
    public Text TextoPontuacaoMaxima;
    private float tempoPontuacaoSalvo;
    private int quantidadeDeZumbisMortos;
    public Text textoZumbisMortos;
    public Text textoChefeAparece;
    public Text weaponText;
    public Text ammunitionText;
    public Text playerActualLife;
    public Text waveNumberText;
    public Text numberOfEnemies;
    public Text playerMaxLifeText;
    public Text playerSpeedText;
    public Text playerAttackText;
    public Text playerArmorText;
    public Text playerAttackSpeedText;
    public GameObject UpgradePanel;
    public GameObject Position1;
    public GameObject LifeUpgrade;
    public GameObject ArmorUpgrade;
    public GameObject AttackUpgrade;
    public GameObject AttackSpeedUpgrade;
    public GameObject waveStatus;
    private Vector2 startPosition = new Vector2(0f, 0f);

    // Use this for initialization
    void Start () {
        scriptControlaJogador = GameObject.FindWithTag("Jogador")
                                .GetComponent<ControlaJogador>();

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida;
        AtualizarSliderVidaJogador();
        Time.timeScale = 1;
        tempoPontuacaoSalvo = PlayerPrefs.GetFloat("PontuacaoMaxima");

        waveStatus.SetActive(true);
    }

    public void AtualizarSliderVidaJogador ()
    {
        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.VidaInicial;
        SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
        if (scriptControlaJogador.statusJogador.Vida >= 0)
        {
            playerActualLife.text = string.Format("{0}", scriptControlaJogador.statusJogador.Vida);
        }
    }

    public void atualizarQuantidadeDeZumbisMortos()
    {
        quantidadeDeZumbisMortos++;
        textoZumbisMortos.text = string.Format("x{0}", quantidadeDeZumbisMortos);
    }

    public void updateWeaponName(string weaponName)
    {
        weaponText.text = string.Format("Arma: {0}", weaponName);
    }

    public void GameOver ()
    {
        playerActualLife.text = string.Format("0");
        PainelDeGameOver.SetActive(true);
        Time.timeScale = 0;

        int minutos = (int)(Time.timeSinceLevelLoad / 60);
        int segundos = (int)(Time.timeSinceLevelLoad % 60);
        TextoTempoDeSobrevivencia.text = 
            "Você sobreviveu por " + minutos + "min e " + segundos + "s";

        AjustarPontuacaoMaxima(minutos, segundos);
    }

    void AjustarPontuacaoMaxima (int min, int seg)
    {
        if(Time.timeSinceLevelLoad > tempoPontuacaoSalvo)
        {
            tempoPontuacaoSalvo = Time.timeSinceLevelLoad;
            TextoPontuacaoMaxima.text = 
                string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalvo);
        }
        if(TextoPontuacaoMaxima.text == "")
        {
            min = (int)tempoPontuacaoSalvo / 60;
            seg = (int)tempoPontuacaoSalvo % 60;
            TextoPontuacaoMaxima.text =
                string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
        }
    }

    public void Reiniciar ()
    {
        SceneManager.LoadScene("game");
    }

    public void AparecerChefeCriado()
    {
        StartCoroutine(DesaparecerTexto(2, textoChefeAparece));
    }

    public void waveNumberTextOnScreen(int waveNumber, int waveNumberEnemies)
    {
        waveNumberText.text = string.Format("Wave {0}", waveNumber);
        numberOfEnemies.text = string.Format("Inimigos restantes: {0}", waveNumberEnemies);
    }

    IEnumerator DesaparecerTexto(float tempoDeSumico, Text textoParaSumir)
    {
        Color corTexto = textoParaSumir.color;
        corTexto.a = 1;
        textoParaSumir.color = corTexto;
        yield return new WaitForSeconds(1);
        float contador = 0;
        while (textoParaSumir.color.a > 0)
        {
            contador += Time.deltaTime / tempoDeSumico;
            corTexto.a = Mathf.Lerp(1, 0, contador);
            textoParaSumir.color = corTexto;
            if(textoParaSumir.color.a <= 0)
            {
                textoParaSumir.gameObject.SetActive(false);
            }
            yield return null;
        }

    }

    public void optionsForUpgrade()
    {
        waveStatus.SetActive(false);
        UpgradePanel.SetActive(true);
        Time.timeScale = 0f;

        int randomIndex1 = Random.Range(0, Position1.transform.childCount);
        int randomIndex2;

        //LOOP PARA ACHAR UM INDEX DIFERENTE DO PRIMEIRO PARA NÃO DAR PROBLEMA QUANDO FOR RANDOMIZAR O UPGRADE, PRA NÃO VIR O MESMO UPGRADE
        do
        {
            randomIndex2 = Random.Range(0, Position1.transform.childCount);
        } while (randomIndex2 == randomIndex1);


        //RANDOMIZA DOIS UPGRADES PARA SEREM EXIBIDOS NA TELA
        Transform randomChild = randomChildren(Position1, randomIndex1);
        Transform randomChild2 = randomChildren(Position1, randomIndex2);
        Debug.Log(randomChild);
        Debug.Log(randomChild2);


        //SETA O ESPAÇAMENTO DOS DOIS UPGRADES NA TELA
        Vector2 buttonSpacing1 = new Vector2(160f, 0f);
        Vector2 buttonSpacing2 = new Vector2(-160f, 0f);


        Vector2 novaPosicao = startPosition + buttonSpacing1;
        randomChild.GetComponent<RectTransform>().anchoredPosition = novaPosicao;

        Vector2 novaPosicao2 = startPosition + buttonSpacing2;
        randomChild2.GetComponent<RectTransform>().anchoredPosition = novaPosicao2;

        //ATIVA E DESATIVA OS UPGRADES NA TELA
        randomChild.gameObject.SetActive(true);
        randomChild2.gameObject.SetActive(true);
    }


    public Transform randomChildren(GameObject father, int randomChildIndex)
    {

        int childCount = father.transform.childCount;

        if (childCount > 0)
        {

            // Acesse o objeto filho com base no índice aleatório
            Transform randomChild = father.transform.GetChild(randomChildIndex);

            return randomChild;

        } else
        {
            return null;
        }

    }

    public void lifeUpgrade()
    {
        scriptControlaJogador.statusJogador.VidaInicial += 40;
        scriptControlaJogador.statusJogador.Vida += 40;
        UpgradePanel.SetActive(false);
        LifeUpgrade.SetActive(false);
        updatePlayerStatusInterface();
        Time.timeScale = 1f;
        waveStatus.SetActive(true);
        AtualizarSliderVidaJogador();
    }

    public void armorUpgrade()
    {
        scriptControlaJogador.statusJogador.armor += 5;
        UpgradePanel.SetActive(false);
        ArmorUpgrade.SetActive(false);
        updatePlayerStatusInterface();
        Time.timeScale = 1f;
        waveStatus.SetActive(true);
    }

    public void attackUpgrade()
    {
        scriptControlaJogador.statusJogador.attack += 1f;
        UpgradePanel.SetActive(false);
        AttackUpgrade.SetActive(false);
        updatePlayerStatusInterface();
        Time.timeScale = 1f;
        waveStatus.SetActive(true);
    }

    public void attackSpeedUpgrade()
    {
        if (scriptControlaJogador.statusJogador.attackSpeed >= 0.01f)
        {
            scriptControlaJogador.statusJogador.attackSpeed -= 0.03f;
        } 
        else 
        {
            scriptControlaJogador.statusJogador.attackSpeed = 0.05f;
        }

        UpgradePanel.SetActive(false);
        AttackSpeedUpgrade.SetActive(false);
        updatePlayerStatusInterface();
        Time.timeScale = 1f;
        waveStatus.SetActive(true);
    }

    public void updatePlayerStatusInterface()
    {
        scriptControlaJogador = GameObject.FindWithTag("Jogador").GetComponent<ControlaJogador>();
        playerMaxLifeText.text = string.Format($"Vida máxima: {scriptControlaJogador.statusJogador.VidaInicial}");
        playerSpeedText.text = string.Format($"Velocidade: {scriptControlaJogador.statusJogador.Velocidade}");
        playerAttackText.text = string.Format($"Ataque: {scriptControlaJogador.statusJogador.attack}");
        playerArmorText.text = string.Format($"Armadura: {scriptControlaJogador.statusJogador.armor}");
        playerAttackSpeedText.text = string.Format($"Velocidade de disparo: {scriptControlaJogador.statusJogador.attackSpeed}/s");

    }
}
