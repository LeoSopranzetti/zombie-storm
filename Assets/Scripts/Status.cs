using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public float VidaInicial = 100;
    public float Vida;
    public float Velocidade = 5;
    public float attack = 1;
    public int armor = 1;
    public float attackSpeed = 0;

    void Awake ()
    {
        Vida = VidaInicial;
    }
}
