using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEzExp;


public class TestEzExp : MonoBehaviour {
    public GameObject prefab;
    GameObject lastCube = null;

    public int trials = 10;

    void Start () {
        // GenerateNewCube();
		//EzExp.Instance.Load("Assets/ezExp/Examples/test_file.csv");
		EzExp.Instance.LoadFile("Assets/ezExp/Examples/test_file.csv", "4", "USER_ID", FileType.CSV, FileType.XML);
		EzExp.Instance.SetRecordFilePath("Assets/ezExp/Examples/saved_data.xml");
    }
	
	void Update () {
        CheckClick();

		// L = Load next trial, S = Start trial, E = End trial
		if (Input.GetKeyUp (KeyCode.L)) { 
			try {
				Trial t = EzExp.Instance.LoadNextTrial (); 
				Log.Debug ("Trial loaded (" + t.ToString () + ")");
				string[] parameters;
				EzExp.Instance.GetParameters(out parameters);

//				for(int i = 0; i < parameters.Length; ++i) {
//					Log.Debug(parameters[i]+" = "+EzExp.Instance.GetParameterData(parameters[i]));
//				}
				EzExp.Instance.SetResultData("10", 10+"");
				EzExp.Instance.SetResultData("5", 5+"");
				EzExp.Instance.SetResultData("42", 42+"");
				EzExp.Instance.SetResultData("92", 92+"");

				EzExp.Instance.SetResultsHeader(new string[]{"10", "92", "5", "42"});
			} catch (AllTrialsPerformedException) { Log.Debug ("No more trial to run"); }
		} else if (Input.GetKeyUp (KeyCode.S)) {
			EzExp.Instance.StartTrial ();
			Log.Debug ("Trial started");
		} else if (Input.GetKeyUp (KeyCode.E)) {
			EzExp.Instance.EndTrial ();
			Log.Debug("Trial ended: "+ EzExp.Instance.GetCurrentTrial().ToString(","));
		}
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == lastCube)
                {
                    NextStep();
                }
            }
        }

    }

    void NextStep()
    {
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
