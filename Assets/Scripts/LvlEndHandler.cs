using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlEnd : MonoBehaviour
{
    public GameObject lvlCompletePanel; // Ссылка на панель "Уровень пройден"
    public Button lvlSelectButton;    // Ссылка на кнопку "Выбор уровня"

    void Start() {
        // Убедись, что панель скрыта в начале
        if (lvlCompletePanel != null) {
            lvlCompletePanel.SetActive(false);
        }

        // Настраиваем кнопку
        if (lvlSelectButton != null) {
            lvlSelectButton.onClick.AddListener(GoToLevelSelect);
        }
        else {
            Debug.LogWarning("Кнопка не привязана!");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player")) {
            Debug.Log("Игрок достиг конца уровня!");
            ShowLevelCompletePanel();
        }
    }

    void ShowLevelCompletePanel() {
        // Показываем панель
        if (lvlCompletePanel != null) {
            lvlCompletePanel.SetActive(true);
        }

        // Отключаем время (чтобы он не двигался)
        Time.timeScale = 0f;
    }

    void GoToLevelSelect() {
        // Переходим на сцену с выбором уровня
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChooseLvl");
    }
}
