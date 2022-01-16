using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStone : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            player.updateStone(value: PlayerPrefs.GetInt("Stone") + 1);
            Destroy(gameObject);
        }
    }
}
