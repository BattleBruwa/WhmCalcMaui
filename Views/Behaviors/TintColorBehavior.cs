using CommunityToolkit.Maui.Behaviors;

namespace WhmCalcMaui.Views.Behaviors
{
    public class TintColorBehavior: Behavior<Image>
    {
        public static readonly BindableProperty TintColorProperty =
  BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintColorBehavior), null, propertyChanged: OnTintColorChanged);

        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }

        private Image? img;

        private static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TintColorBehavior behavior && behavior.img is not null)
            UpdateColor(behavior.img, (Color)newValue);
        }

        protected override void OnAttachedTo(Image bindable)
        {
            img = bindable;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Image bindable)
        {
            img = null;
            base.OnDetachingFrom(bindable);
        }

        private static void UpdateColor(Image image,  Color color)
        {
            if (image is null || color is null)
            {
                return;
            }

            var behavior = image.Behaviors.OfType<IconTintColorBehavior>().FirstOrDefault();

            if (behavior is not null)
            {
                image.Behaviors.Remove(behavior);
            }
            image.Behaviors.Add(new IconTintColorBehavior { TintColor = color });

        }
    }
}
