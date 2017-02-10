using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEzExp;
using FileWriter;
// TODO : use emum ExperimentState 
//TODO : faire une bool "reprnedre la ou on était" qui reprend le dernier utilisateur, etc ? 

namespace UnityEzExp
{
	#region exceptions
	/// <summary>
	/// Exception triggered if the participant ID was not found when loading the data.
	/// </summary>
	public class ParticipantIDNotFoundException: Exception { public ParticipantIDNotFoundException(string msg): base(msg){} }
	/// <summary>
	/// Exception thrown when all trials have been performed.
	/// </summary>
	public class AllTrialsPerformedException: Exception {};
	/// <summary>
	/// Exception thrown while trying to access to a trial but none has been loaded yet.
	/// </summary>
	public class TrialNotLoadedException : Exception {};
	#endregion

	/// <summary>
	/// The class <see cref="UnityEzExp.Experiment"/> is used to Load and Save data about the experiment configuration. At the beginning, it will load data from a .csv, .xml or .json file 
	/// and will save the results in an output file 
	/// </summary>
    public class Experiment
    {
		#region attributes
        /// <summary>
        /// Parameters are the "header" of the file
        /// </summary>
		public string[] _parameters;

		/// <summary>
		/// List of all trials for a given user
		/// </summary>
     	List<Trial> _trials = new List<Trial>();
        
		// paths of files to load and save data
		string _inputFilePath;
		string _outputFilePath;
		/// <summary>
		/// Type of files to handle. For now, all files (input or output share the same format).
		/// </summary>
		FileType _fileType;

		// name of the column containing participant IDs
		string _participantHeader;
		// ID of the current participant
		string _participantID;

        int _currentTrialIndex = -1;
		#endregion

		#region constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="UnityEzExp.Experiment"/> class.
		/// </summary>
		/// <param name="inputFilePath">Input file path to load data from.</param>
		/// <param name="participantHeader">Name of the column containing participants IDs.</param>
		/// <param name="participantID">ID of the participant for whom we want to load experiment data.</param>
		public Experiment(string inputFilePath, string participantHeader, string participantID, FileType fileType = FileType.CSV)
        {
			_inputFilePath = inputFilePath;
			_participantHeader = participantHeader;
			_participantID = participantID;
			_fileType = fileType;
			_currentTrialIndex = -1;
        }
		#endregion

		#region getters/setters
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <returns>The parameters.</returns>
		public string[] GetParameters() { return _parameters; }
		/// <summary>
		/// Sets the parameters.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		public void SetParameters(string[] parameters) {  
			_parameters = new string[parameters.Length];
			for(int i = 0; i < parameters.Length; i++) { _parameters[i] = parameters[i]; }
		}

		/// <summary>
		/// Gets the index of the current trial.
		/// </summary>
		/// <returns>The current trial index.</returns>
		public int GetCurrentTrialIndex() { return _currentTrialIndex; }
		/// <summary>
		/// Sets the index of the current trial.
		/// </summary>
		/// <param name="currentTrialIndex">Current trial index.</param>
		public void SetCurrentTrialIndex(int currentTrialIndex) { _currentTrialIndex = currentTrialIndex; }

		/// <summary>
		/// Add a trial to the list.
		/// </summary>
		/// <param name="trials">One or several trials to add.</param>
		public void SetTrial(params Trial[] trials)
		{
			foreach (Trial parameter in trials)
				_trials.Add(parameter);
		}

		/// <summary>
		/// Sets the trials.
		/// </summary>
		/// <param name="trials">Trials.</param>
		void SetTrials(List<Trial> trials) 
		{
			_trials = new List<Trial> ();
			foreach (Trial t in trials) { _trials.Add(t); }
		}

		// FIXME should not allow that kind of behavior -> parameters are fixed in the init file
//		/// <summary>
//		/// Add a list of undefined number of parameters manually
//		/// </summary>
//		/// <param name="parameters">Undefined number of parameters</param>
//		public void AddParameter(params string[] parameters)
//		{
//			foreach (string parameter in parameters)
//				_parameters.Add(parameter);
//		}

//		/// <summary>
//		/// Set the parameters from list
//		/// </summary>
//		/// <param name="parameters">List of parameters</param>
//		void SetParameters(List<string> parameters)
//		{
//			_parameters = parameters;
//		}
		#endregion

		#region load/save
		/// <summary>
		/// Load data from a file, with the provided format. Possibility to specify the separation character in .csv files.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <param name="fileType">Format of the file to parse.</param>
		/// <param name="encoding">Encoding of the file to parse.</param>
		/// <param name="separation">Separation character used in .csv files.</param>
		public void LoadFile(Encoding encoding, char separation = ',')
        {
            switch(_fileType)
            {
                case FileType.CSV:
                    LoadCSV(encoding, separation);
                    break;
				case FileType.JSON:
					LoadJSON (encoding);
                    break;
				case FileType.XML:
					LoadXML (encoding);
					break;
            }
        }

        /// <summary>
        /// Load the config file as CSV
        /// </summary>
        /// <returns>Return the first trial</returns>
		void LoadCSV(Encoding encoding, char separation)
        {
            // List<List<string>> data = CsvFileReader.ReadAll(EzExp.Instance.inputFile, Encoding.UTF8);
			StreamReader reader = new StreamReader (_inputFilePath);
			int lineIndex = 0;

			_trials = new List<Trial> ();
			// read init file until the end
			while(!reader.EndOfStream) {
				string line = reader.ReadLine ();
				// lines starting with # are considered comments
				if (line.StartsWith ("#")) { continue; } 
				else {
					// this should be the HEADER
					if (lineIndex == 0) { 
						_parameters = line.Split (separation); 
						// remove useless spaces
						for (int i = 0; i < _parameters.Length; i++) { _parameters[i] = _parameters [i].Trim (); }
					}
					// this should be a TRIAL
					else {
						string[] values = line.Split (separation);
						// check if the trial corresponds to the user id 
						int headerCol = Array.IndexOf<string>(_parameters, _participantHeader);
						if (values [headerCol] == _participantID) {
							_trials.Add (new Trial (this, values));
						}
					}
					lineIndex++;
				}
			}

			// participant ID wasn't found
			if (_trials.Count == 0) { throw new ParticipantIDNotFoundException(_participantID+" was not found in "+_participantHeader+" section in the loaded file. ("+_inputFilePath+")"); }
        }

		void LoadJSON(Encoding encoding)
        {
			// TODO
            Log.Warning("Not supported");
        }


		void LoadXML(Encoding encoding)
		{
			// TODO
			Log.Warning("Not supported");
		}


		/// <summary>
		/// Records data about the current trial.
		/// </summary>
		/// <param name="encoding">Encoding of the file.</param>
		/// <param name="separation">Separation characters use for .csv format.</param>
		public void SaveCurrentTrial(Encoding encoding, string separation = ",")
		{
			switch (_fileType) {
			case FileType.CSV:
				SaveCurrentTrialCSV ();
				break;
			case FileType.JSON:
				SaveCurrentTrialJSON ();
				break;
			case FileType.XML:
				SaveCurrentTrialXML ();
				break;
			}
		}

		void SaveCurrentTrialCSV() 
		{ 
			// TODO 
		}

		void SaveCurrentTrialJSON() 
		{ 
			// TODO 
		}

		void SaveCurrentTrialXML() 
		{ 
			// TODO 
		}
		#endregion


		#region trials
		/// <summary>
		/// Return the next trial in the list and increase the <see cref="UnityEzExp.Experiment._currentTrialIndex"/>. This should be called before <see cref="UnityEzExp.Experiment.StartTrial"/>.
		/// </summary>
		/// <returns>The trial.</returns>
		public Trial LoadNextTrial()
		{
			_currentTrialIndex++;
			if (_trials.Count <= _currentTrialIndex) { throw new AllTrialsPerformedException (); } 
			else {
				return _trials [_currentTrialIndex];
			}
		}

		/// <summary>
		/// Gets the current trial.
		/// </summary>
		public Trial GetCurrentTrial()
		{
			if (_currentTrialIndex < 0) { throw new TrialNotLoadedException (); } 
			else if (_trials.Count <= _currentTrialIndex) { throw new AllTrialsPerformedException (); } 
			else { return _trials[_currentTrialIndex]; }
		}

		/// <summary>
		/// Starts the current trial. A trial has to be loaded before calling this function (<see cref="UnityEzExp.Experiment.LoadNextTrial"/>).
		/// </summary>
		public void StartTrial()
		{
			if (_currentTrialIndex < 0) { throw new TrialNotLoadedException (); }
			else if (_trials.Count <= _currentTrialIndex) { throw new AllTrialsPerformedException (); } 
			else {
				Trial t = _trials[_currentTrialIndex];
				t.StartTrial();
			}
		}

		/// <summary>
		/// Ends the current trial. A trial has to be started before calling this function (<see cref="UnityEzExp.Experiment.StartTrial"/>).
		/// </summary>
		public void EndTrial()
		{
			if (_currentTrialIndex < 0) { throw new TrialNotLoadedException (); }
			else if (_trials.Count <= _currentTrialIndex) { throw new AllTrialsPerformedException (); } 
			else {
				Trial t = _trials[_currentTrialIndex];
				t.EndTrial();
			}
		}

		/// <summary>
		/// Records data about the trial. A trial has to be loaded before calling this function (<see cref="UnityEzExp.Experiment.LoadNextTrial"/>).
		/// </summary>
		/// <param name="name">Name of the data.</param>
		/// <param name="value">Value of the data.</param>
		public void RecordTrialData(string name, string value)
		{
			if (_currentTrialIndex < 0) { throw new TrialNotLoadedException (); }
			else if (_trials.Count <= _currentTrialIndex) { throw new AllTrialsPerformedException (); } 
			else {
				Trial t = _trials[_currentTrialIndex];

			}
		}
		#endregion


        /*
         * TODO : 
         *         /// <summary>
        /// Removes the attribute with the given name
        /// </summary>
        /// <returns><c>true</c>, if attribute was removed, <c>false</c> otherwise.</returns>
        public bool RemoveParameter(string name)
        {
            bool res = _attributes.ContainsKey(name);
            _attributes.Remove(name);
            return res;
        }

        /// <summary>
        /// Gets the attributes names.
        /// </summary>
        /// <returns>The attributes names.</returns>
        public string[] GetParamterNames()
        {
            string[] res = new string[_attributes.Count];
            _attributes.Keys.CopyTo(res, 0);
            return res;
        }*/
    }
}
 