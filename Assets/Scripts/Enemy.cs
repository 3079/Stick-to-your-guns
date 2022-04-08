using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public enum State 
    {
        immobile,
        trajectory,
        chase
    }
    public bool canAim = true;
    public float aimingSpeed;
    public State state;
    [Header("Rotation")]    
    public bool rotating = false;
    public float rotationSpeed;
    [Header("Trajectory Parameters")]
    public List<Transform> waypoints;
    public float minWaypointDistance = 0.1f;
    private int currentWaypoint = 0;
    [Header("Chase Parameters")]
    public float stopDistance;
    public float meeleDamage = 3f;
    private Vector2 lookDir;
    private GameObject player;
    private bool hasGun = false;
    private Room room;
    void Awake() 
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentHP = HP;
        guns.Clear();

        guns.AddRange(transform.GetComponentsInChildren<Weapon>());
        
        for (int i = 0; i < guns.Count; i ++)
        {
            Weapon gun = guns[i];
            SetOwner(gun);
        }

        Transform tmp = transform.parent.Find("Waypoints");
        for (int i = 0; i < tmp.childCount; i++)
            waypoints.Add(tmp.GetChild(i));

        active = false;
    }
    void FixedUpdate()
    {
        if(active)
        {
            hasGun = guns.Count > 0;
            DecreaseCooldown();
            if(rotating)
                rb.rotation += rotationSpeed * Time.deltaTime;
            switch(state) 
            {
                case State.immobile:
                    Immobile();
                    break;
                case State.trajectory:
                    Patrol();
                    break;
                case State.chase:
                    Chase();
                    break;
            }
        }
    }

    void Patrol() 
    {
        if (!hasGun)
        {
            rotating = false;
            state = State.chase;
            return;
        }
        if(canAim)
            Aim();
        if(waypoints.Count != 0)
        {
            Vector2 distance = waypoints[currentWaypoint].position - transform.position;
            if(distance.magnitude <= minWaypointDistance)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                distance = waypoints[currentWaypoint].position - transform.position;
            }
            Vector2 move = distance.normalized;
            rb.MovePosition(rb.position + move * speed * Time.deltaTime);
            Fire();
        }
    }

    public void SetActive() 
    {
        active = true;
        player = GameObject.Find("Player");
    }

    void Chase()
    {
        Vector2 move = player.transform.position - transform.position;
        Aim();
        if(hasGun) 
        {
            if(move.magnitude > stopDistance)
                rb.MovePosition(rb.position + move.normalized * speed * Time.deltaTime);
            else
                Fire();
        } else 
        {
            rb.MovePosition(rb.position + move.normalized * speed * Time.deltaTime);
        }
    }

    void Immobile()
    {
        Vector2 move = player.transform.position - transform.position;
        if(canAim)
            Aim();
        if (hasGun)
            Fire();
        else 
        {
            rotating = false;
            state = State.chase;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player && state == State.chase && !hasGun)
        {
            player.GetComponent<Player>().TakeDamage(meeleDamage);
            OnDeath();
        }
    }
    public void SetRoom(Room room)
    {
        this.room = room;
    }

    public override void OnDeath()
    {
        room.enemies.Remove(this);
        room.EnemyDead();
        base.OnDeath();
    }

    void Aim() 
    {
        Vector2 move = player.transform.position - transform.position;
        rb.rotation = Mathf.LerpAngle(rb.rotation, Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg, aimingSpeed * Time.deltaTime);
    }
}
