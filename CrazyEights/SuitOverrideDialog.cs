using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrazyEightsCardLib;

namespace CrazyEights
{
    public partial class SuitOverrideDialog : Form
    {
        bool _userDismissed = false;
        public SuitOverrideDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _userDismissed = true;
            this.DialogResult = DialogResult.OK;
        }

        public CardSuit Suit
        {
            get 
            {
                if (radioButtonClubs.Checked)
                    return CardSuit.Clubs;
                else if (radioButtonDiamonds.Checked)
                    return CardSuit.Diamonds;
                else if (radioButtonHearts.Checked)
                    return CardSuit.Hearts;
                else if (radioButtonSpades.Checked)
                    return CardSuit.Spades;
                else
                    return CardSuit.Clubs;
            }
        }

        private void SuitOverrideDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_userDismissed)
            {
                e.Cancel = true;
            }
        }
    }
}