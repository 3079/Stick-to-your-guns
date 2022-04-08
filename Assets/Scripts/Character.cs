using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    public bool active = true;
    protected Rigidbody2D rb;
    public float speed = 5f;
    public List<Weapon> guns;
    protected Vector2 movement;
    protected Vector2 lookDirection;
    public float HP = 5;
    protected float currentHP;
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
    }

    public void DecreaseCooldown() 
    {
        for (int i = 0; i < guns.Count; i ++)
        {
            Weapon gun = guns[i];
            gun.SetCooldownTimer(Mathf.Clamp(gun.GetCooldownTimer() - Time.deltaTime, 0f, gun.cooldown));
        }
    }
        
    public void Fire()
    {
        for (int i = 0; i < guns.Count; i ++)
        {
            Weapon gun = guns[i];
            gun.Shoot();
        }
    }

    public void ConnectGun(Weapon weapon)
    {
        guns.Add(weapon);
        weapon.transform.SetParent(this.transform);
        weapon.gameObject.GetComponent<Collider2D>().enabled = false;
        weapon.SetCooldownTimer(0);
        SetOwner(weapon);
    }

    public void SetOwner(Weapon weapon)
    {
        if(this.tag == "Enemy")
            weapon.SetOwner(Weapon.Owner.enemy);
        if(this.tag == "Player")
            weapon.SetOwner(Weapon.Owner.player);
        if(weapon.type == Weapon.Type.rifle)
        {
            weapon.GetComponent<Rifle>().SetLayer();
        }
    }

    public virtual void OnHit() {}

    public virtual void TakeDamage(float value) 
    {
        if(active)
        {
            currentHP -= value;
            Debug.Log(value);
            if (currentHP <= 0)
            {
                Debug.Log(currentHP);
                OnDeath();
            }
        }
    }

    public void DropWeapons() 
    {
        for (int i = 0; i < guns.Count; i++)
            guns[i].gameObject.GetComponent<Collider2D>().enabled = true;
        transform.DetachChildren();
        // for (int i = 0; i < guns.Count; i ++)
        // {
        //     Weapon gun = guns[i];
        //     GameObject drop = Instantiate(gun, gun.transform.position, Quaternion.identity).gameObject;
        //     drop.transform.localScale = gun.transform.lossyScale;
        // }
    }

    public void DestroyWeapon(Weapon weapon)
    {
        guns.Remove(weapon);
        Destroy(weapon.gameObject, 1f);
    }

    public virtual void OnDeath() 
    {
        DropWeapons();
        Destroy(transform.parent.gameObject);
    }

}
