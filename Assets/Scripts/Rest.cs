using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : IState
{
    FiniteStateMachine _fsm;
    Npc _npc;
    public Rest(FiniteStateMachine fsm, Npc npc)
    {
        _fsm = fsm;
        _npc = npc;
    }
    public void OnEnter()
    {
        _npc.rest = true;
        _npc.ColorNPC(Color.green);
    }
    public void OnUpdate()
    {
        _npc.b = null;
        _npc.boid = null;
        if (_npc.rest)
        {
            _npc.energy += Time.deltaTime * 10;
        }

        if (_npc.energy >= 100)
        {
            _npc.rest = false;
            _fsm.UpdateState(States.Patrol);
        }
    }
    public void OnExit()
    {
        _npc.ColorNPC(Color.blue);
    }
}
