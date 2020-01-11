using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControlador : MonoBehaviour
{



    [Header("General")]
    [HideInInspector] public string estado = "EnJuego";
    public Transform pelota;
    public Transform par_explocion;
    [HideInInspector] public Vector2 pos_inicial;


    [Header("Rotacion")]
    public float velocidadRotacion;
    //private bool rotando;


    [Header("Salto")]
    [Range (0,10)]
    public float velocidadSalto = 10;
    [HideInInspector] public float fallMultiplier = 2.5f;
    [HideInInspector] public float lowJumpMultiplier = 2.0f;

    [HideInInspector] public Transform sueloCheck;
    [HideInInspector] public LayerMask sueloCapa;

    public Rigidbody2D rbd2;
    private bool saltando;
    private bool enSuelo;


    [Header("Corriendo")]
    public float velocidadHorizontal = 10;
    private bool corriendo = false;

    private bool puedeMorir = false;



    void Start()
    {
    }

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

            if(rbd2.velocity.magnitude <= 0)
            {
                if (!puedeMorir) puedeMorir = true;
                else
                {
                    GameOver();
                    return;
                }
            }

            rbd2.velocity = new Vector2(velocidadHorizontal * Time.deltaTime, rbd2.velocity.y);
            //Debug.Log(rbd2.velocity.magnitude);

        }
    }


    void Rotacion()
    {
        Vector3 rotacionFInal = new Vector3(0, 0, -180);
        pelota.DORotate(rotacionFInal, velocidadRotacion).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    void GameOver()
    {
        MenuManager.instance.Cambio_Menu("Menu GameOver");

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
        MenuManager.instance.Cambio_Menu("Menu Juego");
        estado = "EnJuego";

        // MenuManager.instance.contenedores_bloques.GetChild(3).

        for (int i = 0; i < MenuManager.instance.contenedores_bloques.GetChild(3).childCount; i++)
        {
            MenuManager.instance.contenedores_bloques.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }


        SetPlayer();
       
    }

    public void SetPlayer()
    {
        rbd2 = GetComponent<Rigidbody2D>();

        puedeMorir = false;

        transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
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

                Debug.Log("Coge estrella");

                break;

            default:
                break;
        }
    }
}
