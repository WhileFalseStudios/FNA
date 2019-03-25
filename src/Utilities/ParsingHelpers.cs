using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    public static class ParsingHelpers
    {
        /// <summary>
        /// Tokenise a given string by the specified character.
        /// </summary>
        /// <param name="input">The string to tokenise</param>
        /// <param name="token">The splitting character</param>
        /// <param name="allowQuotesToEscape">Allow quoted sections to use the tokenising character without escaping</param>
        /// <param name="allowSlashToEscape">Allow the use of a backslash to escape the tokenising character</param>
        /// <returns></returns>
        public static List<string> Tokenise(string input, char token, bool allowQuotesToEscape, bool allowSlashToEscape)
        {
            List<string> tokens = new List<string>();
            bool inEscapeQuotes = false;
            bool slashEscape = false;
            StringBuilder currentToken = new StringBuilder();

            foreach (var c in input)
            {
                slashEscape = false;

                if (c == token && (!inEscapeQuotes && !slashEscape))
                {
                    tokens.Add(currentToken.ToString());
                    currentToken.Clear();
                }
                else if (c == '"' && allowQuotesToEscape && !slashEscape)
                {
                    inEscapeQuotes = !inEscapeQuotes;
                    currentToken.Append(c);
                }
                else if (c == '\\' && !slashEscape)
                {
                    slashEscape = true;
                }
                else
                {
                    currentToken.Append(c);
                }                
            }

            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            return tokens;
        }
    }
}
