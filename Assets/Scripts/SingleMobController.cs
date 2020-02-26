using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMobController : MonoBehaviour
{

    public enum State
    {
        Idle,
        Roaming,
        Aggressive,
        Passive
    }

    public State state;
    public AgentMove2 party;
    public Transform enemy;
    public float roamingRadius;
    public float aggressiveRadius;
    public bool debug;
    public Vector3 originalPosition;

    void Awake()
    {
        originalPosition = new Vector3(party.transform.position.x, party.transform.position.y, party.transform.position.z);
    }

    void Update()
    {
        if (state == State.Idle)
        {
            return;
        }

        var dist = Vector3.Distance(transform.position, enemy.position);
        if (dist < aggressiveRadius && state != State.Passive)
        {
            state = State.Aggressive;
        }

        switch (state)
        {
            case State.Idle:
                return;
            case State.Roaming:
                Roaming();
                break;
            case State.Aggressive:
                FindEnemy();
                break;
            case State.Passive:
                Flee();
                break;
        }
    }

    void Roaming()
    {
        if (party.agent.remainingDistance > 0.1f)
        {
            return;
        }

        var randomPos = Random.insideUnitCircle * roamingRadius;
        Vector3 target = new Vector3(randomPos.x + originalPosition.x, originalPosition.y, randomPos.y + originalPosition.z);
        party.SetPath(target);

    }

    void FindEnemy()
    {
        var dist = Vector3.Distance(transform.position, enemy.position);
        if (aggressiveRadius < dist)
        {
            state = State.Roaming;
            return;
        }
        party.SetPath(enemy.position);
    }

    void Flee()
    {
        var dist = Vector3.Distance(transform.position, enemy.position);
        if (aggressiveRadius < dist)
        {
            state = State.Roaming;
            return;
        }
        var delta = originalPosition - enemy.position;
        party.SetPath(originalPosition + delta);
        if (debug)
        {
            Debug.DrawLine(originalPosition, originalPosition + delta, Color.yellow, 0.1f, false);
        }
    }
}