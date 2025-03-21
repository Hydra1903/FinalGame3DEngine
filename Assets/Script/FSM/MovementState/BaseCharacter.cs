using UnityEngine;
public class BaseCharacter
{
    public int maxHealth { get; set; }
    public int health { get; set; }
    public float maxMana { get; set; }
    public float mana { get; set; }
    public BaseCharacter(int maxHealth, int health, float maxMana, float mana)
    {
        this.maxHealth = maxHealth;
        this.health = health;   
        this.mana = mana;
        this.maxMana = maxMana;
    }
}
