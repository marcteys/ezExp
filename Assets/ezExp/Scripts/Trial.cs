using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains information about a trial. All class attributes have to be added at runtime to keep an abstract level.
/// </summary>
public class Trial {

	#region exceptions
	public class DifferentSizeException: Exception { public DifferentSizeException(string msg): base(msg){} };
	#endregion

	#region attributes
	/// <summary>
	/// 
	/// </summary>
	private Dictionary<string, string> _attributes;
	#endregion

	#region constructors
	/// <summary>
	/// Create a new trial with the given attributes
	/// </summary>
	/// <param name="attributes">Attributes names</param>
	public Trial(string[] attributes) 
	{
		foreach (string attr in attributes) {
			_attributes.Add (attr, "");
		}
	}


	/// <summary>
	/// Create a new trial with the given attributes and values associated. 
	/// </summary>
	/// <param name="attributes">Attributes names</param>
	/// <param name="values">Attributes values</param>
	public Trial(string[] attributes, string[] values) 
	{
		if (attributes.Length != values.Length) { throw new DifferentSizeException ("Attributes ("+attributes.Length+") and Values ("+values.Length+") array don't match in length");}
		for (int i = 0; i < attributes.Length; i++) {
			_attributes.Add (attributes[i], values[i]);
		}
	}

	/// <summary>
	/// Create a new trial by copying the given attributes dictionary (name -> value)
	/// </summary>
	/// <param name="attributes">Dictionary of attributes.</param>
	public Trial(Dictionary<string, string> attributes) 
	{
		foreach (KeyValuePair<string, string> pair in attributes) {
			_attributes.Add(pair.Key, pair.Value);
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
	/// Returns a string concatenating all attributes values.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Trial"/>.</returns>
	public string ToString()
	{
		// should take output file into account?
		string res = "";
		foreach (KeyValuePair<string,string> pair in _attributes) {
			res += pair.Key + "=" + pair.Value;
		}
		return res.Substring (0, res.Length-1);
	}
	#endregion
}
