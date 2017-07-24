using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {

	Car car;
	public float rotate, speed;

	public float heading;

	// Use this for initialization
	void Start () {
        car = gameObject.GetComponent<Car> ();

		heading = transform.rotation.eulerAngles.y - 90;

    }

    // Update is called once per frame
    void Update () {
		heading = transform.rotation.eulerAngles.y - 90;

		float angle = car.headingAngle;

		transform.Rotate (new Vector3 (0, angle, 0));

		float dir = heading / 180 * Mathf.PI;

        speed = car.currentSpeed;

        float nx = - speed * Mathf.Cos (dir);
		float nz = speed * Mathf.Sin (dir);

		Vector3 newsp = new Vector3(nx,0,nz);

		Vector3 newpos = transform.position + newsp * Time.deltaTime;
		transform.position = newpos;

        //Debug.Log("Motor: speed " + speed + " angle " + angle);
	}

    public float Clamp(float val, float min, float max)
    {
        if (val < min)
        {
            return min;
        }
        if (val > max)
        {
            return max;
        }
        return val;
    }
}
