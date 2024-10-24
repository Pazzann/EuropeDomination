using Godot;

namespace EuropeDominationDemo.Scripts.Utils
{
	/// <summary>
	/// A custom TextureRect that supports animated textures.
	/// </summary>
	public partial class AnimatedTextureRect : TextureRect
	{
		/// <summary>
		/// Frames per second for the animation.
		/// </summary>
		private double _fps = 30.0f;

		/// <summary>
		/// Time delta between frames.
		/// </summary>
		private double _frameDelta;

		/// <summary>
		/// Refresh rate for the animation.
		/// </summary>
		private float _refreshRate = 1.0f;

		/// <summary>
		/// Indicates whether the animation should start playing automatically.
		/// </summary>
		[Export] public bool AutoPlay = false;

		/// <summary>
		/// The name of the current animation.
		/// </summary>
		[Export] public string CurrentAnimation = "default";

		/// <summary>
		/// The index of the current frame.
		/// </summary>
		[Export] public int FrameIndex;

		/// <summary>
		/// Indicates whether the animation is currently playing.
		/// </summary>
		[Export] public bool Playing;

		/// <summary>
		/// Speed scale for the animation.
		/// </summary>
		[Export] public double SpeedScale = 1.0f;

		/// <summary>
		/// The SpriteFrames resource containing the animation frames.
		/// </summary>
		[Export] public SpriteFrames SpriteFrames;

		/// <summary>
		/// Sets the current frame of the animation.
		/// </summary>
		/// <param name="value">The index of the frame to set. If -1, sets an empty frame.</param>
		public void SetFrame(int value)
		{
			if (value == -1)
			{
				SetEmptyFrame();
				return;
			}

			Texture = SpriteFrames.GetFrameTexture(CurrentAnimation, value);
			FrameIndex = value;
		}

		/// <summary>
		/// Sets an empty frame for the animation.
		/// </summary>
		public void SetEmptyFrame()
		{
			Texture = null;
			FrameIndex = -1;
		}

		/// <summary>
		/// Called when the node enters the scene tree for the first time.
		/// </summary>
		public override void _Ready()
		{
			FrameIndex = 0;
			//_fps = SpriteFrames.GetAnimationSpeed(CurrentAnimation);
			//_refreshRate = SpriteFrames.GetFrameDuration(CurrentAnimation, FrameIndex);
			if (AutoPlay)
				Play(CurrentAnimation);
		}

		/// <summary>
		/// Plays the animation.
		/// </summary>
		/// <param name="animationName"></param>
		public void Play(string animationName)
		{
			SetFrame(1);
			FrameIndex = 0;
			_frameDelta = 0;
			CurrentAnimation = animationName;
			_getAnimationData();
			Playing = true;
		}

		/// <summary>
		/// Processes the animation.
		/// </summary>
		/// <param name="delta"></param>
		public override void _Process(double delta)
		{
			if (SpriteFrames == null || !Playing)
				return;
			if (!SpriteFrames.HasAnimation(CurrentAnimation))
			{
				Playing = false;
				return;
			}

			_getAnimationData();
			_frameDelta = SpeedScale * delta;
			if (_frameDelta >= _refreshRate / _fps)
			{
				Texture = _getNextFrame();
				_frameDelta = 0;
			}
		}

		/// <summary>
		/// Gets the next frame.
		/// </summary>
		/// <returns></returns>
		private Texture2D _getNextFrame()
		{
			FrameIndex += 1;
			var frameCount = SpriteFrames.GetFrameCount(CurrentAnimation);
			if (FrameIndex >= frameCount)
			{
				FrameIndex = 0;
				if (!SpriteFrames.GetAnimationLoop(CurrentAnimation))
					Playing = false;
			}

			_getAnimationData();
			return SpriteFrames.GetFrameTexture(CurrentAnimation, FrameIndex);
		}

		/// <summary>
		/// Gets the animation data.
		/// </summary>
		private void _getAnimationData()
		{
			_fps = SpriteFrames.GetAnimationSpeed(CurrentAnimation);
			_refreshRate = SpriteFrames.GetFrameDuration(CurrentAnimation, FrameIndex);
		}

		/// <summary>
		/// Resumes the animation.
		/// </summary>
		public void Resume()
		{
			Playing = true;
		}

		/// <summary>
		/// Pauses the animation.
		/// </summary>
		public void Pause()
		{
			Playing = false;
		}

		/// <summary>
		/// Stops the animation.
		/// </summary>
		public void Stop()
		{
			FrameIndex = 0;
			Playing = false;
		}
	}
}
