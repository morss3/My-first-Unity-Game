using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject gameIntroPanel;    // Панель с описанием игры
    public TextMeshProUGUI introText;    // Текст описания
    public Button continueButton;        // Кнопка "Далее"

    private string[] introLines = new string[]
    {
        "Я — призрак-почтальон, связующий миры живых и мёртвых.",
        "Моя задача — доставлять письма, которые несут последние слова, полные любви и боли.",
        "Я помогаю душам обрести покой, но этот путь полон слёз и опасностей...",
        "Ты готов отправиться в это путешествие?"
    };
    private int currentLineIndex = 0;

    private void Start() {
        // Скрываем панель в начале
        if (gameIntroPanel != null) {
            gameIntroPanel.SetActive(false);
        }

        // Настраиваем кнопку "Далее"
        if (continueButton != null) {
            continueButton.onClick.AddListener(ShowNextIntroLine);
        }
        else {
            Debug.LogWarning("Кнопка 'ContinueButton' не привязана!");
        }
    }
    public void ChooseLvl() {
        SceneManager.LoadScene("ChooseLvl");
        //SceneManager.LoadScene(1);
    }
    public void Exit() {
        Application.Quit();
        Debug.Log("Exit");
    }
    public void Back() {
        SceneManager.LoadScene("MainMenu");
        //SceneManager.LoadScene(0);
    }
    public void Play() {
        // Вместо прямого перехода на SampleScene показываем описание
        StartIntro();
    }
    void StartIntro() {
        if (gameIntroPanel != null) {
            gameIntroPanel.SetActive(true);
        }
        currentLineIndex = 0;
        ShowNextIntroLine();
    }
    void ShowNextIntroLine() {
        if (currentLineIndex < introLines.Length) {
            introText.text = introLines[currentLineIndex];
            currentLineIndex++;
        }
        else {
            // Переходим на сцену выбора уровня
            ChooseLvl();
        }
    }

}
