
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.Drawing;

public class OtherHelper : MonoBehaviour
{
    //NotifyIcon 设置托盘相关参数
    NotifyIcon notifyIcon = new NotifyIcon();

    //托盘图标的宽高
    int _width = 50, _height = 50;

    public void Init()
    {
        InitTray();
        Show("休息提醒助手已成功启动！");
    }

    public void Show(string str)
    {
        notifyIcon.BalloonTipText = str;
        notifyIcon.ShowBalloonTip(2500);
    }

    private void InitTray()
    {
        //托盘气泡显示内容
        notifyIcon.Text = "休息提醒助手";
        //托盘按钮是否可见 
        notifyIcon.Visible = true;
        notifyIcon.Icon = SetTrayIcon(@UnityEngine.Application.streamingAssetsPath + "/icon.png", _width, _height);
        /*MenuItem help = new MenuItem("帮助");
        help.Click += new EventHandler(help_Click);*/
        MenuItem exit = new MenuItem("关闭");
        exit.Click += new EventHandler(exit_Click);
        MenuItem[] childen = new MenuItem[] { exit};
        notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
    }

    /// <summary>  
    /// 帮助选项  
    /// </summary>  
    /// <param name="sender"></param>  
    /// <param name="e"></param>  
    private void help_Click(object sender, EventArgs e)
    {
        UnityEngine.Application.OpenURL("https://www.xuefei.net.cn");
    }

    private void exit_Click(object sender, EventArgs e)
    {
        UnityEngine.Application.Quit();
    }

    /// <summary>
    /// 设置程序托盘图标
    /// </summary>
    /// <param name="iconPath">图标路径</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns>图标</returns>
    private Icon SetTrayIcon(string iconPath, int width, int height)
    {
        Bitmap bt = new Bitmap(iconPath);
        Bitmap fitSizeBt = new Bitmap(bt, width, height);
        return Icon.FromHandle(fitSizeBt.GetHicon());
    }
}

/*

using UnityEngine;
using System.Windows.Forms; //该命名空间需要导入System.Windows.Forms.dll
//可在路径：Unity安装目录/Editor\Data\Mono\lib\mono\2.0里找到
using System.Drawing; //该命名空间需要导入System.Drawing.dll
//可在路径：Unity安装目录/Editor\Data\Mono\lib\mono\2.0里找到
using System;
using System.Runtime.InteropServices;

public class Tray
{
    //NotifyIcon 设置托盘相关参数
    private static NotifyIcon _notifyIcon = new NotifyIcon();

    //托盘图标的宽高
    private static int _width = 40, _height = 40;

    //做托盘图标的图片，这里用了.png格式
    private static Texture2D iconTexture2D;
    private static IntPtr currentWindowPtr;

    //调用该方法将运行程序显示到托盘
    public static void InitTray()
    {
        currentWindowPtr = WindowsForm.GetForegroundWindow(); //记住当前窗口句柄
        _notifyIcon.BalloonTipText = "我的托盘"; //托盘气泡显示内容
        _notifyIcon.Text = "这是托盘提示信息";
        _notifyIcon.Icon = new System.Drawing.Icon(SystemIcons.Warning, 40, 40); //托盘图标
        _notifyIcon.Icon = CustomTrayIcon(@UnityEngine.Application.streamingAssetsPath + "/icon.png", _width, _height);
        _notifyIcon.ShowBalloonTip(2000); //托盘气泡显示时间
        _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick; //双击托盘图标响应事件
        ShowTray();
        //HideTray();
    }

    /// <summary>
    /// 设置程序托盘图标
    /// </summary>
    /// <param name="iconPath">图标路径</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns>图标</returns>
    private static Icon CustomTrayIcon(string iconPath, int width, int height)
    {
        Bitmap bt = new Bitmap(iconPath);
        Bitmap fitSizeBt = new Bitmap(bt, width, height);
        return Icon.FromHandle(fitSizeBt.GetHicon());
    }

    private static Icon CustomTrayIcon(System.Drawing.Image img, int width, int height)
    {
        Bitmap bt = new Bitmap(img);
        Bitmap fitSizeBt = new Bitmap(bt, width, height);
        return Icon.FromHandle(fitSizeBt.GetHicon());
    }

    /// <summary>
    /// byte[]转换成Image
    /// </summary>
    /// <param name="byteArrayIn">二进制图片流</param>
    /// <returns>Image</returns>
    public static System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
    {
        if (byteArrayIn == null)
            return null;
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
        {
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            ms.Flush();
            return returnImage;
        }
    }


    public static void DestroyDoubleClick()
    {
        _notifyIcon.MouseDoubleClick -= NotifyIcon_MouseDoubleClick;
    }

    /// <summary>
    /// 显示托盘图标
    /// </summary>
    public static void ShowTray()
    {
        _notifyIcon.Visible = true; //托盘按钮是否可见
    }

    /// <summary>
    /// 隐藏托盘图标
    /// </summary>
    public static void HideTray()
    {
        _notifyIcon.Visible = false; //托盘按钮是否可见
    }

    /// <summary>
    /// 双击托盘图标、程序最大化、并 托盘图标隐藏
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            WindowsForm.OnClickMaximize(currentWindowPtr);
            _notifyIcon.Visible = false; //托盘按钮是否可见
        }
    }
}

public class WindowsForm
{
    [DllImport("user32.dll")] //需要导入该dll
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow();

    const int SW_Hide = 0; //隐藏任务栏图标  
    const int SW_SHOWRESTORE = 1; //还原  
    const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}  
    const int SW_SHOWMAXIMIZED = 3; //最大化  
    const int GWL_STYLE = -16;
    const int WS_POPUP = 0x800000;
    const uint SWP_SHOWWINDOW = 0x0040;

    /// <summary>
    /// 最大化   
    /// </summary>
    public static void OnClickMaximize(IntPtr hwnd)
    {
        ShowWindow(hwnd, SW_SHOWMAXIMIZED);
    }

    /// <summary>
    /// 最小化
    /// </summary>
    /// <param name="hwnd"></param>
    public static void OnClickMinimize(IntPtr hwnd)
    {
        ShowWindow(hwnd, SW_SHOWMINIMIZED);
    }
}


//测试类
public class TestTray : MonoBehaviour
{
    /// <summary>
    /// 测试代码  放到继承自Mono的类中 
    /// Q键生成托盘图标
    /// E键显示图标
    /// 图标双击事件为最大化窗体
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Tray.InitTray();");
            Tray.InitTray();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Tray.ShowTray();
        }
    }
}
*/