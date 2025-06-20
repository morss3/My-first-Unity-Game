using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pauseGame;
    public GameObject pauseGameMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseGame) {
                Resume();
            }
            else {
                Pause();
            }
        }
            
    }

    public void Resume() {
        pauseGameMenu.SetActive(false); //панель паузы неактивна
        Time.timeScale = 1f; //игра в норм режиме
        pauseGame = false; //игра не на паузе
    }
    public void Pause() {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;
    }
    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
