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

        var currentSpeed = 0.0f;

        var initialDirection = (targetPosition - gameObject.transform.position).normalized;
        var initialPosition = gameObject.transform.position;

        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.01 && (targetPosition - gameObject.transform.position).normalized == initialDirection)
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

            if (currentSpeed < Speed)
            {
                currentSpeed += Acceleration * Time.deltaTime;
            }
            else if ((targetPosition - gameObject.transform.position).magnitude <= MowerRadius)
            {
                currentSpeed -= Acceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = Speed;
            }

            var newPosition = (targetPosition - gameObject.transform.position).normalized * currentSpeed * Time.deltaTime + gameObject.transform.position;
            if ((newPosition - initialPosition).magnitude >= (targetPosition - initialPosition).magnitude)
            {
                newPosition = targetPosition;
            }

            gameObject.GetComponent<Rigidbody>().MovePosition(newPosition);

            CheckForGrassBelowMowerFromAbove();
            CheckForGrassBetweenCurrentAndPreviousPosition();

            yield return null;
        }

        gameObject.transform.position = targetPosition;
        IsMoving = false;
    }
}
