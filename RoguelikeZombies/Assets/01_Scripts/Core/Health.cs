using UnityEngine;

namespace Core
{
    public class Health : MonoBehaviour, IActor
    {
        [SerializeField] private float max = 100f;
        [SerializeField] private float current;
        public float Current => current;
        public float Max => max;
        public bool IsAlive => current > 0f;

        void Awake() { current = max; }
        public void TakeDamage(float amount)
        {
            if (!IsAlive) return;
            current = Mathf.Max(0, current - amount);
            if (!IsAlive) SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }
    }
}
