using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    const string LEFT = "left";
    const string RIGHT = "right";

    public float moveSpeed = 2.5f;
    public Vector3 baseScale;
    public float baseCastDist = 0.5f;
    public float baseSightDist = 5f;
    public string facingDirection;
    public Transform castPos;
    public Transform eyePos;
    public AiStateId lastState;
    public int huntTime = 10;
    public float chaseSpeed = 3f;
    public bool isHunting;

    public void ChangeFacingDirection(string newDirection, AiAgent agent)
    {
        Vector3 newScale = agent.config.baseScale;
        if (newDirection == "left")
            newScale.x = -agent.config.baseScale.x;
        else
            newScale.x = agent.config.baseScale.x;

        agent.transform.localScale = newScale;
        agent.config.facingDirection = newDirection;
    }

    public bool CanSeePlayer(AiAgent agent, float ChaseMult)
    {
        bool val = false;
        float castDist = agent.config.baseSightDist * ChaseMult;
        if (agent.config.facingDirection == LEFT)
            castDist = -(agent.config.baseSightDist * ChaseMult);

        Vector3 targetPos = agent.config.eyePos.position;
        targetPos.x += castDist;

        Debug.DrawLine(agent.config.eyePos.position, targetPos, Color.blue);
        if (Physics2D.Linecast(agent.config.eyePos.position, targetPos, 1 << LayerMask.NameToLayer("Player")))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }

    public bool IsHittingWall(AiAgent agent)
    {
        bool val = false;
        float castDist = agent.config.baseCastDist;
        if (agent.config.facingDirection == LEFT)
            castDist = -agent.config.baseCastDist;

        Vector3 targetPos = agent.config.castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(agent.config.castPos.position, targetPos, Color.green);
        if (Physics2D.Linecast(agent.config.castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        return val;
    }

    public bool IsNearEdge(AiAgent agent, float CastDistMult)
    {
        bool val = false;
        float castDist = agent.config.baseCastDist * CastDistMult;

        Vector3 targetPos = agent.config.castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(agent.config.castPos.position, targetPos, Color.red);
        if (Physics2D.Linecast(agent.config.castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }
        return val;
    }
}
