using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    private int _itemsCollected = 0;
    private int _playerHP = 10;
    public int MaxItems = 4;

    public TMP_Text HealthText;
    public TMP_Text ItemText;
    public TMP_Text ProgressText;
    public GameObject WinPanel;
    public GameObject LossPanel;

    void Start()
    {
        Time.timeScale = 1f;

        ItemText.text = "Items: " + _itemsCollected;
        HealthText.text = "Health: " + _playerHP;

        if (WinPanel != null) WinPanel.SetActive(false);
        if (LossPanel != null) LossPanel.SetActive(false);
    }

    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = "Supplies: " + _itemsCollected;

            if (_itemsCollected >= MaxItems)
            {
                ProgressText.text = "You successfully collected all supplies!";
                if (WinPanel != null)
                    WinPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                int remaining = MaxItems - _itemsCollected;
                ProgressText.text = "Supply collected, " + remaining + " more to go!";
            }
        }
    }

    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            HealthText.text = "Health: " + _playerHP;

            if (_playerHP <= 0)
            {
                UpdateScene("You lost all health points!");
                if (LossPanel != null) LossPanel.SetActive(true);
            }
            else
            {
                ProgressText.text = "Ouch... be careful! -2 Health points.";
            }
        }
    }
    private void UpdateScene(string message)
    {
        ProgressText.text = message;
        Time.timeScale = 0f;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
