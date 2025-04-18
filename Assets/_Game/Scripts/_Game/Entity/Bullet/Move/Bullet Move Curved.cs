using UnityEngine;

public class BulletMoveCurved : MonoBehaviour
{
    private IMove moveStrategy;
    private Transform target;
    private BulletController bulletController;
    private BulletData data;
    private float speed;

    private void Awake()
    {
        bulletController = GetComponentInParent<BulletController>();
        /*if (bulletController == null)
        {
            Debug.LogError("BulletController not found in parent of BulletMoveStraight!");
        }*/
    }

    public void SetBulletMove(BulletData bulletData)
    {
        this.data = bulletData;
    }

    private void OnEnable()
    {
        float rotationSpeed = data != null ? data.rotationSpeed : 10f;
        float curveHeight = data != null ? data.curvedHeight : 2f;
        speed = data != null ? data.speed : 5f;
        moveStrategy = new CurvedMove(rotationSpeed, curveHeight);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (transform.parent != null && target != null)
        {
            moveStrategy.Move(transform.parent, speed, Time.deltaTime, target);
        }
        /*else
        {
            bulletController.BulletDespaw.DespawnObject();
        }*/
    }
}