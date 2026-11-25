using Lex;
using Lex.Dsl;
using Lex.Parser;
using Lex.Tokens;

namespace Cubase.Macro.Services.Lex
{
    public class LexParser
    {
        public LexParser()
        {

        }


        public List<Token> Parse(string macroText)
        {
            var tokens = new List<Token>();

            using LexicalParser parser = LexicalParserFactory.CreateFrom("""
                standard comments
                keywords 'var', 'if', 'then', 'else', 'end', 'null', 'true', 'false', 'and', 'or', 'not'
                identifiers
                numbers
                double quoted strings
                bounders
                predefined operators
                whitespace
              """);

            parser.SetSource(macroText.AsReader());
            while (!parser.IsAtEnd())
            {
                tokens.Add(parser.GetNextToken());
                //var typ = token.GetType();
                //Console.WriteLine(token);
            }
            return tokens;
        }
    }
}
