﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;
using HarryPotterQuiz.Resources;
using System.Collections.Generic;

namespace HarryPotterQuiz.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<QuestionViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<QuestionViewModel> Items { get; private set; }
        private IEnumerable<QuestionViewModel> originalQuestions;

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            XDocument doc = XDocument.Load("questions.xml");
            var list = from query in doc.Descendants("quiz")
                        select new QuestionViewModel
                        {
                            ID = (string)query.Element("id"),
                            Question = (string)query.Element("question"),
                            Answers = query.Elements("answer").Select(x => x.Value).ToArray(),
                            Correct = (int)query.Element("correct")
                        };

            this.originalQuestions = list;
            
            this.IsDataLoaded = true;
        }

        public void selectRandomQuestions()
        {
            Random random = new Random();
            ObservableCollection<QuestionViewModel> result = new ObservableCollection<QuestionViewModel>();

            List<QuestionViewModel> items = this.originalQuestions.Cast<QuestionViewModel>().ToList();
            int i = App.numOfQuestions;
            
            while (i > 0)
            {
                var index = random.Next(items.Count());
                result.Add(items[index]);
                items.RemoveAt(index);
                i--;
            }
            this.Items = result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}