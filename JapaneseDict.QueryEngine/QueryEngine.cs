using JapaneseDict.Models;
using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace JapaneseDict.QueryEngine
{
    public class QueryEngine
    {
        
        public static class MainDictQueryEngine
        {
            private static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));
            /// <summary>
            /// Query MainDict database using the given keyword
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static IEnumerable<MainDict> Query(string key)
            {
               
                    return _conn.Query<MainDict>("SELECT * FROM MainDict WHERE JpChar = ?", key);
                

            }
            /// <summary>
            /// Fuzzy query MainDict database using the given keyword
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static IEnumerable<MainDict> FuzzyQuery(string key)
            {
               
                    return _conn.Query<MainDict>("SELECT * FROM MainDict WHERE JpChar LIKE ?", key + "%");
               
            }
            /// <summary>
            /// Query MainDict database using the given keyword and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<MainDict>> QueryForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        var result = new ObservableCollection<MainDict>(_conn.Query<MainDict>("SELECT * FROM MainDict WHERE JpChar = ?", key.Replace(" ", "").Replace("　", "")));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<MainDict>();
                            err.Add(new MainDict() { JpChar = key, Explanation = "没有本地释义" });
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<MainDict>();
                    }
                });

            }
            public static async Task<ObservableCollection<MainDict>> QueryCn2JpForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        //@"\[[^\]]+\]" Regex pat for removing [XXX]
                        //@"[\（][\s\S]*[\）]" Regex pat for removing （XXX）
                        var queryres = _conn.Table<MainDict>().Select(s=> s).Where(w => 
                        {
                            string a = Regex.Replace(Regex.Replace(w.PreviewExplanation, @"\[[^\]]+\]", ""), @"[\（][\s\S]*[\）]", "");
                            if(a.Contains(key + "；")||a.Contains(key + "，")|| w.PreviewExplanation.EndsWith("] " + key + " ..."))
                            {
                                return true;
                            }
                            return false;
                        });
                        var result = new ObservableCollection<MainDict>(queryres);
                        
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<MainDict>();
                            err.Add(new MainDict() { JpChar = key, Explanation = "没有本地释义" });
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<MainDict>();
                    }
                });

            }
            /// <summary>
            /// Query MainDict database using given index and return the result in ObservableCollection type
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<MainDict>> QueryForUIAsync(int id)
            {
                return await Task.Run(() =>
                {

                    var result = new ObservableCollection<MainDict>(_conn.Query<MainDict>("SELECT * FROM MainDict WHERE ID = ?", id));
                    if (result.Count != 0)
                    {
                        return result;
                    }
                    else
                    {
                        var err = new ObservableCollection<MainDict>();
                        err.Add(new MainDict() { JpChar = "没有本地释义", Explanation = "请查看网络释义" });
                        return err;
                    }

                });

            }
            public static async Task<ObservableCollection<MainDict>> FuzzyQueryForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        var result = new ObservableCollection<MainDict>(_conn.Query<MainDict>("SELECT * FROM MainDict WHERE JpChar LIKE ?", key.Replace(" ", "").Replace("　", "") + "%").Take(5));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<MainDict>();
                            err.Add(new MainDict() { JpChar = key, Explanation = "没有本地释义" });
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<MainDict>();
                    }
                });
            }
            public static ObservableCollection<MainDict> GetTable()
            {
                return new ObservableCollection<MainDict>(_conn.Table<MainDict>());
            }
            private static void CloseConnection()
            {
                _conn.Close();
            }
            private static void Reconnect()
            {
                _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));
            }
        }
        public static class UserDefDictQueryEngine
        {
            private static SQLiteConnection _noteconn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));

            private static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));
            /// <summary>
            /// Insert an item into UserDefDict db
            /// </summary>
            /// <param name="jpchar"></param>
            /// <param name="explanation"></param>
            /// <param name="Kana"></param>
            public static void Add(string jpchar,string explanation,string Kana)
            {
                
                _noteconn.CreateTable<UserDefDict>();
                _noteconn.Insert(new UserDefDict() { JpChar = jpchar, Explanation = explanation, Kana = Kana });
                _noteconn.Commit();
            }
            /// <summary>
            /// Insert an item into UserDefDict db
            /// </summary>
            /// <param name="id"></param>
            public static void Add(int id)
            {
                try
                {
                    _noteconn.CreateTable<UserDefDict>();
                    var item = _conn.Query<MainDict>("SELECT * FROM MainDict WHERE ID = ?", id).FirstOrDefault();
                    item.IsInUserDefDict = true;
                    _conn.Update(item);
                    _noteconn.Insert(new UserDefDict(item) { OriginID = id });
                    _noteconn.Commit();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
               
            }
            /// <summary>
            /// Remove the item with given originId from UserDefDict db
            /// </summary>
            /// <param name="originId"></param>
            public static void Remove(int originId)
            {
                
                var item = _conn.Query<MainDict>("SELECT * FROM MainDict WHERE ID = ?", originId).FirstOrDefault();
                item.IsInUserDefDict = false;
                _conn.Update(item);
                _noteconn.Delete(_noteconn.Query<UserDefDict>("SELECT * FROM UserDefDict WHERE OriginID = ?", originId).First());
                _noteconn.Commit();
            }
            /// <summary>
            /// Get items from UserDefDict db
            /// </summary>
            /// <returns></returns>
            public static ObservableCollection<UserDefDict> Get()
            {
                 
                _noteconn.CreateTable<UserDefDict>();
                return new ObservableCollection<UserDefDict>(_noteconn.Table<UserDefDict>());
            }
            /// <summary>
            /// An async copy of Get()
            /// </summary>
            /// <returns></returns>
            public static async Task<ObservableCollection<UserDefDict>> GetAsync()
            {
                
                return await Task.Run(() => 
                {
                    _noteconn.CreateTable<UserDefDict>();
                    return new ObservableCollection<UserDefDict>(_noteconn.Table<UserDefDict>());
                });
                
            }
            /// <summary>
            /// Merge database
            /// </summary>
            /// <param name="path"></param>
            public static void MergeDb(string path)
            {
                SQLiteConnection _mergeConn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path,path));
                _mergeConn.CreateTable<UserDefDict>();
                foreach(var i in _mergeConn.Table<UserDefDict>())
                {
                    //put the data from given db into main db
                    _noteconn.Insert(i);
                    var item = _conn.Query<MainDict>("SELECT * FROM MainDict WHERE ID = ?", i.OriginID).FirstOrDefault();
                    item.IsInUserDefDict = true;
                    _conn.Update(item);
                }
                _noteconn.CreateTable<UserDefDict>();
                //remove duplicate rows
                _noteconn.Query<UserDefDict>("DELETE FROM UserDefDict WHERE ID NOT IN (SELECT MAX(ID) ID FROM UserDefDict GROUP BY OriginID)");
                _mergeConn.Close();
            }
            public static void SyncDb()
            {
                OnedriveHelper.RequestAuth();
                OnedriveHelper.SyncNotebook();
            }
            private static void CloseConnection()
            {
                _noteconn.Close();
            }
            private static void Reconnect()
            {
                _noteconn= new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));
            }
        }
        public static class OnlineQueryEngine
        {
            /// <summary>
            /// Query online
            /// </summary>
            /// <param name="keyword"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<OnlineDict>> Query(string keyword)
            {
                var res = new ObservableCollection<OnlineDict>();
                res.Add(new OnlineDict() { JpChar = keyword, Explanation = await OnlineService.JsonHelper.GetTranslateResult(keyword, "jp", "zh") });
                return res;
            }
            public static async Task<ObservableCollection<OnlineDict>> Query(string keyword,string originLang,string targetLang)
            {
                var res = new ObservableCollection<OnlineDict>();
                res.Add(new OnlineDict() { JpChar = keyword, Explanation = await OnlineService.JsonHelper.GetTranslateResult(keyword, originLang, targetLang) });
                return res;
            }
        }
        
    }
}
