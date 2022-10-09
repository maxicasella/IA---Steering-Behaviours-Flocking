using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{
    FiniteStateMachine _fsm;
    GameObject[] _wp;
    Npc _npc;
    public Patrol(FiniteStateMachine fsm, Npc npc)
    {
        _fsm = fsm;
        _npc = npc;
    }
    public void OnEnter()
    {
        _wp = _npc.waypoints;
        _npc.patrol = true;
    }
    public void OnUpdate()
    {
        _npc.boid = null;
        _npc.b = null;
        PatrolAction();

        if (_npc.energy <= 0)
        {
            _npc.patrol = false;
            _fsm.UpdateState(States.Rest);
        }

        _npc.Ischase();

        if (_npc.boid != null && _npc.energy >= 0f)
        {
            _npc.patrol = false;
            _fsm.UpdateState(States.Chase);
        }
    }
    public void OnExit()
    {
        
    }
    void PatrolAction()
    {
        GameObject wp = _wp[_npc.actualWp];
        Vector3 direction = wp.transform.position - _npc.transform.position;
        _npc.transform.forward = direction;
        _npc.transform.position += _npc.transform.forward * _npc.speed * Time.deltaTime;
        _npc.energy -= Time.deltaTime*2;

        if (direction.magnitude <= 0.1f)
        {
            _npc.actualWp++;
            if (_npc.actualWp > _wp.Length - 1) _npc.actualWp = 0;
        }
    }
}
