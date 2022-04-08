using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public AudioManager audioManager;
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject doors;
    private bool entered = false;
    private bool cleared = false;
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1, 0f, 0.3f);
        Gizmos.DrawCube(this.transform.position, GetComponent<BoxCollider2D>().size);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Enemy enemy = other.GetComponent<Enemy>();
        enemies.Add(enemy);
        if(enemy != null)
            
        entered = true;
        else
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                ActivateEnemies();
                if(enemies.Count > 0)
                    doors.SetActive(true);
            }
        }
        if(!cleared)
        {
            audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.SetPhase(AudioManager.phases.fightA);
        }
    }

    private void ActivateEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
            if(!enemies[i].active) 
            {
                enemies[i].SetActive();
                enemies[i].SetRoom(this);
            }
    }

    public void EnemyDead()
    {
        if(enemies.Count <= 0)
        {
            cleared = true;
            doors.SetActive(false);
            if(entered)
            {
                audioManager = Object.FindObjectOfType<AudioManager>();
                audioManager.SetPhase(AudioManager.phases.peaceA);
            }
        }
    }
}
