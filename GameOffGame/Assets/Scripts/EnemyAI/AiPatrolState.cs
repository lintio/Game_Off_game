using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolState : AiState
{
    const string LEFT = "left";
    const string RIGHT = "right";

    float timer = 0;
    bool checkingTime;
    bool timerDone;

    float sightMult = 1f;

    public AiStateId GetId() {
        return AiStateId.Patrol;
    }
    public void Enter(AiAgent agent) {
        if (agent.config.isHunting)
        {
            timer = 0;
            sightMult = 1.5f;
            checkingTime = true;
            Debug.Log("Enemy State 'Hunting'");
        }
        else
        {
            Debug.Log("Enemy State 'Patrolling'");
        }
    }
    public void Exit(AiAgent agent) {
    }
    public void Update(AiAgent agent) {
        
        if (checkingTime)
        {
            timer += Time.deltaTime;
            if (timer >= agent.config.huntTime)
            {
                timerDone = true;
                checkingTime = false;
                timer = 0;
            }
        }

        if (timerDone)
        {
            agent.config.isHunting = false;
            timer = 0;
            timerDone = false;
            checkingTime = false;
            agent.stateMachine.ChangeState(agent.config.lastState);
        }
    }

    public void FixedUpdate(AiAgent agent) {
        float vx = agent.config.moveSpeed;
        if (agent.config.facingDirection == "left")
            vx = -agent.config.moveSpeed;

        agent.rb2d.velocity = new Vector2(vx, agent.rb2d.velocity.y);

        if (agent.config.CanSeePlayer(agent, sightMult))
        {
            if (!agent.config.isHunting)
            {
                agent.config.lastState = agent.stateMachine.currentState;
            }
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

        if (agent.config.IsHittingWall(agent) || agent.config.IsNearEdge(agent, sightMult))
        {
            if (agent.config.facingDirection == LEFT)
                agent.config.ChangeFacingDirection(RIGHT, agent);
            else
                agent.config.ChangeFacingDirection(LEFT, agent);
        }
    }
}
