using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControlador : MonoBehaviour
{

    #region VARIABLES GENERALES
    [Header("General")]
    public string estado = "EnJuego";
    public Transform pelota;
    public Transform par_explocion;
    [HideInInspector] public Vector2 pos_inicial;

    private int direccion = 1;

    #endregion


    #region VARIABLES ROTACION
    [Header("Rotacion")]
    public float velocidadRotacion;
    #endregion

    #region VARIABLES SALTO
    [Header("Salto")]
    public float velocidadSalto = 10;
    public float saltoVertical;
    public float saltoHorizontal;


    private float dash = 1;

    [HideInInspector] public float fallMultiplier = 2.5f;
    [HideInInspector] public float lowJumpMultiplier = 2.0f;

    [HideInInspector] public Transform sueloCheck;
    [HideInInspector] public LayerMask sueloCapa;

    [HideInInspector] public Rigidbody2D rbd2;
    private bool saltando;
    private bool enSuelo;
    private bool dashing;

    #endregion


    #region VARIABLES CORRER
    [Header("Corriendo")]
    public float velocidadHorizontal = 10;

    private bool corriendo = false;
    private bool puedeMorir = false;
    #endregion


    void Update()
    {

        if(estado == "EnJuego")
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (!corriendo)
                {
                    corriendo = true;
                    Rotacion();
                }
                else
                {
                    if (!saltando)
                    {
                        saltando = true;
                    }
                }
            }
        }


    }

    private void FixedUpdate()
    {
        Movimiento();
    }


    public void Display()
    {
        rbd2 = GetComponent<Rigidbody2D>();
    }


    void SaltoMejorado()
    {
        if (rbd2.velocity.y < 0)
        {
            rbd2.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbd2.velocity.y > 0 && !Input.GetMouseButton(0))
        {
            rbd2.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    void SaltoNormal()
    {
        rbd2.velocity = Vector2.up * velocidadSalto;
    }
    void Movimiento()
    {
        enSuelo = false;
        enSuelo = Physics2D.Linecast(transform.position, sueloCheck.position, sueloCapa);

        if (enSuelo && saltando)
        {
            enSuelo = false;
            SaltoNormal();
        }

        saltando = false;

        if (corriendo)
        {
            rbd2.velocity = new Vector2(direccion*velocidadHorizontal * Time.deltaTime*dash, rbd2.velocity.y);
        }
    }


    void Rotacion()
    {
        Vector3 rotacionFInal = new Vector3(0, 0, -180);
        pelota.DORotate(rotacionFInal, velocidadRotacion).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    void GameOver()
    {
        //MenuManager.instance.Cambio_Menu("Menu GameOver");

        estado = "GameOver";

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        rbd2.bodyType = RigidbodyType2D.Static;

        //par_explocion.gameObject.SetActive(true);  <========= COLOCAR PARTICULA

        corriendo = false;
        pelota.DOKill();


    }

    public void Restart()
    {
        //MenuManager.instance.Cambio_Menu("Menu Juego");
        estado = "EnJuego";
        
        /*for (int i = 0; i < MenuManager.instance.contenedores_bloques.GetChild(3).childCount; i++)
        {
            MenuManager.instance.contenedores_bloques.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }*/


        SetPlayer();
       
    }

    public void SetPlayer()
    {
        rbd2 = GetComponent<Rigidbody2D>();

        puedeMorir = false;

        transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        rbd2.bodyType = RigidbodyType2D.Dynamic;

        transform.position = pos_inicial;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Espina":

                GameOver();

                break;

            case "Bloque":


                break;

            case "Item 1":

                break;


            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Estrella":

               // Debug.Log("Coge estrella");

                break;

            default:
                break;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {

            if (Input.GetMouseButtonDown(0))
            {
                switch (collision.tag)
                {
                    case "Item_1":
                        
                        SaltoDoble();

                        break;

                    case "Item_2":

                        SaltoAlto();

                        break;

                    case "Item_3":

                        SaltoLargo();

                        break;

                    default:
                        break;
                }

                collision.GetComponent<Item>().Desvanecer();
            }

        }
    }


    public void SaltoDoble()
    {
        rbd2.velocity = Vector2.zero;
        SaltoNormal();

    }

    public void SaltoAlto()
    {
        rbd2.velocity = Vector2.zero;
        rbd2.velocity = Vector2.up * velocidadSalto * 2f;
    }

    public void SaltoLargo()
    {
        if (!dashing)
        {
            rbd2.velocity = Vector2.zero;

            SaltoNormal();

            dashing = true;
            dash = saltoHorizontal;

            DOTween.To(() => dash, x => dash = x, 1, 1.5f).OnComplete(() => { dashing = false; dash = 1; });
        }
    }

}
