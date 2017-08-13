using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GUIController : MonoBehaviour {

    public List<Text> texts = new List<Text>();
    private List<String> varTexts = new List<String>();
    public GameObject carPanel;
    public Text carID;
    public Text carFitness;
    public Image carWheel;
    public Image speedDial;
    public Text steerText;
    public Text speedText;
    public Button killButton;
    public Toggle lockBestFitnessToggle;

    public List<UILineRenderer> nnInputLinesNode = new List<UILineRenderer>();
    public List<UILineRenderer> nnHiddenLinesNode = new List<UILineRenderer>();
    public List<UILineRenderer> nnInputLines = new List<UILineRenderer>();
    public List<UILineRenderer> nnHiddenLines = new List<UILineRenderer>();


    // Use this for initialization
    void Start () {
        killButton.onClick.AddListener(onButtonKillClicked);
        lockBestFitnessToggle.onValueChanged.AddListener(onBestFitnessToggle);

        uint inputsNum = SimMaster.instance.gga.inputNum;
        uint[] hiddenArr = SimMaster.instance.gga.hiddenArr;
        uint outputNum = SimMaster.instance.gga.outputNum;

        float xd = 100;
        float yd = gameObject.GetComponent<RectTransform>().rect.height - 10;
        float w = (float)(carPanel.gameObject.GetComponent<RectTransform>().rect.height * 0.85 / (inputsNum*2));
        float h = w;
        float space = h;

        // input nodes
        /*for (int i = 0; i < inputsNum; i++)
        {
            addUILine(nnInputLinesNode, new Vector2(xd, yd - (i * space + i * h)), new Vector2(xd, yd - (i * space + i * h + w)), h, Color.green);
        }

        // hidden nodes
        float hiddenWidth = (float)(carPanel.gameObject.GetComponent<RectTransform>().rect.width * 0.85);
        for (int i = 0; i < hiddenArr.Length; i++)
        {
            for (int j = 0; j < hiddenArr[i]; j++)
            {
                //float x = xd +  (hiddenWidth / (hiddenArr.Length + 1)) * (i + 1);
                float x = hiddenWidth;
                addUILine(nnHiddenLinesNode, new Vector2(x, yd - (j * space + j * h)), new Vector2(x, yd - (j * space + j * h + w)), h, Color.red);
            }
        }*/

        // input lines to hidden
        /*for (int i = 0; i < nnInputLinesNode.Count; i++)
        {
            for (int j = 0; j < nnHiddenLinesNode.Count; j++)
            {
                addUILine(nnInputLines,
                    new Vector2(nnInputLinesNode[i].Points[1].x, nnInputLinesNode[i].Points[1].y + h/2),
                        new Vector2(nnHiddenLinesNode[j].Points[0].x, nnHiddenLinesNode[j].Points[0].y - h/2), 
                        h/10, 
                        Color.green);
            }
        }*/

        // wieghts as dots
        /*for (int i = 0; i < nnInputLinesNode.Count; i++)
        {
            for (int j = 0; j < nnHiddenLinesNode.Count; j++)
            {
                float x = xd + w + i * space + i * w;
                float x2 = x + w;
                float y = yd - (j * space + j * h);
                addUILine(nnHiddenLinesNode, new Vector2(x, y), new Vector2(x2, y), h, Color.blue);
            }
        }*/

    }

    void addUILine(List<UILineRenderer> ls, Vector2 v1, Vector2 v2, float thickness, Color c)
    {
        GameObject go = new GameObject();
        go.transform.parent = carPanel.gameObject.transform;
        
        UILineRenderer line = go.AddComponent(typeof(UILineRenderer)) as UILineRenderer;
        line.Points = new Vector2[2];
        line.Points[0] = v1;
        line.Points[1] = v2;
        line.LineThickness = thickness;
        line.color = c;

        //Renderer rend = go.AddComponent(typeof(Renderer)) as Renderer;
        //rend.material.color = Color.white;

        ls.Add(line);
    }
	
	// Update is called once per frame
	void Update ()
	{
        // car
	    Car car = SimMaster.instance.selectedCar;
        carID.text = car.id.ToString();
	    carFitness.text = car.fitness.ToString();

        var rotationVector = carWheel.transform.rotation.eulerAngles;
	    rotationVector.z = MapInterval(car.rawAngle, 0.0f, 1.0f, -90, 90); 
        carWheel.transform.rotation = Quaternion.Euler(-rotationVector);
	    steerText.text = car.rawAngle.ToString();

        rotationVector.z = MapInterval(car.rawSpeed,0.0f, 1.0f, 170, -60);
	    speedDial.transform.rotation = Quaternion.Euler(rotationVector);
	    speedText.text = car.rawSpeed.ToString();


        // texts / configs
        varTexts.Add(Time.time.ToString("00.00"));
	    varTexts.Add(SimMaster.instance.gga.generation.ToString());
	    varTexts.Add(GGA.avarageFitness.ToString("00.00"));
        varTexts.Add(GGA.bestFitness.ToString());
	    varTexts.Add(SimMaster.instance.gga.fitnessGoal.ToString());
        varTexts.Add(SimMaster.instance.gga.populationNumber.ToString());
	    varTexts.Add(SimMaster.instance.gga.liveGenomes.Count.ToString());
	    varTexts.Add(SimMaster.instance.gga.deadGenomes.Count.ToString());	    
        varTexts.Add(SimMaster.instance.gga.randomMargin.ToString());
	    varTexts.Add(SimMaster.instance.gga.mutateRate.ToString());

	    for (int i = 0; i < texts.Count; i++)
	    {
	        texts[i].text = varTexts[i];
	    }

	    varTexts.Clear();

    }

    void onButtonKillClicked()
    {
        SimMaster.instance.selectedCar.Kill();
    }

    private float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
    {
        if (val >= srcMax) return dstMax;
        if (val <= srcMin) return dstMin;
        return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
    }

    private void onBestFitnessToggle(bool val)
    {
        SimMaster.instance.lockBestFitness = val;
    }

}
