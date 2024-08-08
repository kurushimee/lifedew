public sealed class InspectableInteractable : Component, IInteractable {
	private Model _model;

	protected override void OnAwake() {
		_model = Components.Get<ModelRenderer>().Model;
	}

	public void OnInteract() {
		GameManager.Instance.OnInspect(_model);
	}
}
