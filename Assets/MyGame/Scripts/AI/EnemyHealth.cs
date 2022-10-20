using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    public float blinkDuration = 0.1f;

    [SerializeField]
    private AIAgent agent;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField]
    private AIHealthBar healthBar;

    void Start()
    {
        agent = GetComponent<AIAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<AIHealthBar>();
        currentHealth = maxHealth;
        SetUp();
    }

    private void SetUp()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if(currentHealth <= 0f)
        {
            Die(direction);
            return;
        }
        StartCoroutine(EnemyFlash());
    }

    private void Die(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateID.Death);
    }

    private IEnumerator EnemyFlash()
    {
        skinnedMeshRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(blinkDuration);
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        StopCoroutine(nameof(EnemyFlash));
    }
}
