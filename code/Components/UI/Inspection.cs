public sealed class Inspection : Component {
	[Property] private ModelRenderer InspectionModel { get; set; }

	public void SetModel(Model model) {
		InspectionModel.Model = model;
	}
}
