using UnityEngine;

public class ChargeStation : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            Debug.Log("Charging");
    }
}
