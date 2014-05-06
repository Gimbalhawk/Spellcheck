using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKTree
{
    class BKTree
    {
        private BKNode m_root = null;
        private static int s_defaultTolerance = 2;

        // Adds a new word to the tree
        public void AddWord(string word)
        {
            if (m_root == null)
            {
                m_root = new BKNode(word);
            }
            else
            {
                m_root.AddNode(word);
            }
        }

        public bool WordExists(string word)
        {
            return m_root != null ? m_root.FindWord(word) : false;
        }

        public IEnumerable<string> GetSpellingSuggestions(string word)
        {
            return GetSpellingSuggestions(word, s_defaultTolerance);
        }

        public IEnumerable<string> GetSpellingSuggestions(string word, int tolerance)
        {
            if (m_root == null) return null;

            List<string> suggestions = new List<string>();
            suggestions.AddRange(m_root.FindSuggestions(word, tolerance));
            return suggestions;
        }

        // Loads the tree from a file consisting of one word per line
        public void LoadDictionary(string path)
        {
            StreamReader file = File.OpenText(path);
            if (file == null)
            {
                return;
            }

            string line;

            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();

                // # denotes comments
                if (line.StartsWith("#"))
                {
                    return;
                }

                AddWord(line);
            }
        }



        private class BKNode
        {
            private string m_word;
            private Dictionary<int, BKNode> m_children;

            public BKNode(string word)
            {
                m_word = word;
                m_children = new Dictionary<int, BKNode>();
            }

            public void AddNode(string word)
            {
                int dist = Levenshtein.Compute(word, m_word);

                // The word's already in the dictionary!
                if (dist == 0) return;

                BKNode child;
                if (m_children.TryGetValue(dist, out child))
                {
                    // We already have another node of equal distance, so add this word to that node's subtree
                    child.AddNode(word);
                }
                else
                {
                    // We don't have a child of that distance, so a new child is added
                    m_children.Add(dist, new BKNode(word));
                }
            }

            public bool FindWord(string word)
            {
                int dist = Levenshtein.Compute(word, m_word);

                // The word's already in the dictionary!
                if (dist == 0) return true;

                BKNode child;
                if (m_children.TryGetValue(dist, out child))
                {
                    // We already have another node of equal distance, so add this word to that node's subtree
                    return child.FindWord(word);
                }

                return false;
            }

            public IEnumerable<string> FindSuggestions(string word, int tolerance)
            {
                List<string> suggestions = new List<string>();

                int dist = Levenshtein.Compute(word, m_word);
                if (dist < tolerance)
                {
                    suggestions.Add(m_word);
                }

                foreach(int key in m_children.Keys)
                {
                    if (key >= dist - tolerance && key <= dist + tolerance)
                    {
                        suggestions.AddRange(m_children[key].FindSuggestions(word, tolerance));
                    }
                }

                return suggestions;
            }
        }
    }
}
