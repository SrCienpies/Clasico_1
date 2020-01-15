using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public CircleCollider2D colision;
    public GameObject icono; 
    public GameObject particulas;

    public void Start()
    {

    }

    public void Desvanecer()
    {
        colision.enabled = false;
        icono.SetActive(false);
        //particulas.SetActive(true);
    }

    public void Restablecer()
    {
        colision.enabled = true;
        icono.SetActive(true);
    }
}
