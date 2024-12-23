using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace @try
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetToken(code.Text);
        }

        static List<(string, string)> MakeTokens(string line)
        {
            var tokenPatterns = new Dictionary<string, string>
            {
                { "KEYWORD", @"\b(اذا|طالما|متغير)\b" },
                { "IDENT", @"[a-zA-Z\u0600-\u06FF][a-zA-Z0-9\u0600-\u06FF]*" },
                { "NUM", @"\b\d+\b" },
                { "OPERATOR", @"[+\-*/=<>!]+" },
                { "ASSIGN", @"=" },
                { "LPAREN", @"\(" },
                { "RPAREN", @"\)" },
                { "LBRACE", @"\{" },
                { "RBRACE", @"\}" },
                { "SEMICOLON", @";" },
                { "WS", @"\s+" }
            };


            var tokenRegex = new Regex(string.Join("|", tokenPatterns.Select(kvp =>
                $"(?<{kvp.Key}>{kvp.Value})"
            )));

            var tokens = new List<(string, string)>();

            // Perform matching
            var matches = tokenRegex.Matches(line);
            foreach (Match match in matches)
            {
                foreach (var kvp in tokenPatterns.Keys)
                {
                    if (match.Groups[kvp].Success)
                    {
                        if (kvp != "WHITESPACE")  // Skip whitespace
                        {
                            tokens.Add((kvp, match.Value));
                        }
                        break;
                    }
                }
            }

            return tokens;
        }


        void GetToken(string s)
        {
            string c = "";
            var tokens = MakeTokens(s);
            foreach (var token in tokens)
            {
                c += $"({token.Item1}, \"{token.Item2}\"){Environment.NewLine}";
            }
            token.Text = c;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
