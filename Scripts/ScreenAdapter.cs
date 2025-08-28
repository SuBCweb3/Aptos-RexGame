using Godot;
using System;

// 屏幕适配器类：管理游戏在不同设备上的屏幕适配
public partial class ScreenAdapter : Node
{
    // 设计分辨率（基准分辨率）
    [Export]
    private Vector2I _designResolution = new Vector2I(1280, 720);
    
    // 最小缩放比例
    [Export]
    private float _minScale = 0.5f;
    
    // 最大缩放比例
    [Export]
    private float _maxScale = 2.0f;
    
    // 是否保持纵横比
    [Export]
    private bool _maintainAspectRatio = true;
    
    // 初始化函数
    public override void _Ready()
    {
        // 连接窗口大小变化信号
        GetTree().Root.SizeChanged += OnWindowResized;
        
        // 初始调整
        OnWindowResized();
    }
    
    // 窗口大小变化时调用
    private void OnWindowResized()
    {
        // 获取当前窗口大小
        Vector2I windowSize = DisplayServer.WindowGetSize();
        
        // 计算缩放比例
        float scaleX = (float)windowSize.X / _designResolution.X;
        float scaleY = (float)windowSize.Y / _designResolution.Y;
        
        // 如果需要保持纵横比，取最小值
        float scale = _maintainAspectRatio ? Mathf.Min(scaleX, scaleY) : Mathf.Max(scaleX, scaleY);
        
        // 限制缩放范围
        scale = Mathf.Clamp(scale, _minScale, _maxScale);
        
        // 应用缩放
        ApplyScale(scale);
    }
    
    // 应用缩放到游戏世界
    private void ApplyScale(float scale)
    {
        // 获取主场景的根节点
        Node mainNode = GetTree().Root.GetChild(0);
        
        // 如果是移动平台，调整UI元素大小
        if (OS.GetName() == "Android" || OS.GetName() == "iOS")
        {
            // 调整触摸按钮大小
            AdjustTouchControlsSize(scale);
            
            // 调整UI文本大小
            AdjustUITextSize(scale);
        }
    }
    
    // 调整触摸控制按钮大小
    private void AdjustTouchControlsSize(float scale)
    {
        // 尝试获取触摸控制节点
        TouchControls touchControls = GetTree().Root.GetNodeOrNull<TouchControls>("main/TouchControls");
        
        if (touchControls != null)
        {
            // 获取按钮
            TouchScreenButton jumpButton = touchControls.GetNode<TouchScreenButton>("MarginContainer/HBoxContainer/LeftControl/JumpButton");
            TouchScreenButton crouchButton = touchControls.GetNode<TouchScreenButton>("MarginContainer/HBoxContainer/RightControl/CrouchButton");
            
            // 调整按钮大小
            float buttonScale = Mathf.Clamp(scale * 1.2f, 1.0f, 2.5f);
            jumpButton.Scale = new Vector2(buttonScale, buttonScale);
            crouchButton.Scale = new Vector2(buttonScale, buttonScale);
        }
    }
    
    // 调整UI文本大小
    private void AdjustUITextSize(float scale)
    {
        // 尝试获取UI节点
        UI ui = GetTree().Root.GetNodeOrNull<UI>("main/UI");
        
        if (ui != null)
        {
            // 获取分数标签
            Label scoreLabel = ui.GetNode<Label>("%ScoreLabel");
            
            // 调整字体大小
            int fontSize = (int)(32 * Mathf.Clamp(scale, 0.8f, 1.5f));
            scoreLabel.AddThemeConstantOverride("font_size", fontSize);
            
            // 获取游戏结束标签
            Label gameOverLabel = ui.GetNode<Label>("MarginContainer/CenterContainer/GameOverContainer/Label");
            gameOverLabel.AddThemeConstantOverride("font_size", fontSize);
        }
    }
}