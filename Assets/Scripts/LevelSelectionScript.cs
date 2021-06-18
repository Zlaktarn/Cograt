using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionScript : MonoBehaviour
{
    [SerializeField]
    Button[] buttons;


    void Start()
    {
        buttons[0].onClick.AddListener(() => StringLevelToLoad("Tutorial"));
        buttons[1].onClick.AddListener(() => LevelLoaded(1));
        buttons[2].onClick.AddListener(() => LevelLoaded(2));
        buttons[3].onClick.AddListener(() => LevelLoaded(3));
        buttons[4].onClick.AddListener(() => LevelLoaded(4));
        buttons[5].onClick.AddListener(() => LevelLoaded(5));
        buttons[6].onClick.AddListener(() => LevelLoaded(6));
        buttons[7].onClick.AddListener(() => StringLevelToLoad("Prototype"));

    }

    void LevelLoaded(int i)
    {
        SceneManager.LoadScene("Level" + i, LoadSceneMode.Single);
        MusicBot music = GameObject.Find("MusicBot").GetComponent<MusicBot>();
        music.PlayGameMusic();
    }
    void StringLevelToLoad(string i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Single);
        MusicBot music = GameObject.Find("MusicBot").GetComponent<MusicBot>();
        music.PlayGameMusic();
    }

    void Update()
    {
        
    }
}
