using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSystemScript : MonoBehaviour
{

    public string mapName = "";
    GameObject player;
    [SerializeField]
    Text victoryText = default;
    [SerializeField]
    Text medalText = default;
    [SerializeField]
    Text recordText = default;
    public static Vector2 startPos;
    float time = 0;
    [SerializeField]
    Text timeText = default;

    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject VictoryScreen;
    bool menuKeyPressed = false;
    bool menuActive = false;

    [SerializeField]
    Button[] buttons;

    [SerializeField]
    Button nextLevelButton;
    [SerializeField]
    string nextLevel;

    [SerializeField]
    float record = 99;

    [SerializeField]
    Slider volumeSlider;

    bool gameStarted = false;

    [Header("Medal Times")]
    [SerializeField]
    float bronzeTime;
    [SerializeField]
    float silverTime;
    [SerializeField]
    float goldTime;
    [SerializeField]
    float devTime;

    [SerializeField]
    Text timeToBeatText;

    [SerializeField]
    Animator medalAnim;

    int medalUnlocked = 4;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        player = GameObject.Find("Player");
        victoryText.text = "";
        startPos = player.transform.position;
        menuKeyPressed = false;
        PauseMenu.SetActive(false);
        menuActive = false;

        buttons[0].onClick.AddListener(BackToGame);
        buttons[1].onClick.AddListener(BackToMenu);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        Time.timeScale = 0;
        VictoryScreen.SetActive(false);
        medalText.text = "";

        nextLevelButton.onClick.AddListener(NextLevel);


        timeToBeatText.text = devTime.ToString() + " S \n" + goldTime.ToString() + " S \n" + silverTime.ToString() +  " S \n" + bronzeTime.ToString() + " S";
    }

    // Update is called once per frame
    void Update()
    {


        if (!menuKeyPressed)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuKeyPressed = true;
                menuActive = true;

                return;
                //Application.Quit();
            }
        }

        if (menuKeyPressed)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuKeyPressed = false;
                menuActive = false;
                return;
                //Application.Quit();
            }
        }

        if(!gameStarted)
        {
            if (Input.anyKeyDown)
            {
                gameStarted = true;
                return;
            }
        }
        

        if (gameStarted)
        {
            if (!GoalScript.Victory)
                time += Time.deltaTime;

            if (!menuActive)
            {
                Time.timeScale = 0.95f;
                PauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
            }
        }
        else
            Time.timeScale = 0;
        

        

        float timer = Mathf.Round(time * 100f) / 100f;
        timeText.text = "" + timer;

        if(Input.GetKeyDown(KeyCode.R))
            Restart();

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetFloat(mapName, 99.99f);
        }

        RecordTimer(timer);
        VolumeController();
    }

    void VolumeController()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        AudioListener.volume = PlayerPrefs.GetFloat("Volume", volumeSlider.value);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    void BackToGame()
    {
        menuKeyPressed = false;
        menuActive = false;
        return;
    }

    void RecordTimer(float timer)
    {
        recordText.text = "" + PlayerPrefs.GetFloat(mapName, 99.99f);

        if(GoalScript.Victory == true)
        {
            if (GoalScript.Victory == true && timer < PlayerPrefs.GetFloat(mapName, 99.99f))
            {
                record = timer;
                PlayerPrefs.SetFloat(mapName, record);
                VictoryScreen.SetActive(true);
                victoryText.text = "Cheese  Retrieved! \n New Record!!\n Time:  " + PlayerPrefs.GetFloat(mapName, record);
                return;
            }
            else if (timer == PlayerPrefs.GetFloat(mapName, 99.99f))
            {
                VictoryScreen.SetActive(true);
                victoryText.text = "Tied  with  old  record!! \n Time:  " + timer;
            }
            else if (timer > PlayerPrefs.GetFloat(mapName, 99.99f))
            {
                VictoryScreen.SetActive(true);
                victoryText.text = "Cheese  Retrieved! \n Time:  " + timer + "\n Record:  " + PlayerPrefs.GetFloat(mapName, record);
            }

            TimerText();

            MedalRewards(PlayerPrefs.GetFloat(mapName, record));
        }
        else
            victoryText.text = " ";
    }


    void BackToMenu()
    {
        MusicBot audio = GameObject.Find("MusicBot").GetComponent<MusicBot>();
        audio.PlayTheme();

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void Restart()
    {
        MovementScript.death = false;
        GoalScript.Victory = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MedalRewards(float record)
    {
        if (record <= devTime)
        {
            medalUnlocked = 0;
            //medal.sprite = devMedal;
            medalAnim.Play("PlatinumMedal");
            PlayerPrefs.SetInt("Medal" + mapName, medalUnlocked);
            medalText.text = "You beat the developer's record!";
            return;
        }
        else if (record <= goldTime)
        {
            medalUnlocked = 1;
            medalAnim.Play("GoldMedal");
            PlayerPrefs.SetInt("Medal" + mapName, medalUnlocked);
            medalText.text = "Gold  Medal  time!";
            return;
        }
        else if (record <= silverTime)
        {
            medalUnlocked = 2;
            medalAnim.Play("SilverMedal");
            PlayerPrefs.SetInt("Medal" + mapName, medalUnlocked);
            medalText.text = "Silver  Medal  time!";
            return;
        }
        else if (record <= bronzeTime)
        {
            medalUnlocked = 3;
            medalAnim.Play("BronzeMedal");
            PlayerPrefs.SetInt("Medal" + mapName, medalUnlocked);
            medalText.text = "Bronze  Medal  time!";
            return;
        }
        else
        {
            medalUnlocked = 4;
            medalAnim.Play("EmptyAnimation");
            PlayerPrefs.SetInt("Medal" + mapName, medalUnlocked);
            return;
        }
    }

    void TimerText()
    {
        if (timer <= devTime)
        {
            medalText.text = "You beat the developer's record!";
        }
        if (timer <= goldTime)
        {
            medalText.text = "Gold  Medal  time!";
        }
        if (timer <= silverTime)
        {
            medalText.text = "Silver  Medal  time!";
        }
        if (timer <= bronzeTime)
        {
            medalText.text = "Bronze  Medal  time!";
        }
    }
}