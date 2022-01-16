using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFirstPlayCheck : MonoBehaviour
{
    private void Awake()
    {
        firstTimePlayState();
    }

    public void firstTimePlayState()
    {
        if (!PlayerPrefs.HasKey("IsFirstTimePlay"))
        {
            PlayerPrefs.SetInt("IsFirstTimePlay", 1);
            PlayerPrefs.SetInt("Hp", 5);
            PlayerPrefs.SetInt("Kunai", 2);
            PlayerPrefs.SetInt("Stone", 0);
            PlayerPrefs.SetInt("clear_level", 0);
        }
    }
}
