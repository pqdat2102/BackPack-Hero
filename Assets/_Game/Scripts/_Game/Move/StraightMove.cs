using UnityEngine;

public class StraightMove : IMove
{
    private Transform target;

    public void Move(Transform objTransform, float speed, float deltaTime, Transform target)
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            objTransform.position += objTransform.right * speed * deltaTime;
            Debug.Log("bay thang khong co target, dan di chuyen thang");
            return;
        }
        /*Debug.Log("co muc tieu di chuyen thang");*/
        // Di chuyển thẳng đến mục tiêu
        objTransform.position = Vector3.MoveTowards(objTransform.position, target.position, speed * deltaTime);
    }
} 