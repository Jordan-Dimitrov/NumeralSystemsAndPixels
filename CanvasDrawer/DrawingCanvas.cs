namespace CanvasDrawer
{
    public class DrawingCanvas : ICloneable
    {
        private uint[][] pixels;

        public DrawingCanvas(int width, int height)
        {
            if (width < 32 || width > 1024 || width % 32 != 0)
            {
                throw new ArgumentException("Invalid width! Width should be in range [32 ... 1024] and should be divisible by 32.");
            }

            if (height < 32 || height > 1024)
            {
                throw new ArgumentException("Invalid height! Height should be in range [32 ... 1024].");
            }

            // Allocate the pixels in the image
            int widthInts = width / 32;
            this.pixels = new uint[height][];
            for (int row = 0; row < height; row++)
            {
                this.pixels[row] = new uint[widthInts];
            }

            // Uncomment when implemented
            //this.FillAllPixels(CanvasColor.White);
        }

        public int Width => this.pixels[0].Length * 32;

        public int Height => this.pixels.Length;

        public int RowCount => this.pixels.Length;

        public int ColCount => this.pixels[0].Length;

        public void FillAllPixels(CanvasColor color)
        {
            uint mask = 0;
            if (color == CanvasColor.White)
            {
                mask = ~mask;
            }

            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    pixels[row][col] = mask;
                }
            }
        }

        public void InvertAllPixels()
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    pixels[row][col] = ~pixels[row][col];
                }
            }
        }

        public CanvasColor GetPixel(int row, int col)
        {
            this.CheckBounds(row, col);

            uint[] targetRow = this.pixels[row];
            int targetCol = col / 32;
            uint targetInt = targetRow[targetCol];
            int targeBitIndex = col % 32;
            targetInt = (targetInt >> targeBitIndex) % 1;

            return (CanvasColor)targetInt;

        }

        public void SetPixel(int row, int col, CanvasColor color)
        {
            CheckBounds(col, row);
            int targetRow = row;
            int targetCollumn = col / 32;
            uint targetInt = pixels[targetCollumn][targetCollumn];
            int bitIndex = col % 32;
            targetInt &= ~(1u << (bitIndex));
            targetInt |= ((uint)color << (bitIndex));
            pixels[targetCollumn][targetCollumn] = targetInt;
        }

        public void DrawHorizontalLine(int row, int startCol, int endCol,
            CanvasColor color)
        {
            for (int i = startCol; i < endCol; i++)
                SetPixel(row, i, color);
        }

        public void DrawVerticalLine(int col, int startRow, int endRow,
            CanvasColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(int startRow, int startCol, int endRow, int endCol,
            CanvasColor color)
        {
            throw new NotImplementedException();
        }

        private void CheckBounds(int height, int width)
        {
            int maxWidth = this.Width - 1;
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
            DrawingCanvas clone = new DrawingCanvas(this.Width, this.Height);

            for (int row = 0; row < this.RowCount; row++)
            {
                Array.Copy(this.pixels[row], clone.pixels[row], this.pixels[row].Length);
            }

            return clone;
        }
    }
}
