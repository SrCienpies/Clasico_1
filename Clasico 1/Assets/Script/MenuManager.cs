using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public ControladorData controlador;

    #region Variables Menu

    private List<RectTransform> lista_menu = new List<RectTransform>();
    public string estado = "Menu Principal";
    public int menu_Actual;
    public int menu_anterior;
    private int dirX;
    private int dirY;

    private Transform canvas;

    #endregion

    #region Variablea Tienda

    public Transform contenedor_tienda;

    private Sprite[] lista_pelotas;

    private GameObject pelota_obj;

    #endregion

    #region Variable Nivel
    public Transform contenedor_niveles;
    public Transform contenedor_index;
    public Transform[] btn_direccion;

    public Color[] color_nivel;

    [HideInInspector] public Color color_activo;
    [HideInInspector] public Color color_inactivo;


    private int grupo_index;
    private int nivel_index;
    #endregion

    public Image pantalla_carga;

    private Texture2D mapa;
    private bool cargando;
    private int total_pixel;

    [Header("Level Manager")]
    private ColorPrefab[] colorMappings;
    private List<int> cantidad_pixels = new List<int>();

    [HideInInspector]public Transform contenedores_bloques;

    // 1 = Bloque normal
    // 2 = Bloque normal debajo
    // 3 = Bloque espina


    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GenerarMenu();
        GenerarBloques();
        OpcionesTienda();
        OpcionesNivel();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) RegresarMenu();

       /* if (Input.GetMouseButtonDown(1))
        {
            cargando = true;
            //Carga();

            StartCoroutine("GenerarNivel");
        }*/


    }


    #region GENERAL

    //public void 

    public void GenerarBloques()
    {
        colorMappings = Resources.LoadAll<ColorPrefab>("SO");

        GameObject contenedor_general =  new GameObject("contenedor_general");
        contenedores_bloques = contenedor_general.transform;

        for (int m = 0; m < colorMappings.Length; m++)
        {
            GameObject contenedor_individual = new GameObject("contenedor bloque " + m);
            //GameObject bloque = new GameObject("Bloque " + m);

            contenedor_individual.transform.SetParent(contenedor_general.transform);


            //bloque.transform.SetParent(contenedor_individual.transform);
            //bloque.gameObject.SetActive(false);
        }

        contenedor_general.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Suelo");
        contenedor_general.transform.GetChild(1).gameObject.AddComponent<Rigidbody2D>();
        contenedor_general.transform.GetChild(1).gameObject.AddComponent<CompositeCollider2D>();

        contenedor_general.transform.GetChild(1).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;


        contenedor_general.gameObject.SetActive(false);
    }

    public void GenerarMenu()
    {
        controlador = ControladorData.instance;

        canvas = transform.parent;

        for (int i = 0; i < canvas.childCount-1; i++)
        {
            lista_menu.Add(canvas.GetChild(i).GetComponent<RectTransform>());
        }
    }

    void FadeIn()
    {
        pantalla_carga.DOFade(0, 0.25f).OnComplete(()=> pantalla_carga.gameObject.SetActive(false));
    }

    #endregion

    #region Menu Opciones

    public void Cambio_Menu(string menu)
    {

        float delay = 0;
        // 0 = Menu Principal
        // 1 = Menu Opciones
        // 2 = Menu Tienda
        // 3 = Menu Donacion
        // 4 = Menu Niveles
        // 5 = Menu Cargando

        switch (menu)
        {
            case "Menu Opciones":

                menu_Actual = 1;
                menu_anterior = 0;
                dirX = 0;
                dirY = 1200;

                break;

            case "Menu Tienda":

                menu_Actual = 2;
                menu_anterior = 0;
                dirX = 2000;
                dirY = 0;

                break;

            case "Menu Nivel":

                menu_Actual = 4;
                menu_anterior = 0;
                dirX = -2000;
                dirY = 0;

                break;

            case "Menu Donacion":

                menu_Actual = 3;
                menu_anterior = 1;
                dirX = 0;
                dirY = 1200;

                break;

            case "Menu Principal":
                
                //menu_Actual = lista_menu[0];
                break;

            case "Menu Juego":

                /*switch (menu_Actual)
                {
                    case 6:


                        break;

                    /*case 5:

                        break;

                    default:
                        break;
                }*/
                menu_anterior = menu_Actual;
                menu_Actual = 5;


                dirX = 0;
                dirY = 1200;

                break;

            case "Menu GameOver":

                menu_Actual = 6;
                menu_anterior = 5;

                dirX = 0;
                dirY = -1200;

                delay = 0.25f;

                break;

            case "Menu Pausa":

                menu_Actual = 10;
                menu_anterior = 5;

                dirX = 0;
                dirY = -1200;

                delay = 0.25f;

                break;

            default:
                break;
        }

        estado = menu;

        CambioMenu(delay);
    }

    public void RegresarMenu()
    {
        float delay = 0;

        switch (estado)
        {
            case "Menu Opciones":

                menu_Actual = 0;
                menu_anterior = 1;
                dirX = 0;
                dirY = -1200;

                estado = "Menu Principal";
                CambioMenu(delay);

                break;

            case "Menu Tienda":

                menu_Actual = 0;
                menu_anterior = 2;
                dirX = -2000;
                dirY = 0;

                estado = "Menu Principal";
                CambioMenu(delay);

                break;

            case "Menu Nivel":

                menu_Actual = 0;
                menu_anterior = 4;
                dirX = 2000;
                dirY = 0;

                estado = "Menu Principal";
                CambioMenu(delay);

                break;

            case "Menu Donacion":

                menu_Actual = 1;
                menu_anterior = 3;
                dirX = 0;
                dirY = -1200;

                estado = "Menu Opciones";
                CambioMenu(delay);

                break;

            case "Menu Juego":

                menu_Actual = 10;
                menu_anterior = 5;
                dirX = 0;
                dirY = -1200;

                estado = "Menu Pausa";
                CambioMenu(delay);

                Transform player = contenedores_bloques.GetChild(0).GetChild(0);
                player.GetComponent<PlayerControlador>().estado = "Pausa";

                break;

            case "Menu Principal":

                Debug.Log("Salir?");
                break;
            default:
                break;
        }

    }

    private void CambioMenu(float delay)
    {
        lista_menu[menu_Actual].gameObject.SetActive(true);
        lista_menu[menu_anterior].DOAnchorPos(new Vector2(dirX, dirY), 0.25f).OnComplete(DesactivarMenu);
        lista_menu[menu_Actual].DOAnchorPos(Vector2.zero, 0.25f).SetDelay(delay);
    }

    private void DesactivarMenu()
    {
        lista_menu[menu_anterior].gameObject.SetActive(false);
    }

    #endregion

    #region Tienda

    void OpcionesTienda()
    {
        lista_pelotas = Resources.LoadAll<Sprite>("Sprites/Plantilla esferas");
        pelota_obj = Resources.Load("Prefabs/pelota") as GameObject;

        for (int i = 1; i < lista_pelotas.Length + 1; i++)
        {
            GameObject pelota_clon = Instantiate(pelota_obj, contenedor_tienda);

            pelota_clon.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = lista_pelotas[i - 1];
            pelota_clon.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "00";
        }
    }

    #endregion

    #region Niveles

    public void Cambio_Grupo_Nivel(string direccion)
    {
        int grupo_anterior = grupo_index;

        switch (direccion)
        {
            case "Siguiente":

                contenedor_niveles.GetChild(1).gameObject.SetActive(true);
                contenedor_niveles.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, contenedor_niveles.GetChild(1).transform.position.y);

                contenedor_niveles.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(-2000, contenedor_niveles.GetChild(0).transform.position.y), 0.25f);
                contenedor_niveles.GetChild(1).GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.25f).OnComplete(DesactivarGrupoNivel);

                grupo_index++;
                break;

            case "Anterior":

                contenedor_niveles.GetChild(1).gameObject.SetActive(true);
                contenedor_niveles.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, contenedor_niveles.GetChild(1).transform.position.y);

                contenedor_niveles.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(2000, 0), 0.25f);
                contenedor_niveles.GetChild(1).GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.25f).OnComplete(DesactivarGrupoNivel);

                grupo_index--;

                break;


            default:
                break;
        }

        Btn_Nivel_Display(contenedor_niveles.GetChild(1));

        contenedor_index.GetChild(grupo_anterior).GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f);
        contenedor_index.GetChild(grupo_anterior).GetComponent<Image>().DOColor(color_inactivo, 0.25f);


        contenedor_index.GetChild(grupo_index).GetComponent<RectTransform>().DOScale(Vector3.one * 1.3f, 0.25f);
        contenedor_index.GetChild(grupo_index).GetComponent<Image>().DOColor(color_activo, 0.25f);

        btn_direccion[0].gameObject.SetActive(false);
        btn_direccion[1].gameObject.SetActive(false);
    }

    void DesactivarGrupoNivel()
    {
        contenedor_niveles.GetChild(0).gameObject.SetActive(false);
        contenedor_niveles.GetChild(0).SetAsLastSibling();

        btn_direccion[0].gameObject.SetActive(true);
        btn_direccion[1].gameObject.SetActive(true);

        if(grupo_index == 0) btn_direccion[0].gameObject.SetActive(false);
        if(grupo_index == 4) btn_direccion[1].gameObject.SetActive(false);

        contenedor_index.GetChild(grupo_index).localScale = Vector3.one * 1.5f;
    }

    void OpcionesNivel()
    {
        for (int a = 0; a < colorMappings.Length; a++)
        {
            cantidad_pixels.Add(new int());
        }

        grupo_index = 0;
        btn_direccion[0].gameObject.SetActive(false);
        contenedor_index.GetChild(grupo_index).localScale = Vector3.one * 1.5f;

        for (int i = 0; i < contenedor_niveles.childCount; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                var btn_nivel = Resources.Load("Prefabs/Opcion_nivel") as GameObject;
                var btn_clon = Instantiate(btn_nivel, contenedor_niveles.GetChild(i));
            }

            Btn_Nivel_Display(contenedor_niveles.GetChild(i));
        }
        
    }

    void Btn_Nivel_Display(Transform grupo)
    {
        for (int i = 0; i < grupo.childCount; i++)
        {
            BtnNivel features = grupo.GetChild(i).GetComponent<BtnNivel>();

            features.color.color = color_nivel[grupo_index];
            features.txt_index.text = (i + 1).ToString();
            features.index = i;
            features.boton.onClick.AddListener(delegate { Carga("Menu Juego"); });
            features.boton.onClick.AddListener(delegate { Carga_Nivel(grupo_index,features.index); });


            switch (features.estado)
            {
                case "Bloqueado":

                    features.contenedor_estrellas.gameObject.SetActive(false);
                    features.txt_index.gameObject.SetActive(false);
                    features.bloqueado.gameObject.SetActive(true);
                    features.boton.interactable = false;

                    break;

                case "Disponible":

                    features.contenedor_estrellas.gameObject.SetActive(false);
                    features.txt_index.gameObject.SetActive(true);
                    features.bloqueado.gameObject.SetActive(false);
                    features.boton.interactable = true;

                    break;

                case "Completado":

                    features.contenedor_estrellas.gameObject.SetActive(true);
                    features.txt_index.gameObject.SetActive(true);
                    features.bloqueado.gameObject.SetActive(false);
                    features.boton.interactable = true;

                    for (int a = 0; a < features.estrellas; a++)
                    {
                        features.contenedor_estrellas.GetChild(a).gameObject.SetActive(true);
                    }

                    break;


                default:
                    break;
            }
        }
    }

    #endregion


   //======================================================================================================

    public void Carga(string estado_juego)
    {
        pantalla_carga.gameObject.SetActive(true);
        pantalla_carga.DOFade(1, 0.25f).OnComplete(()=> Pantalla_Carga(estado_juego));
    }
    
    void Carga_Nivel(int grupo_, int nivel_)
    {
        grupo_index = grupo_;
        nivel_index = nivel_;
    }

    void Pantalla_Carga(string paso)
    {
        switch (paso)
        {
            case "Menu Juego":

                lista_menu[4].gameObject.SetActive(false);
                lista_menu[5].gameObject.SetActive(true);
                lista_menu[6].gameObject.SetActive(false);

                contenedores_bloques.gameObject.SetActive(true);

                cargando = true;
                StartCoroutine("GenerarNivel");

                Cambio_Menu(paso);

                break;

            case "Menu Niveles":

                lista_menu[4].anchoredPosition = Vector3.zero;

                lista_menu[4].gameObject.SetActive(true);
                lista_menu[5].gameObject.SetActive(false);
                lista_menu[6].gameObject.SetActive(false);

                contenedores_bloques.gameObject.SetActive(false);

                estado = "Menu Nivel";

                FadeIn();
                break;


                //OCULTAR EL JUEGO

            default:
                break;
        }

    }

    public void Restar()
    {
        contenedores_bloques.GetChild(0).GetChild(0).GetComponent<PlayerControlador>().Restart();
    }

    IEnumerator GenerarNivel()
    {
        mapa = Resources.Load<Texture2D>("Nivel/Grupo_" + (grupo_index+1) + "/Nivel_" + nivel_index);

        for (int i = 0; i < cantidad_pixels.Count; i++)
        {
            cantidad_pixels[i] = 0;
        }

        while (cargando)
        {

            for (int x = 0; x < mapa.width; x++)
            {
                for (int y = 0; y < mapa.height; y++)
                {
                    GenerarBloque(x, y);
                }
            }

            cargando = false;


            Transform player = contenedores_bloques.GetChild(0).GetChild(0);
            Vector3 cam_offset = new Vector3(3.5f, 0.5f, 0.0f);

            player.GetComponent<PlayerControlador>().estado = "EnJuego";
            player.GetComponent<PlayerControlador>().pos_inicial = player.position;
            player.GetComponent<PlayerControlador>().SetPlayer();

            ControladorData.instance.camara.gameObject.transform.position = player.transform.position + cam_offset;
            ControladorData.instance.camara.target = player;

            yield return null;
        }


        FadeIn();
    }

    void GenerarBloque(int x, int y)
    {
        Color pixelColor = mapa.GetPixel(x, y);

        mapa.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            //EL PIXEL ES TRANSPARENTE
            return;
        }
        
        for (int a = 0; a < colorMappings.Length; a++)
        {
            if (colorMappings[a].color.Equals(pixelColor))
            {
                cantidad_pixels[a]++;
                Transform contenedor = contenedores_bloques.GetChild(a);

                if (cantidad_pixels[a] > contenedor.childCount)
                {
                    GameObject bloque_extra = Instantiate(colorMappings[a].bloquePrefab, contenedor.transform);
                    bloque_extra.transform.SetAsFirstSibling();
                }

                Vector2 posicion = new Vector2(x, y);

                contenedor.GetChild(0).gameObject.SetActive(true);
                contenedor.GetChild(0).transform.position = posicion;

                contenedor.GetChild(0).SetAsLastSibling();
            }
        }
        
    }




}
