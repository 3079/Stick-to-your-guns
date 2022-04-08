using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Weapon
{
    private void Awake()
    {
        Initialize();
        ammo = 1;
    }
    public override void Fire()
    {
        Bullet bullet = Instantiate(this.bullet, transform.position + transform.right * 0.35f, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = owner == Owner.player ? 9 : 13;
        bullet.Launch(transform.right + transform.up * Random.Range(-spread, spread), speed, damage, playerMovement);
        audioManager.Play("pistol" + Random.Range(1, 4));
    }
}
