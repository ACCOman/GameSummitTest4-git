using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    [SerializeField] private string cargoTag = "Pickable"; // Kargonun tagı

    private void OnTriggerEnter(Collider other)
    {
        // Əgər triggerə girən obyekt kargo-dursa
        if (other.CompareTag(cargoTag))
        {
            // Kargo obyektini sil
            Destroy(other.gameObject);
            Destroy(gameObject);

            // Burada oyunçuya xal və ya mükafat verə bilərsən
            Debug.Log("Kargo uğurla çatdirildi!");
        }
    }
}
