using System;
using System.Drawing; // contains funtionality to read and write image files, but only on windows

using static System.Console;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PA2_Class
{
    // break into tiles, lot of tiles, use linked list, should have a tile class inside the retina class
    class Retina
    {
        class Tile
        {
            public const int Len = 8;

            //want to move those outside pixels inide the tiles
            public Color[,] Pixels { get; set; }

            public Tile( /*color [ , ] pixels lamer aproach below */ )
            {
                Pixels = new Color[Len, Len];
                // now have a tile that can hold 8 x 8 piels
            }
        }

        Tile /* Color */ [,] Tiles; /* Pixels */ // no constructor
        
        // height and width properties as to not lose knowledge of the original image size
        public int Height { get; private set; }
		public int Width { get; private set; }

        public Retina(string path)
        {
            Image<Rgba32> img6L = Image.Load<Rgba32>(path);

            // but this is in division, cast to double
            // but it's still an int , so round up
            int tileRows = (int)Math.Ceiling((double)img6L.Height / Tile.Len);
            int tileCols = (int)Math.Ceiling((double)img6L.Width / Tile.Len);

            Tiles = new Tile[tileRows, tileCols];

            //~ Pixels = new Color[ img6L.Height, img6L.Width ];

            // iterate through initial Tile Array
            for (int i = 0; i < tileRows; i++)
            {
                for (int j = 0; j < tileCols; j++)
                {
                    // create a new array within the array (2 more for loops)
                    Tiles[i, j] = new Tile();

                    for (int x = 0; x < Tile.Len; x++)
                    {
                        for (int y = 0; y < Tile.Len; y++)
                        {
                           
                            if(i * Tile.Len + x > img6L.Height || j * Tile.Len + y > img6L.Width)
                            {
                                // for out of bounds (tiles along right and bot), change pixels to black
                                Tiles[i, j].Pixels[x, y] = Color.FromArgb(255, 0, 0, 0);
                            }

                            // else, change pixels to be colour of image
                            else
                            {
                                Rgba32 p = img6L[i * Tile.Len + x, j * Tile.Len + y];
                                //  for pixel

                                // need constructor
                                Color c = Color.FromArgb(p.A, p.R, p.G, p.B);
                                //~ Pixels [y, x] = c;

                                Tiles[i, j].Pixels[x, y] = c;
                            }
                        }
                    }
                }
            }
        }

        public void SaveToFile(string path)
        {
            Image<Rgba32> img6L = new Image<Rgba32>(Tiles.GetLength(0)*Tile.Len, Tiles.GetLength(1) * Tile.Len);

            //same prcoess as above
			//iterate through the array within the array
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    for (int x = 0; x < Tile.Len; x++)
                    {
                        for (int y = 0; y < Tile.Len; y++)
                        {
                            Color c = Tiles[i, j].Pixels[x, y];
                            img6L[i*Tile.Len + x, j*Tile.Len + y] = new Rgba32(c.R, c.G, c.B, c.A);
                        }
                    }
                }
            }
            img6L.Save(path);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            // we want the image as 8x8 pixels in 1 tile, and a 2D array of tiles, need a tile object ot hold 8x8 array,
            const string path = "20051020_63711_0100_PP.png";
            Retina retina = new Retina(path);
            retina.SaveToFile("test.png");

        }
    }
}
