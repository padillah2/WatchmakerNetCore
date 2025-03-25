using System;
using System.Drawing;
using System.Windows.Forms;

namespace BiomorphNetcore
{
    public class BiomorphImage : PictureBox
    {
        private Bitmap backgroundImage;
        private Pen forePen;
        private Biomorph? thisBiomorph;

        private int minX;
        private int minY;
        private int maxX;
        private int maxY;

        public Biomorph Biomorph => thisBiomorph;
        public new Image BackgroundImage => backgroundImage;
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                forePen = new Pen(base.ForeColor);
            }
        }

        public BiomorphImage()
        {
            backgroundImage = new Bitmap(this.Width, this.Height);

            forePen = new Pen(ForeColor);
        }

        public void draw(Biomorph biomorph)
        {
            thisBiomorph = biomorph;

            // Dispose of old bitmap
            backgroundImage.Dispose();

            // Create a new bitmap with the new size
            backgroundImage = new Bitmap(this.Width - 1, this.Height);

            int x = this.Width / 2;
            int y = this.Height / 2;

            int[][] pattern = biomorph.getPatternPhenotype();
            int depth = biomorph.getLengthPhenotype();

            drawTree(x, y, depth,
                2, // Initial direction should be 2 or 6 to ensure horizontal symmetry.
                pattern[0], // dx
                pattern[1]); // dy

            using var drawGraphics = Graphics.FromImage(backgroundImage);
            drawGraphics.DrawLine(forePen, 0, 0, this.Width - 1, 0);
            drawGraphics.DrawLine(forePen, 0, this.Width - 1, this.Width - 1, this.Height - 1);
            drawGraphics.DrawLine(forePen, this.Width - 1, this.Height - 1, 0, this.Height - 1);
            drawGraphics.DrawLine(forePen, 0, this.Height - 1, 0, 0);

            var genes = biomorph.GetGenotype();
            var stringSize = drawGraphics.MeasureString("W", DefaultFont);

            for (int i = 0; i < genes.Length; i++)
            {
                drawGraphics.DrawString(genes[i].ToString(), DefaultFont, Brushes.Black, new Point(0, (i * 11)  ));
            }

            base.BackgroundImage = backgroundImage;

        }

        private void drawTree(int x, int y, int depth, int direction, int[] dx, int[] dy)
        {
            // Get the graphics object from the new bitmap
            using var drawGraphics = Graphics.FromImage(backgroundImage);

            // Make sure direction wraps round in the range 0 - 7.
            direction = (direction + 8) % 8;

            int x2 = x + depth * dx[direction];
            int y2 = y + depth * dy[direction];

            minX = Math.Min(minX, x);
            minY = Math.Min(minY, y);
            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);

            drawGraphics.DrawLine(forePen, x, y, x2, y2);

            if (depth > 0)
            {
                // Recursively draw the left and right branches of the tree.
                drawTree(x2, y2, depth - 1, direction - 1, dx, dy);
                drawTree(x2, y2, depth - 1, direction + 1, dx, dy);
            }
        }
    }
}
