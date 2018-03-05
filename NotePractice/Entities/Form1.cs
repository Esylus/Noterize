using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotePractice.Entities;
using radio42.Multimedia.Midi;

namespace NotePractice
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeMidi();
            InitializeOnLoad();
            
        }

        private KeyRandomizer userKeyListObject;
        private Statistics sessionStatistics;
        private GameTimer sessionTimer;
        private FadeTimer pointFade;
        private Focus sessionFocus;
        private bool disableKeyBoard = true;
        private MidiInputDevice _inDevice = null;
        private Dictionary<int, string> numberedNoteNames;
        private Dictionary<string, string> trebleMidiNamesNoteNames;
        private Dictionary<string, string> bassMidiNamesNoteNames;

       

        public void InitializeOnLoad()
        {
            LoadPresetComboBox();
            NoteClear();
            LedgerLineClear();
            CheckBoxesVisable();
            cbPreset.Text = "Load Preset";
            cmbMidi.Text = "Typing Keyboard";
            CreateDictNoteNamesAndInts();
            CreateDictMidiNamesAndNoteNames();
        }

        private void btnPractice_Click(object sender, EventArgs e)
        {
            userKeyListObject = new KeyRandomizer(PutUserSelectedKeysIntoList());
            sessionStatistics = new Statistics();
            sessionTimer = new GameTimer();

            NoteClear();          
            LedgerLineClear();
            Timer.Start();
            disableKeyBoard = false;
            GetRandomKeyAndDisplay();
            CheckBoxesHidden();
            lblTimerDisplay.Visible = true;
            StartMidi();
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
            StatisticDisplaysClear();
            CheckboxClear();
            CheckBoxesVisable();
            disableKeyBoard = true;
            StopMidi();
        }
        
        // GAME OPERATION---------------------------------------------------------------------

        private List<int> PutUserSelectedKeysIntoList()
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

            if (usersSelectedKeys.Count < 2)
            { // EDGE CASE - KeyRandomizer.cs method prevents any key from repeating 
                // if only a single key is selected the below default will select entire home row

                MessageBox.Show("Must select minimum two notes as notes are unable to repeat themselves. " +
                                "Now practice the treble cleff or hit Reset and select again!");
                usersSelectedKeys.Clear();
                CheckboxClear();
                int[] range = { 7, 8, 9, 10, 11, 12, 13, 14 };
                usersSelectedKeys.AddRange(range);
                CheckBox[] cbRange = new CheckBox[] { cb7, cb8, cb9, cb10, cb11, cb12, cb13, cb14 };
                foreach (CheckBox cb in cbRange)
                {
                    cb.Checked = true;
                }
            }
            return usersSelectedKeys;
        }      

        private void GetRandomKeyAndDisplay()
        {
            if (sessionFocus != null)
            {
                if (sessionFocus.FocusModeEnabled)
                { // use focus list accumulated by tracking user performance from first round

                    userKeyListObject.ExtractUserRandomKeyToMember(sessionFocus.FocusList);
                }
                else
                { // use normal list for first round to accumulate user performance data

                    userKeyListObject.ExtractUserRandomKeyToMember(userKeyListObject.UserSelectedKeyList);
                }
            }
            else
            { // if the focus button is not pressed use normal default list

                userKeyListObject.ExtractUserRandomKeyToMember(userKeyListObject.UserSelectedKeyList); // Default list
            }
         
            GamePlaySwitch(userKeyListObject.CurrentRandomKey);
        }

 // ------------------------Creating Reference Dictionaries ---------------------------------------

        private void CreateDictNoteNamesAndInts()
        {
            List<string> noteImageNames = new List<string>();

            for (int i = 0; i < 42; i++)
            {
                noteImageNames.Add("N" + i);
            }

            numberedNoteNames = new Dictionary<int, string>();

            int count = 0;
            foreach (String str in noteImageNames)
            {
                numberedNoteNames.Add(count, str);
                count++;
            }
        }

        private void CreateDictMidiNamesAndNoteNames()
        { // associate incoming midi notes with GUI note labels 

            trebleMidiNamesNoteNames = new Dictionary<string, string>()
            {
                {"N0", "E2"},{"N1", "F2"},{"N2", "G2"},{"N3", "A2"},{"N4", "B2"},{"N5", "C3"},{"N6", "D3"},
                {"N7", "E3"},{"N8", "F3"},{"N9", "G3"},{"N10", "A3"},{"N11", "B3"},{"N12", "C4"},{"N13", "D4"},
                {"N14", "E4"},{"N15", "F4"},{"N16", "G4"},{"N17", "A4"},{"N18", "B4"},{"N19", "C5"},{"N20", "D5"},
                {"N21", "E5"},{"N22", "F5"}
            };

            bassMidiNamesNoteNames = new Dictionary<string, string>()
            {
                { "N23", "G0"},{"N24", "A0"},{"N25", "B0"},{"N26", "C1"},{"N27", "D1"},{"N28", "E1"},{"N29", "F1"},
                {"N30", "G1"},{"N31", "A1"},{"N32", "B1"},{"N33", "C2"},{"N34", "D2"},{"N35", "E2"},{"N36", "F2"},
                {"N37", "G2"},{"N38", "A2"},{"N39", "B2"},{"N40", "C3"},{"N41", "D3"}
            };
        }

//-------------------------------------------Midi Functionality------------------------

        public void InitializeMidi()
        {
            int[] inPorts = MidiInputDevice.GetMidiPorts();

            foreach (int port in inPorts)
            {
                string name = MidiInputDevice.GetDeviceDescription(port);
                this.cmbMidi.Items.Add(name);
            }
        }

        public void StartMidi()
        {
            _inDevice = new MidiInputDevice(this.cmbMidi.SelectedIndex);
            _inDevice.MessageFilter = BuildFilter();
            _inDevice.MessageReceived += new MidiMessageEventHandler(InDevice_MessageRecieved);
            _inDevice.Open();
            _inDevice.Start();
        }

        public void StopMidi()
        {
            if (_inDevice != null && _inDevice.IsStarted)
            {
                _inDevice.Stop();
                _inDevice.Close();
                _inDevice.MessageReceived -= new MidiMessageEventHandler(InDevice_MessageRecieved);
            }
        }

        private MIDIMessageType BuildFilter()
        { // to filter out midi clock signals

            MIDIMessageType filter = MIDIMessageType.Unknown;

            filter |= MIDIMessageType.SystemRealtime;

            return filter;
        }

        private void InDevice_MessageRecieved(object sender, MidiMessageEventArgs e)
        { // parse incoming midi message, find correct note from dictionary, get note integer and see if user is right or wrong

            if (e.IsShortMessage)
            {
                string rawMidi = e.ShortMessage.ToString();
                string[] parsedMidi = rawMidi.Split();
                string midiKey = parsedMidi[4].Substring(4, 2);

                if (parsedMidi[3] == "NoteOn")
                {
                    string trebleMidiNoteName = "";

                    foreach (KeyValuePair<string, string> name in trebleMidiNamesNoteNames)
                    {
                        if (name.Value == midiKey)
                        {
                            trebleMidiNoteName = name.Key;
                        }
                    }

                    string bassMidiNoteName = "";

                    foreach (KeyValuePair<string, string> name in bassMidiNamesNoteNames)
                    {
                        if (name.Value == midiKey)
                        {
                            bassMidiNoteName = name.Key;
                        }
                    }

                    int trebleMidiKeyInt = 0;

                    foreach (KeyValuePair<int, string> note in numberedNoteNames)
                    {
                        if (note.Value == trebleMidiNoteName)
                        {
                            trebleMidiKeyInt = note.Key;
                        }
                    }

                    int bassMidiKeyInt = 0;

                    foreach (KeyValuePair<int, string> note in numberedNoteNames)
                    {
                        if (note.Value == bassMidiNoteName)
                        {
                            bassMidiKeyInt = note.Key;
                        }
                    }

                    if ((trebleMidiKeyInt == userKeyListObject.CurrentRandomKey) ||
                        (bassMidiKeyInt == userKeyListObject.CurrentRandomKey))
                    {
                        UserAnswerRight();
                    }
                    else 
                    {
                        UserAnswerWrong();   
                    }
                }
            }

            // to handle additional midi messages if needed
            else if (e.IsSysExMessage){}
            else if (e.EventType == MidiMessageEventType.Opened){}
            else if (e.EventType == MidiMessageEventType.Closed){}
            else if (e.EventType == MidiMessageEventType.Started){}
            else if (e.EventType == MidiMessageEventType.Stopped){}
        }



        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {// to handle keyboard input - if user presses right key then get another note, otherwise keep racking up points

            if (disableKeyBoard)
            {
                return;
            }

            if ((((((((e.KeyCode == Keys.D) && (new[] { 41, 34, 27, 20, 13, 6}).Contains(userKeyListObject.CurrentRandomKey))
                  || ((e.KeyCode == Keys.C) && (new[] { 40, 33, 26, 19, 12, 5 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.B) && (new[] { 39, 32, 25, 18, 11, 4 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.A) && (new[] { 38, 31, 24, 17, 10, 3 }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.G) && (new[] { 37, 30, 23, 16, 9, 2  }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.F) && (new[] { 36, 29, 22, 15, 8, 1  }).Contains(userKeyListObject.CurrentRandomKey)))
                  || ((e.KeyCode == Keys.E) && (new[] { 35, 28, 21, 14, 7, 0  }).Contains(userKeyListObject.CurrentRandomKey)))                         
            {
                UserAnswerRight();              
            }           
            else if(e.KeyCode >=Keys.A && e.KeyCode <=Keys.Z)
            {
                UserAnswerWrong();
            }         
        }

        private void UserAnswerRight()
        {
            pointFade = new FadeTimer(0, 255, 0); // for green and red score display
            pointFade.PositiveOrNegativePoints = true;
            FadeTimer.Start();

            if (cbFocus.Checked) // if in focus mode, collect user performance data
            {
                sessionFocus.RecordUserResults(userKeyListObject.CurrentRandomKey, 1);
            }

            sessionStatistics.Correct++;
            sessionStatistics.Total++;
            sessionStatistics.TotalPoints += 5;
            NoteClear();
            LedgerLineClear();
            GetRandomKeyAndDisplay();
            GetScoreAndDisplayStatistics();
        }

        private void UserAnswerWrong()
        {
            pointFade = new FadeTimer(255, 0, 0);
            pointFade.PositiveOrNegativePoints = false;
            FadeTimer.Start();

            if (cbFocus.Checked)
            {
                sessionFocus.RecordUserResults(userKeyListObject.CurrentRandomKey, 0);
            }

            sessionStatistics.Total++;
            sessionStatistics.TotalPoints -= 3;
            GetScoreAndDisplayStatistics();
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

        private void GetScoreAndDisplayStatistics()
        {
            decimal correct = sessionStatistics.Correct;
            decimal total = sessionStatistics.Total;
            decimal accuracy = sessionStatistics.Accuracy;
            int totalPoints = sessionStatistics.TotalPoints;

            lblTotalDisplay.Text = totalPoints.ToString();

            if (total != 0)
            {
                accuracy = sessionStatistics.CalculateAccuracy(correct, total);
                lblAccuracyDisplay.Text = accuracy.ToString("P");
            }
        }

        private void StatisticDisplaysClear()
        {
            lblPointsDisplay.Text = "";
            lblAccuracyDisplay.Text = "";
            lblTotalDisplay.Text = "";
            lblTimerDisplay.Text = "";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            lblTimerDisplay.Text = (sessionTimer.TimerCount).ToString();
            sessionTimer.TimerCount--;

            if (sessionTimer.TimerCount == 0)
            {// when round is over

                Timer.Stop();
                CheckBoxesVisable();
                lblTimerDisplay.Visible = false;
                disableKeyBoard = true;
                StopMidi();

                if (cbFocus.Checked)
                { // if focus mode enabled - create focus list based on users first round performance
                    sessionFocus.CreateFocusList();
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

        private void LoadPresetComboBox()
        {
            if (File.Exists("NoterizePresets.sqlite"))
            {
                PresetDBHelper.ConnectToDatabase();
                PresetDBHelper.PrintPresetsComboBox(cbPreset);
                cbPreset.Text = "Default";
            }
            else
            { 
                PresetDBHelper.CreateNewDatabase();
                PresetDBHelper.ConnectToDatabase();
                PresetDBHelper.CreateTable();
                PresetDBHelper.InsertDefault();
                PresetDBHelper.PrintPresetsComboBox(cbPreset);
                cbPreset.Text = "Default";
            }
        }

        private void RefreshPresetComboBox()
        {
            cbPreset.Items.Clear();
            PresetDBHelper.ConnectToDatabase();
            PresetDBHelper.PrintPresetsComboBox(cbPreset);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {   // gather all user selected keys and look up in dictionary to get the int value
            // add int to list to make a user list saved as a string
            // put string and name of string into preset object and save to db

            try
            {
                List<int> usersSelectedKeys = new List<int>();
             
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

                string myPresetList = "";
                string name = cbPreset.Text;

                    foreach (int keyNum in usersSelectedKeys)
                    {
                        myPresetList += keyNum.ToString() + " ";
                    }

                if (string.IsNullOrEmpty(name) || (string.IsNullOrEmpty(myPresetList)))
                {
                    throw new ApplicationException();  // to ensure that blank presets are not saved
                }

                Preset newPreset = new Preset(name, myPresetList);

                PresetDBHelper.ConnectToDatabase();
                PresetDBHelper.InsertPreset(newPreset);
                RefreshPresetComboBox();
                MessageBox.Show("New Preset Created ");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Saving preset requires a name and keys selected ");
            }
        }

        private Dictionary<int, CheckBox> LookUpCheckBoxes()
        {
         Dictionary<int, CheckBox> NumberAllCheckBoxes = new Dictionary<int, CheckBox>();

            List<CheckBox> allCheckBoxNames = new List<CheckBox>()
            {
                cb0,cb1,cb2,cb3,cb4,cb5,cb6,cb7,cb8,cb9,cb10,cb11,cb12,cb13,cb14,cb15,cb16,cb17,cb18,cb19,cb20,cb21,
                cb22,cb23,cb24,cb25,cb26,cb27,cb28,cb29,cb30,cb31,cb32,cb33,cb34,cb35,cb36,cb37,cb38,cb39,cb40,cb41
            };

            int count = 0;
            foreach (CheckBox cb in allCheckBoxNames)
            {
                NumberAllCheckBoxes.Add(count, cb);
                count++;
            }
            return NumberAllCheckBoxes;
        }

        private void cbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {          
            PresetDBHelper.ConnectToDatabase();

            string preset = "";
            preset = PresetDBHelper.GetPresetList(cbPreset.Text);

            preset = preset.TrimEnd();
            string[] presetStrings = preset.Split();

            List<int> presetInts = new List<int>();

                foreach (string str in presetStrings)
                {

                    presetInts.Add(Convert.ToInt32(str));
                }

                foreach (CheckBox cb in pnCheckBoxes.Controls.OfType<CheckBox>())
                {
                    cb.Checked = false;
                }

            Dictionary<int, CheckBox> NumberedCheckBoxNames = LookUpCheckBoxes();

            foreach (int cbInt in presetInts)
            {
                foreach (KeyValuePair<int, CheckBox> keyString in NumberedCheckBoxNames)
                {
                    // look up checkbox name in Dictionary and add number to list
                    if (cbInt == keyString.Key)
                    {
                        keyString.Value.Checked = true;
                    }
                }               
            }

        }

        private void GamePlaySwitch(int currentKey)
            {
                switch (currentKey)
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
    }
}