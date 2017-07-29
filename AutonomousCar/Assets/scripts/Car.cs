using System.Collections;
using System.Collections.Generic;
using FANNCSharp.Float;
using UnityEngine;

public class Car : MonoBehaviour
{

    public int id = 0;

    public bool hasFailed = false;

    public float MAX_ROTATION; //max rotate speed
    public float MAX_SPEED;
    public float headingAngle; //Degrees
    public float currentSpeed; // speed

    public SimMaster master;
    //public NNet neuralnet;
    public FANNCSharp.Float.NeuralNet neuralnet;
    public GGenome genome;
    public RayCast raycast;
    public Motor motor;
    public hit hit;    

    public int fitness = 0;
    public int genomeIndx = 0;

    public Vector3 defaultpos;
    public Quaternion defaultrot;

    public Camera cam;

    // Use this for initialization
    void Start () {
        hasFailed = false;

        //neuralnet = gameObject.GetComponent<NNet>();
        //neuralnet.CreateNet(1, 9, 8, 2);
        //neuralnet = genome.brain;
        raycast = gameObject.GetComponent<RayCast>();
        motor = gameObject.GetComponent<Motor>();
        hit = gameObject.GetComponent<hit>();

        defaultpos = transform.position;
        defaultrot = transform.rotation;

        cam.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
	{

	    if (genome == null) return;

        fitness = hit.checkpoints;
	    
        hasFailed = hit.crash;

        if (!hasFailed)
        {

             genome.fitness = fitness;// record fitness

            /*List<float> inputs = new List<float>();
            for (int i = 0; i < raycast.rays.Length; i++)
            {
                float dist = Normalise(raycast.rays[i]);
                inputs.Add(dist);
            }*/

            float[] inputs = new float[RayCast.raysNumber];
            for (int i = 0; i < raycast.rays.Length; i++)
            {
                float dist = Normalise(raycast.rays[i]);
                inputs[i] = dist;
            }

            float[] result = neuralnet.Run(inputs);
            //neuralnet.SetInput(inputs);
            //neuralnet.refresh();

            //float speed = neuralnet.GetOutput(0);
            //float angle = neuralnet.GetOutput(1);

            float speed = result[0];
            float angle = result[1];

            currentSpeed = Clamp(speed * MAX_SPEED, -MAX_SPEED, MAX_SPEED);
            headingAngle = angle * MAX_ROTATION * 2 - MAX_ROTATION;

            //Debug.Log("Car raw output: speed " + speed + " angle " + angle);
            //Debug.Log("Car serialized output: speed " + currentSpeed + " angle " + headingAngle);
        }
        
    }

    void OnMouseDown()
    {
        selectThisCar();
    }

    public void selectThisCar()
    {
        master.selectedCar = this;
        cam.enabled = true;

        foreach (GameObject carObj in SimMaster.instance.testSubjects)
        {
            Car car = carObj.GetComponent<Car>();
            if (car != this)
                car.disSelectThisCar();
        }
    }

    public void disSelectThisCar()
    {
        cam.enabled = false;
    }

    public void setGenome(GGenome gen)
    {
        genome = null;
        neuralnet = null;
        genome = gen;
        neuralnet = gen.brain;
    }

    public float Normalise(float x)
    {
        float depth = x / RayCast.RayCast_Length;
        return  depth * 2 - 1;
    }

    public void ClearFailure()
    {
        genome = null;
        neuralnet = null;
        hasFailed = false;
        
        fitness = 0;
        transform.position = defaultpos;
        transform.rotation = defaultrot;

        if (hit != null)
        {
            hit.crash = false;
            hit.checkpoints = 0;
        }

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
