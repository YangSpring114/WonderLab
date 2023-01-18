using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.Modules.Const;
using WonderLab.Modules.Models;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 字符串处理工具类
    /// </summary>
    public class StringToolkit
    {
        /// <summary>
        /// mod搜索前检查步骤(中文名检查并转换)
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static ModLangDataModel ModNameSearchCheck(string modname)
        {
            modname = modname.Trim();
            List<ModLangDataModel> mods = new List<ModLangDataModel>();//缓存集合
            ModLangDataModel res = null!;
            if (IsChinese(modname))
            {
                //中文请求关键字处理
                foreach (var i in InfoConst.ModLangDatas)
                    if (i.Value.Chinese.Contains(modname))
                    {
                        mods.Add(i.Value);
                        if (i.Value.Chinese.Split(" (").First() == modname)
                            res = i.Value;
                    }

                if (res is null)//未匹配到一模一样的mod，在缓存中寻找相似的mod
                {
                    mods.ForEach(x =>
                    {
                        res = x;
                    });
                }

                if (res is not null)//去处常见连接词
                {
                    res.CurseForgeId = res.Chinese.Split(" (")[1].Split(")").First();
                    res.CurseForgeId = res.CurseForgeId.Trim();
                }
            }
            return res;
        }

        public static bool IsChinese(string input) => Regex.IsMatch(input, "[\u4e00-\u9fbb]");

        //public void DlCfProjectSub(string RawFilter)
        //{
        //    RawFilter = RawFilter.ToLower();
        //    Console.WriteLine("[Download] CurseForge 工程列表搜索原始文本：" + RawFilter);
        //    // 中文请求关键字处理
        //    bool IsChineseSearch = Regex.IsMatch(RawFilter, @"[\u4e00-\u9fbb]");
        //    if (IsChineseSearch && !string.IsNullOrEmpty(RawFilter))
        //    {
        //        //if (Task.Input.IsModPack)
        //        //    throw new Exception("整合包搜索仅支持英文");
        //        ReleaseCfDatabase();
        //        // 构造搜索请求
        //        List<SearchEntry<string>> SearchEntries = new List<SearchEntry<string>>();
        //        foreach (var Entry in DlCfProjectDb.Values)
        //        {
        //            if (Entry.ChineseName ?? "".Contains("动态的树"))
        //                continue; // 傻逼 Mod 的附属太多了
        //            SearchEntries.Add(new SearchEntry<string>()
        //            {
        //                Item = Entry.ChineseName,
        //                SearchSource = new List<KeyValuePair<string, double>>()
        //                {
        //                    new KeyValuePair<string, double>(Entry.ChineseName + Entry.CurseForgeId, 1)
        //                }
        //            });
        //        }
        //        // 获取搜索结果
        //        var SearchResults = Search(SearchEntries, RawFilter, 3);
        //        if (SearchResults.Count == 0)
        //            throw new Exception("无搜索结果，请尝试搜索英文名称");
        //        string SearchResult = "";
        //        for (var i = 0; i <= SearchResults.Count - 1; i++)
        //        {
        //            if (!SearchResults(i).AbsoluteRight && i >= Math.Min(2, SearchResults.Count - 1))
        //                break; // 把 3 个结果拼合以提高准确度
        //            SearchResult += SearchResults(i).Item.Replace(" - ", "§").Split("§").First.Replace(" (", "|").Split("|").Last.TrimEnd(")") + " ";
        //        }
        //        Log("[Download] CurseForge 工程列表中文搜索原始关键词：" + SearchResult, LogLevel.Developer);
        //        // 去除常见连接词
        //        string RealFilter = "";
        //        foreach (var Word in SearchResult.Split(" "))
        //        {
        //            if (
        //            {
        //                "the",
        //        "of",
        //        "a",
        //        "mod"
        //            }.Contains(Word.ToLower()))
        //        continue;
        //            if (SearchResult.Split(" ").Count() > 3 &&
        //            {
        //                "ftb"
        //            }.Contains(Word.ToLower()))
        //        continue;
        //            RealFilter += Word + " ";
        //        }
        //        Task.Input.SearchFilter = RealFilter;
        //        Log("[Download] CurseForge 工程列表中文搜索最终关键词：" + RealFilter, LogLevel.Developer);
        //    }
        //    // 驼峰英文请求关键字处理
        //    var SpacedKeywords = RegexReplace(Task.Input.SearchFilter, "$& ", "([A-Z]+|[a-z]+?)(?=[A-Z]+[a-z]+[a-z ]*)");
        //    var ConnectedKeywords = Task.Input.SearchFilter.Replace(" ", "");
        //    var AllPossibleKeywords = (SpacedKeywords + " " + IsChineseSearch ? Task.Input.SearchFilter : ConnectedKeywords + " " + RawFilter).ToLower;
        //    // 最终处理关键字：分割、去重
        //    List<string> RightKeywords = new List<string>();
        //    foreach (var Keyword in AllPossibleKeywords.Split(" "))
        //    {
        //        if (Keyword.Trim == "")
        //            continue;
        //        if (Keyword == "forge" || Keyword == "fabric" || Keyword == "for" || Keyword == "mod")
        //        {
        //            Log("[Download] 已跳过搜索关键词 " + Keyword, LogLevel.Developer);
        //            continue;
        //        }
        //        RightKeywords.Add(Keyword);
        //    }
        //    if (RawFilter.Length > 0 && RightKeywords.Count == 0)
        //        Task.Input.SearchFilter = RawFilter; // 全都被过滤掉了
        //    else
        //        Task.Input.SearchFilter = Join(ArrayNoDouble(RightKeywords), " ").ToLower;
        //    // 例外项：OptiForge、OptiFabric（拆词后因为包含 Forge/Fabric 导致无法搜到实际的 Mod）
        //    if (RawFilter.Replace(" ", "").ToLower().Contains("optiforge"))
        //        Task.Input.SearchFilter = "optiforge";
        //    if (RawFilter.Replace(" ", "").ToLower().Contains("optifabric"))
        //        Task.Input.SearchFilter = "optifabric";
        //    Log("[Download] CurseForge 工程列表搜索最终文本：" + Task.Input.SearchFilter, LogLevel.Developer);
        //    // 正式获取
        //    var Url = Task.Input.GetAddress();
        //    Log("[Download] 开始获取 CurseForge 工程列表：" + Url);
        //    JObject RequestResult = NetGetCodeByRequestRetry(Url, IsJson: true, Encode: Encoding.UTF8);
        //    var FileList = GetCfProjectListFromJson(RequestResult("data"), Task.Input.IsModPack);
        //    if (FileList.Count == 0)
        //        throw new Exception("无搜索结果");
        //    // 重新排序
        //    if (!string.IsNullOrEmpty(Task.Input.SearchFilter))
        //    {
        //        // 排序分 = 搜索相对相似度 + 默认排序相对活跃度
        //        Dictionary<DlCfProject, double> Scores = new Dictionary<DlCfProject, double>();
        //        List<SearchEntry<DlCfProject>> Entry = new List<SearchEntry<DlCfProject>>();
        //        for (var i = 1; i <= FileList.Count; i++)
        //        {
        //            var File = FileList(i - 1);
        //            Scores.Add(File, (1 - (i / (double)FileList.Count)));
        //            Entry.Add(new SearchEntry<DlCfProject>() { Item = File, SearchSource = new List<KeyValuePair<string, double>>() { new KeyValuePair<string, double>(IsChineseSearch ? File.ChineseName : File.Name, 1), new KeyValuePair<string, double>(File.Description, 0.05) } });
        //        }
        //        var SearchResult = Search(Entry, RawFilter, 101, -1);
        //        foreach (var OneResult in SearchResult)
        //            Scores[OneResult.Item] += OneResult.Similarity / (double)SearchResult(0).Similarity;
        //        // 根据排序分得出结果
        //        var ResultList = Sort(Scores.ToList(), (KeyValuePair<DlCfProject, double> Left, KeyValuePair<DlCfProject, double> Right) =>
        //        {
        //            return Left.Value > Right.Value;
        //        });
        //        FileList = new List<DlCfProject>();
        //        foreach (var Result in ResultList)
        //            FileList.Add(Result.Key);
        //    }
        //    Task.Output = new DlCfProjectResult() { Projects = FileList, Index = RequestResult("pagination")("index"), RealCount = RequestResult("pagination")("totalCount") };
        //}
    }
}
