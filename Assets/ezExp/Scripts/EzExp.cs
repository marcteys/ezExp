using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FileWriter;
/* TODO :
 * - Timer qui va du début a la fin  
 * - Un système de "pause" automatique entre les Trials
 * */

namespace UnityEzExp
{
    //TODO : Mettre ailleurs
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        NONE
    };


    public enum SaveType
    {
        ALL, // All data stored in one signe file
        USER, // Foreach new user, we save 
        TRIAL
    };

    public class EzExp : MonoBehaviour
    {

        #region exceptions
        public class TrialNotLoadedException : Exception { };
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
                if (ezExpManager.Length == 0)
                {
                    Log.Warning("EzExp not present on the scene. Creating a new one.");
                    ezExpManager = new EzExp[1] { new GameObject("EzExp").AddComponent<EzExp>() };
                }

                // instanciate the new instance and return the value
                _instance = ezExpManager[0];
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

        public SaveType saveType = SaveType.ALL;

        public string inputFile;

        public string outputFolder;

        public bool useStartScreen; // Si on est sur la scène de base, on charge le truc de base
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


        #region Experiment
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Load the variables file to prepare the experiment </summary>
        /// <param name="filepath">File path to load data from</param>
        /// <seealso cref="SomeMethod(string)">
        /// Notice the use of the cref attribute to reference a specific method </seealso>
        public void Load(string filepath)
        {
            trials = new ArrayList();
            currentTrialIndex = -1;

            List<List<string>> data = CsvFileReader.ReadAll(filepath, Encoding.UTF8);
            string[] header = data[0].ToArray();
            for (int i = 1; i < data.Count; i++)
            {
                string[] line = data[i].ToArray();
                Trial trial = new Trial(header, line);
                Log.Debug(trial.ToString());
                trials.Add(trial);
            }
        }

        /// <summary>
        /// Launch at the very beginning of the experiment. Should load files containing exp data, prepare timers, get ready for recording
        /// </summary>
        public void StartExperiment(bool autoGetUserId = false ) { }

        /// <summary>
        /// Should be called when the experiment is over to check recording files (and display/throw some messages/events?)
        /// </summary>
        public void EndExperiment()
        {
            /* TODO 
             * - write the file
             * - End all timers
             * - clear trials
             */

        }

        #endregion


        #region User

        #endregion

        #region Trial
        /// <summary>
        /// Loads the next trial in the list loaded from the init file
        /// </summary>
        public Trial LoadNextTrial()
        {
            if (currentTrialIndex + 1 < trials.Count)
            {
                return trials[++currentTrialIndex] as Trial;
            }
            else
            {
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
            if (0 <= trialIndex && trialIndex < trials.Count)
            {
                currentTrialIndex = trialIndex;
                return (Trial)trials[currentTrialIndex];
            }
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }

        public void InitTrial()
        {

        }


        /// <summary>
        /// Starts the loaded trial.
        /// </summary>
        public void StartTrial()
        {
            if (currentTrialIndex == -1)
            {
                throw new TrialNotLoadedException();
            }
            else
            {
                // TODO start timer associated to that Trial
                Trial ct = (Trial)trials[currentTrialIndex];
                ct.StartTrial();
            }
        }

        /// <summary>
        /// Ends the loaded trial.
        /// </summary>
        public Trial EndTrial()
        {
            if (currentTrialIndex == -1)
            {
                throw new TrialNotLoadedException();
            }
            else
            {
                // TODO stop timer associated to the Trial and save results
                Trial ct = (Trial)trials[currentTrialIndex];
                ct.EndTrial();
                // TODO record results

                return ct;
            }
        }

        /// <summary>
        /// Return the current Trial
        /// </summary>
        /// <returns></returns>
        public Trial GetCurrentTrial()
        {
            return trials[currentTrialIndex] as Trial;
        }


        #endregion

        // If the input csv is not made, build one custom
        #region Simple 

        /// <summary>
        /// Define all parameters (columns) we want to use
        /// </summary>
        /// <param name="values">Udefined numer of values (columns)</param>
        public void SetParameters(params string[] values)
        {
            for(int i=0; i< values.Length;i++)
            {
                Log.Debug("The value " + values[i] + " should be added as a new column");
            }
        }

        /// <summary>
        /// Set a value
        /// </summary>
        /// <param name="paramName">Param name (in column)</param>
        /// <param name="value">Value to update</param>
        public void SetValue(string paramName, object value)
        {
            Debug.Log("Should cast value " + value + " to a string, but with a reformating of the comas ',' and add it in the column 'paramName'");
            //TODO what happend in case of override ?
        }
        #endregion

        #region Timers 
        /// <summary>
        /// This should start automatically
        /// </summary>
        /// <param name="timerName"></param>
        /// <param name="defaultFormat"></param>
        public void StartTimer(string timerName, string defaultFormat = "")
        {
            Trial currentTrial = GetCurrentTrial();
            if (currentTrial != null)
            {

            } else
            {

            }
        }

        public string EndTimer(string timerName)
        {
            Trial currentTrial = GetCurrentTrial();

            string timer = currentTrial.EndTimer(timerName);
            return timer;
        }


        #endregion
    }
}