using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;

namespace dbs_oobe
{
    /// <summary>
    /// 关于时间
    /// </summary>
    public class DTime
    {
        /// <summary>
        /// 延时等待函数
        /// </summary>
        /// <param name="delaynum">等待时间（毫秒）</param>
        public static void Delay(int delaynum)
        {
            DateTime time = DateTime.Now;//获得当前时间
            while ((DateTime.Now - time).TotalMilliseconds <= delaynum) ;//等待
        }
    }
    /// <summary>
    /// 系统命令相关
    /// </summary>
    public class Syscmd
    {
        //dosCommand Dos命令语句
        public string ExecuteCMD(string CmdCommand)
        {
            return ExecuteCMD(CmdCommand, 10);
        }
        /// <summary>
        /// 执行cmd命令，返回cmd命令的输出
        /// </summary>
        /// <param name="command">cmd命令</param>
        /// <param name="seconds">等待命令执行的时间（单位：毫秒），
        /// 如果设定为0，则无限等待</param>
        /// <returns>返回DOS命令的输出</returns>
        public static string ExecuteCMD(string command, int seconds)
        {
            string output = ""; //输出字符串
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.CreateNoWindow = true;//不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
        /// <summary>
        /// 执行powershell指令
        /// </summary>
        /// <param name="commands">指令</param>
        /// <param name="mseconds">等待时间(毫秒)</param>
        /// <returns></returns>
        public static string ExecutePwsh(string commands, int mseconds)
        {
            string output = ""; //输出字符串
            if (commands != null && !commands.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "powershell.exe";//设定需要执行的命令
                startInfo.Arguments = "-Command " + commands;//“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.CreateNoWindow = true;//不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        if (mseconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束
                        }
                        else
                        {
                            process.WaitForExit(mseconds); //等待进程结束，等待时间为指定的毫秒
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
        /// <summary>
        /// 以指定的方式启动实例
        /// </summary>
        /// <param name="path">应用程序实例的路径及名称</param>
        /// <param name="values">实例参数
        /// SW_NORMAL 正常启动	SW_MIN 最小化启动
        /// SW_MAX 最大化启动 SW_HIDE 隐藏启动</param>
        public static void ShellExecute(string path, int values)
        {
            if (values == 3)
            {
                ExecuteCMD(path, 0);
            }
            else
            {
                switch (values)
                {
                    case 0:
                        ExecuteCMD("start " + path, 100);
                        break;
                    case 1:
                        ExecuteCMD("start /MIN " + path, 100);
                        break;
                    case 2:
                        ExecuteCMD("start /MAX " + path, 100);
                        break;
                    default:
                        break;
                }
            }
        }
        public const int SW_NORMAL = 0;
        public const int SW_MIN = 1;
        public const int SW_MAX = 2;
        public const int SW_HIDE = 3;
    }
    /// <summary>
    /// API信息窗口
    /// </summary>
    public class WinMessage
    {
        [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr handle, string message, string title, int type);//MessageBox函数导入
        public const int MB_OK = 0;//只有一个确定按钮
        public const int MB_YESNO = 0x01;//带有两个按钮：是，否
        public const int MB_RAF = 0x02;//带有3个按钮：重试，跳过，取消
        public const int MB_YESNOCANCEL = 0x03;//带有3个按钮：是，否，取消
        public const int MB_RETRYCANCEL = 0x05;//带有2个按钮：重试，取消
        public const int MB_CRC = 0x06;//带有3个按钮：取消，重试，继续
        public const int ICON_ERROR = 0x10;//错误图标
        public const int ICON_QUESTION = 0x20;//询问图标
        public const int ICON_WARNING = 0x30;//警告(惊叹号)图标
        public const int ICON_INFORMATION = 0x40;//信息图标
        public const int SOUND_NORMAL = 0x50;//这是啥来着……
    }
    /// <summary>
    /// 音频处理相关
    /// </summary>
    public class Media
    {
        [DllImport("winmm.dll")]
        public static extern int PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        public const uint SND_SYNC = 0x0000;//同步播放
        public const uint SND_ASYNC = 0x0001;//异步播放
        public const uint SND_NODEFAULT = 0x0002;
        public const uint SND_MEMORY = 0x0004;//内存地址
        public const uint SND_LOOP = 0x0008;//循环播放
        public const uint SND_NOSTOP = 0x0010;
        public const uint SND_NOWAIT = 0x00002000;
        public const uint SND_ALIAS = 0x00010000;
        public const uint SND_ALIAS_ID = 0x00110000;
        public const uint SND_FILENAME = 0x00020000;//从文件播放
        public const uint SND_RESOURCE = 0x00040004;//资源文件
    }
    /// <summary>
    /// 系统配置
    /// </summary>
    public class WSystemd
    {
        public class Systemctl
        {
            [DllImport("kernel32.dll", EntryPoint = "SetComputerNameEx")]
            public static extern int apiSetComputerNameEx(int type, string lpComputerName);//有bug,别用
            public const int ASC_NORMAL_TYPE = 5;
            /// <summary>
            /// 获取系统计算机名
            /// </summary>
            /// <returns></returns>
            public static string GetComputerName()
            {
                try
                {
                    return Environment.GetEnvironmentVariable("ComputerName");
                }
                catch
                {
                    return "";
                }
            }
            /// <summary>
            /// 修改计算机名(通过注册表)
            /// </summary>
            /// <param name="newname">新名称</param>
            public static void SetComputerName(string newname)
            {
                RegistryKey pregkey;
                pregkey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\ComputerName\\ComputerName", true);
                pregkey.SetValue("ComputerName", newname);
                pregkey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Services\\Tcpip\\Parameters", true);
                pregkey.SetValue("NV Hostname", newname);
                pregkey.SetValue("Hostname", newname);
                pregkey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName", true);
                pregkey.SetValue("ComputerName", newname);
                pregkey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters", true);
                pregkey.SetValue("NV Hostname", newname);
                pregkey.SetValue("Hostname", newname);
            }
        }
        public class Users
        {
            /// <summary>
            /// 修改Administrator的密码
            /// </summary>
            /// <param name="NewPass">新密码</param>
            public static void ResetAdminPass(string NewPass)
            {
                //Create New Process
                System.Diagnostics.Process QProc = new System.Diagnostics.Process();
                //Do Something To hide Command(cmd) Window
                QProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                QProc.StartInfo.CreateNoWindow = true;
                //Call Net.exe
                QProc.StartInfo.WorkingDirectory = "C://Windows//SYSTEM32";
                QProc.StartInfo.FileName = "net.exe";
                QProc.StartInfo.UseShellExecute = false;
                QProc.StartInfo.RedirectStandardError = true;
                QProc.StartInfo.RedirectStandardInput = true;
                QProc.StartInfo.RedirectStandardOutput = true;
                //Prepare Command for Exec
                QProc.StartInfo.Arguments = @" user Administrator " + NewPass;
                QProc.Start();
                //MyProc.WaitForExit();
                QProc.Close();
            }
        }
    }
    /// <summary>
    /// 下拉菜单
    /// </summary>
    public class drop_down_list
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int IDS { get; set; }
    }
    /// <summary>
    /// 字符串操作
    /// </summary>
    public class Estring
    {
        public static string Int16ToHEX(int num)
        {
            string fc, lc, st;
            int fn, ln;
            fn = num / 16;
            ln = num % 16;
            switch (fn)
            {
                case 0:
                    fc = "0";
                    break;
                case 1:
                    fc = "1";
                    break;
                case 2:
                    fc = "2";
                    break;
                case 3:
                    fc = "3";
                    break;
                case 4:
                    fc = "4";
                    break;
                case 5:
                    fc = "5";
                    break;
                case 6:
                    fc = "6";
                    break;
                case 7:
                    fc = "7";
                    break;
                case 8:
                    fc = "8";
                    break;
                case 9:
                    fc = "9";
                    break;
                case 10:
                    fc = "A";
                    break;
                case 11:
                    fc = "B";
                    break;
                case 12:
                    fc = "C";
                    break;
                case 13:
                    fc = "D";
                    break;
                case 14:
                    fc = "E";
                    break;
                case 15:
                    fc = "F";
                    break;
                default:
                    fc = "";
                    break;
            }
            switch (ln)
            {
                case 0:
                    lc = "0";
                    break;
                case 1:
                    lc = "1";
                    break;
                case 2:
                    lc = "2";
                    break;
                case 3:
                    lc = "3";
                    break;
                case 4:
                    lc = "4";
                    break;
                case 5:
                    lc = "5";
                    break;
                case 6:
                    lc = "6";
                    break;
                case 7:
                    lc = "7";
                    break;
                case 8:
                    lc = "8";
                    break;
                case 9:
                    lc = "9";
                    break;
                case 10:
                    lc = "A";
                    break;
                case 11:
                    lc = "B";
                    break;
                case 12:
                    lc = "C";
                    break;
                case 13:
                    lc = "D";
                    break;
                case 14:
                    lc = "E";
                    break;
                case 15:
                    lc = "F";
                    break;
                default:
                    lc = "";
                    break;
            }
            st = fc + lc;
            return st;
        }
    }
    /// <summary>
    /// 这是啥来着
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T properValue, T newValue, string properName = null)
        {
            if (object.Equals(properValue, newValue))
                return false;
            properValue = newValue;
            OnPropertyChanged(properName);
            return true;
        }
        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    /// <summary>
    /// Application拓展定义
    /// </summary>
    public partial class App : Application
    {
        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);
        public static void DoEvents()
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, exitFrameCallback, nestedFrame);
            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status !=
            DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }
        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as
            DispatcherFrame;
            frame.Continue = false;
            return null;
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int i;
        public static int nowt = 0;
        public string locate = "";
        public bool isfinished = false;
        private BackgroundWorker bgWorker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            //支持报告进度更新
            bgWorker.WorkerReportsProgress = true;
            //支持异步取消
            bgWorker.WorkerSupportsCancellation = true;
            //将DoWork_Handler绑定在RunWorkerAsync()
            bgWorker.DoWork += DoWork_Handler;
            //将ProgressChanged_Handler绑定在ReportProgress()
            bgWorker.ProgressChanged += ProgressChanged_Handler;
            //退出时发生
            bgWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;
            //创建下拉菜单List
            List<drop_down_list> Drop_down_f_g = new List<drop_down_list>();
            Drop_down_f_g.Add(new drop_down_list { Name = "安徽", ID = 0, IDS = 1 });
            Drop_down_f_g.Add(new drop_down_list { Name = "北京", ID = 1, IDS = 2 });
            Drop_down_f_g.Add(new drop_down_list { Name = "重庆", ID = 2, IDS = 3 });
            Drop_down_f_g.Add(new drop_down_list { Name = "福建", ID = 3, IDS = 4 });
            Drop_down_f_g.Add(new drop_down_list { Name = "广东", ID = 4, IDS = 5 });
            Drop_down_f_g.Add(new drop_down_list { Name = "甘肃", ID = 5, IDS = 6 });
            Drop_down_f_g.Add(new drop_down_list { Name = "广西壮族自治区", ID = 6, IDS = 7 });
            Drop_down_f_g.Add(new drop_down_list { Name = "贵州", ID = 7, IDS = 8 });
            Drop_down_f_g.Add(new drop_down_list { Name = "河北", ID = 8, IDS = 9 });
            Drop_down_f_g.Add(new drop_down_list { Name = "湖北", ID = 9, IDS = 10 });
            Drop_down_f_g.Add(new drop_down_list { Name = "黑龙江", ID = 10, IDS = 11 });
            Drop_down_f_g.Add(new drop_down_list { Name = "河南", ID = 11, IDS = 12 });
            Drop_down_f_g.Add(new drop_down_list { Name = "湖南", ID = 12, IDS = 13 });
            Drop_down_f_g.Add(new drop_down_list { Name = "海南", ID = 13, IDS = 14 });
            Drop_down_f_g.Add(new drop_down_list { Name = "吉林", ID = 14, IDS = 15 });
            Drop_down_f_g.Add(new drop_down_list { Name = "江苏", ID = 15, IDS = 16 });
            Drop_down_f_g.Add(new drop_down_list { Name = "江西", ID = 16, IDS = 17 });
            Drop_down_f_g.Add(new drop_down_list { Name = "辽宁", ID = 17, IDS = 18 });
            Drop_down_f_g.Add(new drop_down_list { Name = "内蒙古自治区", ID = 18, IDS = 19 });
            Drop_down_f_g.Add(new drop_down_list { Name = "宁夏回族自治区", ID = 19, IDS = 20 });
            Drop_down_f_g.Add(new drop_down_list { Name = "青海", ID = 20, IDS = 21 });
            Drop_down_f_g.Add(new drop_down_list { Name = "山东", ID = 21, IDS = 22 });
            Drop_down_f_g.Add(new drop_down_list { Name = "上海", ID = 22, IDS = 23 });
            Drop_down_f_g.Add(new drop_down_list { Name = "四川", ID = 23, IDS = 24 });
            Drop_down_f_g.Add(new drop_down_list { Name = "山西", ID = 24, IDS = 25 });
            Drop_down_f_g.Add(new drop_down_list { Name = "陕西", ID = 25, IDS = 26 });
            Drop_down_f_g.Add(new drop_down_list { Name = "天津", ID = 26, IDS = 27 });
            Drop_down_f_g.Add(new drop_down_list { Name = "新疆维吾尔自治区", ID = 27, IDS = 28 });
            Drop_down_f_g.Add(new drop_down_list { Name = "西藏自治区", ID = 28, IDS = 29 });
            Drop_down_f_g.Add(new drop_down_list { Name = "云南", ID = 29, IDS = 30 });
            Drop_down_f_g.Add(new drop_down_list { Name = "浙江", ID = 30, IDS = 31 });
            Drop_down_f_g.Add(new drop_down_list { Name = "香港", ID = 31, IDS = 32 });
            Drop_down_f_g.Add(new drop_down_list { Name = "澳门", ID = 32, IDS = 33 });
            Drop_down_f_g.Add(new drop_down_list { Name = "台湾", ID = 33, IDS = 34 });
            Drop_down_f_g.Add(new drop_down_list { Name = "海外", ID = 34, IDS = 35 });
            //绑定数据
            this.CLocate.ItemsSource = Drop_down_f_g;
            this.CLocate.DisplayMemberPath = "Name";
            this.CLocate.SelectedValuePath = "IDS";
            this.CLocate.SelectedIndex = 0;
        }
        /// <summary>
        /// 用于全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;//还原窗口（非最小化和最大化）
            this.WindowStyle = System.Windows.WindowStyle.None; //仅工作区可见，不显示标题栏和边框
            this.ResizeMode = System.Windows.ResizeMode.NoResize;//不显示最大化和最小化按钮
            this.Topmost = true;    //窗口在最前（仅在Debug时注释）
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            //UI初始化
            this.LWelcome.Visibility = Visibility.Visible;
            this.LTodo.Visibility = Visibility.Visible;
            this.LInit.Visibility = Visibility.Visible;
            this.LContinue.Visibility = Visibility.Visible;
            this.LIdentify.Visibility = Visibility.Visible;
            this.LNotify1.Visibility = Visibility.Collapsed;
            this.LNotify2.Visibility = Visibility.Collapsed;
            this.LShowCom.Visibility = Visibility.Collapsed;
            this.LIsChCom.Visibility = Visibility.Collapsed;
            this.RChComY.Visibility = Visibility.Collapsed;
            this.RChComN.Visibility = Visibility.Collapsed;
            this.LChComErr.Visibility = Visibility.Collapsed;
            this.LChComNot.Visibility = Visibility.Collapsed;
            this.TChComSt.Visibility = Visibility.Collapsed;
            this.LIsChPass.Visibility = Visibility.Collapsed;
            this.TChPasst.Visibility = Visibility.Collapsed;
            this.CNoChPass.Visibility = Visibility.Collapsed;
            this.LLocate.Visibility = Visibility.Collapsed;
            this.CLocate.Visibility = Visibility.Collapsed;
            this.LLogin.Visibility = Visibility.Collapsed;
            this.LNoLog.Visibility = Visibility.Collapsed;
            this.LMail.Visibility = Visibility.Collapsed;
            this.LPasswd.Visibility = Visibility.Collapsed;
            this.TMail.Visibility = Visibility.Collapsed;
            this.TPasswd.Visibility = Visibility.Collapsed;
            this.ILogo.Visibility = Visibility.Visible;
            this.ICard.Visibility = Visibility.Collapsed;
            this.IComName.Visibility = Visibility.Collapsed;
            this.IUser.Visibility = Visibility.Collapsed;
            this.BPasswd.Visibility = Visibility.Collapsed;
            this.IPas1.Visibility = Visibility.Collapsed;
            this.IPas2.Visibility = Visibility.Collapsed;
            this.IPas3.Visibility = Visibility.Collapsed;
            this.IPas4.Visibility = Visibility.Collapsed;
            this.IPas5.Visibility = Visibility.Collapsed;
            this.IPas6.Visibility = Visibility.Collapsed;
            this.ILocate.Visibility = Visibility.Collapsed;
            this.IInter.Visibility = Visibility.Collapsed;
            //内容初始化
            this.LIdentify.Content = "ID:" + Syscmd.ExecuteCMD("type C:\\IDS\\ID.txt", 0);
            this.LShowCom.Content = "您的计算机名:" + WSystemd.Systemctl.GetComputerName();
            //初始任务
            Syscmd.ExecuteCMD("taskkill /f /im dbs_form.exe", 0);
            DTime.Delay(500);
            Syscmd.ExecuteCMD("del /f /s /q dbs_form.exe", 0);
            //初始动画
            for (i = 255; i >= 0; i -= 5)
            {
                this.GridA.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                App.DoEvents();
                DTime.Delay(3);
            }
            this.GridA.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 传参
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CLocate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int test;
            object a;
            test = Convert.ToInt32(this.CLocate.SelectedValue.ToString());
            switch(test)//从数字对应到字符串(35个)
            {
                case 1:
                    locate = "安徽";
                    break;
                case 2:
                    locate = "北京";
                    break;
                case 3:
                    locate = "重庆";
                    break;
                case 4:
                    locate = "福建";
                    break;
                case 5:
                    locate = "广东";
                    break;
                case 6:
                    locate = "甘肃";
                    break;
                case 7:
                    locate = "广西壮族自治区";
                    break;
                case 8:
                    locate = "贵州";
                    break;
                case 9:
                    locate = "河北";
                    break;
                case 10:
                    locate = "湖北";
                    break;
                case 11:
                    locate = "黑龙江";
                    break;
                case 12:
                    locate = "河南";
                    break;
                case 13:
                    locate = "湖南";
                    break;
                case 14:
                    locate = "海南";
                    break;
                case 15:
                    locate = "吉林";
                    break;
                case 16:
                    locate = "江苏";
                    break;
                case 17:
                    locate = "江西";
                    break;
                case 18:
                    locate = "辽宁";
                    break;
                case 19:
                    locate = "内蒙古自治区";
                    break;
                case 20:
                    locate = "宁夏回族自治区";
                    break;
                case 21:
                    locate = "青海";
                    break;
                case 22:
                    locate = "山东";
                    break;
                case 23:
                    locate = "上海";
                    break;
                case 24:
                    locate = "四川";
                    break;
                case 25:
                    locate = "山西";
                    break;
                case 26:
                    locate = "陕西";
                    break;
                case 27:
                    locate = "天津";
                    break;
                case 28:
                    locate = "新疆维吾尔自治区";
                    break;
                case 29:
                    locate = "西藏自治区";
                    break;
                case 30:
                    locate = "云南";
                    break;
                case 31:
                    locate = "浙江";
                    break;
                case 32:
                    locate = "香港";
                    break;
                case 33:
                    locate = "澳门";
                    break;
                case 34:
                    locate = "台湾";
                    break;
                case 35:
                    locate = "海外";
                    break;
            }
        }
        /// <summary>
        /// 按下“下一步”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNext_Click(object sender, RoutedEventArgs e)
        {
            BNext.IsEnabled = false;
            switch(nowt)
            {
                case 0:
                    Syscmd.ExecuteCMD("del /f /s /q dbs_form.exe", 10);
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.LWelcome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LWelcome.Visibility = Visibility.Collapsed;
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.LTodo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LTodo.Visibility = Visibility.Collapsed;
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.LInit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LInit.Visibility = Visibility.Collapsed;
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.LContinue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LContinue.Visibility = Visibility.Collapsed;
                    for (i = 170; i > 10; i -= 5)
                    {
                        this.LIdentify.Margin = new Thickness(50, Convert.ToDouble(i), 0, 0);
                        App.DoEvents();
                        DTime.Delay(10);
                    }
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.ILogo.Visibility = Visibility.Collapsed;
                    this.ICard.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.GMask.Visibility = Visibility.Collapsed;
                    this.LNotify1.Visibility = Visibility.Visible;
                    this.LNotify2.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LNotify1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.LNotify2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    nowt += 1;
                    break;
                case 1:
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.LNotify1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.LNotify2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LNotify1.Visibility = Visibility.Collapsed;
                    this.LNotify2.Visibility = Visibility.Collapsed;
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.ICard.Visibility = Visibility.Collapsed;
                    this.IComName.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.GMask.Visibility = Visibility.Collapsed;
                    App.DoEvents();
                    this.LShowCom.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LShowCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LIsChCom.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LIsChCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.RChComN.Visibility = Visibility.Visible;
                    this.RChComY.Visibility = Visibility.Visible;
                    for (i = 255; i > 0; i -= 5)
                    {
                        this.GChPu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.GChPu.Visibility = Visibility.Collapsed;
                    nowt += 1;
                    break;
                case 2:
                    int ischcom;
                    if (this.RChComY.IsChecked == true) ischcom = 1;
                    else if (this.RChComN.IsChecked == true) ischcom = 0;
                    else
                    {
                        ischcom = -1;
                        this.LChComErr.Visibility = Visibility.Visible;
                        App.DoEvents();
                        for (i = 0; i <= 100; i++)
                        {
                            int aqs;
                            aqs = i % 4;
                            switch (aqs)
                            {
                                case 0:
                                    this.LChComErr.Margin = new Thickness(50, 330, 0, 0);
                                    break;
                                case 1:
                                    this.LChComErr.Margin = new Thickness(48, 330, 0, 0);
                                    break;
                                case 2:
                                    this.LChComErr.Margin = new Thickness(50, 330, 0, 0);
                                    break;
                                case 3:
                                    this.LChComErr.Margin = new Thickness(52, 330, 0, 0);
                                    break;
                                default:
                                    break;
                            }
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                    }
                    if(ischcom == 1)
                    {
                        nowt += 1;
                        this.LChComErr.Visibility = Visibility.Collapsed;
                        for (i = 255; i > 0; i -= 5)
                        {
                            this.LIsChCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LIsChCom.Visibility = Visibility.Collapsed;
                        this.GChPu.Visibility = Visibility.Visible;
                        for (i = 0; i < 255; i += 5)
                        {
                            this.GChPu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LChComNot.Visibility = Visibility.Visible;
                        this.TChComSt.Visibility = Visibility.Visible;
                        for (i = 0; i <= 255; i += 5)
                        {
                            this.LChComNot.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.TChComSt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                    }
                    else if(ischcom == 0)
                    {
                        nowt += 2;
                        this.LChComErr.Visibility = Visibility.Collapsed;
                        for (i = 255; i >= 0; i -= 5)
                        {
                            this.LShowCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LShowCom.Visibility = Visibility.Collapsed;
                        for (i = 255; i > 0; i -= 5)
                        {
                            this.LIsChCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LIsChCom.Visibility = Visibility.Collapsed;
                        this.GChPu.Visibility = Visibility.Visible;
                        for (i = 0; i < 255; i += 5)
                        {
                            this.GChPu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.GMask.Visibility = Visibility.Visible;
                        for (i = 0; i <= 255; i += 5)
                        {
                            this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                            App.DoEvents();
                            DTime.Delay(1);
                        }
                        this.IComName.Visibility = Visibility.Collapsed;
                        this.IUser.Visibility = Visibility.Visible;
                        this.BPasswd.Visibility = Visibility.Visible;
                        for (i = 255; i >= 0; i -= 5)
                        {
                            this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                            App.DoEvents();
                            DTime.Delay(1);
                        }
                        this.GMask.Visibility = Visibility.Collapsed;
                        App.DoEvents();
                        DTime.Delay(50);
                        this.IPas1.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.IPas2.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.IPas3.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.IPas4.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.IPas5.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.IPas6.Visibility = Visibility.Visible;
                        App.DoEvents();
                        DTime.Delay(100);
                        this.LIsChPass.Visibility = Visibility;
                        for (i = 0; i <= 255; i += 5)
                        {
                            this.LIsChPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.TChPasst.Visibility = Visibility.Visible;
                        for (i = 0; i <= 255; i += 5)
                        {
                            this.TChPasst.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.TChPasst.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.CNoChPass.Visibility = Visibility.Visible;
                    }
                    break;
                case 3:
                    string comname;
                    comname = this.TChComSt.Text;
                    if(comname == "")
                    {
                        for (i = 255; i >= 0; i -= 5)
                        {
                            this.LChComNot.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.TChComSt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                            this.TChComSt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.LShowCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LChComNot.Visibility = Visibility.Collapsed;
                        this.TChComSt.Visibility = Visibility.Collapsed;
                        this.LShowCom.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        WSystemd.Systemctl.SetComputerName(comname);
                        for (i = 255; i >= 0; i -= 5)
                        {
                            this.LChComNot.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.TChComSt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                            this.TChComSt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            this.LShowCom.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                            App.DoEvents();
                            DTime.Delay(5);
                        }
                        this.LChComNot.Visibility = Visibility.Collapsed;
                        this.TChComSt.Visibility = Visibility.Collapsed;
                        this.LShowCom.Visibility = Visibility.Collapsed;
                    }
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.IComName.Visibility = Visibility.Collapsed;
                    this.IUser.Visibility = Visibility.Visible;
                    this.BPasswd.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.GMask.Visibility = Visibility.Collapsed;
                    App.DoEvents();
                    DTime.Delay(50);
                    this.IPas1.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.IPas2.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.IPas3.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.IPas4.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.IPas5.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.IPas6.Visibility = Visibility.Visible;
                    App.DoEvents();
                    DTime.Delay(100);
                    this.LIsChPass.Visibility = Visibility;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LIsChPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.TChPasst.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.TChPasst.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TChPasst.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.CNoChPass.Visibility = Visibility.Visible;
                    nowt += 1;
                    break;
                case 4:
                    if(this.CNoChPass.IsChecked == true)
                    {
                        //这里不修改密码
                    }
                    else
                    {
                        if(TChPasst.Text == "")
                        {
                            //这里不修改密码
                        }
                        else
                        {
                            WSystemd.Users.ResetAdminPass(this.TChPasst.Text);
                        }
                    }
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LIsChPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TChPasst.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TChPasst.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LIsChPass.Visibility = Visibility.Collapsed;
                    this.TChPasst.Visibility = Visibility.Collapsed;
                    this.CNoChPass.Visibility = Visibility.Collapsed;
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.IUser.Visibility = Visibility.Collapsed;
                    this.BPasswd.Visibility = Visibility.Collapsed;
                    this.ILocate.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.GMask.Visibility = Visibility.Collapsed;
                    App.DoEvents();
                    this.LLocate.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLocate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.CLocate.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.CLocate.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        this.CLocate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    nowt += 1;
                    break;
                case 5:
                    Syscmd.ExecuteCMD("del C:\\IDS\\LOCATE.DAT /f /s /q", 0);
                    Syscmd.ExecuteCMD("echo " + locate + " >> C:\\IDS\\LOCATE.DAT", 0);
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLocate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LLocate.Visibility = Visibility.Collapsed;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.CLocate.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        this.CLocate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.CLocate.Visibility = Visibility.Collapsed;
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.ILocate.Visibility = Visibility.Collapsed;
                    this.IInter.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.GMask.Visibility = Visibility.Collapsed;
                    App.DoEvents();
                    this.LLogin.Visibility = Visibility.Visible;
                    this.LNoLog.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLogin.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.LNoLog.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LMail.Visibility = Visibility.Visible;
                    this.TMail.Visibility = Visibility.Visible;
                    for (i = 0;i <= 255; i += 5)
                    {
                        this.LMail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TMail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TMail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LPasswd.Visibility = Visibility.Visible;
                    this.TPasswd.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LPasswd.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TPasswd.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TPasswd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    nowt += 1;
                    break;
                case 6:
                    int nc = 0;
                    Syscmd.ExecuteCMD("del /f /s /q C:\\IDS\\Account.dat", 0);
                    Syscmd.ExecuteCMD("echo " + TMail.Text + ";" + TPasswd.Text + " >> C:\\IDS\\Account.dat", 0);
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLogin.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.LNoLog.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LLogin.Visibility = Visibility.Collapsed;
                    this.LNoLog.Visibility = Visibility.Collapsed;
                    this.GMask.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GMask.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(1);
                    }
                    this.IInter.Visibility = Visibility.Collapsed;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LMail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TMail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TMail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        this.LPasswd.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TPasswd.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        this.TPasswd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "F0F0F0"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LMail.Visibility = Visibility.Collapsed;
                    this.TMail.Visibility = Visibility.Collapsed;
                    this.LPasswd.Visibility = Visibility.Collapsed;
                    this.TPasswd.Visibility = Visibility.Collapsed;
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LIdentify.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    this.LIdentify.Visibility = Visibility.Collapsed;
                    //-------------------------------------------------
                    //下面是后台部署动画
                    //-------------------------------------------------
                    //您好
                    //我们正在进行一些后台设置
                    //这可能需要几分钟 __ 请勿关闭计算机
                    //--请尽情使用吧
                    //-------------------------------------------------
                    this.GridB.Visibility = Visibility.Visible;
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(6);
                    }
                    Mouse.OverrideCursor = Cursors.None;
                    this.LLast.Content = "您好";
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(1500);
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(500);
                    this.LLast.Content = "我们正在进行一些后台设置";
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(1500);
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(500);
                    this.LLast.Content = "这可能需要几分钟";
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(1000);
                    for (i = 0; i <= 136; i++)
                    {
                        this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + "0000"));
                        App.DoEvents();
                        DTime.Delay(20);
                    }
                    if (!bgWorker.IsBusy) bgWorker.RunWorkerAsync();
                    while(true)
                    {
                        if (isfinished == true) break;
                        nc = 1;
                        for (i = 0; i <= 136; i++)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF88" + Estring.Int16ToHEX(i) + "00"));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                        if (isfinished == true) break;
                        nc = 2;
                        for (i = 136; i >= 0; i--)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + "8800"));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                        if (isfinished == true) break;
                        nc = 3;
                        for (i = 0; i <= 136; i++)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0088" + Estring.Int16ToHEX(i)));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                        if (isfinished == true) break;
                        nc = 4;
                        for (i = 136; i >= 0; i--)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00" + Estring.Int16ToHEX(i) + "88"));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                        if (isfinished == true) break;
                        nc = 5;
                        for (i = 0; i <= 136; i++)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + "0088"));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                        if (isfinished == true) break;
                        nc = 6;
                        for (i = 136; i >= 0; i--)
                        {
                            this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8800" + Estring.Int16ToHEX(i)));
                            App.DoEvents();
                            DTime.Delay(20);
                        }
                    }
                    switch(nc)
                    {
                        case 1:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + Estring.Int16ToHEX(i) + "00"));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                        case 2:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00" + Estring.Int16ToHEX(i) + "00"));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                        case 3:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00" + Estring.Int16ToHEX(i) + Estring.Int16ToHEX(i)));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                        case 4:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000" + Estring.Int16ToHEX(i)));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                        case 5:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + "00" + Estring.Int16ToHEX(i)));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                        case 6:
                            for (i = 136; i >= 0; i--)
                            {
                                this.GridB.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Estring.Int16ToHEX(i) + "0000"));
                                App.DoEvents();
                                DTime.Delay(20);
                            }
                            break;
                    }
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(500);
                    this.LLast.Content = "请尽情使用吧";
                    for (i = 0; i <= 255; i += 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(1500);
                    for (i = 255; i >= 0; i -= 5)
                    {
                        this.LLast.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(1000);
                    this.ILast.Visibility = Visibility.Visible;
                    this.GLast.Visibility = Visibility.Visible;
                    for (i = 255; i >= 0; i--)
                    {
                        this.GLast.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
                        App.DoEvents();
                        DTime.Delay(5);
                    }
                    DTime.Delay(2000);
                    Mouse.OverrideCursor = null;
                    Application.Current.Shutdown();
                    break;
                default:
                    WinMessage.MessageBox(IntPtr.Zero, "未定义的实例！\n请检查程序运行情况或通知开发者。", "Fatal Error", WinMessage.MB_OK | WinMessage.ICON_ERROR);
                    Application.Current.Shutdown();
                    break;
            }
            BNext.IsEnabled = true;
        }
        /// <summary>
        /// 选中“不修改密码”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CNoChPass_Checked(object sender, RoutedEventArgs e)
        {
            this.TChPasst.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
            this.TChPasst.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
            this.TChPasst.IsEnabled = false;
            App.DoEvents();
        }
        /// <summary>
        /// 取消选中“不修改密码”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CNoChPass_Unchecked(object sender, RoutedEventArgs e)
        {
            this.TChPasst.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
            this.TChPasst.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            this.TChPasst.IsEnabled = true;
            App.DoEvents();
        }
        /// <summary>
        /// 后台线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            //TODO:在这里放置代码
            Syscmd.ExecuteCMD("attrib /d +H C:\\IDS", 0);
            DTime.Delay(30000);
            isfinished = true;
            this.Dispatcher.Invoke(new Action(delegate
            {
                //TODO:在这里放置更改UI的代码
                //INFO:只放置用于更改UI的代码
            }));
        }
        /// <summary>
        /// 执行完成或正常退出后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            if (args.Cancelled)
            {
                //如果取消
                //TODO:在这里放置代码
            }
            else
            {
                //如果正常结束
                //TODO:在这里放置代码
            }
        }
        /// <summary>
        /// 进度更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
        {
            //TODO:在这里放置代码
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new System.Windows.Forms.MethodInvoker(delegate ()
            {

            }));
        }
    }
}
