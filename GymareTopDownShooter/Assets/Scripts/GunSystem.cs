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

    BulletManager bulletPool;
    float fireCool;
    float currentRecoil;
    int currentAmmo;

    bool wasUsed;

    public void Use(string targetTag, Transform usePoint)
    {
        if (fireCool > 0 || currentAmmo <= 0) return;
        print(currentAmmo);
        fireCool = 60/fireRate;
        currentAmmo -= 1;
        if (bulletsPerShot == 1)
        {
            bulletPool.MakeBullet(usePoint.position, usePoint.eulerAngles.z + Random.Range(-currentRecoil, currentRecoil), 20f, dmg, targetTag);
            currentRecoil += recoil;
            return;
        }
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            print(usePoint.rotation.z * Mathf.Rad2Deg);
            bulletPool.MakeBullet(usePoint.position, usePoint.eulerAngles.z + Random.Range(-recoil, recoil), 20f, dmg, targetTag);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletPool = GameObject.Find("BulletPool").GetComponent<BulletManager>();
        currentAmmo = ammo;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentRecoil = Mathf.Clamp(currentRecoil - Time.fixedDeltaTime, 0, recoil * 3);
        fireCool -= Time.fixedDeltaTime;
    }
}
