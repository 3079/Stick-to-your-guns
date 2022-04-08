using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool invincible = false;
    public float invincibilityTime = 3;
    void Update()
    {
        DecreaseCooldown();
        if(Input.GetMouseButton(0))
            Fire();

        if(Input.GetMouseButtonUp(0))
            for (int i = 0; i < guns.Count; i++)
                if(guns[i].type == Weapon.Type.minigun)
                    guns[i].GetComponent<Minigun>().SetFiring(false);
    }

    private void FixedUpdate() 
    {
        Vector2 position = rb.position;
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = move.normalized * speed;
        position += movement * Time.deltaTime;
        rb.MovePosition(position);
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position;
        lookDirection.Normalize();
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public override void TakeDamage(float value) 
    {
        if(!invincible) 
        {
            currentHP -= value;
            if(currentHP <= 0)
                OnDeath();
            StartCoroutine(Invincibility());
        }
    }

    public IEnumerator Invincibility() 
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }
}
