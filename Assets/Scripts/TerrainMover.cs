// TerrainMover.cs
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TerrainMover : MonoBehaviour
{
    public Terrain terrain;
    private NavMeshAgent agent;
    private Quaternion lookRotation;

    void Start()
    {
        terrain = FindObjectOfType<Terrain>();
        agent = GetComponent<NavMeshAgent>();  //< cache NavMeshAgent component
        agent.updateRotation = false;          //< let us control the rotation explicitly
        lookRotation = transform.rotation;     //< set original rotation
    }

    Vector3 GetTerrainNormal()
    {
        Vector3 terrainLocalPos = transform.position - terrain.transform.position;
        Vector2 normalizedPos = new Vector2(terrainLocalPos.x / terrain.terrainData.size.x,
                                            terrainLocalPos.z / terrain.terrainData.size.y);
        return terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
    }

    void Update()
    {
        Vector3 normal = GetTerrainNormal();
        Vector3 direction = agent.steeringTarget - transform.position;
        direction.y = 0.0f;
        if (direction.magnitude > 0.1f ||  normal.magnitude > 0.1f) {
            Quaternion qLook = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion qNorm = Quaternion.FromToRotation(Vector3.up, normal);
            lookRotation = qNorm * qLook;
        }
        // soften the orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime / 0.2f);
    }
}
