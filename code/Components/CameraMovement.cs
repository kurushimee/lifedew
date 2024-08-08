public sealed class CameraMovement : Component {
	[Property][Category("Object References")] private PlayerMovement Player { get; set; }
	[Property][Category("Object References")] private GameObject Head { get; set; }

	private Angles _eyeAngles;
	private Vector3 _currentOffset;

	protected override void OnStart() {
		ConsoleSystem.SetValue("sensitivity", 0.0000001f);
	}

	protected override void OnUpdate() {
		if (GameManager.Instance.State != PlayerState.walking) return;
		AimCamera();
	}

	private void AimCamera() {
		_eyeAngles += Input.AnalogLook;
		_eyeAngles = _eyeAngles.WithPitch(_eyeAngles.pitch.Clamp(-89.9f, 89.9f));
		Head.Transform.Rotation = _eyeAngles.ToRotation();

		// Calculate ducking offset
		var targetOffset = Vector3.Zero;
		if (Player.IsDucking) targetOffset = Vector3.Down * 32f;
		_currentOffset = Vector3.Lerp(_currentOffset, targetOffset, Time.Delta * 10f);

		Transform.Position = Head.Transform.Position + _currentOffset;
		Transform.Rotation = Head.Transform.Rotation;
	}
}
