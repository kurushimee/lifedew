public sealed class ButtonInteractable : Component, IInteractable {
	[Property] private float AnimationSpeed { get; set; } = 1f;
	[Property] private Vector3 PressedPositionOffset { get; set; }
	[Property] private SoundEvent ButtonPressSound { get; set; }

	private Vector3 _originalPosition;
	private Vector3 _pressedPosition;
	private Vector3 _nextPosition;
	private bool _isPressed;

	protected override void OnAwake() {
		_originalPosition = Transform.World.Position;
		_pressedPosition = _originalPosition + PressedPositionOffset;
		_nextPosition = _originalPosition;
	}

	protected override void OnUpdate() {
		Transform.Position = Transform.World.Position.LerpTo(_nextPosition, Time.Delta * AnimationSpeed);
	}

	public void OnInteract() {
		if (_isPressed) {
			_nextPosition = _originalPosition;
			_isPressed = false;
		} else {
			_nextPosition = _pressedPosition;
			Sound.Play(ButtonPressSound, Transform.World.Position);
			_isPressed = true;
		}
	}
}
