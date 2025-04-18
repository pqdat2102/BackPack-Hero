using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class AreaImpact : AreaAbstract
{
    [Header("Bullet Impact")]
    [SerializeField] protected CapsuleCollider2D capsuleCollider2D;
    [SerializeField] protected Rigidbody2D rb2d;

    private float lifetime;
    private float damage;
    private Transform associatedEnemy;

    private float timeGetDamage = 0.1f; // thời gian giữa 2 lần kích hoạt collider
    private float nextToggleTime = 0f; // thời gian tiếp theo kích hoạt collider
    private bool isColliderEnabled = true; // trạng thái kích hoạt collider
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
        this.LoadRigidbody();
    }

    protected virtual void LoadCollider()
    {
        if (this.capsuleCollider2D != null) return;
        this.capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        this.capsuleCollider2D.isTrigger = true;
        Debug.Log(transform.name + ": Load Collider", gameObject);
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb2d != null) return;
        this.rb2d = GetComponent<Rigidbody2D>();
        this.rb2d.gravityScale = 0f;
        Debug.Log(transform.name + ": Load Rigidbody", gameObject);
    }

    public void Initialize(float damage, float lifetime, float timeGetDamage, Transform enemy)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        this.associatedEnemy = enemy;
        this.timeGetDamage = timeGetDamage;

        if (areaController != null)
        {
            if (areaController.AreaDamageSender != null)
            {
                areaController.AreaDamageSender.SetDamageFromConfig(damage);
            }
        }
        else
        {
            Debug.LogError("areaController is null in AreaImpact Initialize!");
        }
        capsuleCollider2D.enabled = true;
        nextToggleTime = Time.time + timeGetDamage;
    }

    public void ScaleSize(float widthMultiplier, float heightMultiplier)
    {
        if (capsuleCollider2D != null)
        {
            Vector2 newSize = capsuleCollider2D.size;
            newSize.x *= widthMultiplier;
            newSize.y *= heightMultiplier;
            transform.parent.localScale = new Vector3(widthMultiplier, heightMultiplier, 1f);
            Debug.Log("Scale size to: {capsuleCollider2D.size}");
        }
    }

    private void Update()
    {
        if (Time.time >= nextToggleTime)
        {
            ToggleColliderState();
            nextToggleTime = Time.time + timeGetDamage;
        }
    }

    private void ToggleColliderState()
    {
        isColliderEnabled = !isColliderEnabled;
        capsuleCollider2D.enabled = isColliderEnabled;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        this.areaController.AreaDamageSender.Send(other.transform);
    }
}