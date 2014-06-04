using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using UMA;

public class AdjustChar : MonoBehaviour {

	public UMAData umaData;
	private UMADnaHumanoid umaDNA;
	
	public void Update() {
		if (umaData == null) {
			umaData = this.GetComponent<UMADynamicAvatar> ().umaData;
			umaDNA = umaData.GetDna<UMADnaHumanoid> ();		
		}
	}

	public void UpdateValue(string attr, float value) {
		if (attr.Equals ("belly")) {
			umaDNA.belly = value;
			umaDNA.waist = value;		
		} else if (attr.Equals ("breasts")) {
			umaDNA.upperWeight = value;
			umaDNA.upperMuscle = value;
			umaDNA.breastSize = value;
		} else if (attr.Equals ("legs")) {
			umaDNA.lowerMuscle = value;
			umaDNA.lowerWeight = value;

		} else if (attr.Equals ("legs_height")) {
			umaDNA.legsSize = 0.4f + value * (0.6f - 0.4f);

		} else if (attr.Equals ("arm")) {
			umaDNA.armWidth = value;
			umaDNA.forearmWidth = value;
			umaDNA.armLength = 0.55f + value * (0.66f - 0.55f);
			umaDNA.forearmLength = 0.4f + value * (0.5f - 0.4f);
		}
		
		umaData.isShapeDirty = true;
		umaData.Dirty ();
	}

}