using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100f;

    private float _currentHealth;

    public bool IsAlive
    {
        get { return _currentHealth > 0f; }
    }

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void Hit(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(0, _currentHealth);
        if (!IsAlive)
        {
            Destroy(gameObject);
        }
    }
}
