using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class AnglerBehaviour : BaseAiMovement
{
    [SerializeField] float roamSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float FOV;
    [SerializeField] float viewRange;
    float damageCooldown = 2;
    float activeCool = 0;


    enum BehaviourStates
    {
        None,
        Wandering,
        Chasing,
    }

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        plr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    BehaviourStates activeBehaviour = BehaviourStates.None;
    public override void Update()
    {
        base.Update();

        if (SeesPlayer() && activeBehaviour != BehaviourStates.Chasing)
        {
            activeBehaviour = BehaviourStates.Chasing;
            if (activeMovementCoroutine != null) StopCoroutine(activeMovementCoroutine);
            activeMovementCoroutine = StartCoroutine(Chase());
            SetSpeed(chaseSpeed);
        }
        else if (!SeesPlayer() && activeBehaviour != BehaviourStates.Wandering)
        {
            activeBehaviour = BehaviourStates.Wandering;
            if (activeMovementCoroutine != null) StopCoroutine(activeMovementCoroutine);
            activeMovementCoroutine = StartCoroutine(Wander());
            SetSpeed(roamSpeed);
        }

            Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Player") && activeCool == 0)
                {
                    //model.Find("Marool - final").GetComponent<Animation>().Play();
                    print("damage");
                    hits[i].transform.GetComponent<PlayerHealth>().TakeDamage(35);

                activeCool = damageCooldown;
                break;
                }
            }

        activeCool = Mathf.Max(0, activeCool - Time.deltaTime);
    }

    IEnumerator Chase()
    {
        bool chasing = true;
        Vector3 lastPlrPos = plr.position; //For "searching" for the player.
        while (chasing)
        {
            bool reached = true;
            if (SeesPlayer() && reached)
            {
                lastPlrPos = lastPlrPos = plr.position;
                reached = false;
                StartCoroutine(MoveToPoint(plr.position, reachedDestination => { reached = reachedDestination; })); // This is gonna look so jank..
                while (!reached)
                {
                    lastPlrPos = SeesPlayer() ? plr.position : lastPlrPos;
                    yield return null;
                }
            }
            else if (!SeesPlayer() && reached) // "Search" for the player.
            {
                reached = false;
                StartCoroutine(MoveToPoint(plr.position, reachedDestination => { reached = reachedDestination; }));
                while (!reached) yield return null;
                if (!SeesPlayer())
                {
                    chasing = false;
                }
            }
            print("new iteration");
            yield return null;
        }
    }

    bool SeesPlayer()
    {
        if (plr == null)
        {
            return false;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, plr.position - transform.position, out hit, 15))
        {
            Debug.DrawRay(transform.position, plr.position - transform.position);
            float closestMag = (hit.transform.position - transform.position).magnitude;
            

            float viewOffset = Vector3.Angle(transform.forward, hit.transform.position - transform.position);
            if (viewOffset <= FOV && hit.transform == plr) return true;
        }

        return false;
    }
}
