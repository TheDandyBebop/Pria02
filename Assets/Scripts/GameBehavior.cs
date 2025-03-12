using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    // Máximo de ítems a recolectar
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

    // Método para actualizar el texto de progreso y pausar el juego
    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f; // Pausa el tiempo del juego
    }

    // Propiedad para manejar los ítems recolectados
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = ItemText.text + Items; // Actualiza el texto de ítems

            if (_itemsCollected >= MaxItems)
            {
                WinButton.gameObject.SetActive(true); // Activa el botón de victoria
                UpdateScene("¡Encontraste todos los ítems!");
            }
            else
            {
                // Muestra cuántos ítems faltan
                ProgressText.text = "Item encontrado, faltan " + (MaxItems - _itemsCollected) + " más!";
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
                LossButton.gameObject.SetActive(true); // Activa el botón de derrota
                UpdateScene("¿Necesitás otra vida?");
            }
            else
            {
                ProgressText.text = "¡Eso dolió!"; // Mensaje de daño
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
