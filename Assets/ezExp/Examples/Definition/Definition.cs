using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEzExp;


public class Definition : MonoBehaviour {
    public GameObject prefab;
    GameObject lastCube = null;

    public int trials = 10;

    void Start () {
        EzExp.Instance.SetParameters("columnName", "other", "lol");
        EzExp.Instance.StartExperiment();
    }

    void Update () {
        CheckClick();
    }

    void CheckClick()
    {
        EzExp.Instance.StartTimer("timerName");

        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == lastCube)
                {
                    EzExp.Instance.SetValue("columnName", hit.point);
                    NextStep();
                }
            }
        }

    }

    void NextStep()
    {
        EzExp.Instance.EndTimer("timerName");
        Destroy(lastCube);
        if(trials >0 )
        {
            GenerateNewCube();
            trials += -1;
        } else
        {
            Finished();
        }
    }

    void Finished()
    {
        Debug.Log("Finished ");
    }

    void GenerateNewCube()
    {
        lastCube = null;

        Vector3 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-2, 2), Random.Range(-5, 5));
        lastCube = GameObject.Instantiate(prefab);
        lastCube.transform.position = randPos;
    }
}
