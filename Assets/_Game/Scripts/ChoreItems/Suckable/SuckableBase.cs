using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SuckableBase : ChoreItemBase {
    [SerializeField]
    private float suckingForce = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    protected GameEventChoreItemSet onChoreItemCompleted;

    private Rigidbody _rigidbody;
    private Coroutine _resetCoroutine;
    public Vector3 Position => transform.position;

    public abstract void Release();
    public void SuckTowards(Vector3 vacuumVelocity, Vector3 vacuumPosition, float radius) {
        Vector3 suckDirection = (vacuumPosition - Position).normalized;
        float distance = Vector3.Distance(Position, vacuumPosition);
        float distanceInPercentage = Mathf.Clamp01(1.1f - Mathf.Clamp01(distance / radius));
        transform.RotateAround(vacuumPosition, Vector3.up, 360f * rotationSpeed * Time.fixedDeltaTime);
        _rigidbody.velocity = vacuumVelocity + (suckDirection * (suckingForce * distanceInPercentage));

        if (!ReferenceEquals(_resetCoroutine, null)) {
            StopCoroutine(ResetVelocity());
        }

        _resetCoroutine = StartCoroutine(ResetVelocity());
    }

    private IEnumerator ResetVelocity() {
       yield return new WaitForSeconds(0.3f);
        _rigidbody.velocity = Vector3.zero;
    }

    protected override void Awake() {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
    }
}
