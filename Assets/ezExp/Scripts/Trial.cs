using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains information about a trial. All class attributes have to be added at runtime to keep an abstract level.
/// </summary>
public class Trial {

	#region exceptions
	/// <summary>
	/// Exception thrown when attributes and values array have different sizes.
	/// </summary>
	public class DifferentSizeException: Exception { public DifferentSizeException(string msg): base(msg){} };
	/// <summary>
	/// Exception thrown on timer duration estimation and start time is greater than ending time
	/// </summary>
	public class TimerNotEndedException: Exception { public TimerNotEndedException(string msg): base(msg){} };
	#endregion

	#region attributes
	/// <summary>
	/// Dictionary containing attributes binding their names to their values.
	/// </summary>
	Dictionary<string, string> _attributes;

	/// <summary>
	/// Dictionary containing pairs representing starting and ending time of timers named as the dictionary key.
	/// This dictionary will always at least contain one main timer for the trial.
	/// </summary>
	Dictionary<string, KeyValuePair<float, float>> _timers;
	#endregion

	#region constructors
	/// <summary>
	/// Default constructor that only adds a main timer to the timers dictionary.
	/// </summary>
	public Trial() 
	{
		_attributes = new Dictionary<string, string> ();
		_timers = new Dictionary<string, KeyValuePair<float, float>> ();
		AddTimer ("main");
	}

	/// <summary>
	/// Create a new trial with the given attributes
	/// </summary>
	/// <param name="attributes">Attributes names</param>
	public Trial(string[] attributes): this()
	{
		_attributes = new Dictionary<string, string> ();
		foreach (string attr in attributes) {
			_attributes.Add (attr, "");
		}
	}


	/// <summary>
	/// Create a new trial with the given attributes and values associated. 
	/// </summary>
	/// <param name="attributes">Attributes names</param>
	/// <param name="values">Attributes values</param>
	public Trial(string[] attributes, string[] values): this()
	{
		if (attributes.Length != values.Length) { throw new DifferentSizeException ("Attributes ("+attributes.Length+") and Values ("+values.Length+") array don't match in length");}
		for (int i = 0; i < attributes.Length; i++) {
			_attributes.Add (attributes[i], values[i]);
		}
	}

	/// <summary>
	/// Create a new trial with the given attributes and values associated. Also create timers that would be used for this trial.
	/// </summary>
	/// <param name="attributes">Names of the attributes.</param>
	/// <param name="values">Values of these attributes.</param>
	/// <param name="timers">Names of the timers used for this Trial.</param>
	public Trial (string[] attributes, string[] values, string[] timers): this(attributes, values)
	{
		foreach (string name in timers) {
			AddTimer (name);
		}
	}

	/// <summary>
	/// Create a new trial by copying the given attributes dictionary (name -> value)
	/// </summary>
	/// <param name="attributes">Dictionary of attributes.</param>
	public Trial(Dictionary<string, string> attributes): this() 
	{
		foreach (KeyValuePair<string, string> pair in attributes) {
			_attributes.Add(pair.Key, pair.Value);
		}
	}

	public Trial(Trial toCopy): this()
	{
		foreach(KeyValuePair<string, string> pair in toCopy._attributes) {
			_attributes.Add (pair.Key, pair.Value);
		}

		foreach(KeyValuePair<string, KeyValuePair<float, float>> pair in toCopy._timers) {
			_timers.Add (pair.Key, pair.Value);
		}
	}
	#endregion

	#region functions
	/// <summary>
	/// Add a new attribute to the dictionary.
	/// </summary>
	/// <returns>
	/// True if the attribute is created in the dictionary, False if it has be overriden
	/// </returns>
	/// <param name="name">Name of the attribute to add</param>
	public bool AddAttribute(string name) { return AddAttribute (name, ""); }

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
		bool res = _attributes.ContainsKey (name);
		if (res) { _attributes.Remove (name); }
		_attributes.Add (name, value);
		return res;
	}

	/// <summary>
	/// Removes the attribute with the given name
	/// </summary>
	/// <returns><c>true</c>, if attribute was removed, <c>false</c> otherwise.</returns>
	public bool RemoveAttribute(string name)
	{
		bool res = _attributes.ContainsKey (name);
		_attributes.Remove (name);
		return res;
	}


	/// <summary>
	/// Add a timer to the dictionary with a given name that will be used to access it in the future.
	/// </summary>
	/// <param name="name">Name of the timer to add.</param>
	public bool AddTimer(string name) 
	{ 
		bool res = _timers.ContainsKey (name);
		if (res) { _timers.Remove (name); }
		_timers.Add (name, new KeyValuePair<float, float>(0f, 0f));
		return res;
	}


	/// <summary>
	/// Starts the timer with the given name
	/// </summary>
	/// <param name="name">Name of the timer.</param>
	public void StartTimer(string name)
	{
		if (!_timers.ContainsKey (name)) { throw new KeyNotFoundException (); }
		_timers[name] = new KeyValuePair<float, float>(Time.time, 0f);
	}

	/// <summary>
	/// Ends the timer with the given name
	/// </summary>
	/// <param name="name">Name of the timer.</param>
	public void EndTimer(string name)
	{
		if (!_timers.ContainsKey (name)) { throw new KeyNotFoundException (); }
		_timers[name] = new KeyValuePair<float, float>(_timers[name].Key, Time.time);
	}

	/// <summary>
	/// Gets the duration of the timer.
	/// </summary>
	/// <param name="name">Name of the timer.</param>
	public float GetTimerDuration(string name)
	{
		if (!_timers.ContainsKey (name)) { throw new KeyNotFoundException (); }
		KeyValuePair<float, float> times = _timers [name];
		if (times.Value < times.Key) { throw new TimerNotEndedException ("Timer "+name+" started at ("+times.Key+") and never finished ("+times.Value+")"); }
		return times.Value - times.Key;
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
		foreach (KeyValuePair<string,string> pair in _attributes) {
			res += pair.Key + "=" + pair.Value+";";
		}

		if (showTimers) {
			foreach (KeyValuePair<string, KeyValuePair<float, float>> pair in _timers) {
				KeyValuePair<float, float> times = pair.Value;
				res += pair.Key+"="+times.Key+"-"+times.Value+";";
			}
		}

		return res.Substring (0, res.Length-1);
	}
	#endregion
}
