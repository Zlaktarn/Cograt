using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordSystem : MonoBehaviour
{
    [SerializeField]
    Text[] recordText;
    [SerializeField]
    Button resetButton;

    [SerializeField]
    Animator[] levelAnim;
    [SerializeField]
    GameObject[] particle;

    [SerializeField]
    Sprite devMedal;
    [SerializeField]
    Sprite goldMedal;
    [SerializeField]
    Sprite silverMedal;
    [SerializeField]
    Sprite bronzeMedal;

    void Start()
    {
        resetButton.onClick.AddListener(ResetScore);

        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].SetActive(false);
        }
    }

    void Update()
    {
        GetRecord(0, "Tutorial");
        GetRecord(1, "Level1");
        GetRecord(2, "Level2");
        GetRecord(3, "Level3");
        GetRecord(4, "Level4");
        GetRecord(5, "Level5");
        GetRecord(6, "Level6");
        GetRecord(7, "Prototype");
    }

    //Snygga till
    void ResetScore()
    {
        PlayerPrefs.SetFloat("Tutorial", 99.99f);
        PlayerPrefs.SetFloat("Level1", 99.99f);
        PlayerPrefs.SetFloat("Level2", 99.99f);
        PlayerPrefs.SetFloat("Level3", 99.99f);
        PlayerPrefs.SetFloat("Level4", 99.99f);
        PlayerPrefs.SetFloat("Level5", 99.99f);
        PlayerPrefs.SetFloat("Level6", 99.99f);
        PlayerPrefs.SetFloat("Prototype", 99.99f);


        PlayerPrefs.SetInt("Medal" + "Tutorial", 4);
        PlayerPrefs.SetInt("Medal" + "Level1", 4);
        PlayerPrefs.SetInt("Medal" + "Level2", 4);
        PlayerPrefs.SetInt("Medal" + "Level3", 4);
        PlayerPrefs.SetInt("Medal" + "Level4", 4);
        PlayerPrefs.SetInt("Medal" + "Level5", 4);
        PlayerPrefs.SetInt("Medal" + "Level6", 4);
        PlayerPrefs.SetInt("Medal" + "Prototype", 4);

        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].SetActive(false);
        }
    }

    void GetRecord(int i, string map)
    {
        float record = PlayerPrefs.GetFloat(map, 99.99f);

        if (record >= 99.99f)
            recordText[i].text = "-none-";
        else
            recordText[i].text = PlayerPrefs.GetFloat(map, 99.99f) + "";

        int medalUnlocked = PlayerPrefs.GetInt("Medal" + map, 5);

        if(medalUnlocked == 0)
        {
            levelAnim[i].Play("PlatinumMedal");
            particle[i].SetActive(true);
        }
        else if (medalUnlocked == 1)
        {
            levelAnim[i].Play("GoldMedal");
        }
        else if (medalUnlocked == 2)
        {
            levelAnim[i].Play("SilverMedal");
        }
        else if (medalUnlocked == 3)
        {
            levelAnim[i].Play("BronzeMedal");
        }
        else
        {
            levelAnim[i].Play("EmptyAnimation");
        }


    }
}
