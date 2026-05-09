using UnityEngine;
public class PickItems : MonoBehaviour
{
    public static PickItems Instance { get; private set; }

    [SerializeField] private Transform handPosition;
    private GameObject carriedObject;

    private void Awake()
    {
        Instance = this;
    }

    // public void TryPickup()
    // {
    //     Ray ray = new Ray(Camera.main.transform.position, transform.forward);
    //     if (Physics.Raycast(ray, out RaycastHit hit, 3f))
    //     {
    //         if (hit.collider.CompareTag("Pickable"))
    //         {
    //             carriedObject = hit.collider.gameObject;

    //             Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
    //             Collider col = carriedObject.GetComponent<Collider>();

    //             rb.isKinematic = true;
    //             col.enabled = false;

    //             carriedObject.transform.SetParent(handPosition);
    //             carriedObject.transform.localPosition = Vector3.zero;
    //         }
    //     }
    // }
    public void TryPickup()
{
    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red); // test üçün

    if (Physics.Raycast(ray, out RaycastHit hit, 3f))
    {
        if (hit.collider.CompareTag("Pickable"))
        {
            carriedObject = hit.collider.gameObject;

            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            Collider col = carriedObject.GetComponent<Collider>();

            rb.isKinematic = true;
            col.enabled = false;

            carriedObject.transform.SetParent(handPosition);
            carriedObject.transform.localPosition = Vector3.zero;
        }
    }
}


    public void DropItem()
    {
        if (carriedObject != null)
        {
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            Collider col = carriedObject.GetComponent<Collider>();

            rb.isKinematic = false;
            col.enabled = true;

            carriedObject.transform.SetParent(null);
            carriedObject = null;
        }
    }
}