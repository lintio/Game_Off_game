using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initalState;
    public AiAgentConfig config;

    public Rigidbody2D rb2d;

    void Start()
    {
        config.facingDirection = "right";
        rb2d = GetComponent<Rigidbody2D>();
        config.baseScale = transform.localScale;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.ChangeState(initalState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
