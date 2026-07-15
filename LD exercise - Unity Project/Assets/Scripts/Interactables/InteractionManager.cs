using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Interactable interactableInRange;
    private StarterAssetsInputs inputs;


    private void Start()
    {
        inputs = FindAnyObjectByType<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (inputs.interact)
        {
            inputs.interact = false;

            if (interactableInRange != null)
            {
                interactableInRange.Interact();
            }
        }
    }

    public void SetNewInteractableInRange(Interactable new_interactable)
    {
        if (interactableInRange == new_interactable)
            return;

        if (interactableInRange != null )
            interactableInRange.ShowInteractable(false);

        interactableInRange = new_interactable;
        interactableInRange.ShowInteractable(true);
    }

    public void RemoveInteractableInRange(Interactable interactable)
    {
        if (interactable == interactableInRange)
        {
            if (interactableInRange != null)
                interactableInRange.ShowInteractable(false);

            interactableInRange = null;
        }
    }

}
