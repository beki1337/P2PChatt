﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;
using TDDD49Lab.Models.WrapperModels;
using TDDD49Lab.ViewModels;



namespace TDDD49Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Func<ITcpClient> createClient = () => new TcpClientWrapper();
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint endPoint) => new TcpListenerWrapper(endPoint.Address.ToString(),endPoint.Port);
            IDataSerialize<IMessage> dataSerialize = new DataSerializer<IMessage>();
            INetworkProtocol<IMessage> messageFactory = new MessageFactory( new DatetimeWrapper());
            IConservation<IMessage>  Conservation = new Conservation<IMessage>(new MemoryStream(), dataSerialize);

            this.DataContext = new MainViewModel(new Chatt(new NetworkHandler<IMessage>(createListner, createClient, messageFactory,dataSerialize), Conservation));
        }


       

        /*
        public void ShakeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a storyboard to animate the window's position.
            Storyboard storyboard = new Storyboard();
            storyboard.Completed += (s, args) => this.Left = originalLeft; // Reset the window position after the animation.

            // Define the animation for horizontal shake.
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(50),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(5), // Number of shakes
                From = 0,
                To = 10,
            };

            // Apply the animation to the TranslateTransform.
            TranslateTransform translateTransform = new TranslateTransform();
            this.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, horizontalAnimation);

            // Store the original window left position to reset it after the animation.
            originalLeft = this.Left;

            // Start the animation.
            storyboard.Begin(this);
        }

        private double originalLeft;
       
        */

    }
}
