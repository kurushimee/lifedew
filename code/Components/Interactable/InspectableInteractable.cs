public sealed class InspectableInteractable : Component, IInteractable {
	private Model _model;

	protected override void OnAwake() {
		_model = Components.Get<ModelRenderer>().Model;
	}

	public void OnInteract() {
		Log.Info("Interacted with an inspectable object");
		GameManager.Instance.State = PlayerState.inspecting;
		GameManager.Instance.OnInspect(_model);
	}
}
