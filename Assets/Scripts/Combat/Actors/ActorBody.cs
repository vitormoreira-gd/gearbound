using UnityEngine;

[RequireComponent(typeof(ActorInfo))]
public class ActorBody : MonoBehaviour
{
    private ActorInfo _actorInfo;
    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _actorInfo = GetComponent<ActorInfo>();
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _actorInfo.OnDamageTaken += HandleDamage;
        _actorInfo.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _actorInfo.OnDamageTaken -= HandleDamage;
        _actorInfo.OnDeath -= HandleDeath;
    }

    private void HandleDamage(float damage)
    {
        Debug.Log($"{_actorInfo.CurrentData.name} tomou {damage} de dano!");

        if(_animator != null)
        {
            _animator.SetTrigger("Hit");
        }
    }

    private void HandleDeath()
    {
        Debug.Log($"{_actorInfo.CurrentData.name} morreu!");

        if (_animator != null)
        {
            _animator.SetTrigger("Die");
        }

        enabled = false;
    }

    public void Move(Vector3 direction)
    {
        if (_rb != null)
        {
            float moveSpeed = _actorInfo.GetStat(StatsType.Speed);

            _rb.linearVelocity = direction.normalized * (moveSpeed * 10f) * Time.deltaTime;
        }

        if(_animator != null)
        {
            _animator.SetFloat("MoveX", direction.x);
            _animator.SetFloat("MoveY", direction.y);
            _animator.SetBool("IsMoving", direction.sqrMagnitude > 0.01f);
        }
    }

    public void Stop()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
        }

        if (_animator != null)
        {
            _animator.SetBool("IsMoving", false);
        }
    }
}
