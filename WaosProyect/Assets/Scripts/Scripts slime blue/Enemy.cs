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

    //Var de la velocidad que rota el eje al doblar las "esquinas"
    public float rotationSpeed = 6;
    void Start()
    {
        
    }
    void Update()
    {
        Movement();
        LookAt();
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
    } //Movimiento del enemigo
    private void LookAt()
    {
        //Detectara la direccion donde esta mirando
        var dir = waypoints[targetIndex].position - transform.position;
        
        //Se hara una rotacion segun la direccion
        var rootTarget = Quaternion.LookRotation(dir);
        
        //Se hara una rotacion mas suavizada segun donde cambie la direccion.
        transform.rotation = Quaternion.Slerp(transform.rotation, rootTarget, rotationSpeed * Time.deltaTime);
    } //Cambio de direccion del enemigo
}
       