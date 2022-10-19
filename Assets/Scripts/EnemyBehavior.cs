using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    private static EnemyBehavior markedForDebugging;
    private static float maxInteractDistance = 3.0f;
    
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

            // If a target was found, navigate to it. Otherwise, just stay put
            if (navTarget != null)
            {
                navMeshAgent.destination = navTarget.transform.position;
                navMeshAgent.isStopped = false;
            }
            else
            {
                navMeshAgent.isStopped = true;
            }
        }
    }

    void OnMouseDown()
    {
        markedForDebugging = this;
        Debug.Log("Enemy marked for debugging. Enable gizmos to see info");
    }

    void OnDrawGizmos()
    {
        if (markedForDebugging != this) return;
        
        Gizmos.color = Color.red;
        Handles.color = Color.red;
        if (navMeshAgent.destination != null && !navMeshAgent.isStopped) Gizmos.DrawLine(transform.position, navMeshAgent.destination);
        String debugText = "Mode: " + pathingMode + "\n" +
                           "To: " + navMeshAgent.destination + " (" + (navMeshAgent.isStopped ? "Stopped" : "Running") + ")\n";
        Handles.Label(transform.position, debugText);
    }

    void DoBehaviorUpdate()
    {
        if (pathingMode == PathingMode.Seated)
        {
            // Check for food on the table, eat it, then leave
            if (markedForDebugging == this) Debug.Log("Seated: ate food");
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
            if (!navTarget.GetComponent<pickUppable>().isPickUppable)
            {
                // Tell the update loop to find a new pick-uppable, since our current target is being held
                if (markedForDebugging == this) Debug.Log("Steal: pickup already taken");
                navTarget = null;
            }
            else if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                // The pick-uppable to steal is within range. Run away with it
                if (markedForDebugging == this) Debug.Log("Steal: pickup grabbed");
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
                if (markedForDebugging == this) Debug.Log("Stealing: bonked");
                // TODO: Drop the pick-uppable we're stealing
                pathingMode = PathingMode.Seating;
                navTarget = null;
            }
            else if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                // We got away. Tell the spawner we've left
                if (markedForDebugging == this) Debug.Log("Stealing: got away");
                FindObjectOfType<EnemySpawner>().OnEnemyLeaving();
                Destroy(this.gameObject);
            }
        }
        else if (pathingMode == PathingMode.Seating)
        {
            // Check if the seat is within range, then sit
            if (Vector3.Distance(transform.position, navTarget.transform.position) <= maxInteractDistance)
            {
                if (markedForDebugging == this) Debug.Log("Seating: sat down");
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
                if (markedForDebugging == this) Debug.Log("Leaving: left");
                Destroy(this.gameObject);
            }
        }
    }
    
    GameObject PickNewTarget<T>() where T: MonoBehaviour
    {
        List<T> foundTargets = new List<T>(FindObjectsOfType<T>());
        if (foundTargets.Count > 0)
        {
            int objToPick = Random.Range(0, foundTargets.Count);
            GameObject pickedObject = foundTargets[objToPick].gameObject;
            
            if (typeof(T) == typeof(EnemySpawner))
            {
                // EnemySpawner has a collection of spawnpoints as children. Pick one of them to go to, instead of the spawner itself
                objToPick = Random.Range(0, pickedObject.transform.childCount);
                pickedObject = pickedObject.transform.GetChild(objToPick).gameObject;
            }
            else if (typeof(T) == typeof(pickUppable))
            {
                // Pick-uppables may be held by other characters. Keep picking a different one until we get one that isn't held
                while (pickedObject && !pickedObject.GetComponent<pickUppable>().isPickUppable)
                {
                    foundTargets.RemoveAt(objToPick);
                    if (foundTargets.Count <= 0)
                    {
                        objToPick = Random.Range(0, foundTargets.Count);
                        pickedObject = foundTargets[objToPick].gameObject;
                    }
                    else
                    {
                        pickedObject = null;
                    }
                }
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
