using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public Texture2D mapa;

    // Start is called before the first frame update
    void Start()
    {
       // GenerarNivel();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerarNivel()
    {
        for (int x = 0; x < mapa.width; x++)
        {
            for (int y = 0; y < mapa.height; y++)
            {
                GenerarBloque(x, y);
            }
        }
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

        Debug.Log(pixelColor);
    }
}
