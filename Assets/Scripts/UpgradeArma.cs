using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArma : MonoBehaviour
{
    public int tempoDeDestruicao = 20;
    public float velocidadeDeRotacao = 50f;
    private ControlaInterface scriptControlaInterface;

    private void Start()
    {

        Destroy(gameObject, tempoDeDestruicao);

    }

    void Update()
    {
        // Rotacionar o objeto horizontalmente ao longo do eixo Y
        transform.Rotate(Vector3.up, velocidadeDeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider objetoDeColisao)
    {
        if (objetoDeColisao.tag == "Jogador")
        {
            objetoDeColisao.GetComponent<ControlaArma>().changeWeapon(gameObject.tag);
            Destroy(gameObject);
        }
    }
}
