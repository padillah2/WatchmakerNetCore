using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BiomorphNetcore
{
    public partial class Form1 : Form
    {
        List<BiomorphImage> pictureBoxes;
        IBiomorphFactory biomorphFactory;

        public Form1()
        {
            InitializeComponent();

            pictureBoxes = new List<BiomorphImage>(9);

            // Pick the appropriate factory
            //biomorphFactory = new DawsonFactory(); //RandomFactory
            biomorphFactory = new RandomFactory(); 

            // Need a new Biomorph
            //Biomorph rootBiomorph = BiomorphFactory.generateRandomCandidate();
            Biomorph rootBiomorph = biomorphFactory.generateCandidateList();

            List<Biomorph> biomes = new List<Biomorph>();


            for (int i = 0; i < (DawsonFactory.GENE_COUNT * 2); i++)
            {
                BiomorphImage newImage = new BiomorphImage()
                { BackColor = Color.AliceBlue, ForeColor = Color.Black, Dock = DockStyle.Fill };
                newImage.Click += NewImage_Click;

                pictureBoxes.Add(newImage);
                biomes.Add(rootBiomorph.Clone());
            }

            tableLayoutPanel1.Controls.Add(pictureBoxes[0], 0, 0);
            tableLayoutPanel1.Controls.Add(pictureBoxes[1], 1, 0);
            tableLayoutPanel1.Controls.Add(pictureBoxes[2], 2, 0);
            tableLayoutPanel1.Controls.Add(pictureBoxes[3], 3, 0);

            tableLayoutPanel1.Controls.Add(pictureBoxes[4], 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxes[5], 1, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxes[6], 2, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxes[7], 3, 1);

            tableLayoutPanel1.Controls.Add(pictureBoxes[8], 0, 2);
            tableLayoutPanel1.Controls.Add(pictureBoxes[9], 1, 2);
            tableLayoutPanel1.Controls.Add(pictureBoxes[10], 2, 2);
            tableLayoutPanel1.Controls.Add(pictureBoxes[11], 3, 2);

            tableLayoutPanel1.Controls.Add(pictureBoxes[12], 0, 3);
            tableLayoutPanel1.Controls.Add(pictureBoxes[13], 1, 3);
            tableLayoutPanel1.Controls.Add(pictureBoxes[14], 2, 3);
            tableLayoutPanel1.Controls.Add(pictureBoxes[15], 3, 3);

            biomes = biomorphFactory.MutatePopulation(biomes[0]);

            for (int index = 0; index < pictureBoxes.Count; index++)
            {
                pictureBoxes[index].draw(biomes[index]);
            }
        }

        private void NewImage_Click(object? sender, EventArgs e)
        {
            BiomorphImage? currentImage = sender as BiomorphImage;
            if (currentImage != null)
            {
                List<Biomorph> biomes = new List<Biomorph>();
                var currentBiomorph = currentImage.Biomorph;

                for (int i = 0; i < DawsonFactory.GENE_COUNT; i++)
                {
                    biomes.Add(currentBiomorph.Clone());
                }

                biomes = biomorphFactory.MutatePopulation(currentBiomorph);

                for (int index = 0; index < pictureBoxes.Count; index++)
                {
                    pictureBoxes[index].draw(biomes[index]);
                }
            }
        }
    }
}