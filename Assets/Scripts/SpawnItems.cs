using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class SpawnItems : MonoBehaviour
{
    public List<GameObject> animals = new List<GameObject>();
    public GameObject spawnCenter;

    private int currentObjects;
    public float ranger = 10.0f;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        for (int i = 0; i < animals.Count; i++)
        {
            int instances = animals[i].GetComponent<EnemyController>().maxInstances;
            for (int j = 0; j < instances; j++)
            {
                SpawnOne(animals[i]);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, float.PositiveInfinity, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    Vector3 RandomPoint(Vector3 center, float range)
    {
        Vector3 result;
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, float.PositiveInfinity, NavMesh.AllAreas))
        {
            result = hit.position;
        }
        else
        {
            result = center;
        }
        return result;
    }

    public void MoveOne(GameObject obj)
    {
        Vector3 spawnPoint;
        RandomPoint(spawnCenter.transform.position, ranger, out spawnPoint);
        obj.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }

    public void SpawnOne(GameObject prefab)
    {
        Vector3 spawnPoint;
        RandomPoint(spawnCenter.transform.position, ranger, out spawnPoint);
        PhotonNetwork.InstantiateSceneObject(prefab.name, spawnPoint, Quaternion.identity, 0, null);
    }
}

 
