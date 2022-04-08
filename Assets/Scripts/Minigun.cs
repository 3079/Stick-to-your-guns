using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    public float minCooldown = 0.1f;
    public float timeTillMinCooldown = 3f;
    public bool firing = false;
    public float holdTime = 0f;
    public float reducedCooldown;
    private void Awake()
    {
        Initialize();
        reducedCooldown = cooldown;
    }
    public override void Fire()
    {
        Bullet bullet = Instantiate(this.bullet, transform.position + transform.right * 0.35f, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = owner == Owner.player ? 9 : 13;
        bullet.Launch(transform.right + transform.up * Random.Range(-spread, spread), speed, damage, playerMovement);
        audioManager.Play("minigun" + Random.Range(1, 5));
    }

    public bool GetFiring()
    {
        return firing;
    }

    public void SetFiring(bool state)
    {
        firing = state;
    }

    private void Update()
    {
        if(firing)
            holdTime = Mathf.Clamp(holdTime + Time.deltaTime, 0f, timeTillMinCooldown);
        else 
            holdTime = Mathf.Clamp(holdTime - Time.deltaTime * 3f * cooldown, 0f, timeTillMinCooldown);
        float tmp = Mathf.Clamp(Mathf.Cos(Mathf.PI * (holdTime / timeTillMinCooldown) / 2f), 0f, 1f);
        tmp = Mathf.Pow(tmp, 3.5f);
        reducedCooldown = Mathf.Clamp(cooldown * tmp, minCooldown, cooldown);
    }
}
