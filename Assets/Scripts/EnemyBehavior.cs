using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameBehavior _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
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
        if (other.name == "Player")
        {
            Debug.Log("Player detected!");

            if (_gameManager != null)
            {
                _gameManager.HP -= 2;
                Debug.Log("Player HP: " + _gameManager.HP);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name == "Player")
            {
                Debug.Log("Player out of range");
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
}
