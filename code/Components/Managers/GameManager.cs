public enum PlayerState {
	walking,
	inspecting,
}

public sealed class GameManager : Component {
	public static GameManager Instance { get; private set; }

	public PlayerState State { get; set; }

	[Property] private Inspection InspectionManager { get; set; }

	protected override void OnAwake() {
		Instance = this;
		Instance.State = PlayerState.walking;
	}

	/// <summary>
	/// Activates the inspection user interface and sets the new item to display.
	/// </summary>
	public void OnInspect(Model model) {
		GameManager.Instance.State = PlayerState.inspecting;

		InspectionManager.SetModel(model);
		InspectionManager.GameObject.Enabled = true;
	}
}
