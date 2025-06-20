using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLvlManager : MonoBehaviour
{
    public Button level1Button;              // Кнопка для уровня 1
    public GameObject preLevelDialoguePanel; // Панель диалога перед уровнем
    public TextMeshProUGUI dialogueText;     // Текст диалога
    public Button nextButton;                // Кнопка "Далее"

    private string[] preLevelDialogue = new string[]
    {
        "Душа: Меня зовут Элиза… Я не могу уйти, пока не передам последние слова.",
        "Элиза: Мой брат винит себя в моей смерти…",
        "Элиза: Мы с ним всегда любили сидеть на старой скамейке возле кладбища…",
        "Элиза: Пожалуйста, доставь это письмо и оставь его на той скамейке.",
        "Элиза: Может, он почувствует облегчение.",
        "Элиза: Но будь осторожен — на твоём пути будут враги.",
        "Почтальон: Я сделаю всё, что в моих силах, Элиза."
    };
    private int currentLineIndex = 0;
    void Start() {
        // Скрываем панель в начале
        if (preLevelDialoguePanel != null) {
            preLevelDialoguePanel.SetActive(false);
        }

        // Настраиваем кнопки
        if (level1Button != null) {
            level1Button.onClick.AddListener(StartPreLevelDialogue);
        }
        if (nextButton != null) {
            nextButton.onClick.AddListener(ShowNextLine);
        }
    }

    void StartPreLevelDialogue() {
        preLevelDialoguePanel.SetActive(true);
        currentLineIndex = 0;
        ShowNextLine();
    }

    void ShowNextLine() {
        if (currentLineIndex < preLevelDialogue.Length) {
            dialogueText.text = preLevelDialogue[currentLineIndex];
            currentLineIndex++;
        }
        else {
            // Загружаем уровень
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
