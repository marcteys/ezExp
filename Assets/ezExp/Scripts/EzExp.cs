using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FileWriter;

//TODO : Mettre ailleurs
public enum LogLevel
{
    DEBUG,
    INFO,
    WARNING,
    ERROR,
    NONE
};

public class EzExp : MonoBehaviour {

	#region exceptions
	public class TrialNotLoadedException: Exception {};
	#endregion

    #region Singleton
    /// <summary>
    /// EzExp unique instance.
    /// Create  a new instance if any EzExp is present on the scene.
    /// Set the EzExp only on the first time.
    /// </summary>
    /// <value>EzExp static instance</value>
    public static EzExp Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

			EzExp[] ezExpManager = FindObjectsOfType(typeof(EzExp)) as EzExp[];
			if (ezExpManager.Length == 0) {
				Log.Warning ("EzExp not present on the scene. Creating a new one.");
				ezExpManager = new EzExp[1] {new GameObject ("EzExp").AddComponent<EzExp> ()};
			} 

			// instanciate the new instance and return the value
			_instance = ezExpManager [0];
			return _instance;
        }
        set
        {
            if (EzExp.Instance == null)
                _instance = value;
            else
            {
                Log.Error("You can only use one EzExp. Destroying the new one attached to the GameObject " + value.gameObject.name);
                Destroy(value);
            }
        }
    }
    private static EzExp _instance = null;
    #endregion

    #region variables
    public LogLevel logLevel = LogLevel.DEBUG;

    public string filePath;
    #endregion

	#region attributes
	/// <summary>
	/// Contains the list of trials for a given user. Should be emptied at the end of the experiment.
	/// </summary>
	static ArrayList trials;

	/// <summary>
	/// Index of the current trial.
	/// </summary>
	static int currentTrialIndex = -1;
	#endregion

	#region functions
    void Awake ()
    {
		// Open filereader
	}

	/// <summary>
	/// Load the variables file to prepare the experiment </summary>
	/// <param name="filepath">File path to load data from</param>
	/// <seealso cref="SomeMethod(string)">
	/// Notice the use of the cref attribute to reference a specific method </seealso>
	public void Load (string filepath)
    {
		trials = new ArrayList ();
		currentTrialIndex = -1;


		List<List<string>> data = CsvFileReader.ReadAll(filepath, Encoding.UTF8);
		string[] header = data [0].ToArray();
		for (int i = 1; i < data.Count; i++) {
			string[] line = data [i].ToArray ();
			Trial trial = new Trial (header, line);
			Log.Debug (trial.ToString());
			trials.Add (trial);
		}
	}

	/// <summary>
	/// Launch at the very beginning of the experiment. Should load files containing exp data, prepare timers, get ready for recording
	/// </summary>
	public void StartExperiment() {}

	/// <summary>
	/// Should be called when the experiment is over to check recording files (and display/throw some messages/events?)
	/// </summary>
	public void EndExperiment() {}

	/// <summary>
	/// Loads the next trial in the list loaded from the init file
	/// </summary>
	public Trial LoadNextTrial() 
	{
		if (currentTrialIndex + 1 < trials.Count) {
			return trials [++currentTrialIndex] as Trial;
		} else {
			throw new System.IndexOutOfRangeException("No more trial to run.");
		}
	}

	/// <summary>
	/// Loads the trial.
	/// </summary>
	/// <param name="trialIndex">Index of the trial to load</param>
	/// <returns>The trial.</returns>
	public Trial LoadTrial(int trialIndex)
	{
		if (0 <= trialIndex && trialIndex < trials.Count) {
			currentTrialIndex = trialIndex;
			return (Trial)trials[currentTrialIndex];
		} else {
			throw new System.IndexOutOfRangeException ();
		}
	}

	/// <summary>
	/// Starts the loaded trial.
	/// </summary>
	public void StartTrial() 
	{
		if (currentTrialIndex == -1) {
			throw new TrialNotLoadedException();	
		} else {
			// TODO start timer associated to that Trial
			Trial ct = (Trial) trials[currentTrialIndex];
			ct.StartTrial();
		}
	}

	/// <summary>
	/// Ends the loaded trial.
	/// </summary>
	public Trial EndTrial()
	{
		if (currentTrialIndex == -1) {
			throw new TrialNotLoadedException ();	
		} else {
			// TODO stop timer associated to the Trial and save results
			Trial ct = (Trial) trials[currentTrialIndex];
			ct.EndTrial();
			// TODO record results

			return ct;
		}
	}
	#endregion
}
