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

    // Holds the path to follow
    public GameObject[] Path;
    public int current = 0;

    protected void Start() {
        agent = GetComponent<NPCController>();
        //wanderOrientation = agent.orientation;
    }

    public void SetTarget(NPCController newTarget)
    {
        target = newTarget;
    }

    public float Face(float currentOrientation, Vector3 velocity)
    {

        Debug.DrawRay(transform.position, velocity*5f, Color.red);
        if (velocity.magnitude > 0)
        {
            Debug.Log(Mathf.Rad2Deg * Mathf.Atan2(velocity.x, velocity.z));
            return Mathf.Rad2Deg*Mathf.Atan2(velocity.x, velocity.z);

        }
        else
        {
            Debug.Log("fail");
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

    public Vector3 PursueArrive()
    {
        return Vector3.zero;
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

    public Vector3 Wander()
    {

    }
    */
    

}
