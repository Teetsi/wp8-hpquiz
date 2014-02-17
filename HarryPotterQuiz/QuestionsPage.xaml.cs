using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HarryPotterQuiz.Resources;

namespace HarryPotterQuiz
{
    public partial class QuestionsPage : PhoneApplicationPage
    {
        public int questionNumber = -1;

        // Constructor
        public QuestionsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("question", out selectedIndex))
                {
                    questionNumber = int.Parse(selectedIndex);
                    DataContext = App.ViewModel.Items[questionNumber];
                    questionIndex.DataContext = String.Format("{0} / {1}", questionNumber + 1, App.numOfQuestions);

                    if (questionNumber == 0)
                    {
                        App.startTime = DateTime.Now;
                    }
                }
            }

            ButtonPanel.Children.Clear();
            var index = 0;
            foreach (string answer in App.ViewModel.Items[questionNumber].Answers)
            {
                var button = new Button();
                var wrap = new TextBlock();
                index++;
                if (index == App.ViewModel.Items[questionNumber].Correct)
                {
                  button.Tag = 1;
                } else {
                  button.Tag = 0;
                }
                wrap.TextWrapping = TextWrapping.Wrap;
                wrap.TextAlignment = TextAlignment.Left;
                wrap.Text = answer;
                button.Click += new RoutedEventHandler(GoNextQuestion);
                button.Content = wrap;
                button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                ButtonPanel.Children.Add(button);
            }

        }


        private void GoNextQuestion(object sender, RoutedEventArgs e)
        {
            var value = (int)((Button)sender).Tag;
            SetScore(value);

            if ((questionNumber+1) < App.numOfQuestions)
            {
                NavigationService.Navigate(new Uri("/QuestionsPage.xaml?question=" + (questionNumber + 1), UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/ResultPage.xaml?score=" + CalcScore(), UriKind.Relative));
            }
        }

        private int CalcScore()
        {
            return App.score.Sum();
        }

        private void SetScore(int value)
        {
            App.score[questionNumber] = value;
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}