using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ��� ������ � UI Slider

public class PlayerHealth : MonoBehaviour {
    [Header("Health Settings")]
    public int currentLives;
    public int playerLives = 3; //max
    public static int numberOfRespawn = 3; //������� ��� ����� �������

    [Header("References")]
    public Animator anim;

    private bool isDead = false;
    private PlayerRespawnManager respawnManager;
    private Slider healthBar; // ����������� ������ �� HealthBar
    void Awake() {
        gameObject.name = "Player"; // ������� ��� ���� � �����
        

    }

    void Start() {
        respawnManager = GameObject.FindObjectOfType<PlayerRespawnManager>();
        currentLives =playerLives; // �������������� ��������

        // ������� HealthBar �� ����
        GameObject healthBarObject = GameObject.FindWithTag("HealthBar");
        if (healthBarObject != null) {
            healthBar = healthBarObject.GetComponent<Slider>();

            // �������������� HealthBar
            if (healthBar != null) {
                healthBar.maxValue = playerLives;
                healthBar.value = currentLives;
            }
            else {
                Debug.LogError("�� ������� � ����� HealthBar ��� ���������� Slider!");
            }
        }
        else {
            Debug.LogError("�� ������ ������ � ����� HealthBar �� �����!");
        }
    }
    public void TakeDamage(int damage) {
        if (isDead) return;

        currentLives -= damage;
        Debug.Log($"Player took {damage} damage, remaining lives: {currentLives}");

        // ��������� healthbar
        if (healthBar != null) {
            healthBar.value = currentLives;
        }
        // ������������� �������� ��������� �����
        anim.SetTrigger("hit");

        if (currentLives <= 0) {
            Die();
        }
    }

    void Die() {
        //isDead = true;

        //// �������� �������� ������
        //anim.SetTrigger("dead");

        //// ��������� ���������� (��������, PlayerController)
        //GetComponent<PlayerComtroller>().enabled = false;

        //Debug.Log("Player died");
        //respawnManager.OnPlayerDeath(); // ��� ������
        //Destroy(gameObject, 1.5f);      // ����� �������� � ����� �������� �����
        //if(numberOfRespawn <= 0) {
        //    // ��������� ���������� (��������, PlayerController)
        //    GetComponent<PlayerComtroller>().enabled = false;
        //    Debug.Log("Game over");
        //    SceneManager.LoadScene("MainMenu");
        //}
        if (isDead) return;
        isDead = true;

        // �������� �������� ������
        anim.SetTrigger("dead");
        
        // ��������� ����������
        var controller = GetComponent<PlayerComtroller>();
        if (controller != null) {
            controller.enabled = false;
        }

        Debug.Log("Player died");

        numberOfRespawn--;
        Debug.Log(numberOfRespawn);
        if (numberOfRespawn <= 0) {
            Debug.Log("Game over");

            Invoke(nameof(LoadGameOverScene),2f);
        }
        else {
            if (respawnManager != null) {
                respawnManager.OnPlayerDeath();
            }
            Destroy(gameObject, 1.5f); // ����� �������� � ����� �������� �����
        }
    }

    public bool IsDead() {
        return isDead;
    }
    private void LoadGameOverScene() {
        numberOfRespawn = 3;
        SceneManager.LoadScene("GameOver");
    }
}

