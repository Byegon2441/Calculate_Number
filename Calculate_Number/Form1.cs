using IronOcr;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Calculate_Number
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
     
        }

      

       

        private void Form1_Load(object sender, EventArgs e)
        {
            //open folder
            var fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);

                Debug.WriteLine("Files found: " + files.Length.ToString(), "Message");
                foreach (string filepath in files)
                {
                    Debug.WriteLine(filepath);

                    //OCR
                    var Ocr = new IronTesseract();
                    Ocr.Language = OcrLanguage.ThaiAlphabetBest;
                    Ocr.AddSecondaryLanguage(OcrLanguage.English);

                    var Input = new OcrInput(filepath);
                    //var Input = new OcrInput("asset/image.png");
                     
                     //Input.Deskew();  // use if image not straight
                     Input.DeNoise(); // use if image contains digital noise
                    var Result = Ocr.Read(Input);


                    //data
                    Label label1 = new Label();
                    label1.AutoSize = true;
                    label1.Text = String.Format("{0}",Result.Text);


                    this.Controls.Add(label1);


                   MatchCollection matches = Regex.Matches(Result.Text, @"=\d+");
                    
                   //MatchCollection matches = Regex.Matches(Result.Text, @"\b(\d + (?:\.(?:[^0]\d |\d[^0]))?)\b");
                    string[] res = matches.Cast<Match>()
                                             .Take(10)
                                             .Select(match => match.Value)
                                             .ToArray();

                    Debug.WriteLine(string.Join(Environment.NewLine, res));

                    //Debug.WriteLine(number);
                }
            }




            
        }
    }
}