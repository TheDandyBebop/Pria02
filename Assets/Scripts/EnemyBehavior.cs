using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Importa la librería de navegación de Unity

public class EnemyBehavior : MonoBehaviour
{
    // Referencia al jugador y a la ruta de patrulla
    public Transform Player;
    public Transform PatrolRoute;

    // Lista de ubicaciones de patrulla
    public List<Transform> Locations;

    // Índice para el recorrido de la patrulla
    private int _locationIndex = 0;

    // Referencia al agente de navegación
    private NavMeshAgent _agent;

    // Vidas del enemigo
    private int _lives = 3;

    void Start()
    {
        // Encuentra al jugador en la escena
        Player = GameObject.Find("Player").transform;

        // Obtiene el componente NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();

        // Inicializa la ruta de patrulla
        InitializePatrolRoute();

        // Mueve al enemigo a la primera ubicación de la patrulla
        MoveToNextPatrolLocation();
    }

    void Update()
    {
        // Si el enemigo llega a su destino, se mueve al siguiente punto
        if (_agent.remainingDistance < 0.2f && !_agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }

    // Propiedad para manejar las vidas del enemigo
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;

            // Si el enemigo se queda sin vidas, se destruye
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
            }
        }
    }

    // Detecta colisión con proyectiles
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1; // Resta una vida
            Debug.Log("Critical hit!");
        }
    }

    // Inicializa la ruta de patrulla guardando sus puntos en la lista
    void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    // Detecta cuando el jugador entra en el rango de ataque
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            _agent.destination = Player.position; // Persigue al jugador
            Debug.Log("Enemigo atacando!");
        }
    }

    // Detecta cuando el jugador sale del rango de ataque
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Enemigo buscando");
        }
    }

    // Mueve al enemigo a la siguiente ubicación en la ruta de patrulla
    void MoveToNextPatrolLocation()
    {
        if (Locations.Count == 0) return;

        _agent.destination = Locations[_locationIndex].position; // Asigna el destino

        // Actualiza el índice para ir al siguiente punto en la ruta
        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }
}
