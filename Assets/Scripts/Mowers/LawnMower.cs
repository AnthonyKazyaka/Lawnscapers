using UnityEngine;
using System.Collections;
using System.Linq;

public class LawnMower : MonoBehaviour
{

    public bool IsMoving = false;
    public float MowerRadius = 0.5f;

    [SerializeField]
    private float _speed = 5.0f;
    public float Speed { get { return _speed; } set { _speed = value; } }

    protected Vector3 previousPosition = new Vector3();

	// Use this for initialization
	protected void Start ()
    {
        previousPosition = gameObject.transform.position;
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        
    }

    protected void LateUpdate()
    {
        previousPosition = gameObject.transform.position;
    }

    public virtual void MoveMower(float horizontal, float vertical)
    {
        var directionVector = Vector3.zero;
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

        var newPosition = gameObject.transform.position + directionVector;

        //Debug.Log("New position: " + newPosition);

        RaycastHit hitInfo;
        bool hitSomething = Physics.Raycast(gameObject.transform.position, directionVector, out hitInfo, directionVector.magnitude);
        if (!hitSomething)
        {
            var closestGrassTile = GameObject.FindObjectsOfType<Grass>()
                                                    //.Where(x => Vector3.Distance(newPosition, x.transform.position) < Vector3.Distance(gameObject.transform.position, hitInfo.point))
                                                    .OrderBy(x => Vector3.Distance(x.transform.position, newPosition)).FirstOrDefault();

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

    protected virtual IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        IsMoving = true;
        CheckForGrassBelowMowerFromAbove();
        CheckForGrassBetweenCurrentAndPreviousPosition();

        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.01)
        {
            while(GameManager.Instance.IsPaused)
            {
                yield return null;
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

    protected void CheckForGrassBelowMowerFromAbove()
    {
        if (!GameManager.Instance.IsPaused)
        {
            RaycastHit hitInfoFromAbove;
            bool hitSomething = Physics.Raycast(gameObject.transform.position, new Vector3(0, 0, 1), out hitInfoFromAbove);

            if (hitSomething)
            {
                Grass grassTile = hitInfoFromAbove.collider.gameObject.GetComponent<Grass>();
                if (grassTile != null && !grassTile.IsMowed)
                {
                    Vector2 grassXY = new Vector2(grassTile.transform.position.x, grassTile.transform.position.y);
                    Vector2 hitpointXY = new Vector2(hitInfoFromAbove.point.x, hitInfoFromAbove.point.y);

                    if (Vector2.Distance(grassXY, hitpointXY) <= MowerRadius)
                    {
                        grassTile.Mow();
                    }
                }
            }
        }
    }

    protected void CheckForGrassBetweenCurrentAndPreviousPosition()
    {
        if(!GameManager.Instance.IsPaused)
        {

        }
        Vector3 offsetPreviousPosition = previousPosition;
        offsetPreviousPosition.z = 0.0f;    // All tiles have a collider going through the plane z = 0
        Vector3 offsetCurrentPosition = gameObject.transform.position;
        offsetCurrentPosition.z = 0.0f;

        Vector3 raycastVector = offsetPreviousPosition - offsetCurrentPosition;

        RaycastHit[] colliderHits = Physics.RaycastAll(offsetCurrentPosition, raycastVector, raycastVector.magnitude);
        Debug.DrawRay(offsetCurrentPosition, raycastVector, Color.red, 1.0f, false);

        var grassTilesHit = colliderHits.Where(x => x.collider.gameObject.GetComponent<Grass>() != null).Select(x => x.collider.gameObject.GetComponent<Grass>());

        if (grassTilesHit.Count() > 0)
        {
            foreach(Grass tile in grassTilesHit)
            {
                tile.Mow();
            }
        }
    }

}
