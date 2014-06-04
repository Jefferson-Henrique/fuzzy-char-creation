using UnityEngine;
using System.Collections;

public class SliderFuzzy : MonoBehaviour {

	public UILabel attrLabel;
	public UILabel attrValueLabel;

	[HideInInspector]
	public AdjustChar adjustChar;

	private UISlider slider;

	// Use this for initialization
	void Start () {
		slider = this.GetComponent<UISlider>();
		UpdateValue();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetAttr(string attr) {
		this.attrLabel.text = attr;
	}

	public void UpdateValue() {
		float value = (Mathf.Round (slider.value * 100)) / 100;
		string valueString = value+"";
		if (value == 0 || value == 1) {
			valueString += ".0";		
		}

		this.attrValueLabel.text = valueString;
		adjustChar.UpdateValue(this.attrLabel.text, value);
	}

}