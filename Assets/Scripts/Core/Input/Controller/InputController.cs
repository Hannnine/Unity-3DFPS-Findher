using UnityEngine;
using UnityEngine.InputSystem;

namespace GameManager.InputManager {
	public class InputController : MonoBehaviour {
		/// <summary> Input Controller /// </summary>
		protected static InputTable inputTable;

		/// <summary> Actions /// </summary>
		// Player
		protected static InputAction APlayerMove = inputTable.Gameplay.Move;
		protected static InputAction APlayerLook = inputTable.Gameplay.Rotation;
		protected static InputAction APlayerJump = inputTable.Gameplay.Jump;
		protected static InputAction APlayerRun = inputTable.Gameplay.Run;
		protected static InputAction APlayerSquat = inputTable.Gameplay.Squat;
		protected static InputAction APlayerCloseAttack = inputTable.Gameplay.CloseAttack;
		protected static InputAction APlayerFire = inputTable.Gameplay.Fire;
		protected static InputAction APlayerReload = inputTable.Gameplay.Reload;
		protected static InputAction APlayerAim = inputTable.Gameplay.Aim;
		protected static InputAction APlayerSkillAb = inputTable.Gameplay.SkillAb;
		protected static InputAction APlayerSkillUlt = inputTable.Gameplay.SkillUlt;

		/// <summary> Init /// </summary>
		protected virtual void Awake() {
			inputTable = new InputTable();
		}

		protected virtual void OnEnable() {
			inputTable.Enable();
		}

		protected virtual void OnDisable() {
			inputTable.Disable();
		}
		protected virtual void Update() {
		}
	}
}