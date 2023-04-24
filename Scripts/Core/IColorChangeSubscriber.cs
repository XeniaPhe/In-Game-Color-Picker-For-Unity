namespace Xenia.ColorPicker.Core
{
    internal interface IColorChangeSubscriber
    {
        void ColorChanged(object sender, ColorChangedEventArgs e);
    }
}
