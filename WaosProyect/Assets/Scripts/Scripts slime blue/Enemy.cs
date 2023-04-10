using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Lista de puntos que seguira el enemigo
    public List<Transform> waypoints = new List<Transform>();
    // Primer movimiento hacia el un punto, P1
    private int targetIndex = 1;
    // Variable que determina la vel.
    public float movementSpeed = 4;
    void Start()
    {
        
    }


    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        // Movimiento que realizara el enemigo para moverse de punto en punto y moverse a la misma vel.
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position,
            movementSpeed * Time.deltaTime);
        //Variable para cuando se acerque a un punto, comience a ir al proximo.
        var distance = Vector3.Distance(transform.position, waypoints[targetIndex].position);
        if (distance <= 0.1f) //Distancia hacia el punto
        {
            if (targetIndex >= waypoints.Count - 1) //Cuando llega al final, el enemigo no siga buscando puntos
            {
                Debug.Log("Has perdido");
                return;
            }
            targetIndex++; //Cambio de dirección
        }
    }
}
       