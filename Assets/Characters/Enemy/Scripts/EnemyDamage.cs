using System;
using System.Collections;
using UnityEngine;


public class EnemyDamage : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float Health;
    private KatanaCollisionHandle katanaCollisionHandle;
    private Animator animator;
    private new Collider collider;
  //  public event Action EnemyDeath;
    //public event Action EnemyRespawn;

    private void Start()
    {
        Health = MaxHealth;
        
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        GameObject playerSword = GameObject.FindWithTag("PlayerSword");
        if (playerSword != null)
        {
            katanaCollisionHandle = playerSword.GetComponent<KatanaCollisionHandle>();
  
        }
    }

    public void Damage(float damageValue)
    {
        Health -= damageValue;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
    
        
        if (Health <= 0)
        {
           // EnemyDeath.Invoke();
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        animator.StopPlayback();
        animator.Play("Die02");
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
      // EnemyRespawn.Invoke();
    }

}