public sealed class PlayerClothing : Component {
	protected override void OnStart() {
		var model = Components.Get<SkinnedModelRenderer>();
		var clothing = ClothingContainer.CreateFromLocalUser();
		clothing.Apply(model);

		foreach (var go in GameObject.Children.Where(x => x.Tags.Has("clothing"))) {
			var clothingModel = go.Components.Get<SkinnedModelRenderer>();
			clothingModel.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
		}
	}
}
