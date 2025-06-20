using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAttack : MonoBehaviour {
    public float lifetime = 0.8f;
    public int damage = 1;

    private Collider2D slashCollider;
    private Animator slashAnimator;

    void Start() {
        // Получаем компоненты на слеш-эффекте
        slashCollider = GetComponent<Collider2D>();
        slashAnimator = GetComponent<Animator>();

        // Выключаем коллайдер на старте
        slashCollider.enabled = false;
    }

    // Метод для активации слеш-эффекта
    public void ActivateSlashEffect() {
        slashCollider.enabled = true;  // Включаем коллайдер на время атаки
        slashAnimator.SetTrigger("attack"); // Запускаем анимацию слеш-эффекта
        // Деактивируем слеш-эффект через 'lifetime' секунд
        Invoke(nameof(DeactivateSlashEffect), lifetime);
    }
    public void DeactivateSlashEffect() {
       slashCollider.enabled = false;
    }
    // Наносим урон врагу при попадании
    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null && !enemy.IsDead()) {
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage,direction); // Наносим урон
            Debug.Log("Enemy damage");
        }
    }
}

