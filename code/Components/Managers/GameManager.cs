public enum PlayerState {
	walking,
	inspecting,
}

public sealed class GameManager : Component {
	public static GameManager Instance { get; private set; }

	public PlayerState State { get; set; } = PlayerState.walking;

	[Property] private Inspection InspectionManager { get; set; }

	protected override void OnAwake() {
		if (Instance != null) GameObject.DestroyImmediate();
		else Instance = this;
	}

	/// <summary>
	/// Activates the inspection user interface and sets the new item to display.
	/// </summary>
	public void OnInspect(Model model) {
		InspectionManager.SetModel(model);
		InspectionManager.Enabled = true;
	}
}
