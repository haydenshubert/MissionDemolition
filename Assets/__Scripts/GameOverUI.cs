using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RestartNormalGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartSkyGame()
    {
        SceneManager.LoadScene(1);
    }
}
