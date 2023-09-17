using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject UpgradePanel;
    public GameObject Position1;
    public GameObject Position2;
    public GameObject LifeUpgrade;
    public GameObject ArmorUpgrade;
    public GameObject AttackUpgrade;
    public GameObject AttackSpeedUpgrade;

    // Use this for initialization
    void Start () {
        scriptControlaJogador = GameObject.FindWithTag("Jogador")
                                .GetComponent<ControlaJogador>();

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida;
        AtualizarSliderVidaJogador();
        Time.timeScale = 1;
        tempoPontuacaoSalvo = PlayerPrefs.GetFloat("PontuacaoMaxima");

    }

    public void AtualizarSliderVidaJogador ()
    {
        SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
    }

    public void atualizarQuantidadeDeZumbisMortos()
    {
        quantidadeDeZumbisMortos++;
        textoZumbisMortos.text = string.Format("x{0}", quantidadeDeZumbisMortos);
    }

    public void updateWeaponNameAndAmmunitionQuantity(int ammunitionQuantity, string weaponName)
    {

        weaponText.text = string.Format("Arma: {0}", weaponName);
        if (weaponName == "Pistola")
        {
            ammunitionText.text = string.Format("Munição: Infinita");
        } else
        {
            ammunitionText.text = string.Format("Munição: {0}", ammunitionQuantity);
        }


    }

    public void GameOver ()
    {
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

    IEnumerator DesaparecerTexto(float tempoDeSumico, Text textoParaSumir)
    {
        textoParaSumir.gameObject.SetActive(true);
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

    public void lifeUpgrade()
    {
        scriptControlaJogador.statusJogador.VidaInicial += 1;
        scriptControlaJogador.statusJogador.Vida += 1;
        UpgradePanel.SetActive(false);
        LifeUpgrade.SetActive(false);
        Time.timeScale = 1f;
    }

    public void armorUpgrade()
    {
        scriptControlaJogador.statusJogador.armor += 5;
        UpgradePanel.SetActive(false);
        ArmorUpgrade.SetActive(false);
        Time.timeScale = 1f;
    }

    public void attackUpgrade()
    {
        scriptControlaJogador.statusJogador.attack += 1;
        UpgradePanel.SetActive(false);
        AttackUpgrade.SetActive(false);
        Time.timeScale = 1f;
    }

    public void attackSpeedUpgrade()
    {
        scriptControlaJogador.statusJogador.attackSpeed += 0.1f;
        UpgradePanel.SetActive(false);
        AttackSpeedUpgrade.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator waitForStart()
    {
        // Aguardar 3 segundos
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;

    }

    public void optionsForUpgrade()
    {
        UpgradePanel.SetActive(true);
        Time.timeScale = 0f;
        // Faça algo com o objeto filho, por exemplo, ative/desative, mova, etc.
        Transform randomChild = randomChildren(Position1);
        Transform randomChild2 = randomChildren(Position2);
        randomChild.gameObject.SetActive(true);
        randomChild2.gameObject.SetActive(true);
    }

    public Transform randomChildren(GameObject father)
    {

        int childCount = father.transform.childCount;

        if (childCount > 0)
        {
            // Gere um índice aleatório
            int randomChildIndex = Random.Range(0, childCount);

            // Acesse o objeto filho com base no índice aleatório
            Transform randomChild = father.transform.GetChild(randomChildIndex);

            return randomChild;

        } else
        {
            return null;
        }

    }
}
