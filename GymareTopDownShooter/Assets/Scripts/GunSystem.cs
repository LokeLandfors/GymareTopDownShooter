using Unity.VisualScripting;
using UnityEngine;

public class GunSystem : MonoBehaviour, IWeapon
{
    [SerializeField] float dmg;
    [SerializeField] bool autoFire;
    [SerializeField] float fireRate;
    [SerializeField] int bulletsPerShot;
    [SerializeField] float recoil;
    [SerializeField] float recoilRecoverSpeed;
    [SerializeField] int ammo;

    PoolManager bulletPool;
    float fireCool;
    float currentRecoil;
    int currentAmmo;

    public void Use()
    {
        if (fireCool > 0 || ammo <= 0) return;
        ammo -= 1;
        if (bulletsPerShot == 1)
        {
            Transform bullet = bulletPool.Enqeue();
            bullet.position = transform.position;
            bullet.rotation = transform.rotation * Quaternion.Euler(0,0,Random.Range(-currentRecoil, currentRecoil));
            currentRecoil += recoil;
            return;
        }
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Transform bullet = bulletPool.Enqeue();
            bullet.position = transform.position;
            bullet.rotation = transform.rotation * Quaternion.Euler(0, 0, Random.Range(-recoil, recoil));
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletPool = GameObject.Find("BulletPool").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fireCool -= Time.fixedDeltaTime;
    }
}
