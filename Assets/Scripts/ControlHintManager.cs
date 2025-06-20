using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHintManager : MonoBehaviour
{
    public GameObject controlsHintPanel; // Панель с подсказками

    void Start() {
        // Показываем панель в начале
        if (controlsHintPanel != null) {
            controlsHintPanel.SetActive(true);
        }

        // Скрываем через 5 секунд
        Invoke("HideControlsHint", 5f);
    }

    void HideControlsHint() {
        if (controlsHintPanel != null) {
            controlsHintPanel.SetActive(false);
        }
    }
}
