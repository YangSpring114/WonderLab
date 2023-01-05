using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Skia;
using Avalonia.Themes.Fluent;
using FluentAvalonia.Styling;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Class.Model.Launch;
using Newtonsoft.Json;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab
{
    public partial class App : Application
    {
        public App()
        {
            //ServicePointManager.DefaultConnectionLimit = 512;
            InitializeModData();
            CheckAsync();
            //Debug.WriteLine(Convert.ToDouble(double.NaN));
        }

        /// <summary>
        /// 活动核心
        /// </summary>
        public static GameCore CurrentGameCore { get; set; } = new();
        public static DataModels Data { get; set; } = new DataModels();
        /// <summary>
        /// 预启动数据检查
        /// </summary>
        public async void CheckAsync()
        {
            JsonToolkit.JsonAllWrite();
            await Task.Delay(900);
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                if (Data is not null)
                {
                    //Java
                    Data.JavaList = Data.JavaList.Distinct().ToList();
                    Data.JavaList.ForEach(x => 
                    {
                        if (!File.Exists(x))
                            Data.JavaList.Remove(x);
                    });

                    //Game Footer
                    Data.GameFooterList = Data.GameFooterList.Distinct().ToList();
                    Data.GameFooterList.ForEach(x =>
                    {
                        if (!Directory.Exists(x))
                            Data.GameFooterList.Remove(x);
                    });

                    //GameCore
                    if (!string.IsNullOrEmpty(Data.FooterPath) && !string.IsNullOrEmpty(Data.SelectedGameCore))
                    {
                        foreach (var i in GameCoreToolkit.GetGameCores(Data.FooterPath))
                        {
                            if (Data.SelectedGameCore.Equals(i.Id))
                                break;
                        }
                    }
                    else Data.SelectedGameCore = string.Empty;
                }
            };
            worker.RunWorkerAsync();
        }
        public async void InitializeModData()
        {
            var al = AvaloniaLocator.Current.GetService<IAssetLoader>();
            await Task.Run(() =>
            {
                using var s = al.Open(new Uri("resm:WonderLab.Resources.ModData.json"));
                StreamReader stream = new(s);
                var model = JsonConvert.DeserializeObject<List<ModLangDataModel>>(stream.ReadToEnd());
                model.ForEach(x => InfoConst.ModLangDatas.Add(x.CurseForgeId, x));

                InfoConst.ModLangDatas.Values.ToList().ForEach(x =>
                {
                    if (x.Chinese.Contains("*"))
                        x.Chinese = x.Chinese.Replace("*",
                            " (" + string.Join(" ", x.CurseForgeId.Split("-").Select(w => w.Substring(0, 1).ToUpper() + w.Substring(1, w.Length - 1))) + ")");
                });
            });
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
        public override void RegisterServices()
        {            
            //AvaloniaLocator.CurrentMutable.Bind<IFontManagerImpl>().ToConstant(new CustomFontManagerImpl());
            base.RegisterServices();
        }
    }

    public class CustomFontManagerImpl : IFontManagerImpl
    {
        private readonly Typeface[] _customTypefaces;
        private readonly string _defaultFamilyName;
        //resm:WonderLab.Resources.HarmonyOS#HarmonyOS Sans
        //Load font resources in the project, you can load multiple font resources
        private readonly Typeface _defaultTypeface =
            new Typeface("resm:WonderLab.Resources.HarmonyOS#HarmonyOS Sans");

        public CustomFontManagerImpl()
        {
            _customTypefaces = new[] { _defaultTypeface };
            _defaultFamilyName = _defaultTypeface.FontFamily.FamilyNames.PrimaryFamilyName;
        }

        public string GetDefaultFontFamilyName()
        {
            return _defaultFamilyName;
        }

        public IEnumerable<string> GetInstalledFontFamilyNames(bool checkForUpdates = false)
        {
            return _customTypefaces.Select(x => x.FontFamily.Name);
        }

        private readonly string[] _bcp47 = { CultureInfo.CurrentCulture.ThreeLetterISOLanguageName, CultureInfo.CurrentCulture.TwoLetterISOLanguageName };

        public bool TryMatchCharacter(int codepoint, FontStyle fontStyle, FontWeight fontWeight, FontFamily fontFamily,
            CultureInfo culture, out Typeface typeface)
        {
            foreach (var customTypeface in _customTypefaces)
            {
                if (customTypeface.GlyphTypeface.GetGlyph((uint)codepoint) == 0)
                {
                    continue;
                }

                typeface = new Typeface(customTypeface.FontFamily.Name, fontStyle, fontWeight);

                return true;
            }

            var fallback = SKFontManager.Default.MatchCharacter(fontFamily?.Name, (SKFontStyleWeight)fontWeight,
                SKFontStyleWidth.Normal, (SKFontStyleSlant)fontStyle, _bcp47, codepoint);

            typeface = new Typeface(fallback?.FamilyName ?? _defaultFamilyName, fontStyle, fontWeight);

            return true;
        }

        public IGlyphTypefaceImpl CreateGlyphTypeface(Typeface typeface)
        {
            SKTypeface skTypeface = default;
            Trace.WriteLine(typeface.FontFamily.Name);
            switch (typeface.FontFamily.Name)
            {
                case "HarmonyOS Sans":
                case FontFamily.DefaultFontFamilyName:
                case "微软雅黑":  //font family name
                   skTypeface = SKTypeface.FromFamilyName(_defaultTypeface.FontFamily.Name); break;
                case "Symbols":
                    skTypeface = SKTypeface.FromFamilyName("Symbols");
                    break;
                //default:
                    //skTypeface = SKTypeface.FromFamilyName(typeface.FontFamily.Name,
                    //    (SKFontStyleWeight)typeface.Weight, SKFontStyleWidth.Normal, (SKFontStyleSlant)typeface.Style);
                //    break;
            }
            //skTypeface = SKTypeface.FromFamilyName("Symbols");

            skTypeface = SKTypeface.FromFamilyName(_defaultTypeface.FontFamily.Name);
            return new GlyphTypefaceImpl(skTypeface);
        }
    }
}
//Symbolsresm:WonderLab.Resources.HarmonyOS#HarmonyOS Sans