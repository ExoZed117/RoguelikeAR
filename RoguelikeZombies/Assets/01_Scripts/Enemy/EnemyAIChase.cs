using UnityEngine;

[RequireComponent(typeof(Core.Health))]
public class EnemyAIChase : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 1.6f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1.0f;

    private Transform target;
    private float lastAttack;
    private Core.Health health;

    public float Current => health.Current;
    public float Max => health.Max;
    public bool IsAlive => health.IsAlive;

    void Awake() { health = GetComponent<Core.Health>(); }
    void Update()
    {
        if (!IsAlive || target == null) return;
        Vector3 to = target.position - transform.position;
        float d = to.magnitude;

        if (d > stopDistance)
        {
            transform.position += to.normalized * speed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, to.normalized, 0.2f);
        }
        else if (Time.time - lastAttack >= attackCooldown)
        {
            Attack();
            lastAttack = Time.time;
        }
    }

    public void SetTarget(Transform t) => target = t;
    public void TakeDamage(float amount) => health.TakeDamage(amount);

    public void Attack()
    {
        if (target != null && target.TryGetComponent<IDamageable>(out var dmg)) dmg.TakeDamage(damage);
    }

    // Simple hook de muerte
    void OnDeath() { gameObject.SetActive(false); }
}
