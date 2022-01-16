using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHp : MonoBehaviour
{
    Player player;
    LevelCanvas level_canvas;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        level_canvas = GameObject.Find("/Canvas").GetComponent<LevelCanvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            //int hp = PlayerPrefs.GetInt("Hp") + 1;
            //player.hp = hp;
            //level_canvas.updateHp(hp);
            player.updateHp(value: PlayerPrefs.GetInt("Hp") + 1);
            Destroy(gameObject);
        }
    }
}
