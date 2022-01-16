using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            string scene_name = SceneManager.GetActiveScene().name;
            string level_string = scene_name.Substring(5);
            int level = int.Parse(level_string);

            PlayerPrefs.SetInt("clear_level", Math.Max(level, PlayerPrefs.GetInt("clear_level", 0)));
            FadeInOut.instance.sceneFadeInOut(scene_name: "LevelSelect");
        }
    }
}
