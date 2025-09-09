using UnityEngine;

public interface IDamageable { void TakeDamage(float amount); bool IsAlive { get; } }
public interface IHealthReadable { float Current { get; } float Max { get; } }
public interface IMovable { void Move(Vector3 direction, float speed); }
public interface IAttacker { void Attack(); }
public interface IActor : IDamageable, IHealthReadable { }
public interface IEnemy : IActor { void SetTarget(Transform target); }
