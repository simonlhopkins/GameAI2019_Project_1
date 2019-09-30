﻿using UnityEngine;
using System.Collections;

public class MillingtonImplementation : MonoBehaviour
{
    // Use this for initialization
    Kinematic playerK;
    Kinematic enemyK;
    SteeringOutput playerSO;
    SteeringOutput enemySO;
    public GameObject player;
    public GameObject target;
    public float maxAcceleration;
    public float maxSpeed;
    public float targetRadius;
    public float slowRadius;


    public float maxAngularAcceleration;
    public float maxRotation;


    public float maxPrediction;


    public float wanderOffset;
    public float wanderRadius;

    public float wanderRate;
    private float wanderOrientation = 0f;

    void Start()
    {
        playerK = new Kinematic();
        enemyK = new Kinematic();

        playerK.maxSpeed = maxSpeed;
        enemyK.maxSpeed = maxSpeed;

        playerK.position = player.GetComponent<Rigidbody>().position;
        playerK.velocity = Vector3.zero;
        playerK.orientation = Mathf.Deg2Rad * player.GetComponent<Rigidbody>().rotation.eulerAngles.y;

        enemyK.position = target.GetComponent<Rigidbody>().position;
        enemyK.velocity = Vector3.zero;
        enemyK.orientation = Mathf.Deg2Rad * target.GetComponent<Rigidbody>().rotation.eulerAngles.y;

        playerSO = new SteeringOutput();
        enemySO = new SteeringOutput();

        Debug.Log(playerK.orientation +", "+ enemyK.orientation);

    }

    // Update is called once per frame
    SteeringOutput empty;

    void Update()
    {
        empty.linear = Vector3.zero;
        empty.angular = 0;
        playerK.position = player.gameObject.GetComponent<Rigidbody>().position;
        enemyK.position = target.gameObject.GetComponent<Rigidbody>().position;




        //playerK.rotation = Mathf.Atan2(-playerK.velocity.x, playerK.velocity.z);
        //enemyK.rotation = Mathf.Atan2(-enemyK.velocity.x, enemyK.velocity.z);

        if (Input.GetKeyDown(KeyCode.F)) { 
            playerSO = new DynamicSeek(playerK, enemyK, maxAcceleration).getSteering();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {

            enemySO = empty;
        }




        playerK.Update(playerSO, maxSpeed, Time.deltaTime);
        enemyK.Update(enemySO, maxSpeed, Time.deltaTime);





        Debug.DrawLine(playerK.position, enemyK.position, Color.cyan);
        Debug.DrawRay(playerK.position, playerSO.linear * 5f, Color.red);
        Debug.DrawRay(enemyK.position, enemySO.linear * 5f, Color.red);

        //update player
        player.gameObject.GetComponent<Rigidbody>().position = playerK.position;
        player.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.up * (Mathf.Rad2Deg * -playerK.orientation));

        //update target
        target.gameObject.GetComponent<Rigidbody>().position = enemyK.position;
        target.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.up * (Mathf.Rad2Deg* -enemyK.orientation));

    }
}

public struct Kinematic
{
    public Vector3 position;
    public float orientation;
    public Vector3 velocity;
    public float rotation; //angular velocity
    public float maxSpeed;
    public void Update(SteeringOutput steering, float _maxSpeed, float time) {

        position += velocity * time;
        //orientation + angular velocity
        orientation += rotation * time;
        //
        velocity += steering.linear * time;
        orientation += steering.angular * time;

        if(velocity.magnitude > _maxSpeed)
        {
            Debug.Log("velocity.magnitude > maxSpeed");
            Debug.Log(_maxSpeed);
            velocity.Normalize();
            velocity *= _maxSpeed;
        }

        Debug.Log(position + " + " + velocity);

    }
}

public struct SteeringOutput {

    public Vector3 linear;
    public float angular;

}

public class DynamicSeek{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;

    public DynamicSeek(Kinematic _character, Kinematic _target, float _maxAcceleration) {
        character = _character;
        target = _target;
        maxAcceleration = _maxAcceleration;
    }
    public SteeringOutput getSteering() {
        SteeringOutput steering = new SteeringOutput();
        steering.linear = target.position - character.position;
        steering.linear.Normalize();
        steering.linear *= maxAcceleration;
        steering.angular = 0;
        return steering;
    }
}

public class DynamicFlee
{
    public Kinematic character;
    public Kinematic target;
    public float maxAcceleration;

    public DynamicFlee(Kinematic _character, Kinematic _target, float _maxAcceleration)
    {
        character = _character;
        target = _target;
        maxAcceleration = _maxAcceleration;
    }
    public SteeringOutput getSteering()
    {
        SteeringOutput steering = new SteeringOutput();
        steering.linear = character.position - target.position;
        steering.linear.Normalize();
        steering.linear *= maxAcceleration;
        steering.angular = 0;
        return steering;
    }
}

public class Arrive {
    Kinematic character;
    Kinematic target;
    float maxAcceleration;
    float maxSpeed;
    float targetRadius;
    float slowRadius;
    float timeToTarget = 0.1f;
    public Arrive(Kinematic _character, Kinematic _target, float _maxAcceleration, float _maxSpeed,
            float _targetRadius, float _slowRadius)
    {
        character = _character;
        target = _target;
        maxAcceleration = _maxAcceleration;
        maxSpeed = _maxSpeed;
        targetRadius = _targetRadius;
        slowRadius = _slowRadius;
    }

    public SteeringOutput getSteering() {
        SteeringOutput steering = new SteeringOutput();
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        if (distance < targetRadius)
        {
            Debug.Log("INSIDE!!!");
            steering.linear = Vector3.zero;
            steering.angular = 0;
            return steering;
        }

        float targetSpeed;
        if(distance > slowRadius) {
            targetSpeed = maxSpeed;
        }
        else {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steering.linear = targetVelocity - character.velocity;
        steering.linear /= timeToTarget;

        if(steering.linear.magnitude > maxAcceleration) {
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }

        steering.angular = 0;

        return steering;
    }
}


public class DynamicAlign {

    public Kinematic character;
    public Kinematic target;
    public float maxAngularAcceleration;
    public float maxRotation;
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public DynamicAlign(Kinematic _character, Kinematic _target, float _maxAngularAcceleration, float _maxRotation,
            float _targetRadius, float _slowRadius)
    {
        character = _character;
        target = _target;
        maxAngularAcceleration = _maxAngularAcceleration;
        maxRotation = _maxRotation;
        targetRadius = _targetRadius;
        slowRadius = _slowRadius;
    }
    public SteeringOutput getSteering() {
        SteeringOutput steering = new SteeringOutput();

        float rotation = target.orientation - character.orientation;
        rotation = Mathf.Clamp(rotation, -Mathf.PI, Mathf.PI);
        Debug.Log("rotation:" + rotation);
        float rotationSize = Mathf.Abs(rotation);



        if (rotationSize < targetRadius) {
            steering.linear = Vector3.zero;
            steering.angular = 0;
            Debug.Log("RETURN");
            return steering;
        }

        float targetRotation;
        if (rotationSize > slowRadius) {
            targetRotation = maxRotation;
        }
        else {
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        if(rotationSize < 0.01f) {
            targetRotation *= 0f;
        }
        else
        {
            targetRotation *= rotation / rotationSize;
        }



        steering.angular = targetRotation - character.rotation;
        steering.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(steering.angular);
        if(angularAcceleration > maxAngularAcceleration) {
            steering.angular /= angularAcceleration;
            steering.angular *= maxAngularAcceleration;
        }

        steering.linear = Vector3.zero;

        Debug.Log(steering.angular);
        return steering;
    }

}

class DynamicPursue {
    Kinematic character;
    Kinematic target;
    float maxAcceleration;
    float maxPrediction;
    DynamicSeek ds;
    public DynamicPursue(Kinematic _character, Kinematic _target, float _maxAcceleration, float _maxPrediction) {
        character = _character;
        target = _target;
        maxAcceleration = _maxAcceleration;
        maxPrediction = _maxPrediction;

        ds = new DynamicSeek(_character, _target, _maxAcceleration);
    }

    public SteeringOutput getSteering() {

        Vector3 explicitTarget = target.position;
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;
        float speed = character.velocity.magnitude;
        float prediction;
        if (speed <= (distance / maxPrediction)) {
            prediction = maxPrediction;
        }
        else {
            prediction = distance / speed;
        }


        ds.target.position += target.velocity * prediction;

        return ds.getSteering();

    }
}

class DynamicEvade
{
    Kinematic character;
    Kinematic target;
    float maxAcceleration;
    float maxPrediction;
    DynamicFlee df;
    public DynamicEvade(Kinematic _character, Kinematic _target, float _maxAcceleration, float _maxPrediction)
    {
        character = _character;
        target = _target;
        maxAcceleration = _maxAcceleration;
        maxPrediction = _maxPrediction;

        df = new DynamicFlee(_character, _target, _maxAcceleration);
    }

    public SteeringOutput getSteering()
    {

        Vector3 explicitTarget = target.position;
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;
        float speed = character.velocity.magnitude;
        float prediction;
        if (speed <= (distance / maxPrediction))
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }


        df.target.position += target.velocity * prediction;

        return df.getSteering();

    }
}

class DynamicFace {
    public Kinematic target;
    public DynamicAlign a;
    public DynamicFace(Kinematic _target, DynamicAlign _a) {
        target = _target;
        a = _a;
    }

    public SteeringOutput getSteering() {
        Vector3 direction = target.position - a.character.position;
        if(direction.magnitude <= 0.001f) {
            SteeringOutput zero;
            zero.linear = Vector3.zero;
            zero.angular = 0;
            return zero;
        }
        a.target = target;
        a.target.orientation = Mathf.Atan2(-direction.x, direction.z);


        return a.getSteering();

    }

}


class DynamicWander {
    float wanderOffset;
    float wanderRadius;

    float wanderRate;
    float wanderOrientation = 0;

    float maxAcceleration;

    DynamicFace f;

    public DynamicWander(float _wanderOffset, float _wanderRadius, float _wanderRate,
                                 float _maxAcceleration, DynamicFace _f)
    {
        wanderOffset = _wanderOffset;
        wanderRadius = _wanderRadius;
        wanderRate = _wanderRate;
        maxAcceleration = _maxAcceleration;
        f = _f;
        wanderOrientation = f.a.character.orientation;
    }
    private float randomBinomial() {
        return Random.value - Random.value;
    }

    private Vector3 asVector(float _orientation) {
        return new Vector3(Mathf.Sin(_orientation), 0f, Mathf.Cos(_orientation));
    }

    public SteeringOutput getSteering() {
        wanderOrientation += randomBinomial() * wanderRate;
        //using face, which is using align which has the characyer;
        float targetOrientation = wanderOrientation + f.a.character.orientation;

        Vector3 target = f.a.character.position + wanderOffset * asVector(f.a.character.orientation);

        target += wanderRadius * asVector(targetOrientation);

        SteeringOutput steering = f.getSteering();

        steering.linear = maxAcceleration * asVector(f.a.character.orientation);

        return steering;
    }


}

