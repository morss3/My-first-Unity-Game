using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Для работы с UI Slider

public class PlayerHealth : MonoBehaviour {
    [Header("Health Settings")]
    public int currentLives;
    public int playerLives = 3; //max
    public static int numberOfRespawn = 3; //сколько раз можно умереть

    [Header("References")]
    public Animator anim;

    private bool isDead = false;
    private PlayerRespawnManager respawnManager;
    private Slider healthBar; // Статическая ссылка на HealthBar
    void Awake() {
        gameObject.name = "Player"; // Сбросим имя даже у клона
        

    }

    void Start() {
        respawnManager = GameObject.FindObjectOfType<PlayerRespawnManager>();
        currentLives =playerLives; // Инициализируем здоровье

        // Находим HealthBar по тегу
        GameObject healthBarObject = GameObject.FindWithTag("HealthBar");
        if (healthBarObject != null) {
            healthBar = healthBarObject.GetComponent<Slider>();

            // Инициализируем HealthBar
            if (healthBar != null) {
                healthBar.maxValue = playerLives;
                healthBar.value = currentLives;
            }
            else {
                Debug.LogError("На объекте с тегом HealthBar нет компонента Slider!");
            }
        }
        else {
            Debug.LogError("Не найден объект с тегом HealthBar на сцене!");
        }
    }
    public void TakeDamage(int damage) {
        if (isDead) return;

        currentLives -= damage;
        Debug.Log($"Player took {damage} damage, remaining lives: {currentLives}");

        // Обновляем healthbar
        if (healthBar != null) {
            healthBar.value = currentLives;
        }
        // Воспроизводим анимацию получения урона
        anim.SetTrigger("hit");

        if (currentLives <= 0) {
            Die();
        }
    }

    void Die() {
        //isDead = true;

        //// Включаем анимацию смерти
        //anim.SetTrigger("dead");

        //// Отключаем управление (например, PlayerController)
        //GetComponent<PlayerComtroller>().enabled = false;

        //Debug.Log("Player died");
        //respawnManager.OnPlayerDeath(); // без поиска
        //Destroy(gameObject, 1.5f);      // игрок исчезнет — затем появится новый
        //if(numberOfRespawn <= 0) {
        //    // Отключаем управление (например, PlayerController)
        //    GetComponent<PlayerComtroller>().enabled = false;
        //    Debug.Log("Game over");
        //    SceneManager.LoadScene("MainMenu");
        //}
        if (isDead) return;
        isDead = true;

        // Включаем анимацию смерти
        anim.SetTrigger("dead");
        
        // Отключаем управление
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
            Destroy(gameObject, 1.5f); // игрок исчезнет — затем появится новый
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

