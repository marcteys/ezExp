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
		EzExp.Instance.Load("Assets/ezExp/Examples/test_file.csv");
    }
	
	void Update () {
        CheckClick();

		// L = Load next trial, S = Start trial, E = End trial
		if (Input.GetKeyUp (KeyCode.L)) { 
			try {
				Trial t = EzExp.Instance.LoadNextTrial (); 
				Log.Debug ("Trial loaded (" + t.ToString () + ")");
			} catch (System.IndexOutOfRangeException e) {
				Log.Debug ("No more trial to run");
			}
		} else if (Input.GetKeyUp (KeyCode.S)) {
			EzExp.Instance.StartTrial ();
			Log.Debug ("Trial started");
		} else if (Input.GetKeyUp (KeyCode.E)) {
			Trial t = EzExp.Instance.EndTrial ();
			Log.Debug ("Trial ended ("+t.ToString(true)+")");
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
