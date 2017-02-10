using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEzExp;
using FileWriter;

namespace UnityEzExp
{
	#region exceptions
	/// <summary>
	/// Exception triggered if the participant ID was not found when loading the data.
	/// </summary>
	public class ParticipantIDNotFoundException: Exception { public ParticipantIDNotFoundException(string msg): base(msg){} }
	#endregion

	/// <summary>
	/// The class <see cref="UnityEzExp.Experiment"/> is used to Load and Save data about the experiment configuration. At the beginning, it will load data from a .csv, .xml or .json file 
	/// and will save the results in an output file 
	/// </summary>
    public class Experiment
    {
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

		// name of the column containing participant IDs
		string _participantHeader;
		// ID of the current participant
		string _participantID;

        static int currentTrialIndex = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityEzExp.Experiment"/> class.
		/// </summary>
		/// <param name="inputFilePath">Input file path to load data from.</param>
		/// <param name="participantHeader">Name of the column containing participants IDs.</param>
		/// <param name="participantID">ID of the participant for whom we want to load experiment data.</param>
		Experiment(string inputFilePath, string participantHeader, string participantID)
        {
			_inputFilePath = _inputFilePath;
			_participantHeader = participantHeader;
			_participantID = participantID;
        }

		/// <summary>
		/// Load data from a file, with the provided format. Possibility to specify the separation character in .csv files.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <param name="fileType">Format of the file to parse.</param>
		/// <param name="encoding">Encoding of the file to parse.</param>
		/// <param name="separation">Separation character used in .csv files.</param>
		void LoadFile(FileType fileType = FileType.CSV, Encoding encoding = Encoding.UTF8, string separation = ",")
        {
            switch(fileType)
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
		void LoadCSV(Encoding encoding, string separation)
        {
            // List<List<string>> data = CsvFileReader.ReadAll(EzExp.Instance.inputFile, Encoding.UTF8);
			StreamReader reader = new StreamReader (_inputFilePath);
			int lineIndex = 0;
			while(reader.EndOfStream) {
				string line = reader.ReadLine ();
				if (line.StartsWith ("#")) {
					continue;
				} else {
					// this should be the HEADER
					if (lineIndex == 0) { _parameters = line.Split (separation); }
					// this should be a TRIAL
					else {
						string[] values = line.Split (separation);
						int headerCol = Array.Find<string>(_parameters, _participantHeader);
						if (values [headerCol] == _participantID) {
							_trials.Add (new Trial ());
						}
					}
					lineIndex++;
				}
			}
            Trial trial = null;
            _trials = new ArrayList();
            currentTrialIndex = -1;

            SetParamters(data[0]);

            for (int i = 1; i < data.Count; i++)
            {
                trial = new Trial(this, data[i]);
                _trials.Add(trial);
            }

            return _trials[0] as Trial;
        }

		void LoadJSON(Encoding encoding)
        {
			// TODO
            Log.Warning("Not supported");
            return null;
        }


		void LoadXML(Encoding encoding)
		{
			// TODO
			Log.Warning("Not supported");
		}

        public void SetTrial(params Trial[] trials)
        {
            foreach (Trial parameter in trials)
                _trials.Add(parameter);
        }

        void SetTrials(List<Trial> trials)
        {

        }

        /// <summary>
        /// Add a list of undefined number of parameters manually
        /// </summary>
        /// <param name="parameters">Undefined number of parameters</param>
        public void AddParameter(params string[] parameters)
        {
            foreach (string parameter in parameters)
                _parameters.Add(parameter);
        }

        /// <summary>
        /// Set the parameters from list
        /// </summary>
        /// <param name="parameters">List of parameters</param>
        void SetParamters(List<string> parameters)
        {
            _parameters = parameters;
        }


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
 