using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEzExp;
using FileWriter;
// TODO : use emum ExperimentState 
//TODO : faire une bool "reprnedre la ou on était" qui reprend le dernier utilisateur, etc ? 

namespace UnityEzExp
{
    public class Experiment
    {
        /// <summary>
        /// Parameters are the "header" of the file
        /// </summary>
        public List<string> _parameters = new List<string>();

      //  List<Trial> _trials = new List<Trial>();
        public ArrayList _trials;

        static int currentTrialIndex = -1;

        Experiment(string[] parameters = null)
        {

        }

        void LoadFile(string filePath, FileType fileType = FileType.CSV)
        {
            switch(fileType)
            {
                case FileType.CSV:
                    LoadCSV();
                    break;
                case FileType.JSON:
                    break;
            }
        }

        /// <summary>
        /// Load the config file as CSV
        /// </summary>
        /// <returns>Return the first trial</returns>
        Trial LoadCSV()
        {
            List<List<string>> data = CsvFileReader.ReadAll(EzExp.Instance.inputFile, Encoding.UTF8);
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

        Trial LoadJSON()
        {
            Log.Warning("Not supported");
            return null;
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
 