using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake ()
    {
		// Open filereader
	}

	/// <summary>
	/// Load the variables file to prepare the experiment </summary>
	/// <param name="test">Currently a testing string that will be logged</param>
	/// <seealso cref="SomeMethod(string)">
	/// Notice the use of the cref attribute to reference a specific method </seealso>
	public void Load (string test)
    {
        Log.Warning(test);
	}
}
