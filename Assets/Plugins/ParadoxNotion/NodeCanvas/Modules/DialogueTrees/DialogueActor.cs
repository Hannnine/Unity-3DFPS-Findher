using UnityEngine;
using GameManager.Localization;
using GameManager.Localization.Tables;

namespace NodeCanvas.DialogueTrees {
	///<summary> A DialogueActor Component.</summary>
	[AddComponentMenu("NodeCanvas/Dialogue Actor")]
	public class DialogueActor : MonoBehaviour, IDialogueActor {

		[SerializeField]
		protected string _name;
		// Localization
		private string _dialogueTable = LocalizationTableManager.Character.CharactersNameTable;

		protected Texture2D _portrait = null;
		[SerializeField]
		protected Texture2D[] _portraitCollection = null;
		[SerializeField]
		protected Color _dialogueColor = Color.white;
		[SerializeField]
		protected Vector3 _dialogueOffset;
		private Sprite _portraitSprite;
		private int _portraitIndex = 0;
		new public string name {
			get { return LocalizationManager.GetLocalizedStringByTableName(_dialogueTable, _name); }
		}

		public Texture2D portrait {
			get { return _portrait; }
			set { _portrait = value; }
		}

		public Texture2D[] portraitCollection {
			get { return _portraitCollection; }
		}

		public Sprite portraitSprite {
			get {
				if (_portraitSprite == null && portrait != null)
					_portraitSprite = Sprite.Create(portrait, new Rect(0, 0, portrait.width, portrait.height), new Vector2(0.5f, 0.5f));
				return _portraitSprite;
			}
			set { _portraitSprite = value; }
		}

		public Color dialogueColor {
			get { return _dialogueColor; }
		}

		public Vector3 dialoguePosition {
			get { return transform.TransformPoint(_dialogueOffset); }
		}

		public int portraitIndex {
			get { return _portraitIndex; }
			set { _portraitIndex = value; }
		}

		// Method to update the portrait based on _portraitCollection
		public void SetPortrait(int index = 0) {
			// Debug.Log("set portrait triggered");
			if (_portraitCollection != null && index >= 0 && index < _portraitCollection.Length) {
				_portrait = _portraitCollection[index];
				// Reset _portraitSprite so it gets recreated with the new texture
				_portraitSprite = null;
			}
			else {
				Debug.LogWarning($"Invalid index {index}. Using default portrait.");
				if (_portraitCollection != null && _portraitCollection.Length > 0) {
					_portrait = _portraitCollection[0];
					_portraitSprite = null;
				}
			}
		}
		///----------------------------------------------------------------------------------------------
		///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

		void Reset() {
			_name = gameObject.name;
		}

		void OnDrawGizmos() {
			Gizmos.DrawLine(transform.position, dialoguePosition);
		}

#endif
	}
}
