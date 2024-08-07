public sealed class PlayerInteraction : Component {
	[Property][Range(10f, 100f, 1f)] private float InteractDistance = 50f;
	[Property][Range(0f, 10f, 0.1f)] private float InteractDeadzone = 2f;

	private PlayerMovement _playerMovement;

	protected override void OnAwake() {
		_playerMovement = Components.Get<PlayerMovement>();
	}

	protected override void OnUpdate() {
		if (GameManager.Instance.State != PlayerState.walking) return;
		Interact();
	}

	private void Interact() {
		if (!Input.Pressed("attack1")) return;

		var head = _playerMovement.Head;
		var tr = Scene.Trace
		   .FromTo(head.Transform.Position, head.Transform.Position + (head.Transform.Rotation.Forward * InteractDistance))
		   .Size(Vector3.One * InteractDeadzone)
		   .WithTag("interactable")
		   .WithoutTags("player")
		   .IgnoreGameObjectHierarchy(GameObject)
		   .Run();

		if (tr.Hit && tr.GameObject.Components.TryGet<IInteractable>(out var interactable))
			interactable.OnInteract();
	}
}
