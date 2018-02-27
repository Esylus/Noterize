using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotePractice.Entities;

namespace NotePractice
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            NoteClear();
            LedgerLineClear();
            CheckBoxesVisable();
            cbPreset.Text = "Treble Clef";
        }

        private KeyRandomizer userKeyListObject;
        private Statistics sessionStatistics;
        private GameTimer sessionTimer;
        private FadeTimer pointFade;
        private Focus sessionFocus;
 

        private void btnPractice_Click(object sender, EventArgs e)
        {
            userKeyListObject = new KeyRandomizer(putUserSelectedKeysIntoList());

            sessionStatistics = new Statistics();

            sessionTimer = new GameTimer();

            NoteClear();
            
            LedgerLineClear();

            Timer.Start();  
            
            getRandomKeyAndDisplay();

            CheckBoxesHidden();

            lblTimerDisplay.Visible = true;
       
        }

        private void cbFocus_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFocus.Checked)
            {
                sessionFocus = new Focus();
            }
            else
            {
                sessionFocus.FocusModeEnabled = false;
            }
        }

        private void btnResetNew_Click(object sender, EventArgs e)
        {
            NoteClear();
            LedgerLineClear();
            Timer.Stop();
            statisticDisplaysClear();
            CheckboxClear();
            CheckBoxesVisable();
        }


        
        // GAME OPERATION---------------------------------------------------------------------


        private List<int> putUserSelectedKeysIntoList()
        {// go through all checkboxes and return list to practice with

             List<int> usersSelectedKeys = new List<int>();

            while (!usersSelectedKeys.Any())
            {

                foreach (CheckBox cb in pnCheckBoxes.Controls.OfType<CheckBox>())
                {
                    if (cb.Checked == true)
                    {
                        string cbParse = cb.Name;
                        cbParse = cbParse.Remove(0, 2);
                        int cbNumber = Convert.ToInt16(cbParse);
                        usersSelectedKeys.Add(cbNumber);
                    }
                }

            if (!usersSelectedKeys.Any()) // EDGE CASE - if no keys selected, select all keys by default          
                CheckBoxSelectAll();

            }
            return usersSelectedKeys;
        }      



        private void getRandomKeyAndDisplay()
        {
            if (sessionFocus != null)
            {
                if (sessionFocus.FocusModeEnabled)
                { // use focus list accumulated by tracking user performance from first round

                    userKeyListObject.extractUserRandomKeyToMember(sessionFocus.FocusList);
                }
                else
                { // use normal list for first round to accumulate user performance data

                    userKeyListObject.extractUserRandomKeyToMember(userKeyListObject.UserSelectedKeyList);
                }
            }
            else
            { // if the focus button is not pressed use normal default list

                userKeyListObject.extractUserRandomKeyToMember(userKeyListObject.UserSelectedKeyList); // Default list
            }
         
            switch (userKeyListObject.CurrentRandomKey)
            {// calls note and ledger line of selected random key

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

        }    

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {// if user presses right key then get another note, otherwise keep racking up points

            if ((((((((e.KeyCode == Keys.D) && (new[] { 41, 34, 27, 20, 13, 6}).Contains(userKeyListObject.CurrentRandomKey))
                  || ((e.KeyCode == Keys.C) && (new[] { 40, 33, 26, 19, 12, 5 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.B) && (new[] { 39, 32, 25, 18, 11, 4 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.A) && (new[] { 38, 31, 24, 17, 10, 3 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.G) && (new[] { 37, 30, 23, 16, 9, 2  }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.F) && (new[] { 36, 29, 22, 15, 8, 1  }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.E) && (new[] { 35, 28, 21, 14, 7, 0  }).Contains(userKeyListObject.CurrentRandomKey)))                         
            {

                pointFade = new FadeTimer(0, 255, 0); // for green and red score display
                pointFade.PositiveOrNegativePoints = true;
                FadeTimer.Start();

                if (cbFocus.Checked) // if in focus mode, collect user performance data
                {
                    sessionFocus.recordUserResults(userKeyListObject.CurrentRandomKey, 1);
                }

                sessionStatistics.Correct++;
                sessionStatistics.Total++;
                sessionStatistics.TotalPoints += 5;
                NoteClear();
                LedgerLineClear();
                getRandomKeyAndDisplay();
            }           
            else if(e.KeyCode >=Keys.A && e.KeyCode <=Keys.Z)
            {

                pointFade = new FadeTimer(255, 0, 0);
                pointFade.PositiveOrNegativePoints = false;
                FadeTimer.Start();

                if (cbFocus.Checked)
                {
                    sessionFocus.recordUserResults(userKeyListObject.CurrentRandomKey, 0);
                }

                sessionStatistics.Total++;
                sessionStatistics.TotalPoints -= 3;
            }
            getScoreAndDisplayStatistics();

        }   

        // MAINTAINENCE------------------------------------------------------------------------

        private void CheckBoxesHidden()
        {
            foreach (var checkBox in pnCheckBoxes.Controls.OfType<CheckBox>())
            {
                checkBox.Visible = false;
            }
        }                

        private void CheckBoxesVisable()
        {
            foreach (var checkBox in pnCheckBoxes.Controls.OfType<CheckBox>())
            {
                checkBox.Visible = true;
            }
        }               

        private void CheckboxClear()
        {
            foreach (var checkBox in pnCheckBoxes.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }        
        }

        private void CheckBoxSelectAll()
        {
            foreach (var checkBox in pnCheckBoxes.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = true;
            }
        }

        private void NoteClear()
        { // clear all notes 

            foreach (var note in this.Controls.OfType<PictureBox>())
            {             
                    note.Visible = false;
            }

            trebleClef.Visible = true;
            bassClef.Visible = true;
        }   

        private void LedgerLineClear()
        {
            Label[] labels = new Label[10] { lblE6, lblC5, lblA5, lblC4, lblA4, lblF4, lblBC4, lblBE2, lblBC2, lblBA2 };

            foreach (Label label in labels)
            {
                label.Visible = false;
            }
        }
       
        // STATISTICS---------------------------------------------------------------------------

        private void getScoreAndDisplayStatistics()
        {
            decimal correct = sessionStatistics.Correct;
            decimal total = sessionStatistics.Total;
            decimal accuracy = sessionStatistics.Accuracy;
            int totalPoints = sessionStatistics.TotalPoints;

            lblTotalDisplay.Text = totalPoints.ToString();

            if (total != 0)
            {
                accuracy = sessionStatistics.calculateAccuracy(correct, total);
                lblAccuracyDisplay.Text = accuracy.ToString("P");
            }
        }

        private void statisticDisplaysClear()
        {
            lblPointsDisplay.Text = "";
            lblAccuracyDisplay.Text = "";
            lblTotalDisplay.Text = "";
            lblTimerDisplay.Text = "";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sessionTimer.TimerCount--;
            lblTimerDisplay.Text = (sessionTimer.TimerCount).ToString();

            if (sessionTimer.TimerCount == 0)
            {// when round is over

                Timer.Stop();
                CheckBoxesVisable();
                lblTimerDisplay.Visible = false;

                if (cbFocus.Checked)
                { // if focus mode enabled - create focus list based on users first round performance
                    sessionFocus.createFocusList();
                    sessionFocus.FocusModeEnabled = true;
                }
            }
        }


        private void FadeTimer_Tick(object sender, EventArgs e)
        { // Animation which allows points added or subtracted to fade away

            pointFade.FadeTimerCount--;
            int endColor = 105; // this was static RGB 105 - will have to mod depend on GUI picture
            int fadeRate = 5;

            if (pointFade.PositiveOrNegativePoints)
            { // if positive points apply green color + fade

                lblPointsDisplay.ForeColor = Color.FromArgb(pointFade.R, pointFade.G, pointFade.B);
                lblPointsDisplay.Text = "+5";

                if (pointFade.R < endColor) pointFade.R += fadeRate;
                if (pointFade.G > endColor) pointFade.G -= fadeRate;
                if (pointFade.B < endColor) pointFade.B += fadeRate;
            }
            else
            { // if negative points apply red color + fade

                lblPointsDisplay.ForeColor = Color.FromArgb(pointFade.R, pointFade.G, pointFade.B);
                lblPointsDisplay.Text = "-3";

                if (pointFade.R > endColor) pointFade.R -= fadeRate;
                if (pointFade.G < endColor) pointFade.G += fadeRate;
                if (pointFade.B < endColor) pointFade.B += fadeRate;
            }

            if (pointFade.FadeTimerCount == 0)
            {
                FadeTimer.Stop();
            }
        }
    }
}