using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace DiceRPG.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Configurar toggle de tema
        ThemeToggle.Checked += (s, e) => ToggleTheme(true);
        ThemeToggle.Unchecked += (s, e) => ToggleTheme(false);
    }

    private void ToggleTheme(bool isDark)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light);
        paletteHelper.SetTheme(theme);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        if (e.ButtonState == MouseButtonState.Pressed)
            this.DragMove();
    }
}