using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private GameBehavior _gameManager;
    private NavMeshAgent _agent;

    public Transform PatrolRoute;
    private List<Transform> _locations = new List<Transform>();
    private int _locationIndex = 0;

    private Transform _player;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").transform;

        if (PatrolRoute != null)
        {
            foreach (Transform child in PatrolRoute)
            {
                _locations.Add(child);
            }
        }

        if (_locations.Count > 0)
        {
            MoveToNextPatrolLocation();
        }
    }

    void Update()
    {
        if (_locations.Count > 0 && _agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (_locations.Count == 0) return;

        _agent.destination = _locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % _locations.Count;
    }

    private int _enemyLives = 3;
    public int EnemyLives
    {
        get { return _enemyLives; }
        private set
        {
            _enemyLives = value;
            if (_enemyLives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");

            if (_gameManager != null)
            {
                _gameManager.HP -= 2;
                Debug.Log("Player HP: " + _gameManager.HP);
            }

            if (_agent != null && _player != null)
            {
                _agent.destination = _player.position;
            }
        }

        else if(other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range");

            if (_locations.Count > 0)
            {
                MoveToNextPatrolLocation();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit! Enemy lives: " + _enemyLives);
        }
    }

    private void TakeDamage(int amount)
    {
        EnemyLives -= amount;
    }
}
