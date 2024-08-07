public sealed class Inspection : Component {
	private ModelRenderer _inspectionModel;

	protected override void OnAwake() {
		_inspectionModel = Components.GetInChildren<ModelRenderer>(true);
    }

	public void SetModel(Model model) {
		_inspectionModel.Model = model;
	}
}
