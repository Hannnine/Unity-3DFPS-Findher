public static class ConfigManager {
	/* Settings */
	public static class Settings {
		public static int ScreenWidth = 800;
		public static int ScreenHeight = 600;
		public static int PPU = 32;

		public static bool IsDebugMode = true;        // Debug mode
		public static bool IsDefaultRunSpeed = false; // Default run speed
		public static bool IsFullscreen = false;      // Fullscreen
		public static bool IsMusicMuted = false;
		public static bool IsEffectsMuted = false;


		public static float MusicVolume = 0.8f;
		public static float EffectsVolume = 0.7f;
	}

	/* UI */
	public static class UI {
		public static string TipBubble = "TipBubble";
	}

	/* Tags */
	public static class Tag {
		public static string Player = "Player";
		public static string NPC = "NPC";
	}

	/* Layers */
	public static class Layer {
	}

	/* Player */
	public static class Player {
		public static float WalkSpeed = 2;
		public static float RunSpeed = 2 * WalkSpeed;
		public static float winkTriggerPosibility = 0.0000001f;
		public static int MaxHealth = 100;
	}

	/* NPC */
	public static class NPC {
		public static string TipBubble = "TipBubble";
		public static float MoveSpeed = 100;
		public static float RunSpeed = 2 * MoveSpeed;
		public static int MaxHealth = 50;
	}
}