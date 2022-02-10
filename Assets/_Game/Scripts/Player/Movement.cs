using FMOD;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _slowSpeed = 0.6f;
    [SerializeField]
    private float _rotationSpeed = 500f;
    [SerializeField]
    private Batteries battery;
    [SerializeField] 
    float batteryDrainRate = 2f;

    private Rigidbody _rigidbody;
    private Vector2 _inputMovement;
    private float _tickDelay = 1f;
    private float _nextTick = 0f;
    
    private Vector3 InputMovement => new Vector3(_inputMovement.x, 0, _inputMovement.y);
    private Vector3 Position => transform.position;
    
    public void OnMovement(InputAction.CallbackContext value)
    {
        _inputMovement = value.ReadValue<Vector2>();
    }

    [SerializeField] 
    private string _driveSound;
    [SerializeField] 
    private string _collisionSound;
    
    FMOD.Studio.EventInstance _driveSoundEvent;
    FMOD.Studio.EventInstance _collisionSoundEvent;
    
    private void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _driveSoundEvent = Sounds.CreateSoundEvent(_driveSound, transform);
        Sounds.PlaySound(_driveSoundEvent, 0.01f);
    }
    
    private void FixedUpdate () {
        Vector3 moveDirection = Position + InputMovement;
        Vector3 direction = (moveDirection - Position).normalized;
        float speedTemp = _speed;

        if (battery.HasLowBattery)
        {
            speedTemp = _slowSpeed;
        }
        
        _rigidbody.AddForce(InputMovement * speedTemp * _rigidbody.mass * Time.deltaTime,ForceMode.Impulse);
        if (moveDirection != Position)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed*Time.fixedDeltaTime);
        }

        if (Time.time >= _nextTick && battery.CurrentEnergy > 0)
        {
            battery.CurrentEnergy -= batteryDrainRate;
            _nextTick = Time.time + _tickDelay;
        }

        float volume = 0.01f + _rigidbody.velocity.magnitude / 200;
        Sounds.ChangeVolume(_driveSoundEvent, volume);
        
    }

    private void OnCollisionEnter(Collision other)
    {
        float volume = 1f + _rigidbody.velocity.magnitude / 2;
        if (other.gameObject.CompareTag("Grabable"))
            volume /= 20f;
        _collisionSoundEvent = Sounds.CreateSoundEvent(_collisionSound, other.transform);
        Sounds.PlaySound(_collisionSoundEvent, volume);
    }
}
