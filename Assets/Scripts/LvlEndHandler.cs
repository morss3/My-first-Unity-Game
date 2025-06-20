using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlEnd : MonoBehaviour
{
    public GameObject lvlCompletePanel; // ������ �� ������ "������� �������"
    public Button lvlSelectButton;    // ������ �� ������ "����� ������"

    void Start() {
        // �������, ��� ������ ������ � ������
        if (lvlCompletePanel != null) {
            lvlCompletePanel.SetActive(false);
        }

        // ����������� ������
        if (lvlSelectButton != null) {
            lvlSelectButton.onClick.AddListener(GoToLevelSelect);
        }
        else {
            Debug.LogWarning("������ �� ���������!");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // ���������, ��� � ������� ����� �����
        if (other.CompareTag("Player")) {
            Debug.Log("����� ������ ����� ������!");
            ShowLevelCompletePanel();
        }
    }

    void ShowLevelCompletePanel() {
        // ���������� ������
        if (lvlCompletePanel != null) {
            lvlCompletePanel.SetActive(true);
        }

        // ��������� ����� (����� �� �� ��������)
        Time.timeScale = 0f;
    }

    void GoToLevelSelect() {
        // ��������� �� ����� � ������� ������
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChooseLvl");
    }
}
