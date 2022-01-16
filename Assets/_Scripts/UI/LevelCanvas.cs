using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    public Text hp;
    public Text kunai;
    public Text stone;

    private void Awake()
    {
        updateHp();
        updateKunai();
        updateStone();
    }

    public void updateHp(int value = -1)
    {
        if(value == -1)
        {
            value = PlayerPrefs.GetInt("Hp");
        }
        else
        {
            PlayerPrefs.SetInt("Hp", value);
        }

        hp.text = $"X{value}";
    }

    public void updateKunai(int value = -1)
    {
        if (value == -1)
        {
            value = PlayerPrefs.GetInt("Kunai");
        }
        else
        {
            PlayerPrefs.SetInt("Kunai", value);
        }

        kunai.text = $"X{value}";
    }

    public void updateStone(int value = -1)
    {
        if (value == -1)
        {
            value = PlayerPrefs.GetInt("Stone");
        }
        else
        {
            PlayerPrefs.SetInt("Stone", value);
        }

        stone.text = $"X{value}";
    }
}
