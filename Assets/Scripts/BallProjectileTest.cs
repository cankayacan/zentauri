using System.Collections;
using UnityEngine;

public class BallProjectileTest : MonoBehaviour
{
    public Ball ball;

    void Start()
    {
        StartCoroutine(nameof(MyCoroutine));
    }

    private IEnumerator MyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            var rnd = new System.Random();
            var directionX = rnd.Next(2) == 0 ? 1 : -1;
            var directionZ = rnd.Next(2) == 0 ? 1 : -1;

            ball.Shoot(new Vector3(directionX * Random.Range(15, 25), 0, directionZ * Random.Range(15, 25) ));
        }
    }
}
