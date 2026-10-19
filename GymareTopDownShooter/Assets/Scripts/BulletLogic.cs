using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    float vel;
    float dmg;
    string hurtTag;

    BulletManager bulletPool;

    private void Start()
    {
        bulletPool = GameObject.FindAnyObjectByType<BulletManager>().GetComponent<BulletManager>();
    }

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

    private void FixedUpdate()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, vel * Time.fixedDeltaTime);
        transform.position += transform.up * vel * Time.deltaTime;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.gameObject.CompareTag(hurtTag))
            {
                hit[i].transform.GetComponent<IDamageable>().Damage(dmg);
                bulletPool.Reqeue(transform);
                gameObject.SetActive(false);
                print("hit enemy");
                return;
            }
            else if (hit[i].transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                print("struck wall");
                bulletPool.Reqeue(transform);
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
