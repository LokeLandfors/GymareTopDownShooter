using UnityEngine;

public class BulletManager : PoolManager
{
    public void MakeBullet(Vector3 position, float rotation, float velocity, float damage, string target)
    {
        Transform bullet = Enqeue();
        bullet.position = position;
        print(rotation);
        bullet.rotation = Quaternion.Euler(0,0,rotation);
        bullet.GetComponent<BulletLogic>().InitiateBullet(velocity, damage, target);
        bullet.gameObject.SetActive(true);
    }
}