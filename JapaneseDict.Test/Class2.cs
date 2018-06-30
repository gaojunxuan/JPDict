using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Test
{
    public static class Class2
    {
        public static void Do()
        {
            var connection = new SQLiteConnection(Console.ReadLine());
            var result = connection.Query<Jlpt>("select * from Jlpt where Kanji like '%/%'");
            foreach (var i in result)
            {
                var a = i.Kanji.Split('/');
                connection.Insert(new Jlpt() { Kanji = a[0], Hiragana = i.Hiragana, Level = i.Level });
                connection.Insert(new Jlpt() { Kanji = a[1], Hiragana = i.Hiragana, Level = i.Level });
                connection.Delete(i);
                connection.Commit();
                Console.WriteLine(i.Kanji);
            }
            var hiragana_result = connection.Query<Jlpt>("select * from Jlpt where Hiragana like '%/%'");
            foreach (var i in hiragana_result)
            {
                var a = i.Hiragana.Split('/');
                connection.Insert(new Jlpt() { Kanji = i.Kanji, Hiragana = a[0], Level = i.Level });
                connection.Insert(new Jlpt() { Kanji = i.Kanji, Hiragana = a[1], Level = i.Level });
                connection.Delete(i);
                connection.Commit();
                Console.WriteLine(i.Kanji);
            }
            var other_result = connection.Query<Jlpt>("select * from Jlpt where Hiragana like '%、%'");
            foreach (var i in other_result)
            {
                var a = i.Hiragana.Split('、');
                connection.Insert(new Jlpt() { Kanji = i.Kanji, Hiragana = a[0], Level = i.Level });
                connection.Delete(i);
                connection.Commit();
                Console.WriteLine(i.Kanji);
            }
        }
        public static void Do2()
        {
            var connection = new SQLiteConnection(Console.ReadLine());
            var mainDict_connection = new SQLiteConnection(Console.ReadLine());
            var table = connection.Table<Jlpt>();
            foreach(var i in table)
            {
                if(!string.IsNullOrEmpty(i.Kanji))
                {
                    var kanjires = mainDict_connection.Query<MainDict>($"select * from MainDict where JpChar = '{i.Kanji}'");
                    var kanares = mainDict_connection.Query<MainDict>($"select * from MainDict where Kana = '{i.Hiragana}'");
                    if(kanares.Count!=0&&kanjires.Count==0)
                    {
                        if(kanares.Count==1)
                        {
                            string exp = kanares.First().Explanation;
                            string[] lines = exp.Split('\n');
                            if(lines[1].StartsWith("["))
                            {
                                string newExp = i.Hiragana + "\n\n" + i.Kanji +"\n"+ lines.Skip(1);
                                System.Diagnostics.Debug.WriteLine(newExp);
                                //mainDict_connection.Insert(new MainDict() { JpChar = i.Kanji, Kana = i.Hiragana, Explanation = newExp });
                            }
                            else
                            {
                                string newExp = i.Hiragana + "\n\n" + lines[1] + "；" + i.Kanji + "\n" + lines.Skip(2);
                                System.Diagnostics.Debug.WriteLine(newExp);
                                //mainDict_connection.Insert(new MainDict() { JpChar = i.Kanji, Kana = i.Hiragana, Explanation = newExp });
                            }
                        }
                        Console.WriteLine(i.Kanji);
                    }
                }
            }
        }
        public static void ReplaceAll()
        {
            var connection = new SQLiteConnection(Console.ReadLine());
            connection.Execute("update MainDict set Explanation = replace(Explanation,'\r','\n')");
            var table = connection.Table<MainDict>();
            string a = table.First().Explanation;
        }
    }
    public class Jlpt
    {
        [PrimaryKey][AutoIncrement]
        public int ID { get; set; }
        public string Kanji { get; set; }
        public string Hiragana { get; set; }
        public string Level { get; set; }
    }
}
