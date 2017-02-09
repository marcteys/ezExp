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

        #region exceptions
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

        #region attributes
        /// <summary>
        /// Dictionary containing attributes binding their names to their values.
        /// </summary>
        Dictionary<string, string> _attributes = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary containing pairs representing starting and ending time of timers named as the dictionary key.
        /// This dictionary will always at least contain one main timer for the trial.
        /// </summary>
        Dictionary<string, EzTimer> _timers = new Dictionary<string, EzTimer>();

        public enum TrialState { NotStarted, Started, Ended };
        TrialState _trialState = TrialState.NotStarted;
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor that only adds a main timer to the timers dictionary.
        /// </summary>
        public Trial(string[] attributes = null, string[] values = null)
        {
            StartTimer("main");
        }

        /*
        /// <summary>
        /// Create a new <see cref="Trial"/> with the given attributes
        /// </summary>
        /// <param name="attributes">Attributes names</param>
        public Trial(string[] attributes) : this()
        {
            _attributes = new Dictionary<string, string>();
            foreach (string attr in attributes)
            {
                _attributes.Add(attr, "");
            }
        }

        /// <summary>
        /// Create a new <see cref="Trial"/> with the given attributes and values associated. 
        /// </summary>
        /// <param name="attributes">Attributes names</param>
        /// <param name="values">Attributes values</param>
        public Trial(string[] attributes, string[] values) : this()
        {
            if (attributes.Length != values.Length) { throw new DifferentSizeException("Attributes (" + attributes.Length + ") and Values (" + values.Length + ") array don't match in length"); }
            for (int i = 0; i < attributes.Length; i++)
            {
                _attributes.Add(attributes[i], values[i]);
            }
        }

        /// <summary>
        /// Create a new <see cref="Trial"/> with the given attributes and values associated. Also create timers that would be used for this trial.
        /// </summary>
        /// <param name="attributes">Names of the attributes.</param>
        /// <param name="values">Values of these attributes.</param>
        /// <param name="timers">Names of the timers used for this Trial.</param>
        public Trial(string[] attributes, string[] values, string[] timers) : this(attributes, values)
        {
            /*
            foreach (string name in timers)
            {
                AddTimer(name);
            }*/
       /* }
    */
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Trial"/> class based on another instance of it.
        /// </summary>
        /// <param name="toCopy">Other <see cref="Trial"/> instance to copy.</param>
        /*
        public Trial(Trial toCopy) : this()
        {
            foreach (KeyValuePair<string, string> pair in toCopy._attributes)
            {
                _attributes.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<string, float[]> pair in toCopy._timers)
            {
                _timers.Add(pair.Key, pair.Value);
            }
        }
        */
        #endregion

        #region functions
        /// <summary>
        /// Add a new attribute to the dictionary.
        /// </summary>
        /// <returns>
        /// True if the attribute is created in the dictionary, False if it has be overriden
        /// </returns>
        /// <param name="name">Name of the attribute to add</param>
        public bool AddAttribute(string name) { return AddAttribute(name, ""); }

        /// <summary>
        /// Add a new attribute to the dictionary with the given value
        /// </summary>
        /// <returns>
        /// True if the attribute is created in the dictionary, False if it has be overriden
        /// </returns>
        /// <param name="name">Name of the attribute to add</param>
        /// <param name="value">Value of this attribute</param>
        public bool AddAttribute(string name, string value)
        {
            bool res = _attributes.ContainsKey(name);
            if (res) { _attributes.Remove(name); }
            _attributes.Add(name, value);
            return res;
        }

        /// <summary>
        /// Removes the attribute with the given name
        /// </summary>
        /// <returns><c>true</c>, if attribute was removed, <c>false</c> otherwise.</returns>
        public bool RemoveAttribute(string name)
        {
            bool res = _attributes.ContainsKey(name);
            _attributes.Remove(name);
            return res;
        }

        /// <summary>
        /// Gets the attributes names.
        /// </summary>
        /// <returns>The attributes names.</returns>
        public string[] GetAttributesNames()
        {
            string[] res = new string[_attributes.Count];
            _attributes.Keys.CopyTo(res, 0);
            return res;
        }

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
            _timers.Add(name, new EzTimer(GetExistingTimer("name").originalStartTime));
        }

        /// <summary>
        /// Ends the timer with the given name
        /// </summary>
        /// <param name="name">Name of the timer.</param>
        public string EndTimer(string name)
        {
            if (GetExistingTimer(name) != null)
            {
                return "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the duration of the timer.
        /// </summary>
        /// <param name="name">Name of the timer.</param>
        public float GetTimerDuration(string name)
        {
            /*
            if (!_timers.ContainsKey(name)) { throw new KeyNotFoundException(); }
            float[] times = _timers[name];
            if (times[0] == times[1]) { throw new TimerNotEndedException("Timer " + name + " started at (" + times[0] + ") and never finished"); }
            return times[1] - times[0];*/
            return 0;
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
        /// Start this trial (starts all timers by default and set state to started)
        /// </summary>
        public void StartTrial()
        {
            if (_trialState != TrialState.NotStarted) { throw new TrialStateException("The trial has already been started"); }
           // StartAllTimers();
            _trialState = TrialState.Started;
        }

        public void EndTrial()
        {
            if (_trialState != TrialState.Started) { throw new TrialStateException("The trial was not started yet"); }
            EndTimer("main");
            _trialState = TrialState.Ended;
        }

        /// <summary>
        /// Returns a string concatenating all attributes values.
        /// </summary>
        /// <param name="showTimers">Used to concatenate timers values at the end of the string.</param>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Trial"/>.</returns>
        public string ToString(bool showTimers = false)
        {
            // TODO should take output file format into account

            string res = "";
            foreach (KeyValuePair<string, string> pair in _attributes)
            {
                res += pair.Key + "=" + pair.Value + ";";
            }

            if (showTimers)
            {
                // TODO : I do'nt understand that
                /*
                foreach (KeyValuePair<string, EzTimer> pair in _timers)
                {
                    float[] times = pair.Value;
                    res += pair.Key + "=" + GetTimerDuration(pair.Key);
                }*/
            }

            return res.Substring(0, res.Length - 1);
        }
        #endregion
    }
}