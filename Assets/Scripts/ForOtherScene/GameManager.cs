using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject gameIntroPanel;    // ������ � ��������� ����
    public TextMeshProUGUI introText;    // ����� ��������
    public Button continueButton;        // ������ "�����"

    private string[] introLines = new string[]
    {
        "� � �������-���������, ��������� ���� ����� � ������.",
        "��� ������ � ���������� ������, ������� ����� ��������� �����, ������ ����� � ����.",
        "� ������� ����� ������� �����, �� ���� ���� ����� ��� � ����������...",
        "�� ����� ����������� � ��� �����������?"
    };
    private int currentLineIndex = 0;

    private void Start() {
        // �������� ������ � ������
        if (gameIntroPanel != null) {
            gameIntroPanel.SetActive(false);
        }

        // ����������� ������ "�����"
        if (continueButton != null) {
            continueButton.onClick.AddListener(ShowNextIntroLine);
        }
        else {
            Debug.LogWarning("������ 'ContinueButton' �� ���������!");
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
        // ������ ������� �������� �� SampleScene ���������� ��������
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
            // ��������� �� ����� ������ ������
            ChooseLvl();
        }
    }

}
