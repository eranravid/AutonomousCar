using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectControllerS : MonoBehaviour
{
    public int id = 0;
    public Text textIndex;
    public Button SelectButton;
    public Text textFitness;

    void OnGUI()
    {
        //SelectButton.onClick;
    }

    public void SetID(int id)
    {
        this.id = id;
        textIndex.text = "#" + id;
        SelectButton.onClick.AddListener(onButtonClicked);
    }

    void onButtonClicked()
    {
        SimMaster.instance.testSubjects[id].GetComponent<Car>().selectThisCar();
    }    

    public void SetFitness(int fitness)
    {
        textFitness.text = "Fitness: " + fitness;
    }
}
