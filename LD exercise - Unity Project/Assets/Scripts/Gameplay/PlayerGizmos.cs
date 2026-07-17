using UnityEditor;
using UnityEngine;

public class PlayerGizmos : MonoBehaviour
{
    public Color minimumSoundColor;
    public Color normalGroundSoundColor;
    public Color quietGroundSoundColor;
    public Color loudGroundSoundColor;
    public Color normalWhenSprintingColor;

    public GameManager gameManager;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = minimumSoundColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, gameManager.minimumSoundRadius);
        Handles.color = normalGroundSoundColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, gameManager.normalGroundSoundRadius);
        Handles.color = quietGroundSoundColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, gameManager.quietGroundSoundRadius);
        Handles.color = loudGroundSoundColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, gameManager.loudGroundSoundRadius);
        Handles.color = normalWhenSprintingColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, gameManager.normalGroundSoundRadius + gameManager.soundRadiusAdditionWhenSprinting);
    }
#endif
}
