using System;
using UnityEngine;

public class EnemyKatanaCollisionHandle : MonoBehaviour
{

    SwordHandler handler;
    private void Start()
    {
        handler = GetComponentInParent<SwordHandler>();
    }
    private void OnTriggerEnter(Collider other)
    {
        float damage = handler.DamegeToPlayer;
    Player_Attacks player_Attacks = other.GetComponent<Player_Attacks>();
        if (other.CompareTag("Player") && player_Attacks.CanPlayerGetHit)
        {
            Player_Health_System player_Health_System = other.GetComponent<Player_Health_System>();
            if (player_Health_System != null)
            {
                player_Health_System.Damage(damage);
            }
            else
            {
                Debug.LogWarning("EnemyDamage component not found on the enemy object.");
            }
        }
    }
}
