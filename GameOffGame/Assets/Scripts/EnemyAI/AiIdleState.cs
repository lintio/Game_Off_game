using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    const string LEFT = "left";
    const string RIGHT = "right";
    float timer = 0;
    float timeToWait = 5f;
    bool checkingTime;
    bool timerDone;

    float sightMult = 1f;

    public AiStateId GetId() {
        return AiStateId.Idle;
    }
    public void Enter(AiAgent agent)
    {
        Debug.Log("Enemy State 'Idle'");
        checkingTime = true;
    }

    public void Exit(AiAgent agent) {
        
    }

    public void Update(AiAgent agent) {
        if (checkingTime && !agent.config.CanSeePlayer(agent, sightMult))
        {
            timer += Time.deltaTime;
            if (timer >= timeToWait)
            {
                timerDone = true;
                checkingTime = false;
                timer = 0;
            }
        }
        else
        {
            agent.config.lastState = agent.stateMachine.currentState;
            timer = 0;
            timerDone = false;
            checkingTime = true;
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

        if (timerDone)
        {
            if (agent.config.facingDirection == LEFT)
                agent.config.ChangeFacingDirection(RIGHT, agent);
            else
                agent.config.ChangeFacingDirection(LEFT, agent);
            checkingTime = true;
            timerDone = false;
        }
    }
    public void FixedUpdate(AiAgent agent) {
    }

}
