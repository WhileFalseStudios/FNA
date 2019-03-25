using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    class SpriteFontLoader : IAssetLoader<SpriteFont>
    {
        public AssetManager Context { get; set; }
        public string FilePath { get; set; }

        public SpriteFont LoadFromStream(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                int lineSpacing = 0;
                string texPage = string.Empty;
                float spacing = 13f;
                List<char> chars = new List<char>();
                List<Rectangle> bounds = new List<Rectangle>();
                List<Rectangle> cropping = new List<Rectangle>();
                List<Vector3> kerning = new List<Vector3>();

                while (true)
                {
                    var input = sr.ReadLine();
                    if (input == null)
                        break;

                    input = input.TrimStart(' ');
                    var tokens = ParsingHelpers.Tokenise(input, ' ', true, false);
                    string lineType = tokens[0];
                    Dictionary<string, string> kvPairs = new Dictionary<string, string>();
                    for (int i = 1; i < tokens.Count; i++)
                    {
                        var split = ParsingHelpers.Tokenise(tokens[i], '=', true, false);
                        kvPairs.Add(split[0], split[1]);
                    }

                    switch (lineType)
                    {
                        case "info":
                            //spacing = float.Parse(kvPairs["spacing"].Split(',')[0]);
                            break;
                        case "common":
                            lineSpacing = int.Parse(kvPairs["lineHeight"]);
                            break;
                        case "page":
                            texPage = kvPairs["file"].Trim('"');
                            break;
                        case "char":
                            chars.Add(kvPairs["letter"][1]); //HACKHACK
                            bounds.Add(new Rectangle(int.Parse(kvPairs["x"]), int.Parse(kvPairs["y"]), int.Parse(kvPairs["width"]), int.Parse(kvPairs["height"])));
                            cropping.Add(new Rectangle(0, 0, 0, 0));
                            kerning.Add(Vector3.One);
                            //spacing = float.Parse(kvPairs["xadvance"]); //FIXME: bmfont has this per-character but we need it set globally for the font, can we hack this together using kerning pairs?
                            break;
                    }
                }

                Texture2D tex = Context.Load<Texture2D>(Path.Combine(Path.GetDirectoryName(FilePath), texPage));
                SpriteFont font = new SpriteFont(tex, bounds, cropping, chars, lineSpacing, spacing, kerning, null);
                return font;
            }
        }
    }
}
