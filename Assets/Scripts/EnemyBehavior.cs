using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    private static float maxInteractDistance = 1.0f;
    
    private NavMeshAgent navMeshAgent;
    private GameObject navTarget;
    private PathingMode pathingMode;
    private bool isBonked = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        pathingMode = PathingMode.StealPickUppable;
    }

    // Update is called once per frame
    void Update()
    {
        DoBehaviorUpdate();
        
        // Path towards an appropriate object depending on the current pathing mode
        // No pathing is necessary if we're sitting down
        if (navTarget == null && pathingMode != PathingMode.Seated)
        {
            if (pathingMode == PathingMode.StealPickUppable)
            {
                navTarget = PickNewTarget<pickUppable>();
            }
            else if (pathingMode == PathingMode.Stealing || pathingMode == PathingMode.Leaving)
            {
                navTarget = PickNewTarget<EnemySpawner>();
            }
            else if (pathingMode == PathingMode.Seating)
            {
                navTarget = PickNewTarget<Sittable>();
            }
            
            navMeshAgent.destination = navTarget.transform.position;
        }
    }

    void DoBehaviorUpdate()
    {
        if (pathingMode == PathingMode.Seated)
        {
            // Check for food on the table, eat it, then leave
            // TODO: Check for food
            // TODO: Eat it
            pathingMode = PathingMode.Leaving;
            navTarget = null;
            
            // Tell the spawner we're leaving
            FindObjectOfType<EnemySpawner>().OnEnemyLeaving();
        }
        else if (navTarget == null)
        {
            // No nav target is set. Bail out since every other behavior check requires one
            return;
        }
        else if (pathingMode == PathingMode.StealPickUppable)
        {
            // Check if the pick-uppable to steal is within range, then run away with it
            if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                // TODO: Pick up the pick-uppable
                pathingMode = PathingMode.Stealing;
                navTarget = null;
            }
        }
        else if (pathingMode == PathingMode.Stealing)
        {
            // Check if we've been bonked or successfully got away
            if (isBonked)
            {
                // We've been bonked. Drop the pick-uppable and go sit down
                // TODO: Drop the pick-uppable we're stealing
                pathingMode = PathingMode.Seating;
                navTarget = null;
            }
            else if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                // We got away. Tell the spawner we've left
                FindObjectOfType<EnemySpawner>().OnEnemyLeaving();
                Destroy(this.gameObject);
            }
        }
        else if (pathingMode == PathingMode.Seating)
        {
            // Check if the seat is within range, then sit
            if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                // TODO: Snap to the seat
                pathingMode = PathingMode.Seated;
                navTarget = null;
            }
        }
        else if (pathingMode == PathingMode.Leaving)
        {
            // Check if the exit is within range, then destroy ourselves
            if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    GameObject PickNewTarget<T>() where T: MonoBehaviour
    {
        T[] foundTargets = FindObjectsOfType<T>();
        if (foundTargets.Length > 0)
        {
            int objToPick = Random.Range(0, foundTargets.Length);
            GameObject pickedObject = foundTargets[objToPick].gameObject;
            
            if (typeof(T) == typeof(EnemySpawner))
            {
                // EnemySpawner has a collection of spawnpoints as children. Pick one of them to go to, instead of the spawner itself
                objToPick = Random.Range(0, pickedObject.transform.childCount);
                pickedObject = pickedObject.transform.GetChild(objToPick).gameObject;
            }
            
            return pickedObject;
        }
        return null;
    }
}

enum PathingMode
{
    StealPickUppable,
    Stealing,
    Seating,
    Seated,
    Leaving
}
