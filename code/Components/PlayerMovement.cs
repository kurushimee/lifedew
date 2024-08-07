using Sandbox.Citizen;

public sealed class PlayerMovement : Component {
	[Property][Category("Movement")][Range(0f, 10f, 0.1f)] private float GroundControl { get; set; } = 4f;
	[Property][Category("Movement")][Range(0f, 1f, 0.1f)] private float AirControl { get; set; } = 0.1f;
	[Property][Category("Movement")][Range(0f, 100f, 1f)] private float MaxAirAccel { get; set; } = 50f;
	[Property][Category("Movement")][Range(0f, 400f, 1f)] private float WalkSpeed { get; set; } = 160f;
	[Property][Category("Movement")][Range(0f, 200f, 1f)] private float DuckSpeed { get; set; } = 90f;

	[Property][Category("Object References")] public GameObject Body { get; private set; }
	[Property][Category("Object References")] public GameObject Head { get; private set; }

	public bool IsDucking { get; private set; }
 
	private Vector3 _wishVelocity;
	private CharacterController _characterController;
	private CitizenAnimationHelper _animationHelper;

	protected override void OnAwake() {
		_characterController = Components.Get<CharacterController>();
		_animationHelper = Components.Get<CitizenAnimationHelper>();
	}

	protected override void OnFixedUpdate() {
		if (GameManager.Instance.State != PlayerState.walking) return;
		
 		BuildWishVelocity();
		Move();
	}

	protected override void OnUpdate() {
		if (GameManager.Instance.State != PlayerState.walking) return;
		
		UpdateDuck();
		RotateBody();
		UpdateAnimations();
	}

	private void BuildWishVelocity() {
		var wishSpeed = IsDucking ? DuckSpeed : WalkSpeed;
		_wishVelocity = Input.AnalogMove.Normal * wishSpeed * Rotation.FromYaw(Head.Transform.Rotation.Yaw());
	}

	private void Move() {
		var gravity = Scene.PhysicsWorld.Gravity;

		if (_characterController.IsOnGround) {
			// Apply friction/acceleration
			_characterController.Velocity = _characterController.Velocity.WithZ(0f);
			_characterController.Accelerate(_wishVelocity);
			_characterController.ApplyFriction(GroundControl, 20f);
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

	private void RotateBody() {
		var targetRotation = Rotation.FromYaw(Head.Transform.Rotation.Yaw());
		Body.Transform.Rotation = Rotation.Lerp(Body.Transform.Rotation, targetRotation, Time.Delta * 10f);
	}

	private void UpdateAnimations() {
		_animationHelper.WithWishVelocity(_wishVelocity);
		_animationHelper.WithVelocity(_characterController.Velocity);
		_animationHelper.AimAngle = Head.Transform.Rotation;
		_animationHelper.IsGrounded = _characterController.IsOnGround;
		_animationHelper.WithLook(Head.Transform.Rotation.Forward, 1f, 0.75f, 0.5f);
		_animationHelper.DuckLevel = IsDucking ? 1f : 0f;
	}

	private void UpdateDuck() {
		if (Input.Pressed("Duck") && !IsDucking) {
			IsDucking = true;
			_characterController.Height = 38f;
		}

		if ((Input.Released("Duck") || !Input.Down("Duck")) && IsDucking) {
			var upTrace = Scene.Trace
			   .FromTo(Head.Transform.Position, Head.Transform.Position + (Vector3.Up * 40f))
			   .Size(Vector3.One * _characterController.Radius * 2f)
			   .WithoutTags("player")
			   .IgnoreGameObjectHierarchy(GameObject)
			   .Run();
			if (!upTrace.Hit) {
				IsDucking = false;
				_characterController.Height = 72f;
			}
		}
	}
}
