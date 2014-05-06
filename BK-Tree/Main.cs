using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKTree;


class Program
{
    static void Main(string[] args)
    {
        BKTree.BKTree tree = new BKTree.BKTree();
        //tree.AddWord("apple");
        //tree.AddWord("oregon");
        //tree.AddWord("bioware");
        Console.Write("Loading... ");
        tree.LoadDictionary("words.txt");
        Console.WriteLine("Loaded");

        IEnumerable<string> result = tree.GetSpellingSuggestions("apple");

        foreach(string s in result)
        {
            Console.Write(s + ", ");
        }

        Console.WriteLine();
    }
}

