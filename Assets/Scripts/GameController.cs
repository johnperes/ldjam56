using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    void Start()
    {
        Instance = this;
    }

    public void NextLevel()
    {
        GameData.CurrentLevel++;
        SceneManager.LoadScene("Level-" + GameData.CurrentLevel);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level-" + GameData.CurrentLevel);
    }
}
