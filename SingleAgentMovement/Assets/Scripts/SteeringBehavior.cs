using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the place to put all of the various steering behavior methods we're going
/// to be using. Probably best to put them all here, not in NPCController.
/// </summary>

public class SteeringBehavior : MonoBehaviour {

    // The agent at hand here, and whatever target it is dealing with
    public NPCController agent;
    public NPCController target;

    // Below are a bunch of variable declarations that will be used for the next few
    // assignments. Only a few of them are needed for the first assignment.

    // For pursue and evade functions
    public float maxPrediction;
    public float maxAcceleration;

    // For arrive function
    public float maxSpeed = 1.0f;
    public float targetRadiusL;
    public float slowRadiusL;
    public float timeToTarget;

    // For Face function
    public float maxRotation;
    public float maxAngularAcceleration;
    public float targetRadiusA;
    public float slowRadiusA;

    // For wander function
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation;
    public float startTime;

    // Holds the path to follow
    public GameObject[] Path;
    public int current = 0;

    protected void Start() {
        agent = GetComponent<NPCController>();

        //test for debug
        slowRadiusL = 10f;
        timeToTarget = 3f;
        //wanderOrientation = agent.orientation;
    }

    public void SetTarget(NPCController newTarget)
    {
        target = newTarget;
    }

    public float Orient(float currentOrientation, Vector3 velocity)
    {

        //Debug.DrawRay(transform.position, velocity*5f, Color.red);
        if (velocity.magnitude > 0)
        {
            //Debug.Log(Mathf.Rad2Deg * Mathf.Atan2(velocity.x, velocity.z));
            return Mathf.Rad2Deg*Mathf.Atan2(velocity.x, velocity.z);

        }
        else
        {
            //Debug.Log("fail");
            return currentOrientation;
        }
    }

    public SteeringData Seek()
    {

        float acceleration = 0.5f;
        Vector3 currentVel = agent.velocity;
        Vector3 desiredVel = Vector3.Normalize(target.position - agent.position) * maxSpeed;
        
        Vector3 steeringVel = desiredVel - currentVel;
        Vector3 returnVelocity = (currentVel + steeringVel).normalized * maxSpeed;
        agent.rotation = align(target);

        gameObject.GetComponent<NPCController>().DrawLine(agent.transform.position, target.position);
        //Debug.Log(target.position);
        Debug.DrawRay(agent.position, returnVelocity, Color.magenta);
        return new SteeringData(returnVelocity, acceleration);
    }


    public SteeringData Flee()
    {
        float acceleration = 0.5f;
        Vector3 currentVel = agent.velocity;
        Vector3 desiredVel = Vector3.Normalize(target.position - agent.position) * maxSpeed;

        Vector3 steeringVel = desiredVel - currentVel;
        Vector3 returnVelocity = (currentVel + steeringVel).normalized * maxSpeed;
        agent.rotation = 180 + face();

        gameObject.GetComponent<NPCController>().DrawLine(agent.transform.position, target.position);

        return new SteeringData(-returnVelocity, acceleration);
    }

    public SteeringData PursueArrive()
    {
        //target
        //float targetAngularAcceleration = target.a
        //anicipate 3 frames ahead
        //refactor with global vars
        Vector3 returnVelocity;
        float acceleration = 0.5f;
        //Vector3 anticipatedTargetPos = target.position + (target.velocity * 1f);
        Vector3 desiredVel = target.position - agent.position;
        // Should draw the circle around the target where we slow down
        gameObject.GetComponent<NPCController>().DrawCircle(target.position, slowRadiusL);
        float distanceToTarget = (desiredVel).magnitude;



        //Debug.Log(distanceToTarget);
        Vector3 currentVel = agent.velocity;

        if (distanceToTarget < slowRadiusL){
            // Inside the slowing area
            Debug.Log("INSIDE RADIUS: "+ (agent.velocity - desiredVel)/Time.deltaTime);
            float targetSpeed = maxSpeed * (distanceToTarget / slowRadiusL);
            //(distanceToTarget / slowRadiusL) percent of the way you are there
            //
            desiredVel = desiredVel.normalized;
            desiredVel *= targetSpeed;

            returnVelocity = desiredVel - agent.velocity;
            returnVelocity /= timeToTarget;
            returnVelocity *= 10f;
            acceleration = 1f;
            return new SteeringData(returnVelocity, acceleration);

        }
        else{
            // Outside the slowing area.

            desiredVel = Vector3.Normalize(desiredVel) * maxSpeed;
        }

        // Set the steering based on this
        //steering
        Vector3 steeringVel = desiredVel - currentVel;
        gameObject.GetComponent<NPCController>().DrawCircle(steeringVel, targetRadiusL);

        returnVelocity = currentVel + steeringVel;


        agent.rotation = face();
        //could return an object
        return new SteeringData(returnVelocity, acceleration); ;
    }
    /*
    public Vector3 Evade()
    {
        
    }

    public Vector3 Align()
    {

    }

    public Vector3 Face()
    {

    }
    */
    public SteeringData Wander(Vector3 current)
    {

        float acceleration = 0.1f;
        startTime += Time.deltaTime;
        if(startTime <= wanderRate)
        {
            return new SteeringData(current, acceleration);
        }
        startTime = 0;
        Vector3 center = transform.forward;
        center.Normalize();
        center *= wanderOffset;
        //Debug.DrawRay(transform.position, center, Color.red);
        GameObject middle = new GameObject();
        //Debug.Log("Middle:");
        middle.transform.position = new Vector3(transform.position.x + center.x, transform.position.y, transform.position.z + center.z);
        gameObject.GetComponent<NPCController>().DrawCircle(middle.transform.position, wanderRadius);
        //Debug.Log(middle.transform.position);
        GameObject follow = new GameObject();
        float angle = Random.Range(0.0f, 360.0f);
        follow.transform.position = new Vector3(middle.transform.position.x + wanderRadius*Mathf.Sin(angle*Mathf.Deg2Rad), middle.transform.position.y, middle.transform.position.z + wanderRadius * Mathf.Cos(angle * Mathf.Deg2Rad));
        //= Transform(transform.position.x + center.x,transform.position.y,transform.position.z + center.z);
        Vector3 velocity = follow.transform.position - transform.position;
        //gameObject.GetComponent<LineRenderer>().
        Debug.DrawRay(transform.position, velocity, Color.red);
        velocity.Normalize();
        velocity *= maxSpeed;
        //gameObject.GetComponent<NPCController>().DrawLine(transform.position, follow.transform.position);
        Destroy(follow);
        Destroy(middle);
        return new SteeringData(velocity, acceleration);
    }

    public float face() {


        Vector3 direction = target.position - agent.position;


        return Mathf.Atan2(-direction.x, direction.z) * Mathf.Rad2Deg;
    }

    public float align(NPCController target) {

        return target.rotation;
    }



}

public class SteeringData
{
    public Vector3 velocity;
    public float acceleration;
    public SteeringData(Vector3 _velocity, float _acceleration) {
        velocity = _velocity;
        acceleration = _acceleration;
    }

}

