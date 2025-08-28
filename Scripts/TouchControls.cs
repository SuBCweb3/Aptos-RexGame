using Godot;
using System;

// 触摸控制类：管理移动设备上的触摸控制按钮
public partial class TouchControls : CanvasLayer
{
    // 跳跃按钮
    private TouchScreenButton _jumpButton;
    // 下蹲按钮
    private TouchScreenButton _crouchButton;
    
    // 初始化函数
    public override void _Ready()
    {
        // 获取按钮引用
        _jumpButton = GetNode<TouchScreenButton>("MarginContainer/HBoxContainer/LeftControl/JumpButton");
        _crouchButton = GetNode<TouchScreenButton>("MarginContainer/HBoxContainer/RightControl/CrouchButton");
        
        // 检查是否为移动平台
        bool isMobile = OS.GetName() == "Android" || OS.GetName() == "iOS";
        
        // 设置按钮可见性
        // 在移动平台上显示，在桌面平台上隐藏
        _jumpButton.Visible = isMobile;
        _crouchButton.Visible = isMobile;
        
        // 调整按钮位置以适应不同屏幕尺寸
        AdjustButtonPositions();
    }
    
    // 调整按钮位置以适应屏幕尺寸
    private void AdjustButtonPositions()
    {
        // 获取屏幕尺寸
        Vector2 screenSize = DisplayServer.WindowGetSize();
        
        // 获取按钮的父节点
        Control leftControl = GetNode<Control>("MarginContainer/HBoxContainer/LeftControl");
        Control rightControl = GetNode<Control>("MarginContainer/HBoxContainer/RightControl");
        
        // 根据屏幕尺寸调整按钮位置
        // 这里使用相对位置，确保在不同尺寸的屏幕上都能正确显示
        _jumpButton.Position = new Vector2(leftControl.Size.X / 4, -100);
        _crouchButton.Position = new Vector2(rightControl.Size.X * 3 / 4, -100);
    }
    
    // 当窗口大小改变时调整按钮位置
    public override void _Notification(int what)
    {
        if (what == NotificationWMSizeChanged)
        {
            AdjustButtonPositions();
        }
    }
}