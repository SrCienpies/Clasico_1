using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public Texture2D png_mapa;

    private ControladorCamara camara;
    private PlayerControlador player;

    [HideInInspector] public UnityEngine.UI.Text levelText;
    [HideInInspector] public UnityEngine.UI.Image loadingBar;
    [HideInInspector] public UnityEngine.UI.Text loadingTxt;
    [HideInInspector] public UnityEngine.UI.Button loadingButton;
    [HideInInspector] public UnityEngine.UI.Button updateButton;
    [HideInInspector] public UnityEngine.UI.Button previewButton;
    [HideInInspector] public UnityEngine.UI.Button restartButton;
    [HideInInspector] public UnityEngine.UI.Button exitButton;

    private float total_pixels;
    private float current_pixels;

    private Transform contenedores_bloques;

    private ColorPrefab[] colores;
    private List<SingleGroup> grupoData = new List<SingleGroup>();

   /* [Header("Limite Horizonral")]
    public float min_X;
    public float max_X;


    [Header("Limite Vertical")]
    public float min_Y;
    public float max_Y;*/


    void Start()
    {
        camara = Camera.main.GetComponent<ControladorCamara>();
        colores = Resources.LoadAll<ColorPrefab>("SO");

        previewButton.interactable = false;

        for (int i = 0; i < colores.Length; i++)
        {
            grupoData.Add(new SingleGroup());
        }

        SetBloques();


    }

    void ColocarCollider()
    {
        contenedores_bloques.transform.GetChild(1).gameObject.AddComponent<CompositeCollider2D>();
        contenedores_bloques.transform.GetChild(1).gameObject.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;

        camara.target = player.transform;
    }

    public void Previsuaizacion()
    {
        for (int i = 0; i < grupoData.Count; i++)
        {
            if (contenedores_bloques.GetChild(i).childCount < grupoData[i].total_bloques)
            {
                int diferencia = grupoData[i].total_bloques - contenedores_bloques.GetChild(i).childCount;

                for (int j = 0; j < diferencia; j++)
                {
                    GameObject nuevo_bloque =  Instantiate(colores[i].bloquePrefab, contenedores_bloques.GetChild(i));

                }
            }

            for (int a = 0; a < grupoData[i].total_bloques; a++)
            {
                contenedores_bloques.GetChild(i).GetChild(a).gameObject.SetActive(true);
                contenedores_bloques.GetChild(i).GetChild(a).transform.position = grupoData[i].posicion[a];
            }
            
        }

        player = contenedores_bloques.GetChild(0).GetChild(0).GetComponent<PlayerControlador>();

        //camara.target = player.transform;

       /* camara.min_X = min_X;
        camara.max_X = max_X;
        camara.min_Y = min_Y;
        camara.max_Y = max_Y;*/

        player.pos_inicial = grupoData[0].posicion[0];
        player.SetPlayer();

        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        updateButton.gameObject.SetActive(false);
        previewButton.gameObject.SetActive(false);

        Invoke("ColocarCollider", 0.1f);



        contenedores_bloques.gameObject.SetActive(true);


    }

    void SetBloques()
    {
        colores = Resources.LoadAll<ColorPrefab>("SO");

        GameObject contenedor_general = new GameObject("contenedor_general");
        contenedores_bloques = contenedor_general.transform;

        for (int m = 0; m < colores.Length; m++)
        {
            GameObject contenedor_individual = new GameObject("contenedor bloque " + m);

            contenedor_individual.transform.SetParent(contenedor_general.transform);
        }

        contenedor_general.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Suelo");


        contenedores_bloques.transform.GetChild(1).gameObject.AddComponent<Rigidbody2D>();
        contenedores_bloques.transform.GetChild(1).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //contenedores_bloques.transform.GetChild(1).gameObject.AddComponent<CompositeCollider2D>();



        contenedor_general.gameObject.SetActive(false);

        for (int i = 0; i < contenedores_bloques.childCount; i++)
        {
            int cantidad = 0;

            switch (i)
            {
                case 0:
                    cantidad = 1;

                    break;

                case 3:
                    cantidad = 3;
                    break;

                default:
                    cantidad = 100;
                    break;
            }

            for (int a = 0; a < cantidad; a++)
            {
                GameObject obj =  Instantiate(colores[i].bloquePrefab, contenedores_bloques.GetChild(i));
                obj.SetActive(false);
            }
        }
    }

    public void Restart()
    {
        player.Restart();
    }

    public void Detener()
    {
        previewButton.gameObject.SetActive(true);
        updateButton.gameObject.SetActive(true);

        contenedores_bloques.gameObject.SetActive(false);

        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        contenedores_bloques.gameObject.SetActive(false);

        Destroy(contenedores_bloques.transform.GetChild(1).gameObject.GetComponent<CompositeCollider2D>());
    }

    public void Generar_Nivel()
    {
        total_pixels = png_mapa.width * png_mapa.height;
        current_pixels = 0;

        loadingButton.interactable = false;

        StartCoroutine(Generar_nivel());
    }

    IEnumerator Generar_nivel()
    {
        levelText.text = png_mapa.name;

        for (int a = 0; a < grupoData.Count; a++)
        {
            grupoData[a].posicion.Clear();
        }



        for (int x = 0; x < png_mapa.width; x++)
        {
            for (int y = 0; y < png_mapa.height; y++)
            {
                BloqueDisplay(x, y);
            }

            yield return null;

        }

        loadingTxt.text = "COMPLETADO";
        previewButton.gameObject.SetActive(true);
        loadingButton.interactable = true;
        previewButton.interactable = true;


    }

    void BloqueDisplay(int x, int y)
    {
        Color pixelColor = png_mapa.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            UpdateHUD();
            return;
        }


        for (int i = 0; i < colores.Length; i++)
        {
            if (pixelColor == colores[i].color)
            {
                grupoData[i].posicion.Add(new Vector2(x, y));
                grupoData[i].total_bloques = grupoData[i].posicion.Count;

            }
        }

        UpdateHUD();

        

        //Debug.Log(current_pixels + "/" + total_pixels);
       // Debug.Log(loadingBar.fillAmount);
    }

    void UpdateHUD()
    {
        current_pixels++;
        loadingBar.fillAmount = (current_pixels / total_pixels);
        loadingTxt.text = (loadingBar.fillAmount * 100).ToString("0") + "%";
    }

}

[System.Serializable]
public class SingleGroup
{
    public int total_bloques;
    public List<Vector2> posicion = new List<Vector2>();

}
