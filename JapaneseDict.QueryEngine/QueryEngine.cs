using JapaneseDict.Models;
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
using SQLite.Net;

namespace JapaneseDict.QueryEngine
{
    public class QueryEngine
    {

        public static class MainDictQueryEngine
        {
            public static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));
            /// <summary>
            /// Query MainDict database using the given keyword
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static IEnumerable<Dict> Query(string key)
            {
                return _conn.Query<Dict>("SELECT * FROM Dict WHERE Keyword = ?", key);
            }
            /// <summary>
            /// Fuzzy query MainDict database using the given keyword
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static IEnumerable<Dict> FuzzyQuery(string key)
            {

                return _conn.Query<Dict>("SELECT * FROM Dict WHERE Keyword LIKE ?", key + "%");

            }
            /// <summary>
            /// Query MainDict database using the given keyword and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<Dict>> QueryForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        var result = new ObservableCollection<Dict>(_conn.Query<Dict>("SELECT * FROM Dict WHERE Keyword = ?", key.Replace(" ", "").Replace("　", "")));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<Dict>
                            {
                                new Dict() { Keyword = key, Definition = "没有本地释义" }
                            };
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<Dict>();
                    }
                });

            }
            /// <summary>
            /// Query MainDict database using given index and return the result in ObservableCollection type
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<Dict>> QueryForUIAsync(int id)
            {
                return await Task.Run(() =>
                {

                    var result = new ObservableCollection<Dict>(_conn.Query<Dict>("SELECT * FROM Dict WHERE ItemId = ?", id));
                    if (result.Count != 0)
                    {
                        return result;
                    }
                    else
                    {
                        var err = new ObservableCollection<Dict>
                        {
                            //new Dict() { Keyword = "没有本地释义", Definition = "请查看网络释义" }
                        };
                        return err;
                    }

                });

            }
            public static async Task<ObservableCollection<Dict>> FuzzyQueryForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        var result = new ObservableCollection<Dict>(_conn.Query<Dict>("SELECT * FROM Dict WHERE Keyword LIKE ?", key.Replace(" ", "").Replace("　", "") + "%").Take(5));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<Dict>
                            {
                                new Dict() { Keyword = key, Definition = "没有本地释义" }
                            };
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<Dict>();
                    }
                });
            }
            private static void CloseConnection()
            {
                _conn.Close();
            }
            private static void Reconnect()
            {
                _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(),Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));
            }
            /// <summary>
            /// Insert an item into Maindict db
            /// </summary>
            /// <param name="keyword"></param>
            /// <param name="definition"></param>
            /// <param name="reading"></param>
            public static void Add(string keyword, string definition, string reading, int id)
            {
                _conn.CreateTable<Dict>();
                var maxid = _conn.ExecuteScalar<int>("SELECT MAX( ID ) FROM Dict ;");
                _conn.Execute($"UPDATE SQLITE_SEQUENCE SET seq = {maxid}  WHERE name = 'MainDict'");
                var entries = _conn.Query<Dict>("SELECT * FROM Dict WHERE ID = ?", id);
                if (entries.Count == 0)
                {
                    _conn.Insert(new Dict() { Keyword = keyword, Definition = definition, Reading = reading, ItemId = id });
                    _conn.Commit();
                }

            }
        }
        public static class KanjiDictQueryEngine
        {
            private static SQLiteConnection _kanjiconn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "kanji.db"));
            private static SQLiteConnection _radconn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "kanjirad.db"));
            /// <summary>
            /// Query the database with the specifed kanji
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static IEnumerable<Dict> Query(string key)
            {
                return _kanjiconn.Query<Dict>("SELECT * FROM Kanjidict WHERE Kanji = ?", key);
            }
            /// <summary>
            /// Query kanji database using the given kanji and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<Kanjidict>> QueryForUIAsync(string key)
            {
                return await Task.Run(() =>
                {
                    if (!(string.IsNullOrEmpty(key)))
                    {
                        var result = new ObservableCollection<Kanjidict>(_kanjiconn.Query<Kanjidict>("SELECT * FROM Kanjidict WHERE Kanji = ?", key.Replace(" ", "").Replace("　", "")));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<Kanjidict>();
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<Kanjidict>();
                    }
                });

            }
            /// <summary>
            /// Query kanji database using the given jlpt level and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<Kanjidict>> QueryForUIAsync(int jlpt)
            {
                return await Task.Run(() =>
                {
                    if (jlpt < 6 & jlpt > 0)
                    {
                        var result = new ObservableCollection<Kanjidict>(_kanjiconn.Query<Kanjidict>("SELECT * FROM Kanjidict WHERE Jlpt = ?", jlpt));
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<Kanjidict>();
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<Kanjidict>();
                    }
                });

            }
            /// <summary>
            /// Query kanji database using the given jlpt level and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<List<Kanjidict>> QueryAsync(int jlpt)
            {
                return await Task.Run(() =>
                {
                    if (jlpt < 6 & jlpt > 0)
                    {
                        var result = _kanjiconn.Query<Kanjidict>("SELECT * FROM Kanjidict WHERE Jlpt = ?", jlpt);
                        if (result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new List<Kanjidict>();
                            return err;
                        }
                    }
                    else
                    {
                        return new List<Kanjidict>();
                    }
                });

            }
            /// <summary>
            /// Query kanjis database using the given kanji and return the result in ObservableCollection type
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static async Task<ObservableCollection<Kanjidict>> QueryForUIAsync(MatchCollection keywords)
            {
                return await Task.Run(() =>
                {
                    if (keywords.Count != 0)
                    {
                        ObservableCollection<Kanjidict> result = new ObservableCollection<Kanjidict>();
                        StringBuilder sb = new StringBuilder();
                        foreach (var ks in keywords)
                        {
                            var distincted = ks.ToString().Distinct();
                            foreach (var k in distincted)
                            {
                                sb.Append(k);                                
                            }
                        }
                        foreach(var s in sb.ToString().Distinct())
                        {
                            foreach (var r in _kanjiconn.Query<Kanjidict>("SELECT * FROM Kanjidict WHERE Kanji = ?", s.ToString()))
                            {
                                r.KanjiRad = new ObservableCollection<KanjiRadical>();
                                var radResult = _radconn.Query<KanjiRadical>("SELECT * FROM KanjiRadical WHERE Kanji = ?", s.ToString());
                                foreach (var x in radResult)
                                {
                                    r.KanjiRad.Add(x);                                   
                                }
                                
                                result.Add(r);
                            }
                        }
                        if (result != null && result.Count != 0)
                        {
                            return result;
                        }
                        else
                        {
                            var err = new ObservableCollection<Kanjidict>();
                            return err;
                        }
                    }
                    else
                    {
                        return new ObservableCollection<Kanjidict>();
                    }
                });

            }
        }
        public static class NotebookQueryEngine
        {
            private static SQLiteConnection _noteconn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));

            private static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));
            /// <summary>
            /// Insert an item into Note db
            /// </summary>
            /// <param name="jpchar"></param>
            /// <param name="explanation"></param>
            /// <param name="Kana"></param>
            [Obsolete]
            public static void Add(string jpchar, string explanation, string Kana)
            {

                //_noteconn.CreateTable<Note>();
                //_noteconn.Insert(new Note() { JpChar = jpchar, Explanation = explanation, Kana = Kana });
                //_noteconn.Commit();
            }
            /// <summary>
            /// Insert an item into Note db
            /// </summary>
            /// <param name="id"></param>
            public static void Add(int id)
            {
                try
                {
                    _noteconn.CreateTable<Note>();
                    var item = _conn.Query<Dict>("SELECT * FROM Dict WHERE ItemId = ?", id).FirstOrDefault();
                    item.IsInNotebook = true;
                    _conn.Update(item);
                    _noteconn.Insert(new Note(item) { ItemId = id });
                    _noteconn.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
            /// <summary>
            /// Remove the item with given originId from Note db
            /// </summary>
            /// <param name="originId"></param>
            public static void Remove(int originId)
            {

                var item = _conn.Query<Dict>("SELECT * FROM Dict WHERE ItemId = ?", originId).FirstOrDefault();
                item.IsInNotebook = false;
                _conn.Update(item);
                _noteconn.Delete(_noteconn.Query<Note>("SELECT * FROM Note WHERE ItemId = ?", originId).First());
                _noteconn.Commit();
            }
            /// <summary>
            /// Get items from Note db
            /// </summary>
            /// <returns></returns>
            public static ObservableCollection<Note> Get()
            {
                _noteconn.CreateTable<Note>(); 
                return new ObservableCollection<Note>(_noteconn.Query<Note>("SELECT * FROM Note"));
            }
            /// <summary>
            /// An async copy of Get()
            /// </summary>
            /// <returns></returns>
            public static async Task<ObservableCollection<Note>> GetAsync()
            {

                return await Task.Run(() =>
                {
                    _noteconn.CreateTable<Note>();
                    return new ObservableCollection<Note>(_noteconn.Query<Note>("SELECT * FROM Note"));
                });

            }
            /// <summary>
            /// Merge database
            /// </summary>
            /// <param name="path"></param>
            public static void MergeDb(string path)
            {
                SQLiteConnection _mergeConn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, path));
                _mergeConn.CreateTable<Note>();
#pragma warning disable CS0612 // Type or member is obsolete
                _mergeConn.CreateTable<UserDefDict>();
                _noteconn.CreateTable<Note>();
                foreach (var i in _mergeConn.Table<Note>())
                {
                    //put the data from given db into main db
                    _noteconn.Insert(i);
                    var item = _conn.Query<Dict>("SELECT * FROM Dict WHERE ItemId = ?", i.ItemId).FirstOrDefault();
                    item.IsInNotebook = true;
                    _conn.Update(item);
                }
                foreach (var i in _mergeConn.Table<UserDefDict>())
                {
                    //put the data from given db into main db
                    _noteconn.Insert(new Note() { ItemId = i.OriginID, Keyword = i.JpChar, Definition = i.PreviewExplanation, Reading = i.Kana });
                    var item = _conn.Query<Dict>("SELECT * FROM Dict WHERE ItemId = ?", i.OriginID).FirstOrDefault();
                    item.IsInNotebook = true;
                    _conn.Update(item);
                }
                //remove duplicate rows
                _noteconn.Query<Note>("DELETE FROM Note WHERE ItemId NOT IN (SELECT MAX(ItemId) ItemId FROM Note GROUP BY ItemId)");
                _mergeConn.Close();
#pragma warning restore CS0612 // Type or member is obsolete
            }
            //Recover from backup
            public static async void CopyFromBackup()
            {
                if(await ApplicationData.Current.LocalFolder.GetFileAsync("cloudnote.db")!=null)
                {
                    MergeDb("cloudnote.db");
                }

            }
            private static void CloseConnection()
            {
                _noteconn.Close();
            }
            private static void Reconnect()
            {
                _noteconn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db"));
            }
        }


    }
}
