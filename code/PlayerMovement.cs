public sealed class PlayerMovement : Component {
	#region Variables
	#region Properties
	[Property][Range(0f, 10f, 0.1f)] private float GroundControl { get; set; } = 4f;
	[Property][Range(0f, 1f, 0.1f)] private float AirControl { get; set; } = 0.1f;
	[Property][Range(0f, 100f, 1f)] private float MaxAirAccel { get; set; } = 50f;
	[Property][Range(0f, 400f, 1f)] private float WalkSpeed { get; set; } = 160f;
	[Property][Range(0f, 200f, 1f)] private float DuckSpeed { get; set; } = 90f;
	#endregion Properties

	#region Fields
	private bool _isDucking;
	private Vector3 _wishVelocity;

	private CharacterController _characterController;
	#endregion Fields
	#endregion Variables

	#region Methods
	#region Lifecycle
	protected override void OnAwake() {
		_characterController = Components.Get<CharacterController>();
	}

	protected override void OnFixedUpdate() {
		BuildWishVelocity();
		Move();
	}

	protected override void OnUpdate() {
		_isDucking = Input.Down("Duck");
	}
	#endregion Lifecycle

	#region Private
	private void BuildWishVelocity() {
		var wishSpeed = _isDucking ? DuckSpeed : WalkSpeed;
		_wishVelocity = Input.AnalogMove.Normal * wishSpeed * Transform.Rotation;
	}

	private void Move() {
		var gravity = Scene.PhysicsWorld.Gravity;

		if (_characterController.IsOnGround) {
			// Apply friction/acceleration
			_characterController.Velocity = _characterController.Velocity.WithZ(0f);
			_characterController.Accelerate(_wishVelocity);
			_characterController.ApplyFriction(GroundControl);
		} else {
			// Apply air control/half-gravity
			_characterController.Velocity += gravity * Time.Delta * 0.5f;
			_characterController.Accelerate(_wishVelocity.ClampLength(MaxAirAccel));
			_characterController.ApplyFriction(AirControl);
		}

		_characterController.Move();

		// Apply 2nd half-gravity after movement
		if (!_characterController.IsOnGround) _characterController.Velocity += gravity * Time.Delta * 0.5f;
		else _characterController.Velocity = _characterController.Velocity.WithZ(0f);
	}
	#endregion Private
	#endregion Methods
}
