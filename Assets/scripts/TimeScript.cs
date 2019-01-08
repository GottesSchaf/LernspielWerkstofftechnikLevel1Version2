using UnityEngine;

public class TimeScript : MonoBehaviour {

	public void DoFastforward()
    {
        Time.timeScale = 3.0f;
    }

    public void DoNormaltime()
    {
        Time.timeScale = 1.0f;
    }
}
