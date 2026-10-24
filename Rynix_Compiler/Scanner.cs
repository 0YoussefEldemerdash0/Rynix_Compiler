using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    KW_VAR, KW_VAL, KW_FUN, KW_CLASS, KW_IF, KW_ELSE, KW_WHILE, KW_FOR, KW_IN,
    KW_RETURN, KW_TRUE, KW_FALSE, KW_NULL, KW_NEW, KW_PRINT, KW_INPUT,
    KW_BREAK, KW_CONTINUE, KW_INIT,
    KW_INT, KW_FLOAT, KW_STRING, KW_BOOL, KW_CHAR, KW_VOID, KW_ARRAY,
    Identifier,
    IntLiteral, FloatLiteral, StringLiteral, CharLiteral,
    Plus, Minus, Multiply, Divide, Modulus,
    Equal, NotEqual, LessThan, GreaterThan, LessEqual, GreaterEqual,
    Assign, Range,
    LParen, RParen, LBrace, RBrace, LBracket, RBracket, Colon, Comma, Dot
}

namespace Rynix_Compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
        public int line;
        public int col;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        private string _src = "";
        private int _pos, _line, _col;

        private bool AtEnd => _pos >= _src.Length;
        private char Cur => _pos < _src.Length ? _src[_pos] : '\0';
        private char Peek(int n = 1) => _pos + n < _src.Length ? _src[_pos + n] : '\0';

        private static bool IsDigit(char c) => c >= '0' && c <= '9';
        private static bool IsLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        private static bool IsIdStart(char c) => IsLetter(c) || c == '_';
        private static bool IsIdPart(char c) => IsLetter(c) || IsDigit(c) || c == '_';

        public Scanner()
        {
            ReservedWords.Add("var", Token_Class.KW_VAR);
            ReservedWords.Add("val", Token_Class.KW_VAL);
            ReservedWords.Add("fun", Token_Class.KW_FUN);
            ReservedWords.Add("class", Token_Class.KW_CLASS);
            ReservedWords.Add("if", Token_Class.KW_IF);
            ReservedWords.Add("else", Token_Class.KW_ELSE);
            ReservedWords.Add("while", Token_Class.KW_WHILE);
            ReservedWords.Add("for", Token_Class.KW_FOR);
            ReservedWords.Add("in", Token_Class.KW_IN);
            ReservedWords.Add("return", Token_Class.KW_RETURN);
            ReservedWords.Add("true", Token_Class.KW_TRUE);
            ReservedWords.Add("false", Token_Class.KW_FALSE);
            ReservedWords.Add("null", Token_Class.KW_NULL);
            ReservedWords.Add("new", Token_Class.KW_NEW);
            ReservedWords.Add("print", Token_Class.KW_PRINT);
            ReservedWords.Add("input", Token_Class.KW_INPUT);
            ReservedWords.Add("break", Token_Class.KW_BREAK);
            ReservedWords.Add("continue", Token_Class.KW_CONTINUE);
            ReservedWords.Add("init", Token_Class.KW_INIT);

            ReservedWords.Add("Int", Token_Class.KW_INT);
            ReservedWords.Add("Float", Token_Class.KW_FLOAT);
            ReservedWords.Add("String", Token_Class.KW_STRING);
            ReservedWords.Add("Bool", Token_Class.KW_BOOL);
            ReservedWords.Add("Char", Token_Class.KW_CHAR);
            ReservedWords.Add("Void", Token_Class.KW_VOID);
            ReservedWords.Add("Array", Token_Class.KW_ARRAY);

            Operators.Add("+", Token_Class.Plus);
            Operators.Add("-", Token_Class.Minus);
            Operators.Add("*", Token_Class.Multiply);
            Operators.Add("/", Token_Class.Divide);
            Operators.Add("%", Token_Class.Modulus);
            Operators.Add("==", Token_Class.Equal);
            Operators.Add("!=", Token_Class.NotEqual);
            Operators.Add("<", Token_Class.LessThan);
            Operators.Add(">", Token_Class.GreaterThan);
            Operators.Add("<=", Token_Class.LessEqual);
            Operators.Add(">=", Token_Class.GreaterEqual);
            Operators.Add("=", Token_Class.Assign);
            Operators.Add("..", Token_Class.Range);
            Operators.Add("(", Token_Class.LParen);
            Operators.Add(")", Token_Class.RParen);
            Operators.Add("{", Token_Class.LBrace);
            Operators.Add("}", Token_Class.RBrace);
            Operators.Add("[", Token_Class.LBracket);
            Operators.Add("]", Token_Class.RBracket);
            Operators.Add(":", Token_Class.Colon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add(".", Token_Class.Dot);
        }

        private void Advance()
        {
            if (AtEnd) return;
            if (_src[_pos] == '\n') { _line++; _col = 1; }
            else _col++;
            _pos++;
        }

        private void AddToken(Token_Class cls, string lexeme, int line, int col)
        {
            Tokens.Add(new Token { lex = lexeme, token_type = cls, line = line, col = col });
        }

        private void AddError(string msg, string lexeme, int line, int col)
        {
            Errors.Error_List.Add($"Line {line}, Col {col}: {msg}" + (string.IsNullOrEmpty(lexeme) ? "" : $"   ->   {lexeme}"));
        }

        private bool PrevIsOperand()
        {
            if (Tokens.Count == 0) return false;
            switch (Tokens[Tokens.Count - 1].token_type)
            {
                case Token_Class.Identifier:
                case Token_Class.IntLiteral:
                case Token_Class.FloatLiteral:
                case Token_Class.StringLiteral:
                case Token_Class.CharLiteral:
                case Token_Class.RParen:
                case Token_Class.RBracket:
                case Token_Class.KW_TRUE:
                case Token_Class.KW_FALSE:
                case Token_Class.KW_NULL:
                    return true;
                default:
                    return false;
            }
        }

        public void StartScanning(string SourceCode)
        {
            _src = SourceCode ?? "";
            _pos = 0; _line = 1; _col = 1;
            Tokens = new List<Token>();
            Errors.Error_List.Clear();

            while (!AtEnd)
            {
                char c = Cur;

                if (c == ' ' || c == '\t' || c == '\r' || c == '\n') { Advance(); continue; }

                if (c == '/' && Peek() == '/') { while (!AtEnd && Cur != '\n') Advance(); continue; }
                if (c == '/' && Peek() == '*') { ScanBlockComment(); continue; }

                if (IsIdStart(c)) { ScanWord(); continue; }

                if (IsDigit(c) || (c == '-' && IsDigit(Peek()) && !PrevIsOperand())) { ScanNumber(c == '-'); continue; }

                if (c == '"') { ScanString(); continue; }
                if (c == '\'') { ScanChar(); continue; }

                ScanSymbol();
            }
        }

        private void ScanBlockComment()
        {
            int startLine = _line, startCol = _col;
            Advance(); Advance();  
            while (!AtEnd && !(Cur == '*' && Peek() == '/')) Advance();
            if (AtEnd)
            {
                AddError("Unterminated multi-line comment", "/*", startLine, startCol);
                return;
            }
            Advance(); Advance(); 
        }

        private void ScanWord()
        {
            int startPos = _pos, startLine = _line, startCol = _col;
            while (IsIdPart(Cur)) Advance();
            string word = _src.Substring(startPos, _pos - startPos);

            FindTokenClass(word, startLine, startCol);
        }

        private void ScanNumber(bool negative)
        {
            int startPos = _pos, startLine = _line, startCol = _col;
            if (negative) Advance();

            int digitsStart = _pos;
            while (IsDigit(Cur)) Advance();
            string intPart = _src.Substring(digitsStart, _pos - digitsStart);

            bool isFloat = false;
            if (Cur == '.' && IsDigit(Peek()))
            {
                isFloat = true;
                Advance();
                while (IsDigit(Cur)) Advance();

                if (Cur == 'e' || Cur == 'E')
                {
                    int k = 1;
                    if (Peek(k) == '+' || Peek(k) == '-') k++;
                    if (IsDigit(Peek(k)))
                    {
                        for (int i = 0; i < k; i++) Advance();
                        while (IsDigit(Cur)) Advance();
                    }
                }
            }

            if (IsIdStart(Cur))
            {
                while (IsIdPart(Cur)) Advance();
                AddError("Invalid token: identifiers cannot start with a digit / malformed number", _src.Substring(startPos, _pos - startPos), startLine, startCol);
                return;
            }

            string numLexeme = _src.Substring(startPos, _pos - startPos);
            if (!isFloat)
            {
                bool leadingZero = intPart.Length > 1 && intPart[0] == '0';
                bool negZero = negative && intPart == "0";
                if (leadingZero || negZero)
                {
                    AddError("Invalid integer literal (leading zeros are not allowed)", numLexeme, startLine, startCol);
                    return;
                }
            }

            AddToken(isFloat ? Token_Class.FloatLiteral : Token_Class.IntLiteral, numLexeme, startLine, startCol);
        }

        private void ScanString()
        {
            int startLine = _line, startCol = _col;
            int startPos = _pos;
            Advance();  

            while (true)
            {
                if (AtEnd || Cur == '\n')
                {
                    AddError("Unterminated string literal", _src.Substring(startPos, _pos - startPos), startLine, startCol);
                    return;
                }
                if (Cur == '\\')
                {
                    if (Peek() == '\0' || Peek() == '\n')
                    {
                        Advance();
                        AddError("Unterminated string literal", _src.Substring(startPos, _pos - startPos), startLine, startCol);
                        return;
                    }
                    Advance(); Advance();
                    continue;
                }
                if (Cur == '"') { Advance(); break; }
                Advance();
            }
            AddToken(Token_Class.StringLiteral, _src.Substring(startPos, _pos - startPos), startLine, startCol);
        }

        private void ScanChar()
        {
            int startLine = _line, startCol = _col;
            int startPos = _pos;
            Advance();  

            if (AtEnd || Cur == '\n')
            {
                AddError("Unterminated character literal", "'", startLine, startCol);
                return;
            }

            if (Cur == '\'')
            {
                Advance();
                AddError("Empty character literal", "''", startLine, startCol);
                return;
            }

            if (Cur == '\\')
            {
                if (Peek() == '\0' || Peek() == '\n')
                {
                    Advance();
                    AddError("Unterminated character literal", _src.Substring(startPos, _pos - startPos), startLine, startCol);
                    return;
                }
                Advance(); Advance();
            }
            else Advance();

            if (Cur == '\'')
            {
                Advance();
                AddToken(Token_Class.CharLiteral, _src.Substring(startPos, _pos - startPos), startLine, startCol);
                return;
            }

            while (!AtEnd && Cur != '\n' && Cur != '\'') Advance();
            if (Cur == '\'')
            {
                Advance();
                AddError("Character literal must contain exactly one character", _src.Substring(startPos, _pos - startPos), startLine, startCol);
            }
            else
            {
                AddError("Unterminated character literal", _src.Substring(startPos, _pos - startPos), startLine, startCol);
            }
        }

        private void ScanSymbol()
        {
            int startLine = _line, startCol = _col;
            char c = Cur;
            char n = Peek();

            string twoCharOp = $"{c}{n}";
            if (Operators.ContainsKey(twoCharOp))
            {
                Advance(); Advance();
                AddToken(Operators[twoCharOp], twoCharOp, startLine, startCol);
                return;
            }

            string oneCharOp = c.ToString();
            if (Operators.ContainsKey(oneCharOp))
            {
                Advance();
                AddToken(Operators[oneCharOp], oneCharOp, startLine, startCol);
                return;
            }

            Advance();
            AddError(c == '!' ? "Unexpected '!' (did you mean '!=' ?)" : "Unknown character", c.ToString(), startLine, startCol);
        }

        void FindTokenClass(string Lex, int line = 1, int col = 1)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            Tok.line = line;
            Tok.col = col;

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
                Tokens.Add(Tok);
            }
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.IntLiteral; 
                Tokens.Add(Tok);
            }
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else
            {
                AddError("Unknown character / undefined token", Lex, line, col);
            }
        }

        bool isIdentifier(string lex)
        {
            if (string.IsNullOrEmpty(lex)) return false;
            if (!IsIdStart(lex[0])) return false;
            for (int i = 1; i < lex.Length; i++)
            {
                if (!IsIdPart(lex[i])) return false;
            }
            return true;
        }

        bool isConstant(string lex)
        {
            if (string.IsNullOrEmpty(lex)) return false;
            foreach (char c in lex)
            {
                if (!IsDigit(c)) return false;
            }
            return true;
        }
    }
}