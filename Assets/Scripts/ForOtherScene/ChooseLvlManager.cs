using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLvlManager : MonoBehaviour
{
    public Button level1Button;              // ������ ��� ������ 1
    public GameObject preLevelDialoguePanel; // ������ ������� ����� �������
    public TextMeshProUGUI dialogueText;     // ����� �������
    public Button nextButton;                // ������ "�����"

    private string[] preLevelDialogue = new string[]
    {
        "����: ���� ����� ������ � �� ���� ����, ���� �� ������� ��������� �����.",
        "�����: ��� ���� ����� ���� � ���� ������",
        "�����: �� � ��� ������ ������ ������ �� ������ �������� ����� ���������",
        "�����: ����������, ������� ��� ������ � ������ ��� �� ��� ��������.",
        "�����: �����, �� ����������� ����������.",
        "�����: �� ���� ��������� � �� ���� ���� ����� �����.",
        "���������: � ������ ��, ��� � ���� �����, �����."
    };
    private int currentLineIndex = 0;
    void Start() {
        // �������� ������ � ������
        if (preLevelDialoguePanel != null) {
            preLevelDialoguePanel.SetActive(false);
        }

        // ����������� ������
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
            // ��������� �������
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
