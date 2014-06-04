using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using UMA;

public class CallFuzzySystem : MonoBehaviour {

	private TcpClient client;
	private StreamWriter sw;
	private StreamReader sr;
	
	private float lastStrength = 0;
	private float currentStrength = 0;
	
	private float lastDodge = 0;
	private float currentDodge = 0;

	private UMAData umaData;
	private UMADnaHumanoid umaDNA;

	// Update is called once per frame
	void Start () {
		client = new TcpClient("localhost", 2000);

		Stream s = client.GetStream ();
		sw = new StreamWriter (s);
		sw.AutoFlush = true;

		sr = new StreamReader(s);
	}

	public void Update() {
		if (umaData == null) {
			umaData = this.GetComponent<UMADynamicAvatar> ().umaData;
			umaDNA = umaData.GetDna<UMADnaHumanoid> ();		
		}
	}

	void OnGUI() {
		bool update = false;

		currentStrength = GUI.HorizontalSlider(new Rect(10, 10, 200, 30), currentStrength, 0, 10);
		if (lastStrength != currentStrength) {
			lastStrength = currentStrength;
			update = true;
		}

		GUI.Label (new Rect(230, 10, 100, 30), ""+currentStrength);

		currentDodge = GUI.HorizontalSlider(new Rect(10, 50, 200, 30), currentDodge, 0, 10);
		if (lastDodge != currentDodge) {
			lastDodge = currentDodge;		
			update = true;
		}

		GUI.Label (new Rect(230, 50, 100, 30), ""+currentDodge);

		if (update) {
			callFuzzy();
		}
	}

	public void callFuzzy() {
		sw.WriteLine ("fuzzy " + lastStrength + " " + lastDodge);

		string[] stringSplitted = sr.ReadLine().Split(' ');
		float valueBelly = float.Parse (stringSplitted [4]);
		float valueBreasts = float.Parse (stringSplitted [3]);
		float valueLegs = float.Parse (stringSplitted [2]);
		float valueLegsHeight = float.Parse (stringSplitted [1]);
		float valueArm = float.Parse (stringSplitted [0]);

		umaDNA.belly = valueBelly;
		umaDNA.waist = valueBelly;
		umaDNA.upperWeight = valueBreasts;
		umaDNA.upperMuscle = valueBreasts;
		umaDNA.breastSize = valueBreasts;
		umaDNA.lowerMuscle = valueLegs;
		umaDNA.lowerWeight = valueLegs;
		umaDNA.legsSize = 0.4f + valueLegsHeight * (0.6f - 0.4f);
		umaDNA.armWidth = valueArm;
		umaDNA.forearmWidth = valueArm;
		umaDNA.armLength = 0.55f + valueArm * (0.66f - 0.55f);
		umaDNA.forearmLength = 0.4f + valueArm * (0.5f - 0.4f);
		
		umaData.isShapeDirty = true;
		umaData.Dirty ();
	}
}