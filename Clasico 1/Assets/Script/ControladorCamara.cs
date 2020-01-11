using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    public float velocidad = 0.125f;
    //public float minY;
    //public float maxY;

    private bool en_Area_Up;

    void Start()
    {
        
    }


    void FixedUpdate()
    {

        if (target)
        {
            Vector3 desirePosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desirePosition, Time.deltaTime * velocidad);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);
        }

    }
}
