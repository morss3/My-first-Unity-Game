using UnityEngine;
using Cinemachine;

public class PlayerRespawnManager : MonoBehaviour {
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private GameObject currentPlayer;

    public CinemachineVirtualCameraBase cam;

    void Start() {
        // ���� ����� ��� ���� � ����� � ��������� ������
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"Found existing player on scene: {currentPlayer.name}");
    }

    public void RespawnPlayer() {
        if (currentPlayer != null) Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        cam.Follow = currentPlayer.transform;
    }

    public void OnPlayerDeath() {
        Invoke(nameof(RespawnPlayer), 2f); // ��������� ������� � ���������
    }

    public GameObject GetCurrentPlayer() {
        return currentPlayer;
    }
}
