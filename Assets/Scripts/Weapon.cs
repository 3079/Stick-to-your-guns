using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Type
    {
        basegun,
        pistol,
        shotgun,
        rifle,
        minigun
    }
    public Type type;
    public float cooldown;
    protected float cooldownTimer;
    public GameObject bullet;
    public float capacity;
    public float damage;
    public float speed;
    public float spread;
    protected Vector2 playerMovement;
    protected float ammo;
    public enum Owner {
        player,
        enemy
    }
    protected Owner owner;
    public AudioManager audioManager;
    public void Shoot() 
    {
        if(cooldownTimer <= 0 && ammo > 0)
        {
            Fire();
            if(type != Type.basegun)
                ConsumeAmmo();
            if(type == Type.minigun) 
            {
                Minigun minigun = this.GetComponent<Minigun>();
                if(!minigun.GetFiring())
                    minigun.SetFiring(true);
                cooldownTimer = minigun.reducedCooldown;
            } else
                cooldownTimer = cooldown;
        }
    }

    public abstract void Fire();

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            character.ConnectGun(this);
        }
    }

    public void SetCooldownTimer(float value)
    {
        cooldownTimer = value;
    }

    public void SetOwner(Owner owner)
    {
        this.owner = owner;
    }

    public float GetCooldownTimer()
    {
        return cooldownTimer;
    }

    public void SetPlayerMovement(Vector2 vector)
    {
        playerMovement = vector;
    }

    public Vector2 GetPlayerMovement()
    {
        return playerMovement;
    }

    public void Initialize() 
    {
        ammo = capacity;
        audioManager = Object.FindObjectOfType<AudioManager>();
    }

    public void ConsumeAmmo() 
    {
        ammo--;
        if(ammo <= 0)
        {
            transform.GetComponentInParent<Character>().DestroyWeapon(this);
        }
    }
}
