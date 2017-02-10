using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEzExp
{
    /// <summary>
    /// Contains information about a trial. All class attributes have to be added at runtime to keep an abstract level.
    /// </summary>
    public class Trial
    {

        #region Exceptions
        /// <summary>
        /// Exception thrown when attributes and values array have different sizes.
        /// </summary>
        public class DifferentSizeException : Exception { public DifferentSizeException(string msg) : base(msg) { } };
        /// <summary>
        /// Exception thrown on timer duration estimation and start time is greater than ending time
        /// </summary>
        public class TimerNotEndedException : Exception { public TimerNotEndedException(string msg) : base(msg) { } };
        /// <summary>
        /// Exception thrown when trying to end a trial that was not started
        /// </summary>
        public class TrialStateException : Exception { public TrialStateException(string msg) : base(msg) { } };
        #endregion

        #region Attributes
        /// <summary>
        /// Array of values associated to parameters referenced in <see cref="UnityEzExp.Experiment"/> .
        /// </summary>
		string[] _parametersData;

		/// <summary>
		/// Dictionary used to save data about the trial dynamically.
		/// </summary>
		Dictionary<string, string> _savedData;

        /// <summary>
        /// Dictionary containing pairs representing starting and ending time of timers named as the dictionary key.
        /// This dictionary will always at least contain one main timer for the trial.
        /// </summary>
        Dictionary<string, EzTimer> _timers = new Dictionary<string, EzTimer>();

        public enum TrialState { NotStarted, Started, Ended };
        TrialState _trialState = TrialState.NotStarted;

        Experiment _parentExperiment = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor that only adds a main timer to the timers dictionary.
        /// </summary>
        public Trial(Experiment experiment, string[] parametersData)
        {
            _parentExperiment = experiment;
            _parametersData = parametersData;
			// FIXME timer should be started on StartTrial()
            // StartTimer("main");
        }

        /*
        /// <summary>
        /// Create a new <see cref="Trial"/> by copying the given attributes dictionary (name -> value)
        /// </summary>
        /// <param name="attributes">Dictionary of attributes.</param>
        public Trial(Dictionary<string, string> attributes) : this()
        {
            foreach (KeyValuePair<string, string> pair in attributes)
            {
                _attributes.Add(pair.Key, pair.Value);
            }
        }
        */
        #endregion

        #region Functions
        /// <summary>
        /// Add a timer to the dictionary with a given name that will be used to access it in the future.
        /// </summary>
        /// <param name="name">Name of the timer to add.</param>
        public void StartTimer(string name)
        {
            if (name == "main" && _timers.Count > 0 ) {
                Log.Error("The timer name'main' is reserved");
                return;
            }

            if (GetExistingTimer(name) != null)
            {
                _timers.Remove(name);
                Log.Warning("A timer with the name <color=#f500f5>" + name + "</color> already exist. Remocing");
            }
            _timers.Add(name, new EzTimer(GetExistingTimer("name")._originalStartTime));
        }

        /// <summary>
        /// Ends the timer with the given name
        /// </summary>
        /// <param name="name">Name of the timer.</param>
        public string EndTimer(string name, TimeFormat format = TimeFormat.MINUTES)
        {
            EzTimer timer = GetExistingTimer(name);
            if (timer != null)
            {
                string time = timer.GetTime(format);
                RemoveTimer(name);
                return time;
            }
            else
            {
                return "ERROR // TODO";
            }

        }

        /// <summary>
        /// Gets the duration of the timer.
        /// </summary>
        /// <param name="name">Name of the timer.</param>
        public float GetTimerDuration(string name)
        {
            EzTimer timer = GetExistingTimer(name);
            if (timer != null)
            {
                return timer.GetTimeSeconds();
            } else
            {
                return 0; // todo : redo
            }
        }

        /// <summary>
        /// Used to 
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        EzTimer GetExistingTimer(string timerName)
        {
            EzTimer existingTimer = null;
            if (!_timers.ContainsKey(timerName)) { throw new KeyNotFoundException(); }
            _timers.TryGetValue(timerName, out existingTimer);
            return existingTimer;
        }

        /// <summary>
        /// Used to 
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        bool RemoveTimer(string timerName)
        {
            _timers.Remove(timerName);
            return true;
        }

        /// <summary>
        /// Gets the timers names.
        /// </summary>
        /// <returns>The timers names.</returns>
        public string[] GetTimersNames()
        {
            string[] res = new string[_timers.Count];
            _timers.Keys.CopyTo(res, 0);
            return res;
        }

        /// <summary>
        /// Starts this trial (starts all timers by default and set state to started).
        /// </summary>
        public void StartTrial()
        {
            if (_trialState != TrialState.NotStarted) { throw new TrialStateException("The trial has already been started"); }
            // StartAllTimers();
            _trialState = TrialState.Started;
        }

		/// <summary>
		/// Ends the trial.
		/// </summary>
        public void EndTrial()
        {
            if (_trialState != TrialState.Started) { throw new TrialStateException("The trial was not started yet"); }
             EndTimer("main");
            _trialState = TrialState.Ended;
        }

		/// <summary>
		/// Saves data about the trial in the Dictionary <see cref="UnityEzExp.Trial._savedData"/>.
		/// </summary>
		/// <param name="name">Name of the data.</param>
		/// <param name="value">Value of the data.</param>
		/// <returns>Whether a new entry was added to the dictionary</returns>
		public bool SaveTrialData(string name, string value)
		{
			bool added = _savedData.ContainsKey(name);
			_savedData[name] = value;
			return added;
		}

        
        /// <summary>
        /// Returns a string concatenating all attributes values.
        /// </summary>
        /// <param name="showTimers">Used to concatenate timers values at the end of the string.</param>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Trial"/>.</returns>
        public string ToString(bool showData = true)
        {
            // TODO should take output file format into account

            string res = "";
            foreach (KeyValuePair<string, string> pair in _attributes)
            {
                res += pair.Key + "=" + pair.Value + ";";
            }

			if(showData) {
				foreach(KeyValuePair<string, string> p in _savedData) {
					res += p.Key+"="+p.Value+";";
				}
			}

            return res.Substring(0, res.Length - 1);
        }
        #endregion
    }
}