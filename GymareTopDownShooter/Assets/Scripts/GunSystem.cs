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

    [SerializeField] Transform bulletPrefab;
    float fireCool;
    int currentAmmo;

    public void Use()
    {
        if (fireCool > 0 || ammo <= 0) return;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fireCool -= Time.fixedDeltaTime;
    }
}
