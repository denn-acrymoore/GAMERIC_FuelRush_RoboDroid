using UnityEngine;

[System.Serializable]
public class CustomGrid : MonoBehaviour
{
    [SerializeField] private GameObject objectWithinGrid;

    public GameObject GetObjectWithinGrid()
    {
        return objectWithinGrid;
    }

    public void SetObjectWithinGrid(GameObject obj)
    {
        objectWithinGrid = obj;
    }

    //private void Start()
    //{
    //    Debug.Log(gameObject.name + ": " + objectWithinGrid);
    //}
}
