namespace ColoredCanvasDrawer
{
    public class DrawingCanvas : ICloneable
    {
        private byte[][] pixels;

        public DrawingCanvas(int width, int height)
        {
            if (width < 32 || width > 1024)
            {
                throw new ArgumentException("Invalid width! Width should be in range [32 ... 1024].");
            }

            if (height < 32 || height > 1024)
            {
                throw new ArgumentException("Invalid height! Height should be in range [32 ... 1024].");
            }

            int widthColor = width * 3;
            this.pixels = new byte[height][];
            for (int row = 0; row < height; row++)
            {
                this.pixels[row] = new byte[widthColor];
            }

            this.FillAllPixels();
        }

        public int Height => this.pixels.Length;

        public int Width => this.pixels[0].Length;

        public void FillAllPixels()
        {
            byte mask = 0;
            mask = (byte)~mask;

            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    this.pixels[row][col] = mask;
                }
            }
        }

        public void InvertAllPixels()
        {
            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    this.pixels[row][col] = (byte)~this.pixels[row][col];
                }
            }
        }

        public Color GetPixel(int row, int col)
        {
            CheckBounds(row, col);

            int pixelOffset = col * 3;

            byte red = this.pixels[row][pixelOffset];
            byte green = this.pixels[row][pixelOffset + 1];
            byte blue = this.pixels[row][pixelOffset + 2];

            return Color.FromArgb(red, green, blue);
        }

        public void SetPixel(int row, int col, Color color)
        {
            CheckBounds(row, col);
            byte pixelValue = pixels[row][col];
            throw new NotImplementedException();
        }

        public void DrawHorizontalLine(int row, int startCol, int endCol, Color color)
        {
            for (int col = startCol; col < endCol; col++)
            {
                this.SetPixel(row, col, color);
            }
        }

        public void DrawVerticalLine(int col, int startRow, int endRow, Color color)
        {
            for (int row = startRow; row < endRow; row++)
            {
                this.SetPixel(row, col, color);
            }
        }

        public void DrawDiagonalLine(int startCol, int startRow, int endCol,
            int endRow, Color color)
        {
            if (Math.Abs(endRow - startRow) < Math.Abs(endCol - startCol))
            {
                if (startCol > endCol)
                    PlotLineLow(endCol, endRow, startCol, startRow, color);
                else
                    PlotLineLow(startCol, startRow, endCol, endRow, color);
            }
            else
            {
                if (startRow > endRow)
                    PlotLineHigh(endCol, endRow, startCol, startRow, color);
                else
                    PlotLineHigh(startCol, startRow, endCol, endRow, color);
            }
        }

        public void DrawRectangle(int startRow, int startCol, int endRow, int endCol, Color color)
        {
            this.DrawHorizontalLine(startRow, startCol, endCol, color);
            this.DrawHorizontalLine(endRow, startCol, endCol, color);
            this.DrawVerticalLine(startCol, startRow, endRow, color);
            this.DrawVerticalLine(endCol, startRow, endRow, color);
        }

        public void DrawTriangle(int startRow, int startCol, int endRow,
            int endCol, Color color)
        {
            this.DrawDiagonalLine(startCol, startRow, endCol, startRow, color);
            this.DrawDiagonalLine(startCol, startRow, (endCol - startCol) / 2, endRow, color);
            this.DrawDiagonalLine(endCol, startRow, (endCol - startCol) / 2, endRow, color);
        }

        private void PlotLineLow(int startCol, int startRow, int endCol,
            int endRow, Color color)
        {
            int dx = endCol - startCol;
            int dy = endRow - startRow;
            int yi = 1;

            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = startRow;

            for (int x = startCol; x <= endCol; x++)
            {
                SetPixel(x, y, color);

                if (D > 0)
                {
                    y += yi;
                    D += 2 * (dy - dx);
                }
                else
                {
                    D += 2 * dy;
                }
            }
        }

        private void PlotLineHigh(int startCol, int startRow, int endCol,
            int endRow, Color color)
        {
            throw new NotImplementedException();
        }

        private void CheckBounds(int height, int width)
        {
            int maxWidth = this.Width * 3 - 1;
            if (width < 0 || width > maxWidth)
            {
                throw new ArgumentException($"Invalid width! Width should be in range [0 ... {maxWidth}].");
            }

            int maxHeight = this.Height - 1;
            if (height < 0 || height > maxHeight)
            {
                throw new ArgumentException($"Invalid height! Height should be in range [0 ... {maxHeight}].");
            }
        }

        public object Clone()
        {
            DrawingCanvas clone = new DrawingCanvas(this.Width / 3, this.Height);

            for (int row = 0; row < this.Height; row++)
            {
                Array.Copy(this.pixels[row], clone.pixels[row], this.pixels[row].Length);
            }

            return clone;
        }
    }
}
