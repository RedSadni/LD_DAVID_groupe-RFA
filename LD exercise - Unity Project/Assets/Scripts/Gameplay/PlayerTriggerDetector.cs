using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDetector : MonoBehaviour
{
    public float metallicPlateRadius = 10f;

    private GameManager gameManager; 
    private InteractionManager interactionManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        interactionManager = FindAnyObjectByType<InteractionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MetallicPlate"))
        {
            if (!gameManager.inputs.invisibilityCheat)
            {
                gameManager.EmitSound(other.transform.position, metallicPlateRadius);
            }
        }
        else if (other.CompareTag("Zombie"))
        {
            if (!gameManager.inputs.invisibilityCheat)
            {
                gameManager.RespawnAvatar();
            }
        }
        else if (other.CompareTag("Checkpoint"))
        {
            gameManager.UpdateCheckpoint(other.transform.parent.position);
        }
        else if (other.CompareTag("Interactable"))
        {
            Interactable interactable_found = other.GetComponent<Interactable>();
            if (interactable_found != null)
            {
                interactionManager.SetNewInteractableInRange(interactable_found);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable_found = other.GetComponent<Interactable>();

            if (interactable_found != null)
            {
                interactionManager.RemoveInteractableInRange(interactable_found);
            }
        }
    }

}
