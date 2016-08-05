using UnityEngine;
using System.Collections;
using System.Linq;

public class PushMower : LawnMower
{

    public override void MoveMower(float horizontal, float vertical)
    {
        RaycastHit hitInfo;
        Vector3 directionVector = Vector3.zero;
        if (System.Math.Abs(horizontal) > 0)
        {
            int direction = horizontal > 0 ? 1 : -1;
            directionVector = new Vector3(direction, 0, 0);
        }
        if (System.Math.Abs(vertical) > 0)
        {
            int direction = vertical > 0 ? 1 : -1;
            directionVector = new Vector3(0, direction, 0);
        }

        bool hitSomething = Physics.Raycast(gameObject.transform.position, directionVector, out hitInfo, 20);
        if (hitSomething)
        {
            var closestGrassTile = GameObject.FindObjectsOfType<Grass>()
                                                .Where(x => Vector3.Distance(gameObject.transform.position, x.transform.position) < Vector3.Distance(gameObject.transform.position, hitInfo.point))
                                                .OrderBy(x => Vector3.Distance(x.transform.position, hitInfo.point)).FirstOrDefault();

            if (closestGrassTile != null)
            {
                Vector3 closestGrassPosition = closestGrassTile.transform.position;
                Vector3 targetPosition = closestGrassPosition - new Vector3(0, 0, .5f * (closestGrassTile.transform.localScale.z + gameObject.transform.localScale.z));

                if (!IsMoving)
                {
                    StartCoroutine(MoveToPosition(targetPosition));
                }
            }
        }
    }
}
