using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    float flipSpeed = 4;

    [Header("Main Menu")]
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    Button levelSelectButton;
    [SerializeField]
    Button storyButton;
    [SerializeField]
    Button optionsButton;
    [SerializeField]
    Button exitButton;
    [SerializeField]
    bool mainMenuBool = true;

    [Header("Level Select")]
    [SerializeField]
    GameObject levelSelect;
    [SerializeField]
    Button LevelToMenuButton;
    [SerializeField]
    bool levelMenuBool = false;

    [Header("Options")]
    [SerializeField]
    GameObject options;
    [SerializeField]
    Button optionsBackButton;
    [SerializeField]
    bool optionsBool = false;

    [Header("Story")]
    [SerializeField]
    GameObject story;
    [SerializeField]
    Button StoryToMenuButton;
    [SerializeField]
    bool storyMenuBool = false;

    AudioSource audioSource;

    float x = 0;
    float y = 0;

    [SerializeField]
    float volume = 0.5f;
    [SerializeField]
    Slider volumeSlider;


    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", volume);
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Time.timeScale = 0.95f;
        levelSelectButton.onClick.AddListener(LevelSelectMenu);
        storyButton.onClick.AddListener(StoryMenu);
        optionsButton.onClick.AddListener(OptionsMenu);
        optionsBackButton.onClick.AddListener(BackToMenu);
        LevelToMenuButton.onClick.AddListener(BackToMenu);
        StoryToMenuButton.onClick.AddListener(BackToMenu);
        exitButton.onClick.AddListener(ExitGame);
        //volume = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    public void LevelSelectMenu()
    {
        audioSource.Play();
        if (mainMenuBool)
        {
            levelMenuBool = true;
            mainMenuBool = false;
        }
    }

    void StoryMenu()
    {
            audioSource.Play();
        if (mainMenuBool)
        {
            storyMenuBool = true;
            mainMenuBool = false;
        }
    }

    void OptionsMenu()
    {
        audioSource.Play();

        if (mainMenuBool)
        {
            optionsBool = true;
            mainMenuBool = false;
        }
    }

    void BackToMenu()
    {
        audioSource.Play();

        if (levelMenuBool)
        {
            mainMenuBool = true;
            levelMenuBool = false;
        }

        if (storyMenuBool)
        {
            mainMenuBool = true;
            storyMenuBool = false;
        }

        if(optionsBool)
        {
            mainMenuBool = true;
            optionsBool = false;
        }
    }

    void Update()
    {
        MainMenu();

        VolumeEditor();
    }

    void MainMenu()
    {
        if (y < 0)
            y = 0;

        if (mainMenuBool && (x != 0 || y != 0))
        {
            if (x < 0)
                x += 1920 * Time.deltaTime * flipSpeed;
            if (x > 0)
                x -= 1920 * Time.deltaTime * flipSpeed;
            if (y > 0)
                y -= 1080 * Time.deltaTime * flipSpeed;

        }
        if (levelMenuBool)
        {
            if (x > -1920)
            {
                x -= 1920 * Time.deltaTime * flipSpeed;
            }
            if (x <= -1920)
                x = -1920;
        }
        if (storyMenuBool)
        {
            if (x < 1920)
            {
                x += 1920 * Time.deltaTime * flipSpeed;
            }
            if (x >= 1920)
                x = 1920;
        }
        if (optionsBool)
        {
            x = 0;
            if (y < 1080)
            {
                y += 1080 * Time.deltaTime * flipSpeed;
            }
            if (y >= 1080)
                y = 1080;
        }

        transform.localPosition = new Vector2(x, y);
    }


    void ExitGame()
    {
        Application.Quit();
    }


    void VolumeEditor()
    {
        volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volume);


        
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", volume);
    }
}
