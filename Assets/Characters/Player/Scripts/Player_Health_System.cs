using System;
using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
public class Player_Health_System : MonoBehaviour
{
    
    //event
    public event Action<float> OnPlayerDamage;
    // local
    Animator animator;
    //[SerializeField]
    GameObject EnemySword;
    EnemyKatanaCollisionHandle EnemyKatanaCollisionHandle;
    Collider Collider;
    // Interface
    public float maxHealth;
    public float CurrentHealth;
    public float HealValue = 10;
    public event Action PlayerDeath;
    public event Action PlayerRespawn;
    private void Awake()
    {
        EnemySword = GameObject.FindWithTag("EnemySword");
        EnemyKatanaCollisionHandle = EnemySword.GetComponent<EnemyKatanaCollisionHandle>();
        animator = GetComponent<Animator>();
        maxHealth = 1000f;
        Collider = GetComponent<Collider>();
        CurrentHealth = maxHealth;
        
       
    }
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Mathf.Clamp(CurrentHealth, 0, 100);
        if (OnPlayerDamage != null)
        {
            OnPlayerDamage.Invoke(CurrentHealth);
        }
        //OnPlayerDamage.Invoke(CurrentHealth);
        if (CurrentHealth <= 0) { Die(); }
    }

    private void Die()
    {

        StartCoroutine(DeathAnimation());
    }
    IEnumerator DeathAnimation()
    {
        animator.SetBool("Dead", true);
        Collider.enabled = false;

        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Dead", false);
        PlayerDeath.Invoke();
        yield return new WaitForSeconds(3f);
        Revive();
    }

    public void Heal()
    {
        CurrentHealth += HealValue;
        Mathf.Clamp(CurrentHealth, 0, 100);
    }

    public void Revive()
    {
        CurrentHealth = maxHealth;
        Collider.enabled = true;
        PlayerRespawn.Invoke();
    }

}
