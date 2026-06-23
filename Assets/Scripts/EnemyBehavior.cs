using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private GameBehavior _gameManager;
    private NavMeshAgent _agent;

    public Transform PatrolRoute;
    private List<Vector3> _localRouteOffsets = new List<Vector3>();
    private int _locationIndex = 0;

    private Transform _player;
    private bool _playerInRange = false;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").transform;

        _localRouteOffsets.Clear();
        if (PatrolRoute != null)
        {
            foreach (Transform child in PatrolRoute)
            {
                Vector3 localOffset = PatrolRoute.InverseTransformPoint(child.position);
                _localRouteOffsets.Add(localOffset);
            }
        }

        if (_localRouteOffsets.Count > 0)
        {
            MoveToNextPatrolLocation();
        }
    }

    void Update()
    {
        if (_playerInRange && _agent != null && _player != null)
        {
            _agent.SetDestination(_player.position);
            return;
        }

        if (_localRouteOffsets.Count > 0 && _agent != null && !_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolLocation();
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (_localRouteOffsets.Count == 0 || _agent == null) return;

        Vector3 targetWorld = transform.TransformPoint(_localRouteOffsets[_locationIndex]);
        _agent.SetDestination(targetWorld);

        _locationIndex = (_locationIndex + 1) % _localRouteOffsets.Count;
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

        else if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range");

            if (_localRouteOffsets.Count > 0)
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
