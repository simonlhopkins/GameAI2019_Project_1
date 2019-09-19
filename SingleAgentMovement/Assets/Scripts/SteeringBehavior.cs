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
        startTime = 0.0f;
        //wanderOrientation = agent.orientation;
    }

    public void SetTarget(NPCController newTarget)
    {
        target = newTarget;
    }

    public float Face(float currentOrientation, Vector3 velocity)
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

    public Vector3 Seek()
    {
        Vector3 velocity = target.position - transform.position;
        velocity.Normalize();
        velocity *= maxSpeed;
        return velocity;
    }

    public Vector3 Flee()
    {
        Vector3 velocity = transform.position - target.position;
        velocity.Normalize();
        velocity *= maxSpeed;
        return velocity;
    }
    /*
    public Vector3 PursueArrive()
    {

    }

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
    public Vector3 Wander(Vector3 current)
    {

        startTime += Time.deltaTime;
        if(startTime <= wanderRate)
        {
            return current;
        }
        startTime = 0;
        Vector3 center = -transform.forward;
        center.Normalize();
        center *= wanderOffset;
        //Debug.DrawRay(transform.position, center, Color.red);
        GameObject middle = new GameObject();
        Debug.Log("Middle:");
        middle.transform.position = new Vector3(transform.position.x + center.x, transform.position.y, transform.position.z + center.z);
        Debug.Log(middle.transform.position);
        GameObject follow = new GameObject();
        float angle = Random.Range(0.0f, 360.0f);
        follow.transform.position = new Vector3(middle.transform.position.x + wanderRadius*Mathf.Sin(angle*Mathf.Deg2Rad), middle.transform.position.y, middle.transform.position.z + wanderRadius * Mathf.Cos(angle * Mathf.Deg2Rad));
        //= Transform(transform.position.x + center.x,transform.position.y,transform.position.z + center.z);
        Vector3 velocity = transform.position - follow.transform.position;
        Debug.DrawRay(transform.position, velocity, Color.red);
        velocity.Normalize();
        velocity *= maxSpeed;
        Destroy(follow);
        Destroy(middle);
        return velocity;
    }
    

}
