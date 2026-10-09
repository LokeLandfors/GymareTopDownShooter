using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    float vel;
    float dmg;
    string hurtTag;

    private void OnDisable()
    {
        vel = 0;
        dmg = 0;
        hurtTag = null;
    }

    public void InitiateBullet(float velocity, float damage, string targetTag)
    {
        vel = velocity;
        dmg = damage;
        hurtTag = targetTag;
    }
}
