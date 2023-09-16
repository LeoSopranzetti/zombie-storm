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
    private string actualWeapon = "Pistola";



    // Use this for initialization
    void Start () {
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") && podeAtirar)
        {
            StartCoroutine(Atirar());
            checkHasAmmo();
        }
	}

    void checkHasAmmo()
    {
        if (totalAmmunition <= 0)
        {
            pistol.SetActive(true);
            taxaDeDisparo = 0.4f;
            scriptControlaInterface.updateWeaponNameAndAmmunitionQuantity(0, "Pistola");
        }
    }

    private IEnumerator Atirar()
    {
        podeAtirar = false; // Impede novos disparos temporariamente

        // Cria uma bala
        InstanciarBala();
        ControlaAudio.instancia.PlayOneShot(SomDoTiro);

        // Aguarda o intervalo de tempo antes de poder atirar novamente
        float milissegundos = taxaDeDisparo * 1000f;
        yield return new WaitForSeconds(milissegundos / 1000f);

        podeAtirar = true; // Permite atirar novamente
    }
    
    void InstanciarBala()
    {
        bulletsFired++; // Incrementa o contador de balas
        if (totalAmmunition >= 1 && actualWeapon == "SMG")
        {
            Instantiate(BalaSMG, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
            scriptControlaInterface.updateWeaponNameAndAmmunitionQuantity(totalAmmunition, actualWeapon);
            --totalAmmunition;
        } else
        {
            Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        }
    
    }

    public void changeWeapon(string tag)
    {
        actualWeapon = tag;

        if (actualWeapon == "SMG")
        {

            totalAmmunition = 30;
            scriptControlaInterface.updateWeaponNameAndAmmunitionQuantity(totalAmmunition, actualWeapon);
            pistol.SetActive(false);
            smg.SetActive(true);
            taxaDeDisparo = 0.2f;
        }


    }
}
