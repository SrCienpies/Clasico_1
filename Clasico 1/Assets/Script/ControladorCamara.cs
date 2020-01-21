using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    [HideInInspector]public Transform target;

    public Vector3 offset;
    public float velocidad = 0.125f;

    [Header("Limite Horizonral")]
    public float min_X;
    public float max_X;
    [Header("Limite Vertical")]
    public float min_Y;
    public float max_Y;

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


            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, min_X, max_X),
                Mathf.Clamp(smoothedPosition.y, min_Y, max_Y),
                -10);
        }

        //Limites();

    }

    void Limites()
    {
        Mathf.Clamp(transform.position.x, min_X, max_X);
        Mathf.Clamp(transform.position.y, min_Y, max_Y);

        /*if (transform.position.x <= min_X) transform.position = new Vector3(min_X, transform.position.y, transform.position.z);
        if (transform.position.x >= max_X) transform.position = new Vector3(max_X, transform.position.y, transform.position.z);
        if (transform.position.y <= min_Y) transform.position = new Vector3(transform.position.x, min_Y, transform.position.z);
        if (transform.position.y <= max_Y) transform.position = new Vector3(transform.position.x, max_Y, transform.position.z);*/
    }
}
