using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class RayCast : MonoBehaviour {
	public static float RayCast_Length = 22.0f; //set detect radius

    public static int raysNumber = 36; // must be odd number
    public float[] rays;
    private Vector3[] angles;

    public List<LineRenderer> lines = new List<LineRenderer>();

    private Vector3 origin;
	private float heading;

    void Start()
    {
        rays = new float[raysNumber];
        angles = new Vector3[raysNumber];

        for (int i = 0; i < raysNumber; i++)
        {
            LineRenderer line = gameObject.AddComponent<LineRenderer>();            
            line.startWidth = 2.5f;
            line.SetVertexCount(2);
            line.useWorldSpace = false;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + Vector3.up);
            lines.Add(line);

        }
        
    }

	// Update is called once per frame
	void Update ()
	{

	    origin = transform.position;// + Vector3.up * 1.2f;
		
		heading = transform.rotation.eulerAngles.y;
		
        for (int i = 0; i < raysNumber; i++)
        {
            int middle = i > Mathf.Ceil(raysNumber / 2) ? -1 : i == Mathf.Ceil(raysNumber / 2) ? 0 : 1;
            float innerIndx = Mathf.Abs(Mathf.Floor(raysNumber / 2) - i);
            float variation = heading + (90 / Mathf.Floor(raysNumber / 2)) * innerIndx * middle;
            float angle = (variation - 0 * innerIndx * middle) / 180 * Mathf.PI;
            angles[i] = new Vector3(origin.x - RayCast_Length * Mathf.Sin(angle), origin.y, origin.z - RayCast_Length * Mathf.Cos(angle));

            RaycastHit hit;
            if (Physics.Linecast(origin, angles[i], out hit) == false)
            {
                hit.distance = RayCast_Length;
            }
            Debug.DrawLine(origin, angles[i], Color.blue);
            Vector3 shortRay = new Vector3(origin.x - hit.distance * Mathf.Sin(angle), origin.y, origin.z - hit.distance * Mathf.Cos(angle));
            Debug.DrawLine(origin, shortRay, Color.red);
            rays[i] = hit.distance;
        }

       // CastRay();

	}

	void CastRay() {

        
        for (int i = 0; i < raysNumber; i++)
        {
            
        }

    }
}
