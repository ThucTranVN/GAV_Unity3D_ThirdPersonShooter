using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    public float blinkDuration = 0.1f;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AIHealthBar healthBar;

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<AIHealthBar>();
        currentHealth = maxHealth;
        SetUp();
        this.OnStart();
    }

    private void SetUp()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            if(hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if(healthBar != null)
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }
        this.OnDamage(direction);
        if(currentHealth <= 0f)
        {
            Die(direction);
            return;
        }
        StartCoroutine(EnemyFlash());
    }

    private void Die(Vector3 direction)
    {
        this.OnDeath(direction);
    }

    private IEnumerator EnemyFlash()
    {
        skinnedMeshRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(blinkDuration);
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        StopCoroutine(nameof(EnemyFlash));
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath(Vector3 direction)
    {

    }

    protected virtual void OnDamage(Vector3 direction)
    {

    }
}
