public sealed class Inspection : Component {
	[Property] private ModelRenderer InspectionModel { get; set; }

	protected override void OnUpdate() {
		if (GameManager.Instance.State != PlayerState.inspecting) return;

		if (Input.EscapePressed) CloseMenu();
	}

	private void CloseMenu() {
		GameManager.Instance.State = PlayerState.walking;
		GameObject.Enabled = false;
	}

	public void SetModel(Model model) {
		InspectionModel.Model = model;
	}
}
