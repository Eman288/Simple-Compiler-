﻿using System;
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
        List<(string, string)> realTokens = new List<(string, string)>();
        int index = 0;
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
            var Tokens = MakeTokens(s);
            foreach (var token in Tokens)
            {
                c += $"({token.Item1}, \"{token.Item2}\"){Environment.NewLine}";
                if (token.Item1 != "WS")
                {
                    realTokens.Add(token);
                }
            }
            token.Text = c;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }

        private void assignment_TextChanged(object sender, EventArgs e)
        {
            // Temporarily unsubscribe to prevent infinite loop
            assignment.TextChanged -= assignment_TextChanged;

            bool somethingWrong;
            string parsedAssignment = ParseAssignment(out somethingWrong);

            // Update the text box with the parsed result
            assignment.Text = parsedAssignment;

            // Re-subscribe to the event
            assignment.TextChanged += assignment_TextChanged;
        }

        private void Expression_TextChanged(object sender, EventArgs e)
        {
            // Temporarily unsubscribe to prevent infinite loop
            Expression.TextChanged -= Expression_TextChanged;

            bool expIsRight;
            string parsedExpression = ParseExpression(out expIsRight);

            // Update the text box with the parsed result
            Expression.Text = parsedExpression;

            // Re-subscribe to the event
            Expression.TextChanged += Expression_TextChanged;
        }

        // parse functions
        string Operator(out bool opIsRight)
        {
            opIsRight = true;
            string op = "";
            if (realTokens[index].Item1 == "OPERATOR")
            {
                if (
                    realTokens[index].Item2 == "+" 
                    || realTokens[index].Item2 == "-"
                    || realTokens[index].Item2 == "*"
                    || realTokens[index].Item2 == "/"
                    )
                {
                    op = realTokens[index].Item2;
                }
                else if (
                    realTokens[index].Item2 == ">"
                    || realTokens[index].Item2 == "<"
                    || realTokens[index].Item2 == "=<"
                    || realTokens[index].Item2 == "=>"
                    || realTokens[index].Item2 == "=="
                    || realTokens[index].Item2 == "!="
                    )
                {
                    op = realTokens[index].Item2;
                    opIsRight = false;
                }
            }
            return op;
        }
        string Term(out bool termIsRight)
        {
            termIsRight = true;
            string term = "";
            // ("Ident", "value")
            if (realTokens[index].Item1 == "IDENT" || realTokens[index].Item1 == "NUM")
            {
                term = realTokens[index].Item2;
            }
            else
            {
                termIsRight = false;
                term = "Wrong Term";
            }
            return term;
        }

        string ParseExpression(out bool expIsRight)
        {
            // Expression rule
            /*
                <expression> → <term> | <term> <operator> <expression>
                <term> → IDENT | NUM
                <operator> → "+" | "-" | "*" | "/"
             */
            expIsRight = true;
            string code = "";
            bool termIsRight;
            bool opIsRight;

            string term = "";
            string op = "";

            // check term
            term = Term(out termIsRight);
            if (termIsRight == false)
            {
                expIsRight = false;
                code = term;
                return (code);
            }
            else
            {
                index += 1;
                code += term;
            }

            // check if the next token is an operator or not
            if (index < realTokens.Count)
            {
                if (realTokens[index].Item1 == "OPERATOR")
                {
                    op = Operator(out opIsRight);
                    if (opIsRight == true)
                    {
                        code += op;
                        index += 1;
                        code += ParseExpression(out expIsRight);
                    }
                    else
                    {
                        index -= 1;
                        return code;
                    }    
                }
                else if (realTokens[index].Item1 == "SEMICOLON")
                {
                    index -= 1;
                    return code;
                }
                else
                {
                    expIsRight = false;
                    code = "Wrong Expression";
                    return code;
                }
            }
            else 
            {
                code = "Wrong Syntax, the index is wrong";   
            }

            return code;
        }

        string ParseAssignment( out bool somethingWrong)
        {
            // assignment rule
            /*
                *  <assignment> → IDENT = <expression> ;
             */
somethingWrong = true;


return ("ddd");
}

}
}
