using System.Collections;
using System.Collections.Generic;
//using System.IO;
using UnityEngine;
using System;

public class ControladorData : MonoBehaviour
{
    public static ControladorData instance;

    public ControladorCamara camara;

    public int monedas;
    public int pelota;
    public int checkpoints;

    //public Transform player;


    //[Header("Level Manager")]
    //public Transform contenedores_bloques;
    //public ColorBloque[] colorMappings;

    public void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CrearJson_PelotasBloqueo();
        }
    }

    void CrearJson_Pelotas()
    {
        Lista_Pelota data = new Lista_Pelota();

        for (int i = 0; i < 99; i++)
        {
            data.lista_pelota.Add(new Pelota());
            data.lista_pelota[i].index = i + 1;
        }

        string dataPath = System.IO.Path.Combine(Application.dataPath, "pelota_data.txt");
        string jsonString = JsonUtility.ToJson(data);

        Guardar(jsonString, dataPath);
        
    }

    void CrearJson_PelotasBloqueo()
    {
        Pelotas_Desbloqueo data = new Pelotas_Desbloqueo();

        for (int i = 0; i < 99; i++)
        {
            data.pelotas_desbloqueo.Add(false);

            data.pelotas_desbloqueo[i] = false;
        }

        string dataPath = System.IO.Path.Combine(Application.dataPath, "pelotaBloqueo_data.txt");

        System.IO.FileStream fs = new System.IO.FileStream(dataPath, System.IO.FileMode.Create);
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


        bf.Serialize(fs, data);
        fs.Close();
    }

    void Guardar(string jsonString, string dataPath)
    {

        using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }
    }
}
