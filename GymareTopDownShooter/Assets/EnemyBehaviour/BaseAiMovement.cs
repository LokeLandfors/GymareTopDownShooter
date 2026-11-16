using System;
using System.Collections;
using Unity.AI.Navigation;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BaseAiMovement : MonoBehaviour // Core movement functionality, intended to be inheriteted from by enemy ai scripts
                                            
{
    public NavMeshAgent agent;
    public Transform model;
    public Transform plr;

    [SerializeField] Transform testTarget;

    float moveSpeed;

    bool hasDestination = false;
    Vector3 activeDestination;
    public Coroutine activeMovementCoroutine;

    virtual public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        print(model);
        plr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool ReachedPathPoint => agent.remainingDistance < agent.stoppingDistance;

    public virtual void Update()
    {
        if (hasDestination)
        {
            float yDif = activeDestination.y - model.position.y;
            if (yDif != 0)
            {
                RaycastHit[] obstacles = Physics.RaycastAll(model.position, activeDestination, (activeDestination - model.position).magnitude);
                if (obstacles.Length == 0) // Nothing is in the way for the ray, the enemy can see the target
                                           // Also im too lazy to change from raycastall to regular raycast with if statement
                {
                    if (yDif > 0)
                    {
                        model.position += Vector3.up * agent.speed * Time.deltaTime;
                    }
                    else if (yDif < 0)
                    {
                        model.position -= Vector3.up * agent.speed * Time.deltaTime;
                    }
                }
            }
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
        agent.speed = moveSpeed;
    }

    public IEnumerator MoveToPoint(Vector3 point, Action<bool> reachedDestination) // This function is awfull and i entirely blame offmeshlinks for being shit
    {
        reachedDestination(false);
        hasDestination = true;
        activeDestination = point;
        if (NavMesh.SamplePosition(point, out NavMeshHit hit, 20f, NavMesh.AllAreas) && // Get target point or closest possible point
            NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, new NavMeshPath())) // ensure path is possible
        {
            agent.SetDestination(hit.position);
        }
        else
        {   // Honestly don't remember what this did, i think it finds the edge on the navmesh closest to the point though so yeah.
            NavMesh.FindClosestEdge(point, out NavMeshHit hit1, NavMesh.AllAreas);
            agent.SetDestination(hit1.position);
        }
            float yOffset = point.y - transform.position.y;
        while (!ReachedPathPoint)
        {
            if (agent.isOnOffMeshLink)
            {
                // I tried to put this into its own ienumerator but for some reason it broke and i have no clue why

                float lerpAmount = 0; // Increased each iteration, loop stops at one
                Vector3 initialOffset = transform.position - agent.currentOffMeshLinkData.startPos; // Needed to prevent the snapping caused
                                                                                                    // by the CompleteOffMeshLink function
                initialOffset.y = 0;
                Vector3 startPos = transform.position;
                Vector3 finalPos = agent.currentOffMeshLinkData.endPos - initialOffset;
                finalPos.y = startPos.y;
                while (lerpAmount < 1)
                {
                    Vector3 newPos = Vector3.Lerp(startPos, finalPos, lerpAmount);
                    agent.Move(newPos - transform.position); // Convert pos to a vector3 offset and move the agent
                    lerpAmount = Mathf.Min(lerpAmount + Time.deltaTime, 1); // Ensure value is limited to avoid overshooting the lerp
                    yield return null;
                }
                Vector3 finnishedPos = transform.position;
                agent.CompleteOffMeshLink();

                agent.nextPosition = finnishedPos; // extra ensurance that the agent doesn't snap.
            }
            yield return null;
        }
        reachedDestination(true); // Reached the point.
        hasDestination = false;
    }

    public IEnumerator Wander() // This is so shit but i don't have the time to make it good
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 origin = transform.position;
            Vector3 rayDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f), Random.Range(-1f, 1f));

            RaycastHit hit;
            Physics.Raycast(origin, rayDir, out hit, 15f); //Supress error because im lazy
            for (int i = 0; i < Random.Range(8, 15); i++) // Randomly increase or decrease the chance of wandering further
            {
                Ray cast = new(origin, rayDir);
                if (Physics.Raycast(origin, rayDir, out hit, 15))
                {
                    Debug.DrawRay(origin, hit.point, Color.cyan);
                    origin = hit.point;
                    rayDir = (new Vector3(Random.Range(-1f, 1f), Random.Range(-0.1f, 0.1f), Random.Range(-1f, 1f)) + hit.normal).normalized;
                }
                else // Hit nothing
                {
                    origin = cast.GetPoint(15);

                    break; // Unnescessary to cast rays since the agent was already given a descent range to move
                }
            }
            bool finnishedMove = false;
            StartCoroutine(MoveToPoint(new Ray(origin, rayDir).GetPoint(hit.distance - 1), reachedDestination => { finnishedMove = reachedDestination; }));
            while (!finnishedMove) yield return null; //Wait until the coroutine has finnished before wandering again
        }
    }
}
