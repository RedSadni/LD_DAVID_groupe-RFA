using UnityEngine;

public class AnimationFromTrigger : MonoBehaviour
{
    [Tooltip("Only triggers if the thing that enters that trigger has this tag")]
    public string triggerTagFilter = "PlayerCharacter";
    [Space]
    public Animator animatorToControl;
    [Tooltip("The script sets this trigger to true in the controller")]
    public string nameOfTheAnimatorTrigger;
    [Space]
    public GameObject[] gameObjectsToActivate;
    [Tooltip("Set false if you want the script to diactivate the gameobjects")]
    public bool itActivates = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == triggerTagFilter)
        {
            LaunchAnimation();
            ActivateGameobjects();
        }
    }

    private void LaunchAnimation()
    {
        if (animatorToControl == null)
            return;

        animatorToControl.SetTrigger(nameOfTheAnimatorTrigger);
    }
    private void ActivateGameobjects()
    {
        if (gameObjectsToActivate.Length == 0)
            return;

        foreach (GameObject game_object in gameObjectsToActivate)
        {
            game_object.SetActive(itActivates);
        }
    }
}
