using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public List<WaveObject> waves = new List<WaveObject>(); //Cada vez que se crea un nuevo objeto, se podra configurar las oleadas
    public bool isWaitingForNextWave; //Tiempo a que empieze la prox oleada
    public bool wavesFinish; //Saber si termino la oleada
    public int currentWave; //Saber si esta en una oleada ahora
    public Transform initPosition; //Punto de inicio de la oleada

    public TextMeshProUGUI counterText; //Contador
    public GameObject buttonNextWave; //Boton para proxima oleada

    private void Start()
    {
        StartCoroutine(ProcesWave()); //Empieza el evento de una oleada
    }
    private void Update()
    {
        CheckCounterAndShowButton(); //Boton para la prox oleada
        checkCounterForNextWave(); //Comprueba si termino el contador para empezar la siguiente oleada
    }

    private void checkCounterForNextWave() //Comprobar que iniciara la prox oleada
    {
        if (isWaitingForNextWave && !wavesFinish) //Comprueba si ya termino la oleada actual y empieza la siguiente
        {
            waves[currentWave].counterToNextWave -= 1 * Time.deltaTime; //Contador
            counterText.text = waves[currentWave].counterToNextWave.ToString("00"); //Interactua con el texto
            if (waves[currentWave].counterToNextWave <= 0) //Llega a 0 activa el evento
            {
                ChangeWave(); //Cambia a la siguiente oleada
                Debug.Log("Set Next Wave");
            }
        }
    }

    public void ChangeWave() //Cambia de oleada a la siguiente
    {
        if (wavesFinish)
            return;
        currentWave++;
        StartCoroutine(ProcesWave());
    }

    private IEnumerator ProcesWave() //Procesa la siguiente oleada
        {
            if (wavesFinish)
                yield break; //Return de baja calidad
            isWaitingForNextWave = false;
            waves[currentWave].counterToNextWave = waves[currentWave].timeForNextWave;
            for (int i = 0; i < waves[currentWave].enemys.Count ; i++) //Creara un enemigo y esperara el tiempo de creacion para crear el prox enemigo
            {
                var enemyGo = Instantiate(waves[currentWave].enemys[i], initPosition.position, initPosition.rotation);
                yield return new WaitForSeconds(waves[currentWave].timerPerCreation);
            }
            isWaitingForNextWave = true; //Comprobara si se ha terminado la oleada
            if (currentWave >= waves.Count-1)
            {
                Debug.Log("Nivel Terminado");
                wavesFinish = true; //terminamos con todas las oleadas
            }
        }
    private void CheckCounterAndShowButton() //Comprobarta el contador y al activar el boton de la proxima oleada
        {
            if (wavesFinish)
            {
                buttonNextWave.SetActive(isWaitingForNextWave);
                counterText.gameObject.SetActive(isWaitingForNextWave);
            }
        }
    }

[System.Serializable]
public class WaveObject //Configuracion de oleadas
{
    public float timerPerCreation = 1; //Cada segundo crea un objeto
    public float timeForNextWave = 10; //Tiempo a esperar para la prox oleada
    [HideInInspector] public float counterToNextWave = 0; //Dejarlo en 0. Tiempo que tarda en empezar la segunda oleada
    public List<Enemy> enemys = new List<Enemy>(); //Lista para agregar enemigos y spawnearlos.
}
