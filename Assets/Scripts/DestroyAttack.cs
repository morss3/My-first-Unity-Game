using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAttack : MonoBehaviour {
    public float lifetime = 0.8f;
    public int damage = 1;

    private Collider2D slashCollider;
    private Animator slashAnimator;

    void Start() {
        // �������� ���������� �� ����-�������
        slashCollider = GetComponent<Collider2D>();
        slashAnimator = GetComponent<Animator>();

        // ��������� ��������� �� ������
        slashCollider.enabled = false;
    }

    // ����� ��� ��������� ����-�������
    public void ActivateSlashEffect() {
        slashCollider.enabled = true;  // �������� ��������� �� ����� �����
        slashAnimator.SetTrigger("attack"); // ��������� �������� ����-�������
        // ������������ ����-������ ����� 'lifetime' ������
        Invoke(nameof(DeactivateSlashEffect), lifetime);
    }
    public void DeactivateSlashEffect() {
       slashCollider.enabled = false;
    }
    // ������� ���� ����� ��� ���������
    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null && !enemy.IsDead()) {
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage,direction); // ������� ����
            Debug.Log("Enemy damage");
        }
    }
}

