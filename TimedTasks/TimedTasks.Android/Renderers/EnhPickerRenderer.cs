using Android.Content;
using Android.Graphics;
using TimedTasks.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(EnhPickerRenderer))]
namespace TimedTasks.Droid.Renderers
{
    public class EnhPickerRenderer : DatePickerRenderer
    {
        public EnhPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            Control.Background.SetColorFilter(Xamarin.Forms.Color.White.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }
    }
}