using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using MMM_Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MMM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private ObservableCollection<GameIconItem> GameIconItemList = new ObservableCollection<GameIconItem>();
        private ObservableCollection<InfoBarItem> InfoBarItemList = new ObservableCollection<InfoBarItem>();
        private Compositor compositor;
        private Visual imageVisual;

        // NEW: 添加一个 D3dxIniConfig 实例来管理d3dx.ini文件的状态
        private D3dxIniConfig d3dxIni;

        public GamePage()
        {
            this.InitializeComponent();

            // 初始化Composition组件
            // 获取Image控件的Visual对象
            imageVisual = ElementCompositionPreview.GetElementVisual(MainWindow.CurrentWindow.mainWindowImageBrush);
            // 获取Compositor实例
            compositor = imageVisual.Compositor;

            //读取配置
            GlobalConfig.SettingCfg.LoadConfig();

            GameIconGridView.ItemsSource = GameIconItemList;
            InfoBarGridView.ItemsSource = InfoBarItemList;

            AddGameIcon();
            AddInfoBarIcon();

            //根据当前配置中存储的游戏名称，依次匹配GameIconItemList

            int index = 0;
            foreach (GameIconItem gameIconItem in GameIconItemList)
            {
                if (gameIconItem.GameName == GlobalConfig.SettingCfg.Value.GameName)
                {
                    GameIconGridView.SelectedIndex = index;
                    break;
                }
                index += 1;
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 执行你想要在这个页面被关闭或导航离开时运行的代码
            GlobalConfig.SettingCfg.SaveConfig();

            // 如果需要，可以调用基类的 OnNavigatedFrom 方法
            base.OnNavigatedFrom(e);
        }

        public void AddInfoBarIcon()
        {

            InfoBarItemList.Add(new InfoBarItem
            {
                Link = "",
                Description = "加入QQ群",
                IconImage = "Assets/InfoBarIcon/QQGroup.png",
            });

            InfoBarItemList.Add(new InfoBarItem
            {
                Link = "https://discord.gg/sMdsGAptss",
                Description = "加入Discord",
                IconImage = "Assets/InfoBarIcon/Discord.png",
            });

            InfoBarItemList.Add(new InfoBarItem
            {
                Link = "https://github.com/StarBobis/MMM",
                Description = "打开Github更新地址",
                IconImage = "Assets/InfoBarIcon/Github.png",
            });

            InfoBarItemList.Add(new InfoBarItem
            {
                Link = "https://github.com/StarBobis/MMM/issues",
                Description = "打开Github的Issue界面提交反馈和改进建议",
                IconImage = "Assets/InfoBarIcon/Feedback.png",
            });
        }

        public void AddGameIcon()
        {
            GameIconItemList.Add(new GameIconItem
            {
                GameIconImage = "Assets/GameIcon/GI.png",
                GameName = "原神",
                GameBackGroundImage = new BitmapImage(new Uri(Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/原神.png")))
            });

            GameIconItemList.Add(new GameIconItem
            {
                GameIconImage = "Assets/GameIcon/HI3.png",
                GameName = "崩坏三",
                GameBackGroundImage = new BitmapImage(new Uri(Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/崩坏三.png")))
            });

            GameIconItemList.Add(new GameIconItem
            {
                GameIconImage = "Assets/GameIcon/HSR.png",
                GameName = "崩坏：星穹铁道",
                GameBackGroundImage = new BitmapImage(new Uri(Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/崩坏：星穹铁道.png")))
            });

            GameIconItemList.Add(new GameIconItem
            {
                GameIconImage = "Assets/GameIcon/ZZZ.png",
                GameName = "绝区零",
                GameBackGroundImage = new BitmapImage(new Uri(Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/绝区零.png")))
            });

            GameIconItemList.Add(new GameIconItem
            {
                GameIconImage = "Assets/GameIcon/鸣潮.png",
                GameName = "鸣潮",
                GameBackGroundImage = new BitmapImage(new Uri(Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/鸣潮.png")))
            });
        }

        private void CreateFadeAnimation()
        {
            // 创建一个淡入淡出动画
            var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(0.0f, 0.0f); // 初始透明度0%
            fadeAnimation.InsertKeyFrame(1.0f, 1.0f); // 目标透明度100%
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(500); // 动画持续时间300毫秒
            fadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay; // 动画延迟行为

            // 应用动画到Image的Visual对象的Opacity属性
            imageVisual.StartAnimation("Opacity", fadeAnimation);
        }
        private void CreateScaleAnimation()
        {
            // 创建一个缩放动画
            var scaleAnimation = compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(0.0f, new Vector3(1.05f, 1.05f, 1.05f)); // 初始缩放比例110%
            scaleAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 1.0f)); // 目标缩放比例100%
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(500); // 动画持续时间300毫秒
            scaleAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay; // 动画延迟行为

            // 应用动画到Image的Visual对象的Scale属性
            imageVisual.StartAnimation("Scale", scaleAnimation);
        }

        public GameIconItem GetCurrentSelectedGameIconItem()
        {
            if (GameIconGridView.SelectedItem != null)
            {
                int index = GameIconGridView.SelectedIndex;
                GameIconItem gameIconItem = GameIconItemList[index];
                return gameIconItem;
            }
            else
            {
                return null;
            }
        }

        // NEW HELPER METHOD: 创建一个新的辅助方法来初始化或重新加载d3dx.ini配置
        private void InitializeD3dxConfig(string migotoFolderPath)
        {
            if (string.IsNullOrEmpty(migotoFolderPath) || !Directory.Exists(migotoFolderPath))
            {
                d3dxIni = null; // 如果路径无效，则清空配置实例
                return;
            }

            string d3dxini_path = Path.Combine(migotoFolderPath, "d3dx.ini");
            if (File.Exists(d3dxini_path))
            {
                // 创建D3dxIniConfig实例，加载文件内容到内存
                d3dxIni = new D3dxIniConfig(d3dxini_path);
                // 从内存中读取配置并更新UI
                ReadPathSettingFromD3dxIni();
            }
            else
            {
                d3dxIni = null; // 如果ini文件不存在，也清空配置实例
            }
        }

        private void GameIconGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameIconGridView.SelectedItem != null)
            {
                int index = GameIconGridView.SelectedIndex;
                GameIconItem gameIconItem = GameIconItemList[index];

                Debug.WriteLine(GlobalConfig.SettingCfg.Value.GameName);
                //设置当前游戏并且保存
                GlobalConfig.SettingCfg.Value.GameName = gameIconItem.GameName;
                GlobalConfig.SettingCfg.SaveConfig();

                string BackgroundPath = Path.Combine(GlobalConfig.Path_Base, "Assets/GameBackground/" + gameIconItem.GameName + ".png");

                CreateScaleAnimation();
                CreateFadeAnimation();
                MainWindow.CurrentWindow.mainWindowImageBrush.Source = gameIconItem.GameBackGroundImage;

                if (File.Exists(GlobalConfig.Path_CurrentGameMainConfigJsonFile))
                {
                    JObject jObject = MMMJsonUtils.ReadJObjectFromFile(GlobalConfig.Path_CurrentGameMainConfigJsonFile);
                    string MigotoFolder = (string)jObject["MigotoFolder"];
                    MigotoPathTextBox.Text = MigotoFolder;
                    GlobalConfig.SettingCfg.Value.CurrentGameMigotoFolder = MigotoFolder;

                    // MODIFIED: 使用新的辅助方法来初始化d3dx.ini的配置
                    InitializeD3dxConfig(MigotoFolder);
                }
                else
                {
                    MigotoPathTextBox.Text = "";
                    ProcessPathTextBox.Text = "";
                    StarterPathTextBox.Text = "";
                    TextBox_LaunchArgs.Text = "";
                    d3dxIni = null; // 清空ini配置
                }
            }
        }

        // MODIFIED: 此方法不再需要路径参数，因为它将使用实例变量d3dxIni
        public void ReadPathSettingFromD3dxIni()
        {
            // 确保d3dxIni实例存在
            if (d3dxIni == null) return;

            // 从d3dxIni实例（内存）中读取属性
            ProcessPathTextBox.Text = d3dxIni.ReadAttribute("target").Trim();
            StarterPathTextBox.Text = d3dxIni.ReadAttribute("launch").Trim();
            TextBox_LaunchArgs.Text = d3dxIni.ReadAttribute("launch_args").Trim();

            string DevStr = d3dxIni.ReadAttribute("dev").Trim();
            if (DevStr.Trim() == "1")
            {
                ToggleSwitch_ShowWarning.IsOn = false;
            }
            else
            {
                // 默认行为，包括"0"或空字符串
                ToggleSwitch_ShowWarning.IsOn = true;
            }
        }


        private async void Button_Choose3DmigotoFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = await CommandHelper.ChooseFolderAndGetPath();
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            MigotoPathTextBox.Text = folderPath;

            string d3dxini_path = Path.Combine(folderPath, "d3dx.ini");
            if (!File.Exists(d3dxini_path))
            {
                _ = MessageHelper.Show("您当前选中的目录中并未含有d3dx.ini配置文件，请确认您是否选中了正确的3Dmigoto目录。");
                d3dxIni = null; // 清空配置
            }
            else
            {
                // MODIFIED: 使用新的辅助方法来初始化d3dx.ini的配置
                InitializeD3dxConfig(folderPath);

                GameIconItem gameIconItem = GetCurrentSelectedGameIconItem();
                if (gameIconItem != null)
                {
                    gameIconItem.MigotoFolder = folderPath;
                    gameIconItem.SaveToJson(GlobalConfig.Path_CurrentGameMainConfigJsonFile);
                    GlobalConfig.SettingCfg.SaveConfig();
                }
            }
        }

        private void Button_InitializePath_Click(object sender, RoutedEventArgs e)
        {
            ProcessPathTextBox.Text = "";
            StarterPathTextBox.Text = "";
            TextBox_LaunchArgs.Text = "";
        }

        private async void Button_ChooseProcessFile_Click(object sender, RoutedEventArgs e)
        {
            string filepath = await CommandHelper.ChooseFileAndGetPath(".exe");
            if (filepath != "")
            {
                ProcessPathTextBox.Text = filepath;
            }
        }

        private async void Button_ChooseStarterFile_Click(object sender, RoutedEventArgs e)
        {
            string filepath = await CommandHelper.ChooseFileAndGetPath(".exe");
            if (filepath != "")
            {
                StarterPathTextBox.Text = filepath;
            }
        }

        // MODIFIED: "保存配置"按钮现在是唯一保存d3dx.ini的地方
        // MODIFIED: 根据新需求修改了保存逻辑
        private async void Button_SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            // 检查d3dxIni实例是否有效
            if (d3dxIni == null)
            {
                await MessageHelper.Show("保存失败：尚未选择有效的3Dmigoto目录，无法找到d3dx.ini文件。");
                return;
            }

            try
            {
                bool hasChanges = false;

                // NEW: 只有当 ProcessPathTextBox 不为空时，才设置 "target" 属性
                if (!string.IsNullOrWhiteSpace(ProcessPathTextBox.Text))
                {
                    d3dxIni.SetAttribute("[loader]", "target", ProcessPathTextBox.Text);
                    hasChanges = true;
                }

                // NEW: 只有当 StarterPathTextBox 不为空时，才设置 "launch" 属性
                if (!string.IsNullOrWhiteSpace(StarterPathTextBox.Text))
                {
                    d3dxIni.SetAttribute("[loader]", "launch", StarterPathTextBox.Text);
                    hasChanges = true;
                }

                // NEW: 只有当 TextBox_LaunchArgs 不为空时，才设置 "launch_args" 属性
                if (!string.IsNullOrWhiteSpace(TextBox_LaunchArgs.Text))
                {
                    d3dxIni.SetAttribute("[loader]", "launch_args", TextBox_LaunchArgs.Text);
                    hasChanges = true;
                }

                // ToggleSwitch 的状态总是有效的，所以照常保存
                if (ToggleSwitch_ShowWarning.IsOn)
                {
                    d3dxIni.SetAttribute("[Logging]", "dev", "0");
                }
                else
                {
                    d3dxIni.SetAttribute("[Logging]", "dev", "1");
                }
                hasChanges = true; // 开关状态也算作一项更改


                if (hasChanges)
                {
                    // 调用一次 Save() 方法，将所有更改写入文件
                    d3dxIni.Save();
                    await MessageHelper.Show("保存成功");
                }
                else
                {
                    // 如果什么都没输入，可以给一个提示（可选）
                    await MessageHelper.Show("没有需要保存的更改。");
                }
            }
            catch (Exception ex)
            {
                await MessageHelper.Show("保存失败：" + ex.ToString());
            }
        }

        private async void Button_OpenD3DXINI_Click(object sender, RoutedEventArgs e)
        {
            await CommandHelper.ShellOpenFile(GlobalConfig.Path_D3DXINI);
        }

        private async void Button_Open3DmigotoFolder_Click(object sender, RoutedEventArgs e)
        {
            await CommandHelper.ShellOpenFolder(GlobalConfig.SettingCfg.Value.CurrentGameMigotoFolder);
        }

        private async void Button_OpenShaderFixesFolder_Click(object sender, RoutedEventArgs e)
        {
            await CommandHelper.ShellOpenFolder(Path.Combine(GlobalConfig.SettingCfg.Value.CurrentGameMigotoFolder, "ShaderFixes\\"));
        }

        // MODIFIED: 移除此处的自动保存逻辑
        private void ToggleSwitch_DllMode_Toggled(object sender, RoutedEventArgs e)
        {
            string dllToCopy = "";
            if (ToggleSwitch_DllMode.IsOn)
            {
                //切换到Play版本的d3d11.dll
                dllToCopy = Path.Combine(GlobalConfig.Path_3DmigotoGameModForkFolder, "ReleaseX64Play\\d3d11.dll");
            }
            else
            {
                //切换到开发版本的d3d11.dll
                dllToCopy = Path.Combine(GlobalConfig.Path_3DmigotoGameModForkFolder, "ReleaseX64Dev\\d3d11.dll");
            }

            string targetDllPath = Path.Combine(GlobalConfig.Path_LoaderFolder, "d3d11.dll");
            try
            {
                File.Copy(dllToCopy, targetDllPath, true);
                _ = CommandHelper.ShellOpenFolder(GlobalConfig.Path_LoaderFolder);
            }
            catch (Exception ex)
            {
                _ = MessageHelper.Show("切换d3d11.dll失败! 当前游戏使用的d3d11.dll可能已被占用，请先关闭游戏进程和游戏的官方启动器。\n\n" + ex.ToString());
            }
        }

        private async void Button_Run3DmigotoLoader_Click(object sender, RoutedEventArgs e)
        {
            string MigotoLoaderExePath1 = Path.Combine(GlobalConfig.Path_LoaderFolder, "3Dmigoto Loader.exe");
            if (!File.Exists(MigotoLoaderExePath1))
            {
                string OriginalMigotoLoaderExePath = Path.Combine(GlobalConfig.Path_Base, "3Dmigoto-GameMod-Fork\\3Dmigoto Loader.exe");
                File.Copy(OriginalMigotoLoaderExePath, MigotoLoaderExePath1, true);
            }

            await CommandHelper.ShellOpenFile(MigotoLoaderExePath1);
        }

        private void GridViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (InfoBarGridView.SelectedItem != null)
            {
                int index = InfoBarGridView.SelectedIndex;
                InfoBarItem infoBarItem = InfoBarItemList[index];
                string link = infoBarItem.Link;
                if (!string.IsNullOrEmpty(link))
                {
                    IAsyncOperation<bool> asyncOperation = Launcher.LaunchUriAsync(new Uri(link));
                }
            }
        }

        // --- REMOVED AUTO-SAVE EVENTS ---
        // 以下事件处理器中的自动保存逻辑已被移除。
        // 用户的输入会保留在UI控件中，直到点击“保存配置”按钮。

        private void ToggleSwitch_ShowWarning_Toggled(object sender, RoutedEventArgs e)
        {
            // 自动保存逻辑已移除
        }

        private void ProcessPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 自动保存逻辑已移除
        }

        private void StarterPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 自动保存逻辑已移除
        }

        private void TextBox_LaunchArgs_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 自动保存逻辑已移除
        }
    }
}