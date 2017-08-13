using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class RayCast : MonoBehaviour {
	public static float RayCast_Length = 22.0f; //set detect radius

    public Material raymat;
    public Material raymat2;

    public static int raysNumber = 36; // must be odd number
    public float[] rays;
    private Vector3[] angles;

    public List<LineRenderer> lines = new List<LineRenderer>();
    public List<GameObject> linesContainers = new List<GameObject>();

    private Vector3 origin;
	private float heading;

    private bool highlighted = false;

    void Start()
    {
        // initial arrays
        rays = new float[raysNumber];
        angles = new Vector3[raysNumber];

        // create the game play ray cast graphical lines
        for (int i = 0; i < raysNumber * 2; i++)
        {
            GameObject go = new GameObject("lineholder"+i);
            LineRenderer line = go.AddComponent<LineRenderer>();
            line.material = i >= raysNumber ? raymat2 : raymat;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.useWorldSpace = false;
            lines.Add(line);
            linesContainers.Add(go);
        }

        if (highlighted)
            highlightCast();
        else
            clearCast();
    }

    // Update is called once per frame
        void Update ()
	{

	    origin = transform.position;// + Vector3.up * 1.2f;
		
		heading = transform.rotation.eulerAngles.y;
		
        for (int i = 0; i < raysNumber; i++)
        {
            // calculate ray position/angle
            int middle = i > Mathf.Ceil(raysNumber / 2) ? -1 : i == Mathf.Ceil(raysNumber / 2) ? 0 : 1;
            float innerIndx = Mathf.Abs(Mathf.Floor(raysNumber / 2) - i);
            float variation = heading + (90 / Mathf.Floor(raysNumber / 2)) * innerIndx * middle;
            float angle = (variation - 0 * innerIndx * middle) / 180 * Mathf.PI;
            angles[i] = new Vector3(origin.x - RayCast_Length * Mathf.Sin(angle), origin.y, origin.z - RayCast_Length * Mathf.Cos(angle));

            // if hit something
            // check the ray cast hit distance
            RaycastHit hit;
            if (Physics.Linecast(origin, angles[i], out hit) == false)
            {
                hit.distance = RayCast_Length;
            }

            // total dintance lines
            Debug.DrawLine(origin, angles[i], Color.blue);

            // game play lines render
            if (highlighted)
            {
                lines[i + raysNumber].SetPosition(0, origin);
                lines[i + raysNumber].SetPosition(1, angles[i]);
            }

            // hit dintance lines
            Vector3 shortRay = new Vector3(origin.x - hit.distance * Mathf.Sin(angle), origin.y, origin.z - hit.distance * Mathf.Cos(angle));
            Debug.DrawLine(origin, shortRay, Color.red);

            // game play lines render
            if (highlighted)
            {
                lines[i].SetPosition(0, origin);
                lines[i].SetPosition(1, shortRay);
            }

            // logical hit distance lines used by the neural network as inputs
            rays[i] = hit.distance;
        }

	}

    public void highlightCast()
    {
        highlighted = true;
        foreach (var go in linesContainers)
        {
            go.SetActive(true);
        }
    }

    public void clearCast()
    {
        highlighted = false;
        foreach (var go in linesContainers)
        {
            go.SetActive(false);
        }
    }

}
