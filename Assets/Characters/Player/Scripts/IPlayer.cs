interface IPlayer 
{
    float maxHealth { get; set; }  // should be called MaxHealth
    float CurrentHealth { get; set; }
   
    float healthValue { get; set; }
    void Damage(float value);
    void Heal();
    void Revive();
    
}
