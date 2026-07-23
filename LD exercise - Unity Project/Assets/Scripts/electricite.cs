using UnityEngine;
public class GroupeElectrogeneSimple : MonoBehaviour
{
    // Cette fonction se lance automatiquement quand un objet entre dans la sphère
    private void OnTriggerEnter(objetQuiEntre)
    {
        // Si ce n'est pas un zombie, on ne fait rien
        if (!objetQuiEntre.CompareTag("zombie"))
            return;
 
        // On récupère tous les scripts du zombie (son "cerveau" : IA, déplacement, attaque...)
        MonoBehaviour[] scriptsDuZombie = objetQuiEntre.GetComponents<MonoBehaviour>();
 
        // Et on les détruit un par un : le zombie est bloqué, comme mort
        foreach (MonoBehaviour script in scriptsDuZombie)
        {
            Destroy(script);
        }
    }
}