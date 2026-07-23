using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class ZombieBehavior : MonoBehaviour
{
    public float delayToLoseInterest = 2f;
    public float viewAngle = 30f;
    public float viewDistance = 5f;
    public TMP_Text debugText;
    public GameObject TriggerKill;

    public Animator animator;

    [HideInInspector] public bool isDead;
    [HideInInspector] public ModifiedFirstPersonController avatarController;

    private NavMeshAgent navAgent;
    private float stateTime;
    private GameManager gameManager;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private enum ZombieState
    {
        Idle,
        Startled,
        Attracted,
        Dead
    }
    private ZombieState currentState;
	
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        stateTime += Time.deltaTime;

        if (!gameManager.inputs.invisibilityCheat)
        {
            CheckVisibility();
        }

        switch (currentState)
        {
            case ZombieState.Idle:
                break;
            case ZombieState.Attracted:
                {
                    if (stateTime >= delayToLoseInterest)
                    {
                        SwitchState(ZombieState.Idle);
                    }
                }
                break;
            case ZombieState.Startled:
                {
                    if (stateTime >= 0.5f)
                    {
                        SwitchState(ZombieState.Idle);
                    }
                }
                break;
            case ZombieState.Dead:
                break;
        }
    }

    public void Attract(Vector3 position, bool from_player)
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                {
                    if (!from_player || CheckLineOfSight(position))
                    {
                        SwitchState(ZombieState.Attracted);
                        navAgent.SetDestination(position);
                    }
                    else
                    {
                        SwitchState(ZombieState.Startled);
                        navAgent.SetDestination(transform.position + (position - transform.position).normalized);
                    }
                }
                break;
            case ZombieState.Startled:
                {
                    stateTime = 0f;
                    navAgent.SetDestination(transform.position + (position - transform.position).normalized);
                }
                break;
            case ZombieState.Attracted:
                {
                    stateTime = 0f;
                    navAgent.SetDestination(position);
                }
                break;
            case ZombieState.Dead:
                break;
        }

    }

    private void CheckVisibility()
    {
        Vector3 to_avatar = Vector3.ProjectOnPlane( (avatarController.transform.position - transform.position), Vector3.up) ;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if ( to_avatar.magnitude > viewDistance)
        {
            return;
        }

        float angle = Vector3.Angle(to_avatar, forward);
        if (angle > viewAngle)
        {
            return;
        }

        if (CheckLineOfSight(avatarController.transform.position))
        {
            SwitchState(ZombieState.Attracted);
            navAgent.SetDestination(avatarController.transform.position);
        }
    }

    private bool CheckLineOfSight (Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;
        direction = direction.normalized;

        RaycastHit hit;

        Physics.Raycast(transform.position + direction + Vector3.up, direction, out hit, distance);
        return (hit.collider.CompareTag("PlayerCharacter"));
    }

    private void SwitchState(ZombieState new_state)
    {
        if (currentState == new_state)
            return;

        stateTime = 0f;

        switch (currentState)
        {
            case ZombieState.Idle:
                navAgent.isStopped = false;
                break;
            case ZombieState.Startled:
                break;
            case ZombieState.Attracted:
                break;
            case ZombieState.Dead:
                if (animator)
                    animator.SetBool("Dead", false);
                TriggerKill.SetActive(true);

                break;
        }

        currentState = new_state;

        switch (currentState)
        {
            case ZombieState.Idle:
                navAgent.isStopped = true;
                debugText.text = "";
                break;
            case ZombieState.Startled:
                debugText.text = "?";
                break;
            case ZombieState.Attracted:
                debugText.text = "!";
                break;
            case ZombieState.Dead:
                debugText.text = "x";
                navAgent.enabled = false;
                if (animator)
                    animator.SetBool("Dead", true);
                TriggerKill.SetActive(false);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ZombieKill")
        {
            SwitchState(ZombieState.Dead);
        }
    }

    public void Respawn()
    {
        SwitchState(ZombieState.Idle);
        navAgent.ResetPath();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void OnDrawGizmos()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up) * viewDistance;

        Vector3 left_limit = Quaternion.Euler(0f, -viewAngle, 0f) * forward;
        Vector3 left_limit_pos = transform.position + Vector3.up + left_limit;
        Vector3 right_limit = Quaternion.Euler(0f, viewAngle, 0f) * forward;
        Vector3 right_limit_pos = transform.position + Vector3.up + right_limit;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, left_limit_pos);
        Gizmos.DrawLine(transform.position + Vector3.up, right_limit_pos);
        Gizmos.DrawLine(right_limit_pos, transform.position + Vector3.up + forward);
        Gizmos.DrawLine(left_limit_pos, transform.position + Vector3.up + forward);
    }
}
