using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using UMA;

public class FuzzyGenerator : MonoBehaviour {

	public string[] termLinguistics;

	public string[] attributesInput;

	public string[] attributesOutput;

	public GameObject nguiParent;

	public GameObject sliderPrefab;

	public UILabel questionLabel;

	public AdjustChar adjustChar;

	public GameObject nextQuestionBtn;

	private int[] questionState;
	private int questionStateIndex = 0;

	private bool created = false;

	private Dictionary<string, UISlider> sliders = new Dictionary<string, UISlider>();

	private FuzzyQuestionary fuzzyQuestionary;

	// Update is called once per frame
	void Start () {
		int numAttrs = attributesInput.Length;
		int numTerms = termLinguistics.Length;

		questionState = new int[numAttrs];

		fuzzyQuestionary = new FuzzyQuestionary();
		ShowQuestion ();
	}

	public void ShowQuestion() {
		string text = "Model a char where:";

		for (int index = 0; index < attributesInput.Length; index++) {
			text += "\n{0} is '{1}'";
			text = string.Format(text, attributesInput[index], termLinguistics[questionState[index]]);
		}

		questionLabel.text = text;
	}

	public void NextQuestion() {
		FuzzyResponse fr = new FuzzyResponse();
		for (int index = 0; index < attributesInput.Length; index++) {
			fr.inputs[attributesInput[index]] = termLinguistics[questionState[index]];
		}
		for (int index = 0; index < attributesOutput.Length; index++) {
			string attr = attributesOutput[index];
			fr.outputs[attr] = sliders[attr].value;
		}
	
		fuzzyQuestionary.responses.Add(fr);

		questionStateIndex++;

		if (questionStateIndex > (Mathf.Pow(termLinguistics.Length, attributesInput.Length) - 1)) {
			FinishQuestions();
			return;
		}

		int remainValue = questionStateIndex;
		for (int index = 0; index < attributesInput.Length; index++) {
			int pos = attributesInput.Length - index - 1;

			int valuePowered = (int)(Mathf.Pow(termLinguistics.Length, pos));
			int currentValue = remainValue / valuePowered;
			remainValue -= currentValue * valuePowered;
			
			questionState[index] = currentValue;
		}

		ShowQuestion();
	}

	public void FinishQuestions() {
		nextQuestionBtn.SetActive(false);

		UnityEngine.Debug.Log(fuzzyQuestionary.ToJSON());
	}

	public void Update() {
	}

	public void LateUpdate() {
		if (!created) {
			float gapIndex = Screen.height * 0.4f;
			foreach (string attr in attributesOutput) {
					SliderFuzzy sliderFuzzy = NGUITools.AddChild (nguiParent, sliderPrefab).GetComponent<SliderFuzzy> ();
					sliderFuzzy.SetAttr (attr);
					sliderFuzzy.adjustChar = adjustChar;

					sliders[attr] = sliderFuzzy.GetComponent<UISlider>();

					Vector3 boxSize = ((BoxCollider)sliderFuzzy.gameObject.collider).size;

					sliderFuzzy.transform.localPosition = new Vector3 (-(boxSize.x + 50), -gapIndex);

					gapIndex += boxSize.y + 10;
			}

			created = true;
		}
	}

}