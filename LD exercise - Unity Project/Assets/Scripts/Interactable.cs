using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactionIndicator;


    private void Awake()
    {
        interactionIndicator.SetActive(false);
    }
    public void ShowInteractable(bool is_interactable)
    {
        interactionIndicator.SetActive(is_interactable);
    }

    public virtual void Interact()
    {

    }
}
