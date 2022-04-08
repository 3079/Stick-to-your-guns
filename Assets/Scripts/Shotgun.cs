using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public int bulletsMin;
    public int bulletsMax;
    public float spreadAngle = 30f;
    private void Awake() 
    {
        Initialize();
    }
    public override void Fire()
    {
        int bullets = Random.Range(bulletsMin, bulletsMax);
        float interval = spreadAngle / (bullets - 1);
        for (int i = -bullets / 2; i < bullets - bullets / 2; i ++) {
            Vector3 direction = Quaternion.Euler(0f, 0f, interval * i) * transform.right;
            Bullet bullet = Instantiate(this.bullet, transform.position + transform.right * 0.45f, Quaternion.identity).GetComponent<Bullet>();
            bullet.gameObject.layer = owner == Owner.player ? 9 : 13;
            bullet.Launch(direction + transform.up * Random.Range(-spread, spread), speed, damage, playerMovement);
        }
        audioManager.Play("shotgun" + Random.Range(1, 3));
    }
}
