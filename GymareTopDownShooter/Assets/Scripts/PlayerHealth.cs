using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float startHealth;
    float hp;
    bool invincible = false;

    void Start() { hp = startHealth; }

    public void SetInvincible(bool newState)
    {
        invincible = newState;
    }
    public void Damage(float dmg)
    {
        if (invincible) return;
        hp -= dmg;
        if (hp <= 0)
        {
            print("dead");
        }
    }
}
