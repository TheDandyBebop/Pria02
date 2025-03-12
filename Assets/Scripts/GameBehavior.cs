using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    // M�ximo de �tems a recolectar
    public int MaxItems = 1;

    // Referencias a los elementos UI
    public TMP_Text HealthText;
    public TMP_Text ItemText;
    public TMP_Text ProgressText;
    public Button WinButton;
    public Button LossButton;

    // Variables privadas
    private int _itemsCollected = 0;
    private int _playerHP = 10;

    void Start()
    {
        // Inicializa los textos con los valores iniciales
        ItemText.text += _itemsCollected;
        HealthText.text += _playerHP;
    }

    // M�todo para actualizar el texto de progreso y pausar el juego
    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f; // Pausa el tiempo del juego
    }

    // Propiedad para manejar los �tems recolectados
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = ItemText.text + Items; // Actualiza el texto de �tems

            if (_itemsCollected >= MaxItems)
            {
                WinButton.gameObject.SetActive(true); // Activa el bot�n de victoria
                UpdateScene("�Encontraste todos los �tems!");
            }
            else
            {
                // Muestra cu�ntos �tems faltan
                ProgressText.text = "Item encontrado, faltan " + (MaxItems - _itemsCollected) + " m�s!";
            }
        }
    }

    // Propiedad para manejar la vida del jugador
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            HealthText.text = "Health: " + HP; // Actualiza el texto de salud

            if (_playerHP <= 0)
            {
                LossButton.gameObject.SetActive(true); // Activa el bot�n de derrota
                UpdateScene("�Necesit�s otra vida?");
            }
            else
            {
                ProgressText.text = "�Eso doli�!"; // Mensaje de da�o
            }

            Debug.LogFormat("Vidas restantes: {0}", _playerHP);
        }
    }

    // Reinicia la escena volviendo al nivel 0
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f; // Reactiva el tiempo del juego
    }
}
