using UnityEngine;

public class LeverGenertator : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        while(Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];
            Vector2 newPostition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);
            Transform newPart = Instantiate(part, newPostition, transform.rotation, transform);
            nextPartPosition = newPart.Find("EndPoint").position;
        }

        if (transform.childCount > 0)
        {
            Transform partoDelete = transform.GetChild(0);

            if (Vector2.Distance(player.transform.position, partoDelete.transform.position) > distanceToDelete)
            {
                Destroy(partoDelete.gameObject);
            }
        }

    }
}
