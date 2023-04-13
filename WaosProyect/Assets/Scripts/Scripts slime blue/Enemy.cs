using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("Movement")] //Encabezado de variables
    public List<Transform> waypoints = new List<Transform>(); // Lista de puntos que seguira el enemigo
    
    // Primer movimiento hacia el un punto, P1
    private int targetIndex = 1;
    
    // Variable que determina la vel.
    public float movementSpeed = 4;

    //Var de la velocidad que rota el eje al doblar las "esquinas"
    public float rotationSpeed = 6;

    [Header("Life")] //Encabezado de la vida
    public float MaxLife = 100; //Valor f de la vida
    public float currentLife = 0; //Valor f vida actual
    public Image fillLifeImage; //La barra de vida
    public Transform canvasRoot; //Var para evitar la rotacion del canvas, barra de vida
    private Quaternion initLifeRotation; //Quat pora evitar la rot del canvas

    private void Awake()
    {
        canvasRoot = fillLifeImage.transform.parent.parent;
        initLifeRotation = canvasRoot.rotation;
    }

    private void Start()
    {
        currentLife = MaxLife; //Se empieza con la vida max
    }
    void Update()
    {
        canvasRoot.transform.rotation = initLifeRotation; //Constantemente el canvas evita la rot con la variable Quater.
        Movement();
        LookAt();
        if (Input.GetKeyDown(KeyCode.Space)) //Test de dmg al enemigo 
        {
            TakeDamage(20);
        }
    }

    //Sección del movimiento y la rotacion
    #region Movement & Rotations

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

    #endregion

    //Sección de la vida (canvas y cantidad) y muerte
    #region Life and Death

    public void TakeDamage(float dmg) //Daño que disminuira la vida del enemigo
    {
        var newLife = currentLife - dmg;
        if (newLife <= 0)
        {
            OnDead(); //Activa el evento donde muere
        }
        //La vida actual movera la barra de vida segun el valor que tenga
        currentLife = newLife;
        var fillValue = currentLife * 1 / 100;
        fillLifeImage.fillAmount = fillValue;
        currentLife = newLife;
    }
    
    //<-- Se podria usar un IEnumerator para la animacion de muerte y daño
    private void OnDead() 
    {
        //<-- Aca se puede colocar las animaciones de muerte *insertar animator*
        currentLife = 0; //Si la vida es 0, se muere, al igual que la barra de vida
        fillLifeImage.fillAmount = 0;
        movementSpeed = 0;
        Destroy(fillLifeImage);
        StartCoroutine(OnDeadEffect());
    } //Evento donde se muere el enemigo

    private IEnumerator OnDeadEffect() //El enemigo se desvanece del camino
    {
        yield return new WaitForSeconds(0.5f); //La cantidad de seg antes de que se ejecute el comando
        var finalPositionY = transform.position.y - 5;
        Vector3 target = new Vector3(transform.position.x, finalPositionY, transform.position.z);
        while (transform.position.y != finalPositionY)
        {
            //Velocidad que realiza para que el "cuerpo vaya bajo tierra"
            transform.position = Vector3.MoveTowards(transform.position, target, 1.5f * Time.deltaTime);
            yield return null; //Evita un bucle infinito
        }
    }

    #endregion
    
}
       