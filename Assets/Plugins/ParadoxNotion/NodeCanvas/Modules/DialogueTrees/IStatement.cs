using ParadoxNotion;
using NodeCanvas.Framework;
using UnityEngine;
using System.Linq;
using GameManager.Localization.Tables;
using GameManager.Localization;
using UnityEditor.Localization;

namespace NodeCanvas.DialogueTrees {

	///<summary>An interface to use for whats being said by a dialogue actor</summary>
	public interface IStatement {
		string text { get; }
		AudioClip audio { get; }
		string meta { get; }
	}

	///<summary>Holds data of what's being said usualy by an actor</summary>
	[System.Serializable]
	public class Statement : IStatement {
		[SerializeField] private string _text = string.Empty;
		[SerializeField] private AudioClip _audio;
		[SerializeField] private string _meta = string.Empty;
		[SerializeField] private DialogueActor _dialogueActor;
		[SerializeField] private Texture2D _portrait;
		[SerializeField] private Texture2D[] _portraitCollection;
		[SerializeField] private int _portraitIndex = 0;

		public DialogueActor dialogueActor {
			get { return _dialogueActor; }
			set { _dialogueActor = value; }
		}
		public Texture2D portrait {
			get { return dialogueActor != null ? dialogueActor.portrait : null; }
			set { if (_dialogueActor != null) _portrait = value; }
		}

		public Texture2D[] portraitCollection {
			get { return dialogueActor != null ? dialogueActor.portraitCollection : null; }
			set { if (_dialogueActor != null) _portraitCollection = value; }
		}

		public int portraitIndex {
			get { return _portraitIndex; }
			set { if (_dialogueActor != null) _portraitIndex = value; }
		}

		public string text {
			get { return _text; }
			set { _text = value; }
		}

		public AudioClip audio {
			get { return _audio; }
			set { _audio = value; }
		}

		public string meta {
			get { return _meta; }
			set { _meta = value; }
		}

		//required
		public Statement() { }
		public Statement(string text) {
			this.text = text;
		}

		public Statement(string text, AudioClip audio) {
			this.text = text;
			this.audio = audio;
		}

		public Statement(string text, AudioClip audio, string meta) {
			this.text = text;
			this.audio = audio;
			this.meta = meta;
		}


		// Set dialogue actor.
		public void SetDialogueActor(DialogueActor actor) {
			_dialogueActor = actor;
		}

		///<summary>Replace the text of the statement found in brackets, with blackboard variables ToString and returns a Statement copy</summary>
		public IStatement BlackboardReplace(IBlackboard bb) {
			var copy = ParadoxNotion.Serialization.JSONSerializer.Clone<Statement>(this);

			copy.text = copy.text.ReplaceWithin('[', ']', (input) => {
				object o = null;
				if (bb != null) { //referenced blackboard replace
					var v = bb.GetVariable(input, typeof(object));
					if (v != null) { o = v.value; }
				}

				if (input.Contains("/")) { //global blackboard replace
					var globalBB = GlobalBlackboard.Find(input.Split('/').First());
					if (globalBB != null) {
						var v = globalBB.GetVariable(input.Split('/').Last(), typeof(object));
						if (v != null) { o = v.value; }
					}
				}
				return o != null ? o.ToString() : input;
			});

			// Localization
			copy.text = copy.text.ReplaceWithin('`', '`', (input) => {
				StringTableCollection table = null;
				string dg = null;
				if (bb != null) { //referenced blackboard replace
					try {
						var v = bb.GetVariable(LocalizationTableManager.Dialogue.DialogueTable, typeof(object));
						if (v != null) { table = v.value as StringTableCollection; }
						dg = LocalizationManager.GetLocalizedString(table, input);
					}
					catch {
						Debug.LogWarning("Table or key not found, Please Check Table: { " + LocalizationTableManager.Dialogue.DialogueTable + " }");
					}

				}


				return dg != null ? dg : input;
			});

			return copy;
		}

		public override string ToString() {
			return text;
		}
	}
}