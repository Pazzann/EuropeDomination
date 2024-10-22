---
title: Animated Texture Rect
---
# Introduction

This document will walk you through the implementation of the <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="8:7:7" line-data="    public partial class AnimatedTextureRect : TextureRect">`AnimatedTextureRect`</SwmToken> class.

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="8:7:7" line-data="    public partial class AnimatedTextureRect : TextureRect">`AnimatedTextureRect`</SwmToken> class extends <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="6:7:7" line-data="    /// A custom TextureRect that supports animated textures.">`TextureRect`</SwmToken> to support animated textures. It includes features for controlling playback, setting frames, and handling animation data.

We will cover:

1. Why we need custom properties and methods.
2. How the animation playback is managed.
3. How the class integrates with other parts of the system.

# Custom properties and methods

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="5">

---

The class defines several properties to manage animation settings and state. These properties are essential for controlling the animation behavior.

```
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
```

---

</SwmSnippet>

These properties include:

- <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="13:5:5" line-data="        private double _fps = 30.0f;">`_fps`</SwmToken> and <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="18:5:5" line-data="        private double _frameDelta;">`_frameDelta`</SwmToken> for frame rate control.
- <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="28:9:9" line-data="        [Export] public bool AutoPlay = false;">`AutoPlay`</SwmToken>, <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="33:9:9" line-data="        [Export] public string CurrentAnimation = &quot;default&quot;;">`CurrentAnimation`</SwmToken>, <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="38:9:9" line-data="        [Export] public int FrameIndex;">`FrameIndex`</SwmToken>, and <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="43:9:9" line-data="        [Export] public bool Playing;">`Playing`</SwmToken> for animation state management.
- <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="48:9:9" line-data="        [Export] public double SpeedScale = 1.0f;">`SpeedScale`</SwmToken> and <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="51:5:5" line-data="        /// The SpriteFrames resource containing the animation frames.">`SpriteFrames`</SwmToken> for animation speed and frame data.

# Setting and updating frames

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="54">

---

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="59:5:5" line-data="        public void SetFrame(int value)">`SetFrame`</SwmToken> method sets the current frame of the animation. It handles both valid frame indices and the special case of setting an empty frame.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="79">

---

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="83:7:7" line-data="        public override void _Ready()">`_Ready`</SwmToken> method initializes the animation when the node enters the scene tree. If <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="88:4:4" line-data="            if (AutoPlay)">`AutoPlay`</SwmToken> is enabled, it starts the animation automatically.

```

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
```

---

</SwmSnippet>

# Animation playback management

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="91">

---

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="96:5:5" line-data="        public void Play(string animationName)">`Play`</SwmToken> method starts the animation, setting the initial frame and resetting relevant state variables.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="105">

---

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="110:7:7" line-data="        public override void _Process(double delta)">`_Process`</SwmToken> method updates the animation on each frame. It calculates the time delta and updates the texture if necessary.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="128">

---

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="133:5:5" line-data="        private Texture2D _getNextFrame()">`_getNextFrame`</SwmToken> method retrieves the next frame in the animation sequence. It handles looping and stops the animation if it reaches the end of a non-looping animation.

```

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
```

---

</SwmSnippet>

# Controlling playback

<SwmSnippet path="/Scripts/Utils/AnimatedTextureRect.cs" line="156">

---

The class provides methods to control playback: <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="160:5:5" line-data="        public void Resume()">`Resume`</SwmToken>, <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="168:5:5" line-data="        public void Pause()">`Pause`</SwmToken>, and <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="176:5:5" line-data="        public void Stop()">`Stop`</SwmToken>.

```

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
```

---

</SwmSnippet>

These methods allow external code to control the animation state.

# Integration example

<SwmSnippet path="/Scripts/UI/GUIPrefabs/GUIGoodEditPanel.cs" line="32">

---

The <SwmToken path="/Scripts/UI/GUIPrefabs/GUIGoodEditPanel.cs" pos="32:5:5" line-data="			a.GetChild&lt;AnimatedTextureRect&gt;(0).SpriteFrames = GlobalResources.GoodSpriteFrames;">`AnimatedTextureRect`</SwmToken> class is used in other parts of the system. For example, in <SwmToken path="/Scripts/UI/GUIPrefabs/GUIGoodEditPanel.cs" pos="8:6:6" line-data="public partial class GUIGoodEditPanel : PanelContainer">`GUIGoodEditPanel`</SwmToken>, it is used to set sprite frames and initialize the frame based on an ID.

```
			a.GetChild<AnimatedTextureRect>(0).SpriteFrames = GlobalResources.GoodSpriteFrames;
			a.GetChild<AnimatedTextureRect>(0).SetFrame(good.Id);
```

---

</SwmSnippet>

This example shows how to assign <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="51:5:5" line-data="        /// The SpriteFrames resource containing the animation frames.">`SpriteFrames`</SwmToken> and set the initial frame for an <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="8:7:7" line-data="    public partial class AnimatedTextureRect : TextureRect">`AnimatedTextureRect`</SwmToken> instance.

# Conclusion

The <SwmToken path="/Scripts/Utils/AnimatedTextureRect.cs" pos="8:7:7" line-data="    public partial class AnimatedTextureRect : TextureRect">`AnimatedTextureRect`</SwmToken> class provides a flexible way to handle animated textures in the system. It includes properties and methods for managing animation state, setting frames, and controlling playback. The integration example demonstrates how to use this class in practice.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
