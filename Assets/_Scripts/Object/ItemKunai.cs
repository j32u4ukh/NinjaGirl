using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKunai : MonoBehaviour
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
            player.updateKunai(value: PlayerPrefs.GetInt("Kunai") + 1);
            Destroy(gameObject);
        }
    }
}
