using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    private void Start()
    {
        //GameManager.Instance.Begin();
    }
    public void resume()
    {
        GameManager.Instance.stateUnpause();
        
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.stateUnpause();
    }

    public void Respawn()
    {
        GameManager.Instance.playerScript.SpawnPlayer();
        GameManager.Instance.stateUnpause();
    }

    public void quit()
    {
        Application.Quit();
    }

}
