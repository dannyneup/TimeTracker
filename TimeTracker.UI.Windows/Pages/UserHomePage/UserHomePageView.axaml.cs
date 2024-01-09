using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace TimeTracker.UI.Windows.Pages.UserHomePage;

public partial class UserHomePageView : UserControl
{
    private Button? playPauseButton;
    
    private readonly UserHomePageViewModel _viewModel;

    public UserHomePageView()
    {
        InitializeComponent();
        InitializeControls();
    }

    private void InitializeControls()
    {
        playPauseButton = this.FindControl<Button>("playPauseButton");
        if (playPauseButton != null) playPauseButton.Content = CreatePlayCanvas();
    }

    private Canvas CreatePlayCanvas()
    {
        var canvas = new Canvas
        {
            Width = 10,
            Height = 10
        };

        var polygon = new Polygon
        {
            Points = new List<Point>(new[]
            {
                new Point(0, 0),
                new Point(0, 10),
                new Point(10, 5)
            }),
            Fill = Brushes.Gray
        };
        canvas.Children.Add(polygon);
        return canvas;
    }

    private Canvas CreatePauseCanvas()
    {
        var canvas = new Canvas
        {
            Width = 10,
            Height = 10
        };

        var rectangle1 = new Rectangle
        {
            Width = 3,
            Height = 10,
            Fill = Brushes.Gray
        };

        var rectangle2 = new Rectangle
        {
            Width = 3,
            Height = 10,
            Fill = Brushes.Gray
        };

        Canvas.SetLeft(rectangle2, 7);

        canvas.Children.Add(rectangle1);
        canvas.Children.Add(rectangle2);

        return canvas;
    }
}