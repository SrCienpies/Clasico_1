using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clases
{

}

[System.Serializable]
public class Pelota
{
    public int index;
    public int precio;
}

[System.Serializable]
public class Lista_Pelota
{
    public List<Pelota> lista_pelota = new List<Pelota>();
}

[System.Serializable]
public class Pelotas_Desbloqueo
{
    public List<bool> pelotas_desbloqueo = new List<bool>();
}

[System.Serializable]
public class Checkpoint
{
    public int cantidad;
    public int precio;
}

[System.Serializable]
public class ColorBloque
{
    public Color color;
    public GameObject bloquePrefab;
}

