using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour, IEnemy
{
    public int MaxHealth, CurrentHealth;
    public int MoveSpeed;
    public int AttackDamage;

    [SerializeField] private Transform _BuildingWingContainer;
    private bool isAlive;
    private Animator _animator;
    private Transform _targetBuildingWing;
    

    void Awake()
    {
        isAlive = true;
        CurrentHealth = MaxHealth;
        _animator = GetComponent<Animator>();
        _animator.SetBool("isAlive", true);
        _targetBuildingWing = DetermineTargetWing();
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    private Transform DetermineTargetWing()
    {
        int childIndex = Random.Range(0, _BuildingWingContainer.childCount);
        return _BuildingWingContainer.GetChild(childIndex);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            InitiateDeathRoutine();
        }
    }

    // This method triggers the death animation, which executes the Die() method at the end via an animation events
    public void InitiateDeathRoutine()
    {
        isAlive = false;
        _animator.SetBool("isAlive", false);
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
    }

    // The animation event at the end of the death animation executes this method, killing the FlyingEye
    public void Die()
    {
        Destroy(gameObject);
    }

    // Heals the enemy by a percentage of its max health
    public void GetHealed(int healPercentage)
    {
        CurrentHealth += (MaxHealth / 100) * healPercentage;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "castleWing")
        {
            IBuildingWing buildingWing = collision.gameObject.GetComponent<CastleWing>();
            buildingWing.RegisterDamage(AttackDamage);
            
            InitiateDeathRoutine();
        }
    }

    public void MoveTowardsTarget()
    {
        if (isAlive)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _targetBuildingWing.position, 
                MoveSpeed * Time.deltaTime
                );
        }
        
    }
}
