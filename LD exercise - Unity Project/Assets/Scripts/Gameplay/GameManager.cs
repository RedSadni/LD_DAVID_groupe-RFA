using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float minimumSoundRadius = 1f;
    public float normalGroundSoundRadius = 15f;
    public float quietGroundSoundRadius = 3f;
    public float loudGroundSoundRadius = 30f;
    public float soundRadiusAdditionWhenSprinting = 15f;
    [Space]
    public Transform startPosition;

    [Space]
    public Text debugText;



    public enum GroundType
    {
        normalGround,
        quietGround,
        loudGround
    }

    [HideInInspector] public ZombieBehavior[] zombies;
    [HideInInspector] public ModifiedFirstPersonController avatarController;
    [HideInInspector] public StarterAssetsInputs inputs;
    private Vector3 lastCheckpointPosition;

    // gizmo
    private Vector3 lastEmitPosition;
    private float lastEmitRadius;
    private float lastEmitTime;

    void Start()
    {
        zombies = FindObjectsByType<ZombieBehavior>(FindObjectsSortMode.None);
        avatarController = FindAnyObjectByType<ModifiedFirstPersonController>();
        inputs = FindAnyObjectByType<StarterAssetsInputs>();

        foreach (ZombieBehavior z in zombies)
        {
            z.avatarController = avatarController;
        }

        lastCheckpointPosition = startPosition.position;
    }

    public void UpdateCheckpoint (Vector3 new_position)
    {
        lastCheckpointPosition = new_position;
    }

    public void EmitSound(Vector3 position, GroundType ground, bool is_sprinting, bool is_croutching)
    {
        float radius = minimumSoundRadius;

        if (is_croutching)
        {
            switch (ground)
            {
                case GroundType.normalGround:
                    break;
                case GroundType.quietGround:
                    break;
                case GroundType.loudGround:
                    radius = quietGroundSoundRadius;
                    break;
            }
        }
        else if (is_sprinting)
        {
            switch (ground)
            {
                case GroundType.normalGround:
                    radius = normalGroundSoundRadius + soundRadiusAdditionWhenSprinting;
                    break;
                case GroundType.quietGround:
                    radius = quietGroundSoundRadius + soundRadiusAdditionWhenSprinting;
                    break;
                case GroundType.loudGround:
                    radius = loudGroundSoundRadius + soundRadiusAdditionWhenSprinting;
                    break;
            }
        }
        else
        {
            switch (ground)
            {
                case GroundType.normalGround:
                    radius = normalGroundSoundRadius;
                    break;
                case GroundType.quietGround:
                    radius = quietGroundSoundRadius;
                    break;
                case GroundType.loudGround:
                    radius = loudGroundSoundRadius;
                    break;
            }
        }

        EmitSound(position, radius);
    }

    public void EmitSound(Vector3 position, float radius, bool from_player = true)
    {
        foreach (ZombieBehavior zombie in zombies)
        {
            if (!zombie.isDead)
            {
                float dist = Vector3.Distance(position, zombie.transform.position);

                if (dist <= radius)
                {
                    zombie.Attract(position, from_player);
                }
            }
        }

        lastEmitPosition = position;
        lastEmitRadius = radius;
        lastEmitTime = 0f;

        string dashes = "";
        for (int i = (int)radius; i > 0f; i -= 2)
        {
            dashes += "- ";
        }
        debugText.text = dashes + radius + " " + dashes;
    }

    public void RespawnAvatar()
    {
        avatarController.Teleport(lastCheckpointPosition);

        ZombieBehavior[] zombie_behaviors = FindObjectsByType<ZombieBehavior>(FindObjectsSortMode.None);

        foreach (ZombieBehavior zombie in zombie_behaviors)
        {
            zombie.Respawn();
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void Update()
    {
        lastEmitTime += Time.deltaTime;

        if (lastEmitTime > 1f)
        {
            debugText.text = "-";
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (lastEmitTime < 2f)
        {
            UnityEditor.Handles.color = new Color(0.9f, 0.3f, 0.1f, 0.4f);
            UnityEditor.Handles.DrawSolidDisc(lastEmitPosition + Vector3.up * 0.1f, Vector3.up, lastEmitRadius);

        }
    }
#endif
}
