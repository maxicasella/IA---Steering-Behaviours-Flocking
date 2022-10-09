using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Rest,
    Patrol,
    Chase
}

public class Npc : MonoBehaviour
{
    FiniteStateMachine _fsm;

    public int actualWp = 0;
    public GameObject[] waypoints;
    public float speed;
    public float maxSpeed;
    public float energy;
    public float visionRange;
    public bool rest;
    public bool chase = false;
    public bool patrol = false;
    public float maxForce;
    public Transform boid;
    public Boids b;
    public float pursuitWeight;
    public Vector3 velocity = Vector3.zero;
    public float pursuitRadius;

    private void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(States.Rest, new Rest(_fsm, this));
        _fsm.AddState(States.Patrol, new Patrol(_fsm, this));
        _fsm.AddState(States.Chase, new Chase(_fsm, this, boid, b));
        _fsm.UpdateState(States.Patrol);
    }

    private void Update()
    {
        _fsm.Update();

        if (energy> 100)
        {
            energy = 100f;
        }
                    
    }
    private void OnDrawGizmos()
    {
        //Rango de Vision
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    public void ColorNPC(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    public void AddForce(Vector3 force)
    {
        velocity = Vector3.ClampMagnitude(velocity + force, maxSpeed);
    }

    public void Ischase()
    {
        foreach (var item in GameManager.instance.boids)
        {
            Vector3 distance = item.transform.position - transform.position;
            if (distance.magnitude <= visionRange)
            {
                boid = item.transform;
                b = item;
            }
        }
    }
}

