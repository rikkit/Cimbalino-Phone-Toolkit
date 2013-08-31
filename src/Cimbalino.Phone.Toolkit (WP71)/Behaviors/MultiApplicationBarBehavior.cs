// ****************************************************************************
// <copyright file="MultiApplicationBarBehavior.cs" company="Pedro Lamas">
// Copyright © Pedro Lamas 2011
// </copyright>
// ****************************************************************************
// <author>Pedro Lamas</author>
// <email>pedrolamas@gmail.com</email>
// <date>17-11-2011</date>
// <project>Cimbalino.Phone.Toolkit</project>
// <web>http://www.pedrolamas.com</web>
// <license>
// See license.txt in this solution or http://www.pedrolamas.com/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;

namespace Cimbalino.Phone.Toolkit.Behaviors
{
    /// <summary>
    /// The behavior that creates a collection of bindable <see cref="Microsoft.Phone.Shell.ApplicationBar" /> controls.
    /// </summary>
    [System.Windows.Markup.ContentProperty("ApplicationBars")]
    public class MultiApplicationBarBehavior : SafeBehavior<FrameworkElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiApplicationBarBehavior" /> class.
        /// </summary>
        public MultiApplicationBarBehavior()
        {
            ApplicationBars = new ApplicationBarCollection();
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            AssociatedObject.LayoutUpdated += AssociatedObjectOnLayoutUpdated;

            base.OnAttached();
        }

        /// <summary>
        /// Gets the <see cref="ApplicationBar"/> collection.
        /// </summary>
        /// <value>The <see cref="ApplicationBar"/> collection.</value>
        [Category("Common")]
        public ApplicationBarCollection ApplicationBars
        {
            get { return (ApplicationBarCollection)GetValue(ApplicationBarsProperty); }
            private set { SetValue(ApplicationBarsProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ApplicationBars" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationBarsProperty =
            DependencyProperty.Register("ApplicationBars", typeof(ApplicationBarCollection), typeof(MultiApplicationBarBehavior), null);

        /// <summary>
        /// Gets or sets the index of the selected Application Bar.
        /// </summary>
        /// <value>the index of the selected Application Bar.</value>
        [Category("Common")]
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="SelectedIndex" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(MultiApplicationBarBehavior), new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// Called after the index of the selected Application Bar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (MultiApplicationBarBehavior)d;

            element.Update();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Application Bar is visible.
        /// </summary>
        /// <value>true if the Application Bar is visible; otherwise, false.</value>
        [Category("Appearance")]
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="IsVisible" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("Visible", typeof(bool), typeof(MultiApplicationBarBehavior), new PropertyMetadata(true, OnIsVisibleChanged));

        /// <summary>
        /// Sets the background colour of every contained Application Bar.
        /// </summary>
        public System.Windows.Media.Color BackgroundColor
        {
            get { return (System.Windows.Media.Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="BackgroundColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof (System.Windows.Media.Color), typeof (MultiApplicationBarBehavior), new PropertyMetadata(default(System.Windows.Media.Color)));
        
        /// <summary>
        /// Sets the foreground colour of every contained Application Bar.
        /// </summary>
        public System.Windows.Media.Color ForegroundColor
        {
            get { return (System.Windows.Media.Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ForegroundColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(System.Windows.Media.Color), typeof(MultiApplicationBarBehavior), new PropertyMetadata(default(System.Windows.Media.Color)));
        
        /// <summary>
        /// The selected application bar.
        /// </summary>
        public ApplicationBar SelectedItem
        {
            get
            {
                var selectedIndex = SelectedIndex;

                if (IsVisible && selectedIndex >= 0 && selectedIndex <= ApplicationBars.Count - 1)
                {
                    var applicationBar = ApplicationBars[selectedIndex];

                    return applicationBar;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Called after the visible state of the Application Bar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (MultiApplicationBarBehavior)d;

            element.Update();
        }

        private void AssociatedObjectOnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            AssociatedObject.LayoutUpdated -= AssociatedObjectOnLayoutUpdated;

            Update();
        }

        private void Update()
        {
            if (DesignerProperties.IsInDesignTool || AssociatedObject == null)
            {
                return;
            }

            var page = AssociatedObject as PhoneApplicationPage ?? AssociatedObject.Parent as PhoneApplicationPage;

            if (page == null)
            {
                throw new Exception("This MultiApplicationBarBehavior element can only be attached to the Page or LayoutRoot elements");
            }

            var applicationBar = SelectedItem;

            if (applicationBar == null)
            {
                page.ApplicationBar = null;
            }
            else
            {
                // The following is required to fix a compatibility issue with the Windows Phone Toolkit CustomMessageBox
                var internalApplicationBar = applicationBar.InternalApplicationBar;

                internalApplicationBar.IsVisible = applicationBar.IsVisible;
                
                if (BackgroundColor != default(System.Windows.Media.Color))
                {
                    internalApplicationBar.BackgroundColor = BackgroundColor;
                }

                if (ForegroundColor != default(System.Windows.Media.Color))
                {
                    internalApplicationBar.ForegroundColor = ForegroundColor;
                }

                page.ApplicationBar = internalApplicationBar;
            }
            
            // raise the changed event _after_ it's been set properly
            var handler = SelectedAppBarChanged;
            if (handler != null)
            {
                handler(this, applicationBar);
            }
        }
        
        /// <summary>
        /// Raised when <see cref="SelectedIndex"/> changes.
        /// </summary>
        public event EventHandler<ApplicationBar> SelectedAppBarChanged;
    }
}