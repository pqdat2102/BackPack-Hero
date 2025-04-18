using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] BulletFindTarget targetFinder;
    [SerializeField] BulletShooter bulletShooter;
    public void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            targetFinder.ResetAssignedBullets();

            dice.SetCanRoll(true);
            dice.StartRoll();

            yield return new WaitUntil(() => dice.IsRolling());
            yield return new WaitUntil(() => !dice.IsRolling());
            //Debug.Log("Dice roll completed!");

            int bulletCount = dice.GetNumberBullet();
            int faceValue = dice.GetFaceIndexValue();
            /*Debug.Log($"Dice: bulletCount={bulletCount}, faceValue={faceValue}");*/

            dice.SetCanRoll(false);
            //Debug.Log($"Dice: bulletCount={bulletCount}, faceValue={faceValue}");

            // Thu thập danh sách quái một lần trước khi bắn
            targetFinder.CacheEnemies(transform.position);
            //Debug.Log($"GamePlayManager: Đã thu thập danh sách quái!");

            yield return StartCoroutine(bulletShooter.SpawnBulletsSequentially(bulletCount, faceValue));
            //Debug.Log($"GamePlayManager: Đã bắn {bulletCount} viên đạn!");

            yield return new WaitForSeconds(0.5f);
        }
    }
}
