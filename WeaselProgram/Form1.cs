using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

// Add a switch to disregard phrase length
// Add capability to include all ASCII characters

namespace WeaselProgram
{
    public partial class Form1 : Form
    {
        Stopwatch runTime;
        BackgroundWorker phraseWorker;
        string weaselPhrase;
        int changePercentage;

        public Form1()
        {
            InitializeComponent();

            phraseWorker = new BackgroundWorker();
            phraseWorker.WorkerSupportsCancellation = true;
            phraseWorker.WorkerReportsProgress = true;
            phraseWorker.ProgressChanged += PhraseWorker_ProgressChanged;
            phraseWorker.DoWork += PhraseWorker_DoWork;
            phraseWorker.RunWorkerCompleted += PhraseWorker_RunWorkerCompleted;

            runTime = new Stopwatch();
            weaselPhrase = string.Empty;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxPhrase.Text))
            {
                _ = MessageBox.Show("The phrase is blank. There is nothing to compute.\r\nPlease enter a phrase and start again.");
                _ = textBoxPhrase.Focus();
                return;
            }

            listBoxResults.Items.Clear();

            textBoxPhrase.Enabled = false;
            buttonStart.Enabled = false;
            numericUpDownChangePercentage.Enabled = false;

            buttonStop.Enabled = true;

            string validatePhrase = textBoxPhrase.Text;
            for (int i = 0; i < validatePhrase.Length; i++)
            {
                if (validatePhrase[i] != 32)

                {
                    if ((validatePhrase[i] < 65) || (validatePhrase[i] > 90))
                    {
                        if ((validatePhrase[i] < 97) || (validatePhrase[i] > 122))
                        {
                            _ = MessageBox.Show("The phrase can only contain characters A - Z, a - z, and <space>. Please remove any special characters and try again.");
                            _ = textBoxPhrase.Focus();
                            return;
                        }
                    }
                }
            }

            if (phraseWorker.IsBusy != true)
            {
                progressBar1.Value = 0;
                labelRunTime.Text = String.Empty;
                labelGuessCount.Text = String.Empty;

                changePercentage = (int)numericUpDownChangePercentage.Value;
                weaselPhrase = validatePhrase;
                phraseWorker.RunWorkerAsync(validatePhrase);
            }
        }

        private void PhraseWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                e.Cancel = true;
                return;
            }

            string? findPhrase = e.Argument?.ToString();

            if (string.IsNullOrWhiteSpace(findPhrase))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                runTime = Stopwatch.StartNew();
                if (!FindPhrase(findPhrase, worker))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void PhraseWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is not Phrase nextGuess)
            {
                phraseWorker.CancelAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(nextGuess.PhraseString))
            {
                return;
            }

            // Add the match to the listBox
            _ = listBoxResults.Items.Add(nextGuess);

            progressBar1.Value = (nextGuess.Score / weaselPhrase.Length) * 100;

            if (listBoxResults.Items.Count > 50000)
            {
                phraseWorker.CancelAsync();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (phraseWorker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                phraseWorker.CancelAsync();
            }
        }

        private void PhraseWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            runTime.Stop();

            if (e.Cancelled == true)
            {
                labelRunTime.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelRunTime.Text = "Error: Something went wrong";
            }
            else
            {
                labelRunTime.Text = runTime.Elapsed.ToString();
            }

            labelGuessCount.Text = listBoxResults.Items.Count.ToString();

            textBoxPhrase.Enabled = true;
            buttonStart.Enabled = true;
            numericUpDownChangePercentage.Enabled = true;
            buttonStop.Enabled = false;

            runTime.Reset();
        }

        private bool FindPhrase(string weaselPhrase, BackgroundWorker worker)
        {
            Phrase nextGuess;
            List<Phrase> attemptPhrases = new();

            // Initialize 100 random strings
            attemptPhrases = NewPhrasePopulation(weaselPhrase);

            do
            {
                if (worker.CancellationPending == true)
                {
                    return false;
                }

                // Mutate the list of strings
                MutatePhraseList(attemptPhrases);

                // Get the closest match
                nextGuess = FuzzyMatch(attemptPhrases, weaselPhrase);

                // Add the match to the listBox
                //listBoxResults.Items.Add(nextGuess.ToString());
                worker.ReportProgress(0, nextGuess);

                attemptPhrases.Clear();

                // New list of 100 strings
                for (int i = 0; i < 100; i++)
                {
                    attemptPhrases.Add(new Phrase(nextGuess.PhraseString));
                }

            } while (nextGuess.PhraseString != weaselPhrase);

            return true;
        }

        private List<Phrase> NewPhrasePopulation(string weaselPhrase)
        {
            Random random = new Random();
            List<Phrase> newList = new List<Phrase>();

            for (int count = 0; count < 100; count++)
            {
                char[] newString = new char[weaselPhrase.Length];
                for (int currentLetter = 0; currentLetter < weaselPhrase.Length; currentLetter++)
                {
                    char newChar = GetValidChar();
                    newString[currentLetter] = (char)newChar;
                }

                newList.Add(new Phrase(new String(newString)));
            }

            return newList;
        }

        private Phrase FuzzyMatch(List<Phrase> attemptPhrases, string weaselPhrase)
        {
            Phrase bestMatch;

            foreach (var currentPhrase in attemptPhrases)
            {
                for (int currentLetter = 0; currentLetter < currentPhrase.PhraseString.Length; currentLetter++)
                {
                    if (currentPhrase.PhraseString[currentLetter] == weaselPhrase[currentLetter])
                    {
                        currentPhrase.Score += 1;
                    }
                }
            }

            bestMatch = attemptPhrases[0];

            foreach (var currentPhrase in attemptPhrases)
            {
                if (currentPhrase.Score > bestMatch.Score)
                { bestMatch = currentPhrase; }
            }

            return bestMatch;
        }

        private void MutatePhraseList(List<Phrase> attemptPhrases)
        {
            bool progress;
            char newChar;
            Random random = new();

            for (int offset = 1; offset < attemptPhrases.Count; offset++)
            {
                Phrase? currentPhrase = attemptPhrases[offset];

                //Reset the score
                currentPhrase.Score = 0;

                for (int currentLetter = 0; currentLetter < currentPhrase.PhraseString.Length; currentLetter++)
                {
                    if (random.Next(100) < changePercentage)
                    {
                        progress = false;
                        do
                        {
                            newChar = GetValidChar();
                            if (checkBoxNoDupes.Checked)
                            {
                                if (newChar != currentPhrase.CharArray[currentLetter])
                                {
                                    progress = true;
                                }
                            }
                            else
                            {
                                progress = true;
                            }
                        } while (!progress);

                        currentPhrase.CharArray[currentLetter] = newChar;
                    }
                }
            }
        }

        private char GetValidChar()
        {
            bool done;
            int newChar;

            Random random = new();

            do
            {
                newChar = random.Next(64, 123);
                if (newChar == 64)
                {
                    newChar = 32;
                    done = true;
                }
                else
                {
                    done = newChar <= 90 || newChar >= 97;
                }

            } while (!done);

            return (char)newChar;
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBoxResults.SelectedItem.ToString());
        }
    }

    internal class Phrase
    {
        private char[] charArray;

        public string PhraseString
        {
            get
            {
                return new string(charArray);
            }
        }

        public char[] CharArray
        {
            get { return charArray; }
        }

        public int Score { get; set; } = 0;

        public Phrase(string phrase)
        {
            charArray = phrase.ToCharArray();
        }

        public override string ToString()
        {
            return new string((new string(charArray)) + " | " + Score);
        }
    }
}
