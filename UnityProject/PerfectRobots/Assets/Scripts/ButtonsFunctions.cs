using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.Instance.stateUnpause();
        
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.Instance.events.firstSelectedGameObject.SetActive(false);
        GameManager.Instance.stateUnpause();
    }

    public void Respawn()
    {
        GameManager.Instance.playerScript.SpawnPlayer();
        GameManager.Instance.stateUnpause();
    }

    public void bStart()
    {
        GameManager.Instance.stateUnpause();
        //GameManager.Instance.startGame = true;
        GameManager.Instance.isPaused = false;
    }

    public void quit()
    {
        Application.Quit();
    }
}
