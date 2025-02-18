#if UNITY_EDITOR

using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;
using NodeCanvas.DialogueTrees;

namespace NodeCanvas.Editor {

	///<summary>A drawer for dialogue tree statements</summary>
	public class StatementDrawer : ObjectDrawer<Statement> {
		// reference to the selected index
		int selectedIndex;
		public override Statement OnGUI(GUIContent content, Statement instance) {
			selectedIndex = instance.portraitIndex;
			if (instance == null) {
				instance = new Statement("...");
			}

			// text
			instance.text = UnityEditor.EditorGUILayout.TextArea(instance.text, Styles.wrapTextArea, GUILayout.Height(100));

			// Portrait Collection
			if (instance.portraitCollection != null && instance.portraitCollection.Length > 0) {
				// Grasp the current portrait index
				string[] options = new string[instance.portraitCollection.Length];
				for (int i = 0; i < options.Length; i++) {
					options[i] = $"Portrait {i}";
				}

				// the toolbar
				selectedIndex = GUILayout.Toolbar(selectedIndex, options);

				// show the selected portrait
				if (instance.portraitCollection[selectedIndex] != null) {
					GUILayout.Label(new GUIContent(instance.portraitCollection[selectedIndex]), GUILayout.Width(100), GUILayout.Height(100));
					instance.portraitIndex = selectedIndex;
				}
			}
			else {
				// error msg
				EditorGUILayout.HelpBox("No portraits available in the collection.", MessageType.Warning);
			}

			// audio
			instance.audio = UnityEditor.EditorGUILayout.ObjectField("Audio File", instance.audio, typeof(AudioClip), false) as AudioClip;

			// meta
			instance.meta = UnityEditor.EditorGUILayout.TextField("Metadata", instance.meta);

			return instance;
		}
	}
}

#endif
