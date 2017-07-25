
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20.0f;
    public float panBorderThickness = 10.0f; 
    public Rect panLimit = new Rect(-100,-380,25,-275);
    public float scrollSpeed = 200.0f;
    public float maxScroll = 500.0f;
    public float minScroll = 100.0f;

    // Update is called once per frame
    void Update ()
	{

	    Vector3 pos = transform.position;

	    if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
	    {
	        pos.z += panSpeed * Time.deltaTime;
	    }

	    if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
	    {
	        pos.z -= panSpeed * Time.deltaTime;
	    }

	    if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
	    {
	        pos.x += panSpeed * Time.deltaTime;
	    }

	    if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
	    {
	        pos.x -= panSpeed * Time.deltaTime;
	    }

	    float scroll = Input.GetAxis("Mouse ScrollWheel");
	    pos.y -= scroll * scrollSpeed * Time.deltaTime;
	    pos.y = Mathf.Clamp(pos.y, minScroll, maxScroll);

        pos.x = Mathf.Clamp(pos.x, panLimit.x, panLimit.width);
	    pos.z = Mathf.Clamp(pos.z, panLimit.y, panLimit.height);

        transform.position = pos;
	}
}
