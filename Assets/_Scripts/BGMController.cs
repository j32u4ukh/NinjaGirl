using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    public AudioClip bgm_main_menu;
    public AudioClip bgm_level_select;
    public AudioClip bgm_normal_level;
    public AudioClip bgm_boss_level;
    public AudioClip btn_decision;
    public AudioClip btn_invalid;
    AudioSource audio_source;

    private void Awake()
    {
        audio_source = GetComponent<AudioSource>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                audio_source.clip = bgm_main_menu;
                audio_source.volume = 0.2f;
                break;
            case "LevelSelect":
                audio_source.clip = bgm_level_select;
                audio_source.volume = 0.2f;
                break;
            case "Level1":
            case "Level2":
                audio_source.clip = bgm_normal_level;
                audio_source.volume = 0.2f;
                break;
            case "Level3":
                audio_source.clip = bgm_boss_level;
                audio_source.volume = 0.2f;
                break;
        }

        if (audio_source.clip)
        {
            audio_source.Play();
        }
    }

    public void playDecisionSound()
    {
        audio_source.volume = 0.2f;
        audio_source.PlayOneShot(btn_decision);
    }

    public void playInvalidSound()
    {
        audio_source.volume = 0.2f;
        audio_source.PlayOneShot(btn_invalid);
    }
}
