using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMove2 : MonoBehaviour
{

    public Transform goal;
    public bool debug;
    public NavMeshAgent agent;
    NavMeshPath path;
    Vector3 pathBegin;

    public float rangePerTick = float.PositiveInfinity;
    public bool ticking;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // SetPath(goal.position);
        if (debug && path != null)
        {
            var oldPath = pathBegin;
            for (var i = 0; i < path.corners.Length; i++)
            {
                Debug.DrawLine(oldPath, path.corners[i], Color.red, 0.1f, false);
                oldPath = path.corners[i];
            }
        }
    }

    public void SetPath(Vector3 position)
    {
        path = new NavMeshPath();
        pathBegin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, float.PositiveInfinity, NavMesh.AllAreas);
        Vector3 finalPosition = hit.position;
        agent.CalculatePath(finalPosition, path);
        if (float.IsInfinity(rangePerTick) || !ticking)
        {
            agent.SetPath(path);
        }


    }
}