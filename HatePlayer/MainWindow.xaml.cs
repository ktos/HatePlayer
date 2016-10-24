using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Ktos.HatePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        private int currentItem = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;
            ofd.Filter = "MP3|*.mp3";

            if (ofd.ShowDialog() == true)
            {
                foreach (var s in ofd.FileNames)
                    playlist.Items.Add(s);
            }

            currentItem = 0;
            playlist.SelectedIndex = currentItem;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri(playlist.Items[currentItem].ToString());
            mediaElement.Play();
            isPlaying = true;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var f = new List<string>();

            foreach (var s in hatelist.Items)
            {
                f.Add(s.ToString());
            }

            File.AppendAllLines(@"hatelist.txt", f);
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();

            if (currentItem + 1 < playlist.Items.Count)
            {
                currentItem++;

                playlist.SelectedIndex = currentItem;

                Play(this, null);
            }
            else
                MessageBox.Show("End of list!");
        }

        private void Hate(object sender, RoutedEventArgs e)
        {
            hatelist.Items.Add(playlist.Items[currentItem]);
            Next(this, null);
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                mediaElement.Pause();
                isPlaying = false;
            }
            else
            {
                mediaElement.Play();
                isPlaying = true;
            }
        }

        private void Skip(object sender, RoutedEventArgs e)
        {
            mediaElement.Position.Add(TimeSpan.FromSeconds(30));
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Next(this, null);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentItem = playlist.SelectedIndex;
            mediaElement.Stop();
            Play(this, null);
        }
    }
}