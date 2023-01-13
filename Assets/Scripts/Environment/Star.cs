using UnityEngine;

public class Star: Target
{
    protected override void OnBallTouched(GameObject part, GameObject ballGameObject)
    {
        base.OnBallTouched(part, ballGameObject);
        PointUtils.IncrementCurrentPoints();
    }
}