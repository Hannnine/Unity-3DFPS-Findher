using UnityEngine;
public static class InputManager {
	/* Input State */
	public enum State {
		None = 0,
		MainMenu = 1 << 0,
		Gameplay = 1 << 1,
		Settings = 1 << 2,
		Dialogue = 1 << 3,
		Pause = 1 << 4,
	};
	public static State currentState = State.None;
	public static void SetInputMode(State state) {
		currentState = state;
		Debug.Log("Current Input State: " + currentState);
	}
}
