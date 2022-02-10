using UnityEngine;

[RequireComponent((typeof(Rigidbody)))]
public class DirtSucker : MonoBehaviour {
    [SerializeField]
    private float radius = 0.5f;
    [SerializeField]
    [Tooltip("Max number of dirt it can collide with at the same time")]
    private int maxNumberOfDirt = 10;
    [SerializeField]
    private string _vacuumingtSound;
    FMOD.Studio.EventInstance _vacuumingtSoundEvent;

    private int _hit = 0;
    private int _dirtLayermask = 1 << 6; // Layer number 6
    private Rigidbody _rigidbody;

    private Vector3 Position => transform.position;

    private float lowerSuctionTime;
    
    private void Setup() {
        _rigidbody = GetComponent<Rigidbody>();
        _vacuumingtSoundEvent = Sounds.CreateSoundEvent(_vacuumingtSound, transform);
        Sounds.PlaySound(_vacuumingtSoundEvent, 0.03f);
    }

    private void TrySuckDirt()
    {
        Collider[] hitCollider = new Collider[maxNumberOfDirt];
        _hit = Physics.OverlapSphereNonAlloc(transform.position, radius, hitCollider, _dirtLayermask);

        if (_hit > 0) {
            for (int i = 0; i < _hit; i++) {
                if (ReferenceEquals(hitCollider[i], null)) {
                    break;
                }

                SuckableBase suckableObject = hitCollider[i].gameObject.GetComponent<SuckableBase>();

                if (!ReferenceEquals(suckableObject, null)) {
                    Sounds.ChangeVolume(_vacuumingtSoundEvent, 0.3f);
                    lowerSuctionTime = Time.time + 0.3f;

                    suckableObject.SuckTowards(_rigidbody.velocity, Position, radius);

                    if (Vector3.Distance(Position, suckableObject.Position) < 0.15) {
                        suckableObject.Release();
                    }
                }
            }
        }
        
            if (Time.time >= lowerSuctionTime)
            {
                Sounds.ChangeVolume(_vacuumingtSoundEvent, 0.03f);
            }
        
    }

    private void Start() => Setup();
    private void FixedUpdate() => TrySuckDirt();

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
