using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHintManager : MonoBehaviour
{
    public GameObject controlsHintPanel; // ������ � �����������

    void Start() {
        // ���������� ������ � ������
        if (controlsHintPanel != null) {
            controlsHintPanel.SetActive(true);
        }

        // �������� ����� 5 ������
        Invoke("HideControlsHint", 5f);
    }

    void HideControlsHint() {
        if (controlsHintPanel != null) {
            controlsHintPanel.SetActive(false);
        }
    }
}
