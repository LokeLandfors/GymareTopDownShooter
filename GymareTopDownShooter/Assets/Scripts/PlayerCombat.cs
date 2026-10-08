using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Hierarchy;
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float grabReach;
    [SerializeField] float throwMinVel; //Minimum velocity to kill
    bool useWeapon => InputSystem.actions.FindAction("Action1").ReadValue<float>() > 0.3f;
    bool holdingUse;
    bool swapWeapon => InputSystem.actions.FindAction("Action2").ReadValue<float>() > 0.3f;
    bool holdingSwap;

    Transform upperBody;
    Transform currentWeapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upperBody = transform.Find("UpperBody");
    }

    void Grab(Transform grabTarget)
    {
        grabTarget.parent = transform;
        grabTarget.gameObject.SetActive(false);
        grabTarget.GetComponent<Collider2D>().enabled = false; //Weapon can no longer be grabbed since it is held
        grabTarget.GetComponent<Rigidbody2D>().simulated = false;
        currentWeapon = grabTarget;
    }

    void Throw()
    {
        currentWeapon.parent = null;
        currentWeapon.position = transform.position;
        currentWeapon.gameObject.SetActive(false);
        //currentWeapon.GetComponent<Collider2D>().isTrigger = false;
        currentWeapon.GetComponent<Collider2D>().enabled = true;
        currentWeapon.GetComponent<Rigidbody2D>().simulated = true;
        currentWeapon.GetComponent<Rigidbody2D>().AddForce(upperBody.up, ForceMode2D.Impulse);
        currentWeapon = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (useWeapon && currentWeapon != null)
        {
            currentWeapon.GetComponent<IWeapon>().Use();
        }

        Transform closestWeapon = GetClosestWeapon();
        //ADd highlight effect function
        if (swapWeapon && !holdingSwap)
        {
            print("doSwap");
            if (currentWeapon != null)
            {
                print("throw");
                Throw();
            }
            if (closestWeapon != null) { print("grab"); Grab(closestWeapon); }
        }
        holdingSwap = swapWeapon;
        holdingUse = useWeapon;
    }

    Transform GetClosestWeapon()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position, grabReach);
        List<Transform> weapons = new List<Transform>();
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<IWeapon>() != null)
            {
                weapons.Add(targets[i].transform);
            }
        }
        if (weapons.Count == 0) return null;
        if (weapons.Count == 1) return weapons[0];
        Transform closest = weapons[0];
        for (int i = 1; i < weapons.Count; i++)
        {
            if ((transform.position - weapons[i].position).magnitude < (transform.position - closest.position).magnitude)
            {
                closest = weapons[i];
            }
        }
        return closest;
    }
}