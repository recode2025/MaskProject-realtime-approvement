using UnityEngine;
using UnityEditor;

/// <summary>
/// SceneTransitionSetup 的自定义编辑器
/// 在Inspector中添加"自动设置门"按钮
/// </summary>
[CustomEditor(typeof(SceneTransitionSetup))]
public class SceneTransitionSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制默认的Inspector
        DrawDefaultInspector();
        
        // 添加空行
        EditorGUILayout.Space();
        
        // 获取目标对象
        SceneTransitionSetup setup = (SceneTransitionSetup)target;
        
        // 添加按钮
        if (GUILayout.Button("自动设置门的位置和大小", GUILayout.Height(30)))
        {
            setup.SetupDoors();
        }
        
        // 添加说明文字
        EditorGUILayout.HelpBox(
            "点击上方按钮将自动配置左右门的：\n" +
            "• 锚点设置为中心\n" +
            "• 宽度 = 屏幕宽度的一半\n" +
            "• 高度 = 屏幕高度\n" +
            "• 位置在屏幕外侧\n" +
            "• 重置旋转和缩放",
            MessageType.Info
        );
    }
}
