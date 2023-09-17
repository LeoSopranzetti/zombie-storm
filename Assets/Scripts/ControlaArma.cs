using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaArma : MonoBehaviour {

    public GameObject Bala;
    public GameObject BalaSMG;
    public GameObject CanoDaArma;
    public AudioClip SomDoTiro;
    private bool podeAtirar = true;
    public float taxaDeDisparo = 0.4f;
    public GameObject pistol;
    public GameObject smg;
    public int totalAmmunition = 0;
    public int bulletsFired = 0;
    private ControlaInterface scriptControlaInterface;
    private string actualClass = "Pistol";
    private Status scritpStatus;



    // Use this for initialization
    void Start () {
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        scritpStatus = GameObject.FindWithTag("Jogador").GetComponent<Status>();

        if (PlayerPrefs.GetString("Class") != null)
        {
            actualClass = PlayerPrefs.GetString("Class");
        }

        taxaDeDisparo = scritpStatus.attackSpeed;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") && podeAtirar)
        {
            taxaDeDisparo = scritpStatus.attackSpeed;
            StartCoroutine(Atirar());
        }
	}


    private IEnumerator Atirar()
    {
        podeAtirar = false; // Impede novos disparos temporariamente

        // Cria uma bala
        InstanciarBala();
        ControlaAudio.instancia.PlayOneShot(SomDoTiro);

        // Aguarda o intervalo de tempo antes de poder atirar novamente
        yield return new WaitForSeconds(taxaDeDisparo);


        podeAtirar = true; // Permite atirar novamente
    }
    
    void InstanciarBala()
    {
        bulletsFired++; // Incrementa o contador de balas
        if (actualClass == "SMG")
        {
            Instantiate(BalaSMG, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        } else if(actualClass == "Pistol")
        {
            Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        }
    
    }

    public void changeWeapon(string tag)
    {
        actualClass = tag;

        if (actualClass == "SMG")
        {
            smg.SetActive(true);
            scriptControlaInterface.updateWeaponName(actualClass);
        } if (actualClass == "Pistol")
        {
            pistol.SetActive(true);
            scriptControlaInterface.updateWeaponName(actualClass);
        }
    }
}
