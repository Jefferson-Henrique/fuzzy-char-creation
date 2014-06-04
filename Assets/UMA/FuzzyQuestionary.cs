using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class FuzzyQuestionary {

	public List<FuzzyResponse> responses = new List<FuzzyResponse>();

	public string ToJSON() {
		string result = "{\"responses\":[";
		foreach (FuzzyResponse fr in responses) {
			result += "{";
			result += "\"inputs\": { ";
			foreach (string key in fr.inputs.Keys) {
				result += "\""+key+"\": \"" + fr.inputs[key] + "\",";
			}
			result = result.Remove(result.Length - 1, 1);
			result += "},";

			result += "\"outputs\": { ";
			foreach (string key in fr.outputs.Keys) {
				result += "\""+key+"\": " + fr.outputs[key] + ",";
			}
			result = result.Remove(result.Length - 1, 1);
			result += "}";

			result += "},";
		}
		result = result.Remove(result.Length - 1, 1);
		result += "]}";

		return result;
	}

}