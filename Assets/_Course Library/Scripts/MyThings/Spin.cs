using UnityEngine;

public class Spin : MonoBehaviour
{

    public Vector3 speed = new Vector3(0, 0, 90);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, Space.World);
    }
}
