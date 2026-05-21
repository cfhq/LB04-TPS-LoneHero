using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameBehavior GameManager;

    private void Start()
    {
        GameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
        Debug.Log(gameObject.name + " : " + transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Item collected!");

            if (GameManager != null)
            {
                GameManager.Items += 1;
            }
        }
    }
}
