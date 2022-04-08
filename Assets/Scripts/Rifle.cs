using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public ContactFilter2D filter;
    public float fadeTime;
    private float fadeTimer;
    LineRenderer lineRenderer;
    private void Awake() 
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Initialize();
    }
    public override void Fire()
    {
        bool done = false;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position + transform.right * 0.5f);
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Physics2D.Raycast(transform.position + transform.right * 0.5f, transform.right, filter, hits, 20f);
        if(hits.Count == 0)
            lineRenderer.SetPosition(1, transform.position + transform.right * 20.5f);
        else
            foreach(RaycastHit2D hit in hits)
            {
                // сделать тут character и сделать маску зависящей от владельца
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                else 
                    if (lineRenderer.positionCount < 3)
                    {
                        lineRenderer.positionCount = 3;
                        lineRenderer.SetPosition(1, hit.point);
                        List<RaycastHit2D> ricochets = new List<RaycastHit2D>();
                        Vector2 reflection = Vector2.Reflect(transform.right, hit.normal);
                        Physics2D.Raycast(hit.point + hit.normal * 0.05f, reflection, filter, ricochets, 20f);
                        // Debug.Log("Ricochet from point " + hit.point);
                        if (ricochets.Count == 0)
                            lineRenderer.SetPosition(2, hit.point + reflection * 20f);
                        else
                            foreach (RaycastHit2D ricochet in ricochets)
                            {
                                Enemy e = ricochet.transform.gameObject.GetComponent<Enemy>();
                            if (e != null)
                            {
                                e.TakeDamage(damage);
                            }
                            else
                                if (!done)
                                {
                                // Debug.Log("Ricochet stopped by point " + ricochet.point);
                                lineRenderer.SetPosition(2, ricochet.point);
                                    done = true;
                                }
                        }
                    }
            }
        fadeTimer = fadeTime;
        audioManager.Play("rifle" + Random.Range(1, 4));
    }

    public void SetLayer() 
    {
        LayerMask layerMask = filter.layerMask;
        layerMask.value += owner == Owner.player ? 1 << 12 : 1 << 8;
        filter.SetLayerMask(layerMask);
    }
    
    private void Update() 
    {
        if(fadeTimer > 0)
        {
            fadeTimer = Mathf.Clamp(fadeTimer - Time.deltaTime, 0f, fadeTime);
            Color color1 = lineRenderer.startColor;
            Color color2 = lineRenderer.endColor;
            color1.a = fadeTimer / fadeTime;
            color2.a = fadeTimer / fadeTime;
            lineRenderer.startColor = color1;
            lineRenderer.endColor = color2;
        }
    }

}
