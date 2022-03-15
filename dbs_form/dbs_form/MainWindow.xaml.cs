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

namespace dbs_form
{
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
	public class Syscmd
	{
		//dosCommand Dos命令语句
		public string ExecuteCMD(string CmdCommand)
		{
			return ExecuteCMD(CmdCommand, 10);
		}
		/// <summary>
		/// 执行DOS命令，返回DOS命令的输出
		/// </summary>
		/// <param name="dosCommand">dos命令</param>
		/// <param name="milliseconds">等待命令执行的时间（单位：毫秒），
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
		public static void ShellExecute(string path,int values)
		{
			if (values == 3)
			{
				ExecuteCMD(path, 0);
			}
			else
			{
				switch(values)
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
	public class Estring
	{
		public static string Int16ToHEX(int num)
		{
			string fc, lc, st;
			int fn, ln;
			fn = num / 16;
			ln = num % 16;
			switch(fn)
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
		public int nowt = 0;
		public string netchk = "I can swallow the glass without hurting my body.\n";
		public int processing = 0;
		public bool ispro = false;
		private BackgroundWorker bgWorker = new BackgroundWorker();
		private int i = 0;
		int month = 0;
		int day = 0;
		public MainWindow()
		{
			Syscmd.ExecuteCMD("del index.php /f /s /q", 50);
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
			//可视化初始化
			this.Lwelcome.Visibility = Visibility.Visible;
			this.Lcontinue.Visibility = Visibility.Visible;
			this.ILoading.Visibility = Visibility.Hidden;
			this.Lweb.Visibility = Visibility.Hidden;
			this.Lnoweb.Visibility = Visibility.Hidden;
			this.Lbirth.Visibility = Visibility.Hidden;
			this.Tm.Visibility = Visibility.Hidden;
			this.Td.Visibility = Visibility.Hidden;
			this.Lm.Visibility = Visibility.Hidden;
			this.Ld.Visibility = Visibility.Hidden;
			this.Lload.Visibility = Visibility.Hidden;
			this.Lnotice.Visibility = Visibility.Hidden;
			this.Lnotice2.Visibility = Visibility.Hidden;
			this.Lnotice3.Visibility = Visibility.Hidden;
			this.Lnotice4.Visibility = Visibility.Hidden;
			this.Lupload.Visibility = Visibility.Hidden;
			this.Lend1.Visibility = Visibility.Hidden;
			this.Lend2.Visibility = Visibility.Hidden;
			//播放音频
			Media.PlaySound("welcome.wav", IntPtr.Zero, Media.SND_FILENAME | Media.SND_ASYNC);
		}
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Space)
			{
				BNext_Click(sender, e);
			}
		}
		private void BCancel_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		private void BNext_Click(object sender, RoutedEventArgs e)
		{
			BaseViewModel ViewModel = new BaseViewModel();
			bool isvol = true;
			switch (nowt)
			{
				case 0:
					for (i = 255; i > 0; i -= 5)
					{
						this.Lwelcome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						this.Lcontinue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						App.DoEvents();
						DTime.Delay(5);
					}
					this.Lwelcome.Visibility = Visibility.Hidden;
					this.Lcontinue.Visibility = Visibility.Hidden;
					this.ILoading.Visibility = Visibility.Visible;
					this.Lweb.Visibility = Visibility.Visible;
					this.Pa1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
					this.Pa2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
					App.DoEvents();
					if (!bgWorker.IsBusy) bgWorker.RunWorkerAsync();//执行后台指令
					break;
				case 1:
					if (Tm.Text == "" || Td.Text == "")
                    {
						WinMessage.MessageBox(IntPtr.Zero, "请以数字形式输入有效的日期！", "日期错误", WinMessage.MB_OK | WinMessage.ICON_ERROR);
						isvol = false;
					}
					else
                    {
						month = Convert.ToInt32(Tm.Text);
						day = Convert.ToInt32(Td.Text);
						isvol = true;
						if (month > 12 || month < 1)
						{
							WinMessage.MessageBox(IntPtr.Zero, "请以数字形式输入有效的日期！", "日期错误", WinMessage.MB_OK | WinMessage.ICON_ERROR);
							isvol = false;
						}
						else
						{
							if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
							{
								if (day > 31 || day < 1)
								{
									WinMessage.MessageBox(IntPtr.Zero, "请以数字形式输入有效的日期！", "日期错误", WinMessage.MB_OK | WinMessage.ICON_ERROR);
									isvol = false;
								}
							}
							else if (month == 4 || month == 6 || month == 9 || month == 11)
							{
								if (day > 30 || day < 1)
								{
									WinMessage.MessageBox(IntPtr.Zero, "请以数字形式输入有效的日期！", "日期错误", WinMessage.MB_OK | WinMessage.ICON_ERROR);
									isvol = false;
								}
							}
							else
							{
								if (day > 29 || day < 1)
								{
									WinMessage.MessageBox(IntPtr.Zero, "请以数字形式输入有效的日期！", "日期错误", WinMessage.MB_OK | WinMessage.ICON_ERROR);
									isvol = false;
								}
							}
						}
					}
					if (isvol)
					{
						if (!bgWorker.IsBusy)
						{
							for (i = 255; i > 0; i -= 5)
							{
								this.Lbirth.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								this.Tm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
								this.Tm.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								this.Td.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "000000"));
								this.Td.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								this.Lm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								this.Ld.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								App.DoEvents();
								DTime.Delay(5);
							}
							this.Lbirth.Visibility = Visibility.Hidden;
							this.Tm.Visibility = Visibility.Hidden;
							this.Td.Visibility = Visibility.Hidden;
							this.Lm.Visibility = Visibility.Hidden;
							this.Ld.Visibility = Visibility.Hidden;
							this.Lload.Visibility = Visibility.Visible;
							this.ILoading.Visibility = Visibility.Visible;
							App.DoEvents();
							bgWorker.RunWorkerAsync();
						}
					}
					break;
				case 2:
					for (i = 255; i > 0; i -= 5)
					{
						this.Lnotice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						this.Lnotice2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						this.Lnotice3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						this.Lnotice4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
						App.DoEvents();
						DTime.Delay(5);
					}
					this.Lupload.Visibility = Visibility.Visible;
					this.Lnotice.Visibility = Visibility.Hidden;
					this.Lnotice2.Visibility = Visibility.Hidden;
					this.Lnotice3.Visibility = Visibility.Hidden;
					this.Lnotice4.Visibility = Visibility.Hidden;
					this.ILoading.Visibility = Visibility.Visible;
					this.Pa3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
					this.Pa4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
					App.DoEvents();
					if (!bgWorker.IsBusy) bgWorker.RunWorkerAsync();
					break;
				case 3:
					Application.Current.Shutdown();
					break;
				default:
					WinMessage.MessageBox(IntPtr.Zero, "未定义的实例！请检查程序运行源\n错误： \'nowt\' 超出允许的范围", "Fatal Error", WinMessage.MB_OK | WinMessage.ICON_ERROR);
					Application.Current.Shutdown();
					break;
			}
		}
		/// <summary>
		/// 后台线程
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void DoWork_Handler(object sender, DoWorkEventArgs args)
		{
			switch(nowt)
			{
				case 0:
					Syscmd.ExecutePwsh("wget -Uri \"https://www.drblack-system.com/index.php\" -OutFile \"index.php\"", 10000);
					break;
				case 1:
					Syscmd.ExecuteCMD("del C:\\IDS\\SR.txt /f /s /q", 0);
					Syscmd.ExecuteCMD("md C:\\IDS", 0);
					if (month < 10)
					{
						Syscmd.ExecuteCMD("echo 0" + Convert.ToString(month) + "-" + Convert.ToString(day) + " > C:\\IDS\\SR.txt", 0);
					}
					else
					{
						Syscmd.ExecuteCMD("echo " + Convert.ToString(month) + "-" + Convert.ToString(day) + " > C:\\IDS\\SR.txt", 0);
					}
					Syscmd.ShellExecute("dbs_dolog.exe", Syscmd.SW_MIN);
					DTime.Delay(1000);
					break;
				case 2:
					Syscmd.ShellExecute("dbs_uplogx.exe", Syscmd.SW_MIN);
					DTime.Delay(5000);
					break;
				default:
					break;
			}
			//Info:只在下面放更改UI界面的代码
			Dispatcher.BeginInvoke(new Action(delegate
			{
				switch (nowt)
				{
					case 0:
						if (File.Exists("index.php"))
						{
							this.ILoading.Visibility = Visibility.Hidden;
							for (i = 255; i > 0; i -= 5)
							{
								this.Lweb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								App.DoEvents();
								DTime.Delay(5);
							}
							this.Lweb.Visibility = Visibility.Hidden;
							this.Lbirth.Visibility = Visibility.Visible;
							this.Tm.Visibility = Visibility.Visible;
							this.Td.Visibility = Visibility.Visible;
							this.Lm.Visibility = Visibility.Visible;
							this.Ld.Visibility = Visibility.Visible;
							nowt += 1;
							App.DoEvents();
						}
						else
						{
							//WinMessage.MessageBox(IntPtr.Zero, "No Server Connect!", "Web Error", WinMessage.MB_OK | WinMessage.ICON_ERROR);
							this.ILoading.Visibility = Visibility.Hidden;
							for (i = 255; i > 0; i -= 5)
							{
								this.Lweb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
								App.DoEvents();
								DTime.Delay(5);
							}
							this.Lweb.Visibility = Visibility.Hidden;
							this.Lnoweb.Visibility = Visibility.Visible;
							App.DoEvents();
							DTime.Delay(10000);
							Application.Current.Shutdown();
						}
						break;
					case 1:
						this.ILoading.Visibility = Visibility.Hidden;
						for (i = 255; i > 0; i -= 5)
						{
							this.Lload.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
							App.DoEvents();
							DTime.Delay(5);
						}
						this.Lload.Visibility = Visibility.Hidden;
						this.Lnotice.Visibility = Visibility.Visible;
						this.Lnotice2.Visibility = Visibility.Visible;
						this.Lnotice3.Visibility = Visibility.Visible;
						this.Lnotice4.Visibility = Visibility.Visible;
						this.Pa2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
						this.Pa3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
						App.DoEvents();
						nowt += 1;
						break;
					case 2:
						this.ILoading.Visibility = Visibility.Hidden;
						for (i = 255; i > 0; i -= 5)
						{
							this.Lupload.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + Estring.Int16ToHEX(i) + "FFFFFF"));
							App.DoEvents();
							DTime.Delay(5);
						}
						this.Lupload.Visibility = Visibility.Hidden;
						this.Lend1.Visibility = Visibility.Visible;
						this.Lend2.Visibility = Visibility.Visible;
						this.Pa4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
						this.Pa5.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
						App.DoEvents();
						nowt += 1;
						//Syscmd.ExecuteCMD("start dbs_oobe.exe", 0);
						break;
					default:
						break;
				}
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
		}
	}
}