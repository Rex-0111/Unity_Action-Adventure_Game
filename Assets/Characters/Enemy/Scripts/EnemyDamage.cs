using System.Collections;
using UnityEngine;


public class EnemyDamage : MonoBehaviour
{
    [Range(0, 100)] [SerializeField]private float Health = 100;
    private KatanaCollisionHandle katanaCollisionHandle;
    private Animator animator;
    private new Collider collider;
    

    private void Start()
    {
        
        
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
        Health = Mathf.Clamp(Health, 0, 100);
    
        
        if (Health <= 0)
        {
            
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
    }

}