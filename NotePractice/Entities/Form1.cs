using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePractice
{
    public partial class Form1 : Form
    {
        // TO DO: Break up this brainstorm code into Classes and OOSD

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)          // start game on form load
        {
            NoteClear();
            LedgerLineClear();
            CheckBoxesVisable();
 
        }

        // global variables and list

        List<int> myChoseNote = new List<int>();
        int randNote;
        int randx; // keep track of output of randomizer
        decimal correct = 0;
        decimal total = 0;
        decimal score = 0;
        int timerCounter = 0;
        int interval = 0;

        
        // GAME OPERATION---------------------------------------------------------------------


        private void noteList()
        {


            foreach (CheckBox cb in pnCheckBoxes.Controls.OfType<CheckBox>())
            {
                if (cb.Checked == true)
                {
                    string cbParse = cb.Name;
                    cbParse = cbParse.Remove(0, 2);
                    int cbNumber = Convert.ToInt16(cbParse);
                    myChoseNote.Add(cbNumber);
                }         
            }

        }      // checkboxes of whose in the list

        private void randomNote()
        {
            CheckBoxesHidden();

            counters();
             
            myChoseNote.Clear();
            noteList();

            Random rnd = new Random();

            randx = rnd.Next(0, myChoseNote.Count);
            try
            {
                randNote = myChoseNote.ElementAt(randx);
            }
            catch (Exception)
            {
                MessageBox.Show("If you don't select at least one box I will select one for you");
            }



            switch (randNote)
            {
                case 41:
                    N41.Visible = true;
                    lblBC4.Visible = true;
                    break;
                case 40:
                    N40.Visible = true;
                    lblBC4.Visible = true;
                    break;
                case 39:
                    N39.Visible = true;
                    break;
                case 38:
                    N38.Visible = true;
                    break;
                case 37:
                    N37.Visible = true;
                    break;
                case 36:
                    N36.Visible = true;
                    break;
                case 35:
                    N35.Visible = true;
                    break;
                case 34:
                    N34.Visible = true;
                    break;
                case 33:
                    N33.Visible = true;
                    break;
                case 32:
                    N32.Visible = true;
                    break;
                case 31:
                    N31.Visible = true;
                    break;
                case 30:
                    N30.Visible = true;
                    break;
                case 29:
                    N29.Visible = true;
                    break;
                case 28:
                    N28.Visible = true;
                    lblBE2.Visible = true;
                    break;
                case 27:
                    N27.Visible = true;
                    lblBE2.Visible = true;
                    break;
                case 26:
                    N26.Visible = true;
                    lblBE2.Visible = true;
                    lblBC2.Visible = true;
                    break;
                case 25:
                    N25.Visible = true;
                    lblBE2.Visible = true;
                    lblBC2.Visible = true;
                    break;
                case 24:
                    N24.Visible = true;
                    lblBE2.Visible = true;
                    lblBC2.Visible = true;
                    lblBA2.Visible = true;
                    break;
                case 23:
                    N23.Visible = true;
                    lblBE2.Visible = true;
                    lblBC2.Visible = true;
                    lblBA2.Visible = true;
                    break;
                    // treble clef below, bass clef above
                case 22:
                    N22.Visible = true;
                    lblE6.Visible = true;
                    lblC5.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 21:
                    N21.Visible = true;
                    lblE6.Visible = true;
                    lblC5.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 20:
                    N20.Visible = true;
                    lblC5.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 19:
                    N19.Visible = true;
                    lblC5.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 18:
                    N18.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 17:
                    N17.Visible = true;
                    lblA5.Visible = true;
                    break;
                case 16:
                    N16.Visible = true;
                    break;
                case 15:
                    N15.Visible = true;
                    break;
                case 14:
                    N14.Visible = true;
                    break;
                case 13:
                    N13.Visible = true;
                    break;
                case 12:
                    N12.Visible = true;
                    break;
                case 11:
                    N11.Visible = true;
                    break;
                case 10:
                    N10.Visible = true;
                    break;
                case 9:
                    N9.Visible = true;
                    break;
                case 8:
                    N8.Visible = true;
                    break;
                case 7:
                    N7.Visible = true;
                    break;
                case 6:
                    N6.Visible = true;
                    break;
                case 5:
                    N5.Visible = true;
                    lblC4.Visible = true;
                    break;
                case 4:
                    N4.Visible = true;
                    lblC4.Visible = true;
                    break;
                case 3:
                    N3.Visible = true;
                    lblC4.Visible = true;
                    lblA4.Visible = true;
                    break;
                case 2:
                    N2.Visible = true;
                    lblC4.Visible = true;
                    lblA4.Visible = true;
                    break;
                case 1:
                    N1.Visible = true;
                    lblC4.Visible = true;
                    lblA4.Visible = true;
                    lblF4.Visible = true;
                    break;
                case 0:
                    N0.Visible = true;
                    lblC4.Visible = true;
                    lblA4.Visible = true;
                    lblF4.Visible = true;
                    break;

            }


        }    // picks random note from available list, calls pic of note

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((((((((e.KeyCode == Keys.D) && (new[] { 41, 34, 27, 20, 13, 6}).Contains(randNote))
                  || ((e.KeyCode == Keys.C) && (new[] { 40, 33, 26, 19, 12, 5 }).Contains(randNote)))
                  || ((e.KeyCode == Keys.B) && (new[] { 39, 32, 25, 18, 11, 4 }).Contains(randNote)))
                  || ((e.KeyCode == Keys.A) && (new[] { 38, 31, 24, 17, 10, 3 }).Contains(randNote)))
                  || ((e.KeyCode == Keys.G) && (new[] { 37, 30, 23, 16, 9, 2  }).Contains(randNote)))
                  || ((e.KeyCode == Keys.F) && (new[] { 36, 29, 22, 15, 8, 1  }).Contains(randNote)))
                  || ((e.KeyCode == Keys.E) && (new[] { 35, 28, 21, 14, 7, 0  }).Contains(randNote)))                         
            {
                if (total == 0)
                {
                    Timer.Start();                 
                }

                correct++;
                NoteClear();
                LedgerLineClear();
                randomNote();
            }
            
            if(e.KeyCode >=Keys.A && e.KeyCode <=Keys.Z)
            {
                total++;
                counters();
            } 
        }   // if key pressed == random note then clear notes and start again

        // MAINTAINENCE------------------------------------------------------------------------

        private void CheckBoxesHidden()
        {
            foreach (var checkBox in this.Controls.OfType<CheckBox>())
            {
                checkBox.Visible = false;
            }
        }                

        private void CheckBoxesVisable()
        {
            foreach (var checkBox in this.Controls.OfType<CheckBox>())
            {
                checkBox.Visible = true;
            }
        }               

        private void CheckboxClear()
        {
            foreach (var checkBox in this.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }        
        }

        private void NoteClear()
        { // clear all notes and sub-lines

            foreach (var note in this.Controls.OfType<PictureBox>())
            {             
                    note.Visible = false;
            }

            trebleClef.Visible = true;
            bassClef.Visible = true;

            
        }     // clears all note images from screen

        private void LedgerLineClear()
        {
            Label[] labels = new Label[10] { lblE6, lblC5, lblA5, lblC4, lblA4, lblF4, lblBC4, lblBE2, lblBC2, lblBA2 };

            foreach (Label label in labels)
            {
                label.Visible = false;
            }

        }

        // GAME CONTROLS------------------------------------------------------------------------

        private void btnTimer_Click(object sender, EventArgs e)     // PRACTICE BUTTON
        {
            timerCounter = 0;
            NoteClear();
            Timer.Start();
            statisticsClear();
            txtSetInterval.Enabled = false;
            randomNote();
            btnTimer.Enabled = false;        
            CheckBoxesHidden();
            cbInterval.Enabled = false;
          
        }   

    

        private void btnReset_Click(object sender, EventArgs e)
        {
            NoteClear();
            LedgerLineClear();
            Timer.Stop();
            btnTimer.Enabled = true;
            CheckBoxesVisable();
            CheckboxClear();
            statisticsClear();
            statisticsShow();
            cbInterval.Enabled = true;
            txtSetInterval.Enabled = true;
            txtSetInterval.Text = "";
            txtTimer1.Text = "";

     
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerCounter++;

            txtTimer1.Text = timerCounter.ToString();

            if (cbInterval.Checked == true)
            {
                interval = Convert.ToInt32(txtSetInterval.Text);

                if (timerCounter == interval)
                {
                    NoteClear();
                    randomNote();
                    timerCounter = 0;
                }
            }

            if (timerCounter == 60)
            {
                Timer.Stop();
                btnTimer.Enabled = true;
                cbInterval.Enabled = true;
                statisticsShow();
            }
        } 


        // STATISTICS---------------------------------------------------------------------------

        private void counters()
        {  // keep track of statistics

            txtCorrect.Text = correct.ToString();
            txtTotal.Text = total.ToString();
            if (total != 0)
            {
                score = correct / total;
                txtScore.Text = score.ToString("P");
            }
        }      

        private void statisticsShow()
        {
            foreach (Control ctrl in pnScore.Controls)
            {      
                    ctrl.Visible = true;
            }
        }

        private void statisticsHide()
        {
            foreach (Control ctrl in pnScore.Controls)
            {
                ctrl.Visible = false;
            }
        }

        private void statisticsClear()
        {
            correct = 0;
            score = 0;
            total = 0;
            txtCorrect.Text = correct.ToString();
            txtTotal.Text = total.ToString();
            txtScore.Text = total.ToString();
        }

      

    }


}