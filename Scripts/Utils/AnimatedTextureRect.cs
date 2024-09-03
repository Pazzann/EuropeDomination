using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

//[Tool]
public partial class AnimatedTextureRect : TextureRect
{
	[Export] public SpriteFrames SpriteFrames;
	[Export] public string CurrentAnimation = "default";

	[Export]
	public int FrameIndex = 0;
	/*{
		get => FrameIndex;
		set
		{
			Texture = SpriteFrames.GetFrameTexture(CurrentAnimation, value);
			FrameIndex = value;
		}
	}*/

	[Export] public double SpeedScale = 1.0f;
	[Export] public bool AutoPlay = false;
	[Export] public bool Playing = false;

	private float _refreshRate = 1.0f;
	private double _fps = 30.0f;
	private double _frameDelta = 0;

	public void SetFrame(int value)
	{
		Texture = SpriteFrames.GetFrameTexture(CurrentAnimation, value);
		FrameIndex = value;
	}

	public override void _Ready()
	{
		FrameIndex = 0;
		_fps = SpriteFrames.GetAnimationSpeed(CurrentAnimation);
		_refreshRate = SpriteFrames.GetFrameDuration(CurrentAnimation, FrameIndex);
		if (AutoPlay)
			Play(CurrentAnimation);
	}

	public void Play(string animationName)
	{
		SetFrame(1);
		FrameIndex = 0;
		_frameDelta = 0;
		CurrentAnimation = animationName;
		_getAnimationData();
		Playing = true;
	}

	public override void _Process(double delta)
	{
		if(SpriteFrames == null || !Playing)
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

	private void _getAnimationData()
	{
		_fps = SpriteFrames.GetAnimationSpeed(CurrentAnimation);
		_refreshRate = SpriteFrames.GetFrameDuration(CurrentAnimation, FrameIndex);
	}

	public void Resume()
	{
		Playing = true;
	}

	public void Pause()
	{
		Playing = false;
	}
	public void Stop()
	{
		FrameIndex = 0;
		Playing = false;
	}
}