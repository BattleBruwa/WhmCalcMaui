namespace WhmCalcMaui.Views.Drawable
{
    public class TargetBoxArrow : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            PathF path = new();
            path.MoveTo(4, dirtyRect.Height - 4);
            path.LineTo(dirtyRect.Width / 2, 4);
            path.LineTo(dirtyRect.Width - 4, dirtyRect.Height - 4);

            canvas.StrokeSize = 12;
            canvas.StrokeLineCap = LineCap.Round;
            canvas.StrokeLineJoin = LineJoin.Round;

            if (Application.Current.PlatformAppTheme is AppTheme.Light || Application.Current.PlatformAppTheme is AppTheme.Unspecified)
            {
                canvas.StrokeColor = (Color)Application.Current.Resources["MainOutlineColorLight"];
            }
            else
            {
                canvas.StrokeColor = (Color)Application.Current.Resources["MainOutlineColorDark"];
            }

            canvas.DrawPath(path);
        }
    }
}
