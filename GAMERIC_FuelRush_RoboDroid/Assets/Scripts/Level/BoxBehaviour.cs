using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject crackedBoxPrefab;
    private float explosionForce = 80f;
    private float explosionRadius = 3f;

    [SerializeField] private AudioSource boxDestroyedSound;

    public void DestroyBox()
    {
        GameObject instance = Instantiate(crackedBoxPrefab, transform.position
            , transform.rotation);

        boxDestroyedSound = instance.GetComponent<AudioSource>();
        boxDestroyedSound.Play();

        Rigidbody[] rbInstances = instance.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbInstances)
        {
            rb.AddExplosionForce(explosionForce, instance.transform.position, explosionRadius);
        }

        Destroy(instance, 3f);
        Destroy(this.gameObject);
    }
}
