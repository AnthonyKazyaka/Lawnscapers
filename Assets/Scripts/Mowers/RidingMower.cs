using UnityEngine;
using System.Collections;
using System.Linq;

public class RidingMower : PushMower
{

    protected override IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        IsMoving = true;
        CheckForGrassBelowMowerFromAbove();
        CheckForGrassBetweenCurrentAndPreviousPosition();

        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.01)
        {
            while (GameManager.Instance.IsPaused)
            {
                yield return null;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var bestPositionToStop = ((targetPosition - gameObject.transform.position).normalized * 0.4f) + gameObject.transform.position;
                var closestGrassTile = GameObject.FindObjectsOfType<Grass>().OrderBy(x => Vector3.Distance(x.transform.position, bestPositionToStop)).FirstOrDefault();
                targetPosition = closestGrassTile.transform.position - new Vector3(0, 0, .5f * (closestGrassTile.transform.localScale.z + gameObject.transform.localScale.z));
            }

            Vector3 newPosition = Vector3.Lerp(gameObject.transform.position, targetPosition, Speed * Time.deltaTime);
            gameObject.transform.position = newPosition;

            CheckForGrassBelowMowerFromAbove();
            CheckForGrassBetweenCurrentAndPreviousPosition();

            yield return null;
        }

        gameObject.transform.position = targetPosition;
        IsMoving = false;
    }
}
