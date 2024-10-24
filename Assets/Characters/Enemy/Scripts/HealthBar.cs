using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    EnemyDamage EnemyDamage;
    Slider Slider;
    void Start()
    {
        Slider = GetComponent<Slider>();
        EnemyDamage = GetComponentInParent<EnemyDamage>(); 
    }
    void Update() => Slider.value  = EnemyDamage.Health / EnemyDamage.MaxHealth;
    
}
