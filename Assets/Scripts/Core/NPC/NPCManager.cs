using GameManager.InputManager;
using UnityEngine;

namespace GameManager.NPCManager {
	public abstract class NPCManager : InputController {
		public enum InteractionType {
			Dialogue = 1 << 0,

			Pickup = 1 << 1,

			All = Dialogue | Pickup
			//...
		};

		/* Interaction */
		protected virtual void OnInteractionSignalReceived(InteractionType type) {
			if (type.HasFlag(InteractionType.Dialogue)) { Debug.Log("Interaction type: Dialogue"); }
			if (type.HasFlag(InteractionType.Pickup)) { Debug.Log("Interaction type: Pickup"); }
		}

		/* Collision */
		protected virtual void OnTriggerEnter2D(Collider2D other) { if (other.tag == ConfigManager.Tag.Player) { Debug.Log("Player entered"); } }
		protected virtual void OnTriggerStay2D(Collider2D other) { if (other.tag == ConfigManager.Tag.Player) { Debug.Log("Player stay"); } }
		protected virtual void OnTriggerExit2D(Collider2D other) { if (other.tag == ConfigManager.Tag.Player) { Debug.Log("Player exited"); } }

		/* Init */
		protected override void Awake() {
			base.Awake();
		}
		protected override void Update() {
			base.Update();
		}


	}
}