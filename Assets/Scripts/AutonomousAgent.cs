using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] Perception flockPerception;
    [SerializeField] Steering steering;
    [SerializeField] AutonomousAgentData agentData;

    public float maxSpeed { get { return agentData.maxSpeed; } }
    public float maxForce { get { return agentData.maxForce; } }


    public Vector3 velocity { get; set; } = Vector3.zero;

    void Update()
    {
        Vector3 acceleration = Vector3.zero;
        


        GameObject[] gameObjects = perception.GetGameObjects();
        if (gameObjects.Length == 0)
        {
            //acceleration += steering.Wander(this);
        }
        //seek / flee
        if(gameObjects.Length != 0)
        {
            Debug.DrawLine(transform.position, gameObjects[0].transform.position);

            acceleration += steering.Flee(this, gameObjects[0]) * agentData.fleeWeight;
            acceleration += steering.Seek(this, gameObjects[0]) * agentData.seekWeight;
            
        }
        //flocking
        gameObjects = flockPerception.GetGameObjects();
        if(gameObjects.Length != 0)
        {
            acceleration += steering.Cohesion(this, gameObjects) * agentData.cohesionWeight;
        }

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;

        if(velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
