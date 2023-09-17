using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int VidaInicial = 100;
    public int Vida;
    public float Velocidade = 5;
    public float attack = 1;
    public float armor = 1;
    public float attackSpeed = 0;

    void Awake ()
    {
        Vida = VidaInicial;
    }
}
