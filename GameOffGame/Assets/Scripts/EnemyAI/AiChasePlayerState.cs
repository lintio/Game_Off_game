using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    const string LEFT = "left";
    const string RIGHT = "right";

    float timer = 0;
    float timeToWait = 5f;
    bool checkingTime;
    bool timerDone;

    float chaseMult = 1.5f;
    public AiStateId GetId() {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AiAgent agent) {
        Debug.Log("Enemy State 'Chasing Player'");
    }

    public void Exit(AiAgent agent) {
    }

    public void Update(AiAgent agent) {
        if (!agent.config.CanSeePlayer(agent, chaseMult))
            checkingTime = true;
        else
            checkingTime = false;

        if (checkingTime && !agent.config.CanSeePlayer(agent , chaseMult))
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
            timer = 0;
        }

        if (timerDone)
        {
            agent.config.isHunting = true;
            agent.stateMachine.ChangeState(AiStateId.Patrol);
        }


    }
    public void FixedUpdate(AiAgent agent) {
        float vx = agent.config.moveSpeed + agent.config.chaseSpeed;
        if (agent.config.facingDirection == "left")
            vx = -(agent.config.moveSpeed + agent.config.chaseSpeed);
        if (agent.config.CanSeePlayer(agent, chaseMult))
        {
            agent.rb2d.velocity = new Vector2(vx, agent.rb2d.velocity.y);
        }
        else
        {
            agent.rb2d.velocity = new Vector2(0, agent.rb2d.velocity.y);
        }

        if (agent.config.IsHittingWall(agent) || agent.config.IsNearEdge(agent, 2.5f))
        {
            agent.rb2d.velocity = new Vector2(0, agent.rb2d.velocity.y);
        }
    }

    
}
    
