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
        List<(string, string)> realTokens = new List<(string, string)>();
        int index = 0;
        string ParseTree = "";
        public Form1()
        {
            InitializeComponent();
            Parse.ReadOnly = true;
            Expression.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetToken(code.Text);
            bool declareIsRight;
            ParseTree = "";
            bool expIsRight;
            Expression.Text = ParseStatementList(out expIsRight);
            //assignment.Text = ParseDeclaration(out declareIsRight);
            ParseTree += $"{Environment.NewLine}";
            index = 0;
            realTokens = new List<(string, string)>();
            Parse.Text = ParseTree;

        }
        static List<(string, string)> MakeTokens(string line)
        {
            var tokenPatterns = new Dictionary<string, string>
    {
        { "KEYWORD", @"\b(اذا|طالما|متغير)\b" },
        { "IDENT", @"[a-zA-Z\u0600-\u06FF][a-zA-Z0-9\u0600-\u06FF]*" },
        { "NUM", @"\b\d+\b" },
        { "OPERATOR", @"(<=|>=|==|!=|<|>|[+\-*/])" },
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
                        if (kvp != "WS")  // Skip whitespace
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
        }

        private void Expression_TextChanged(object sender, EventArgs e)
        {
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
                    //ParseTree += $"\n <OPERATOR> => ${realTokens[index].Item1}\n${realTokens[index].Item1} => ${realTokens[index].Item2}\n";
                }
                else if (
                    realTokens[index].Item2 == ">"
                    || realTokens[index].Item2 == "<"
                    || realTokens[index].Item2 == "<="
                    || realTokens[index].Item2 == ">="
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
            if (index < realTokens.Count)
            {
                if (realTokens[index].Item1 == "IDENT" || realTokens[index].Item1 == "NUM")
                {
                    term = realTokens[index].Item2;
                    //ParseTree += $"\n <term> => ${realTokens[index].Item1}\n${realTokens[index].Item1} => ${realTokens[index].Item2}\n";
                }
                else
                {
                    termIsRight = false;
                    term = "Wrong Term";
                }
            }
            else
            {
                term = "Missing Term";
                termIsRight = false;
            }
            
            return term;
        }
        string Condition(out bool condIsRight)
        {
            // Condition rule
            /*
             * <condition> → <expression> <relational_operator>
                <expression>
             */
            ParseTree += $"{Environment.NewLine}<condition> → <expression> <relational_operator> <expression>{Environment.NewLine}";
            condIsRight = true;
            string cond = "";
            bool expIsRight, opIsRight;
            string c;
            ParseTree += $"<expression> → ";
            c = ParseExpression(out expIsRight);
            if (expIsRight)
            {
                cond += c;
                string op = Operator(out opIsRight);
                index += 1;
                if (opIsRight == false) // it is a relational operator
                {
                    cond += op;
                    ParseTree += $"{Environment.NewLine}<expression> → ";
                    c = ParseExpression(out expIsRight);
                    if (expIsRight)
                    {
                        cond += c;
                    }
                    else
                    {
                        cond = "Wrong in condition, expression is wrong" +
                                $"{Environment.NewLine}Problem: {c}";
                    }
                }
                else
                {
                    cond = "Wrong in condition, operator is wrong" +
                        $"{Environment.NewLine}Problem: {op} is not a relational operator";
                }
            }
            else
            {
                cond = "Wrong in condition, expression is wrong" +
                    $"{Environment.NewLine}Problem: {c}";
            }
            return cond;
        }
        string Statement(out bool stIsRight)
        {
            // statement rule
            /*
             * <statement> → <declaration> | <assignment> |
                            <if_statement> | <while_statement>
             */
            string st = "";
            stIsRight = true;
            bool dIsRight, ifIsRight, whileISRight, assignIsRight;
            if (index < realTokens.Count && realTokens[index].Item1 == "KEYWORD")
            {
                // call one of the three: declaration || if || while
                if (realTokens[index].Item2  == "متغير")
                {
                    // call declaration
                    string de = ParseDeclaration(out dIsRight);
                    if (dIsRight)
                    {
                        st = de;
                    }
                    else
                    {
                        stIsRight = false;
                        st = $"Wrong in statement {Environment.NewLine}" +
                                $"Problem: {de}";
                    }
                }
                else if (realTokens[index].Item2 == "طالما")
                {
                    // call while
                    string wh = ParseWhile(out whileISRight);
                    if (whileISRight)
                    {
                        st = wh;
                    }
                    else
                    {
                        stIsRight = false;
                        st = $"Wrong in statement {Environment.NewLine}" +
                                $"Problem: {wh}";
                    }
                }
                else if (realTokens[index].Item2 == "اذا")
                {
                    // call if
                    string IF = ParseAssignment(out ifIsRight);
                    if (ifIsRight)
                    {
                        st = IF;
                    }
                    else
                    {
                        stIsRight = false;
                        st = $"Wrong in statement {Environment.NewLine}" +
                                $"Problem: {IF}";
                    }
                }
                else
                {
                    stIsRight = false;
                    st = $"Wrong in statement {Environment.NewLine}" +
                                    $"Problem: Not a known Keyword";
                }

            }
            else if (index < realTokens.Count && realTokens[index].Item1 == "IDENT")
            {
                // call the assignment
                string assign = ParseAssignment(out assignIsRight);
                if (assignIsRight)
                {
                    st = assign;
                }
                else
                {
                    stIsRight = false;
                    st = $"Wrong in statement {Environment.NewLine}" +
                            $"Problem: {assign}";
                }
            }
            else
            {
                stIsRight = false;
                st = $"Wrong in statement {Environment.NewLine}" +
                                $"Problem: Not a known statement";
            }
            return st;
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
                ParseTree += "<term>";
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
                        ParseTree += $"<operator> <expression>{Environment.NewLine}<expression> => ";
                        string s = ParseExpression(out expIsRight);
                        if (expIsRight)
                        {
                            code += s;
                        }
                        else
                        {

                            return code;
                        }
                    }
                    else
                    {
                        return code;
                    }    
                }
                else if (
                    realTokens[index].Item1 == "SEMICOLON" 
                    || realTokens[index].Item1 == "RPAREN"
                    || realTokens[index].Item1 == "ASSIGN"
                    )
                {
                    return code;
                }
                else
                {
                    expIsRight = false;
                    return code;
                }
            }
            else 
            {
                expIsRight = false;
                code = "Wrong Syntax, the index is wrong";   
            }

            return code;
        }

        string ParseAssignment( out bool assignIsRight)
        {
            // assignment rule
            /*
                *  <assignment> → IDENT = <expression> ;
             */
            ParseTree += $"<assignment> → IDENT = <expression> ;{Environment.NewLine}";
            assignIsRight = true;
            bool expIsRight;
            string assign = "";
            if (realTokens[index].Item1 == "IDENT")
            {
                assign += realTokens[index].Item2;
                index += 1;
                if (realTokens[index].Item1 == "ASSIGN")
                {
                    assign += realTokens[index].Item2;
                    index += 1;
                    ParseTree += $"<expression> → ";
                    string check = ParseExpression(out expIsRight);
                    if (expIsRight)
                    {
                        assign += check;
                        if (index < realTokens.Count && realTokens[index].Item1 == "SEMICOLON")
                        {
                            assign += realTokens[index].Item2;
                            index += 1;
                            return assign;
                        }
                        else
                        {
                            assign = "Missing Semicolon";
                            return assign;
                        }
                    }
                    else
                    {
                        if (index >= realTokens.Count || realTokens[index].Item1 != "SEMICOLON")
                        {
                            assign = "Missing Semicolon";
                        }
                        else
                        {
                            assign = "Wrong in expression in assignment";
                        }
                    }
                }
                else
                {
                    assignIsRight = false;
                    assign = "Assign is wrong, missing '=' ";
                    return assign;
                }
            }
            else
            {
                assignIsRight = false;
                assign = "Assign is wrong, missing identifier";
                return assign;
            }    

            return assign;
}

        string ParseDeclaration(out bool declareIsRight)
        {
            // assignment rule
            /*
                *  <declaration> → متغير IDENT = <expression> ;
             */
            ParseTree += $"<declaration> → متغير IDENT = <expression> ;{Environment.NewLine}";
            declareIsRight = true;
            bool expIsRight;
            string declare = "";
            if (index < realTokens.Count && realTokens[index].Item1 == "KEYWORD")
            {
                declare += realTokens[index].Item2;
                index += 1;
                if (realTokens[index].Item1 == "IDENT")
                {
                    declare += realTokens[index].Item2;
                    index += 1;
                    if (realTokens[index].Item1 == "ASSIGN")
                    {
                        declare += realTokens[index].Item2;
                        index += 1;
                        ParseTree += $"<expression> → ";
                        string check = ParseExpression(out expIsRight);
                        Expression.Text = check + $"  ${expIsRight}";
                        if (expIsRight)
                        {
                            declare += check;
                            if (index < realTokens.Count && realTokens[index].Item1 == "SEMICOLON")
                            {
                                declare += realTokens[index].Item2;
                                index += 1;
                                return declare;
                            }
                            else
                            {
                                declare = "Missing Semicolon";
                                return declare;
                            }
                        }
                        else
                        {
                            if (index >= realTokens.Count || realTokens[index].Item1 != "SEMICOLON")
                            {
                                declare = "Missing Semicolon";
                            }
                            else
                            {
                                declare = "Wrong in expression in declaration";
                            }
                        }
                    }
                    else
                    {
                        declareIsRight = false;
                        declare = "Assign is wrong, missing '=' ";
                        return declare;
                    }
                }
                else
                {
                    declareIsRight = false;
                    declare = "declare is wrong, missing identifier";
                    return declare;
                }
            }
            else
            {
                declareIsRight = false;
                declare = "declare is wrong, missing keyword";
                return declare;
            }
            

            return declare;
        }

        string ParseWhile(out bool whileIsRight)
        {
            // while rule
            /*
             * <while_statement> → طالما ( <condition> ) 
             * {
                <statement_list> 
                }
             */
            whileIsRight = true;
            string whle = "";
            bool conIsRight, stIsRight;
            if (index < realTokens.Count && realTokens[index].Item1 == "KEYWORD")
            {
                whle += realTokens[index].Item2;
                index += 1;
                if (index < realTokens.Count && realTokens[index].Item1 == "LPAREN")
                {
                    whle += realTokens[index].Item2;
                    index += 1;
                    string con = Condition(out conIsRight);
                    if (conIsRight)
                    { 
                        whle += con;
                        if (index < realTokens.Count && realTokens[index].Item1 == "RPAREN")
                        {
                            whle += realTokens[index].Item2;
                            index += 1;
                            if (index < realTokens.Count && realTokens[index].Item1 == "LBRACE")
                            {
                               whle += realTokens[index].Item2;
                                index += 1;
                                string st = ParseStatementList(out stIsRight);
                                if (stIsRight)
                                {
                                    whle += st;
                                    if (index < realTokens.Count && realTokens[index].Item1 == "RBRACE")
                                    {
                                        whle += realTokens[index].Item2;
                                        index += 1;
                                        // correct while loop
                                        return whle;
                                    }
                                    else
                                    {
                                        whileIsRight = false;
                                        whle = $"Wrong in while statment{Environment.NewLine}" +
                                                $"Problem: Missing  RBRACE";
                                    }
                                }
                                else
                                {
                                    whileIsRight = false;
                                    whle = $"Wrong in while statment{Environment.NewLine}" +
                                            $"Problem: {st}";
                                }
                            }
                            else
                            {

                                whileIsRight = false;
                                whle = $"Wrong in while statment{Environment.NewLine}" +
                                    $"Problem: Missing LBRACE";
                            }
                        }
                        else
                        {
                            whileIsRight = false;
                            whle = $"Wrong in while statment{Environment.NewLine}" +
                                    $"Problem: Missing RPAREN";
                        }
                    }
                    else
                    {
                        whileIsRight = false;

                        whle = $"Wrong in while statment{Environment.NewLine}" +
                                $"Problem: {con}";
                    }
                }
                else
                {
                    whileIsRight = false;

                    whle = $"Wrong in while statment{Environment.NewLine}" +
                    $"Problem: Missing LPAREN";
                }
            }
            else
            {
                whileIsRight = false;
                whle = $"Wrong in while statment{Environment.NewLine}" +
                    $"Problem: Missing Keyword";
            }
            return whle;
        }

        string ParseIf(out bool ifIsRight)
        {
            // if rule
            /*
             * <if_statement> → اذا>) condition>) {<statement_list>}
             */
            ifIsRight = true;
            bool conIsRight, stIsRight;
            string ifcode = "";
            if (index < realTokens.Count && realTokens[index].Item1 == "KEYWORD")
            {
                ifcode += realTokens[index].Item2;
                index += 1;
                if (index < realTokens.Count && realTokens[index].Item1 == "LPAREN")
                {
                    ifcode += realTokens[index].Item2;
                    index += 1;
                    string con = Condition(out conIsRight);
                    if (conIsRight)
                    {
                        ifcode += con;
                        if (index < realTokens.Count && realTokens[index].Item1 == "RPAREN")
                        {
                            ifcode += realTokens[index].Item2;
                            index += 1;
                            if (index < realTokens.Count && realTokens[index].Item1 == "LBRACE")
                            {
                                ifcode += realTokens[index].Item2;
                                index += 1;

                                string st = ParseStatementList(out stIsRight);
                                if (stIsRight)
                                {
                                    ifcode += st;
                                    if (index < realTokens.Count && realTokens[index].Item1 == "RBRACE")
                                    {
                                        ifcode += realTokens[index].Item2;
                                        index += 1;
                                        return ifcode;
                                    }
                                    else
                                    {
                                        ifIsRight = false;
                                        ifcode = $"Wrong in if statment{Environment.NewLine}" +
                                        $"Problem: Missing RBRACE";
                                    }
                                }
                                else
                                {
                                    ifIsRight = false;
                                    ifcode = $"Wrong in if statment{Environment.NewLine}" +
                                            $"Problem: {st}";
                                }
                            }
                            else
                            {
                                ifIsRight = false;
                                ifcode = $"Wrong in if statment{Environment.NewLine}" +
                                $"Problem: Missing LBRACE";
                            }
                        }
                        else
                        {
                            ifIsRight = false;
                            ifcode = $"Wrong in if statment{Environment.NewLine}" +
                                $"Problem: Missing RPAREN";
                        }
                    }
                    else
                    {
                        ifIsRight = false;
                        ifcode = $"Wrong in if statment{Environment.NewLine}" +
                            $"Problem: {con}";
                    }
                }
                else
                {
                    ifIsRight = false;
                    ifcode = $"Wrong in if statment{Environment.NewLine}" +
                         $"Problem: Missing LPAREN";
                }
            }
            else
            {
                ifIsRight = false;
                ifcode = $"Wrong in if statment{Environment.NewLine}" +
                    $"Problem: Missing Keyword";
            }
            return ifcode;
        }
        string ParseStatementList(out bool stateListIsRight)
        {
            // statementList Rule
            /*
             * <statement_list> → <statement> <statement_list> | ε
             */
            stateListIsRight = true;
            string statementListCode = "";
            bool stIsRight;
            string st = Statement(out stIsRight);
            if (stIsRight)
            {
                statementListCode += st;
                if (index < realTokens.Count)
                {
                    string s = ParseStatementList(out stateListIsRight);
                    if (stateListIsRight)
                    {
                        statementListCode += s;
                    }
                    else
                    {
                        stateListIsRight = false;
                        statementListCode = $"Wrong in statement List{Environment.NewLine}" +
                                        $"Problem: {s}";
                    }
                }
                else
                {
                    return statementListCode;
                }
            }
            else
            {
                stateListIsRight = false;
                statementListCode = $"Wrong in statement List{Environment.NewLine}" +
                                $"Problem: {st}";
            }
            return statementListCode;
        }

    }
}
