using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMove : MonoBehaviour
{

    public int fleeDistance;
    public int roamDistance;
    public int aggressiveDistance;
    public int idleSpeed;
    public int chaseSpeed;

    public bool isAggressive;
    public bool staysAggressive;
    public NavMeshAgent agent;
    public DontMoveInvisible dontMoveInvisible;

    public Animator animator;
    public bool isRoamingIdle;

    private NavMeshPath path;
    private Vector3 randomPoint;
    private Vector3 destination;
    private bool isVisible;
    public GameObject playerObject; //fix this
    public bool isAttacking = false;
    public bool canAttack = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (isAttacking && canAttack)
        {
            StartCoroutine(AttackRate());
            collision.gameObject.GetComponent<DamagePlayer>().TakeDamage(gameObject.GetComponent<EnemyController>().damageToPlayerAmount);
            gameObject.GetComponent<EnemyController>().PlayBiteSound();
        }
    }

    IEnumerator AttackRate()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }



    private void OnCollisionStay(Collision collision)
    {
        if (isAttacking && canAttack)
        {
            StartCoroutine(AttackRate());
            collision.gameObject.GetComponent<DamagePlayer>().TakeDamage(gameObject.GetComponent<EnemyController>().damageToPlayerAmount);
            gameObject.GetComponent<EnemyController>().PlayBiteSound();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAggressive && other.gameObject.layer == 13)
        {
            playerObject = other.gameObject;
            AttackPlayer(playerObject);
        }
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isRoamingIdle = true;
        StartCoroutine(RoamingIdle());
        path = new NavMeshPath();
    }
    void Update()
    {
        if (isAttacking)
        {
            if (Vector3.Distance(agent.transform.position, playerObject.transform.position) < agent.stoppingDistance + 2)
            {
                agent.updateRotation = false;
                RotateTowards(playerObject.transform);
            }
            else
            {
                agent.updateRotation = true;
            }
        }
        else
        {
            agent.updateRotation = true;
        }

        animator.SetFloat("Blend", agent.velocity.magnitude);
        if(agent.velocity.magnitude < 0.1f)
        {
            animator.SetFloat("AnimSpeed", .5f);
        }
        else
        {
            animator.SetFloat("AnimSpeed", agent.velocity.magnitude);
        }
        if (isRoamingIdle)
        {
            return;
        }

        
    }

    
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    bool RandomPoint(Vector3 currentPosition, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            randomPoint = currentPosition + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    bool RandomPoint(Vector3 currentPosition, Vector3 enemyPosition, float range, out Vector3 result)
    {

        Vector3 delta = currentPosition - enemyPosition;
        for (int i = 0; i < 30; i++)
        {
            randomPoint = currentPosition + new Vector3(Random.value * delta.normalized.x, Random.value * delta.normalized.y, Random.value * delta.normalized.z) * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        for (int i = 0; i < 20; i++)
        {
            randomPoint = currentPosition + new Vector3(Random.value * -delta.normalized.x, Random.value * -delta.normalized.y, Random.value * -delta.normalized.z) * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        if (RandomPoint(currentPosition, range, out result))
        {
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    public void Flee(GameObject player)
    {
        if(gameObject.name == "Chicken King")
        {
            isAttacking = true;
        }
        isRoamingIdle = false;
        playerObject = player;
        if (isAggressive)
        {
            AttackPlayer(player);
            return;
        }
        if (dontMoveInvisible.isVisible)
        {
            if (agent.remainingDistance < agent.stoppingDistance + 0.1f)
            {
                if (RandomPoint(transform.position, player.transform.position, fleeDistance, out destination))
                {
                    Move(destination);
                }
            }
        }
    }

    public void AttackPlayer(GameObject player)
    {
        StartCoroutine(ChasePlayer(player));    
    }

    public void Move(Vector3 dest)
    {
        try
        {

            agent.SetDestination(dest);
        }
        catch
        {

        }
    }


    IEnumerator ChasePlayer(GameObject player)
    {
        float refrestTargetRate = .1f;
        Rigidbody playerRigidBody = player.GetComponent<Rigidbody>();
        isAttacking = true;
        agent.speed = chaseSpeed;
        agent.acceleration = chaseSpeed * 2f;
     //   agent.autoBraking = false;
        float speedOffset = agent.speed / 2.5f;
        while (isAttacking && aggressiveDistance > Vector3.Distance(gameObject.transform.position, player.transform.position))
        {
        //    Vector3 playerVelocity = playerRigidBody.velocity;
            float agentSpeed = this.agent.speed + speedOffset;
            Vector3 firstPlayerPosition = player.transform.position;
            yield return new WaitForSeconds(0.05f);
            Vector3 secondPlayerPosition = player.transform.position;
            Vector3 playerDirection = secondPlayerPosition - firstPlayerPosition;
            Vector3 playerVelocity = (playerDirection / 0.05f);

            //      Debug.Log("First Player Position: " + firstPlayerPosition);

            //    Vector3 interceptVector = FindInterceptVector(agent.transform.position, agentSpeed, firstPlayerPosition, playerVelocity);
            //     Debug.Log("Intercept Position: " + interceptVector);
            // Find the time of collision (distance / relative velocity)          
            //     Vector3 futurePosition = player.transform.position + (futureDistance);

            //   Vector3 agentPos = agent.transform.position;
            //   float distanceToTargetFuturePosition = Vector3.Distance(agentPos, futurePosition);

            //   Vector3 interceptLocation = (((nextPlayerPosition + (directionOfTravel * playerSpeed) - agent.transform.position) / agentSpeed) + agent.transform.position);

            // === variables you need ===
            //how fast our shots move
            //objects

            // === derived variables ===
            //positions
            Vector3 shooterPosition = agent.transform.position;
            Vector3 targetPosition = player.transform.position;
            //velocities
            Vector3 shooterVelocity = agent.GetComponent<Rigidbody>() ? agent.GetComponent<Rigidbody>().velocity : Vector3.zero;
            Vector3 targetVelocity = playerRigidBody ? playerRigidBody.velocity : Vector3.zero;

            //calculate intercept
            Vector3 interceptPoint = FirstOrderIntercept
            (
                shooterPosition,
                shooterVelocity,
                agentSpeed,
                targetPosition,
                playerVelocity
            );
            if(Vector3.Distance(interceptPoint, targetPosition) > 20)
            {
                interceptPoint = targetPosition;
            }
            //now use whatever method to launch the projectile at the intercept point
            Move(interceptPoint);

            yield return new WaitForSeconds(refrestTargetRate);


        }
        SetRoamingIdle();
    }
    public void SetRoamingIdle()
    {
        StopAllCoroutines();
        isRoamingIdle = true;
        isAttacking = false;
        StartCoroutine(RoamingIdle());
    }

    IEnumerator RoamingIdle()
    {
        agent.speed = idleSpeed;
        agent.autoBraking = true;
        while (isRoamingIdle)
        {
            if (dontMoveInvisible.isVisible && agent.remainingDistance < agent.stoppingDistance + 0.1f)
            {
              //  yield return new WaitForSeconds(1f));
                if (RandomPoint(transform.position, roamDistance, out destination))
                {
                  //  agent.CalculatePath(destination, path);
                    //  agent.SetPath(path);
                    Move(destination);
                }
            }
            yield return new WaitForSeconds(Random.Range(1, 30));
        }
    }

    private Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed,
    Vector3 targetOrigin, Vector3 targetVel)
    {


        Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);

        // Decompose the target's velocity into the part parallel to the
        // direction to the cannon and the part tangential to it.
        // The part towards the cannon is found by projecting the target's
        // velocity on dirToTarget using a dot product.
        Vector3 targetVelOrth =
        Vector3.Dot(targetVel, dirToTarget) * dirToTarget;

        // The tangential part is then found by subtracting the
        // result from the target velocity.
        Vector3 targetVelTang = targetVel - targetVelOrth;

        /*
        * targetVelOrth
        * |
        * |
        *
        * ^...7  <-targetVel
        * |  /.
        * | / .
        * |/ .
        * t--->  <-targetVelTang
        *
        *
        * s--->  <-shotVelTang
        *
        */

        // The tangential component of the velocities should be the same
        // (or there is no chance to hit)
        // THIS IS THE MAIN INSIGHT!
        Vector3 shotVelTang = targetVelTang;

        // Now all we have to find is the orthogonal velocity of the shot

        float shotVelSpeed = shotVelTang.magnitude;
        if (shotVelSpeed > shotSpeed)
        {
            // Shot is too slow to intercept target, it will never catch up.
            // Do our best by aiming in the direction of the targets velocity.
            return targetVel.normalized * shotSpeed;
        }
        else
        {
            // We know the shot speed, and the tangential velocity.
            // Using pythagoras we can find the orthogonal velocity.
            float shotSpeedOrth =
            Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;

            float timeToCollision = ((shotOrigin - targetOrigin).magnitude - 2)
                    / (shotVelOrth.magnitude - targetVel.magnitude);

            // Calculate where the shot will be at the time of collision
            Vector3 shotVel = shotVelOrth + shotVelTang;
            Vector3 shotCollisionPoint = targetOrigin + targetVel * timeToCollision;
            // Finally, add the tangential and orthogonal velocities.
            //   return shotVelOrth + shotVelTang;

            return shotCollisionPoint;
        }
    }


    //first-order intercept using absolute target position
    public static Vector3 FirstOrderIntercept
    (
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }


}