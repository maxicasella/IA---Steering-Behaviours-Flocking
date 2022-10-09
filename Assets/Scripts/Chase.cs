using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    FiniteStateMachine _fsm;
    Npc _npc;
    Transform _boid;
    Boids _b;
    public Chase(FiniteStateMachine fsm, Npc npc, Transform boid, Boids b)
    {
        _fsm = fsm;
        _npc = npc;
        _boid = boid;
        _b = b;

    }
    public void OnEnter()
    {
        _npc.chase = true;
        _npc.ColorNPC(Color.red);
    }
    public void OnUpdate()
    {
        _npc.AddForce(Pursuit()*_npc.pursuitWeight);

        if (_npc.energy <= 0)
        {
            _npc.chase = false;
            _fsm.UpdateState(States.Rest);
            
        }

        //if (_npc.chase == false) _fsm.UpdateState(States.Patrol);
    }
    public void OnExit()
    {
        _npc.boid = null;
        _npc.b = null;
        _npc.ColorNPC(Color.blue);
    }

    Vector3 Pursuit()
    {
        Vector3 futurePos = _npc.boid.position +_npc.b.Velocity() - _npc.transform.position;
        Vector3 desired = (futurePos - _npc.transform.position);
        desired.Normalize();
        desired *= _npc.speed;
        _npc.energy -= Time.deltaTime * 4;

        Vector3 steering = desired - _npc.velocity;
        steering = Vector3.ClampMagnitude(steering, _npc.maxForce);

        _npc.transform.position += _npc.velocity * Time.deltaTime;
        _npc.transform.forward = _npc.velocity;

        return steering;
    }

}
