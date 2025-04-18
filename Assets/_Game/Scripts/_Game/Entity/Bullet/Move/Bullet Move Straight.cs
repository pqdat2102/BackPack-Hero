using UnityEngine;

public class BulletMoveStraight : MonoBehaviour
{
    private IMove moveStrategy;
    private Transform target;
    private BulletController bulletController;
    private BulletData data;
    public float speed;

    private void Awake()
    {
        bulletController = GetComponentInParent<BulletController>();
        if (bulletController == null)
        {
            Debug.LogError("BulletController not found in parent of BulletMoveStraight!");
        }
    }

    public void SetBulletMove(BulletData bulletData)
    {
        this.data = bulletData;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnEnable()
    {
        speed = data != null ? data.speed : 10f;
        moveStrategy = new StraightMove();
    }

    private void Update()
    {
        if (transform.parent != null && target != null)
        {
            moveStrategy.Move(transform.parent, speed, Time.deltaTime, target);
        } 
        else
        {
            bulletController.BulletDespaw.DespawnObject();
        }
    }
}