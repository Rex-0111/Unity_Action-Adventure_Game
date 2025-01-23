using System;
using UnityEngine;

public class KatanaCollisionHandle : MonoBehaviour
{
    [SerializeField] float Damage;
    public float Buffs;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Get the EnemyDamage component from the collided enemy
            EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                // Apply damage to the enemy
                enemyDamage.Damage(Damage + Buffs);
            }
            else
            {
                Debug.LogWarning("EnemyDamage component not found on the enemy object.");
            }
        }
    }
}
