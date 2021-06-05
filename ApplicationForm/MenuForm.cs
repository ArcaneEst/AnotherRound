using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnotherRound
{
    public partial class MenuForm : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Maximized;
        }

        public MenuForm()
        {
            BackColor = Color.Gray;

            var buttonsList = GenerateAllButtons(this);

            PlaceAllButtonsInControls(buttonsList, this);
        }

        private static void PlaceAllButtonsInControls(List<Button> buttonsList, Form form)
        {
            for (var i = 0; i < buttonsList.Count; i++)
            {
                var currentButton = buttonsList[i];
                var currentI = i;
                form.SizeChanged += (e, args) => {
                    var move = form.ClientSize.Height / 2 / buttonsList.Count;

                    currentButton.Location = new Point(form.ClientSize.Width / 4, 
                        form.ClientSize.Height / 4 + (move * currentI));

                    currentButton.Size = new Size(form.ClientSize.Width / 2, 
                        form.ClientSize.Height / 2 / buttonsList.Count);
                };

                form.Controls.Add(currentButton);
            }
        }

        private static List<Button> GenerateAllButtons(Form form)
        {
            return new List<Button>() {  NewGameButton(form), SurvivalModeButton(form), ExitButton() };
        }

        private static Button NewGameButton(Form form)
        {
            var button = new Button();
            button.Text = "New Game";
            button.Font = new Font("Arial", 24, FontStyle.Bold);
            button.ForeColor = Color.Green;
            button.BackColor = Color.White;
            button.Click += (e, args) => 
            {
                var game = new MainForm(0);
                game.FormClosed += (e, args) => form.Show();
                form.Hide();
                game.Show();
            };

            return button;
        }

        private static Button SurvivalModeButton(Form form)
        {
            var button = new Button();
            button.Text = "Survival mode";
            button.Font = new Font("Arial", 24, FontStyle.Bold);
            button.ForeColor = Color.Coral;
            button.BackColor = Color.White;
            button.Click += (e, args) =>
            {
                var game = new MainForm(1);
                game.FormClosed += (e, args) => form.Show();
                form.Hide();
                game.Show();
            };

            return button;
        }

        private static Button ExitButton()
        {
            var button = new Button();
            button.Text = "Exit game";
            button.Font = new Font("Arial", 24, FontStyle.Bold);
            button.ForeColor = Color.Red;
            button.BackColor = Color.White;
            button.Click += (e, args) => Application.Exit();

            return button;
        }
    }
}
