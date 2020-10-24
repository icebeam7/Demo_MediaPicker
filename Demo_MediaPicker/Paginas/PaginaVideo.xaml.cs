using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;

namespace Demo_MediaPicker.Paginas
{
    public partial class PaginaVideo : ContentPage
    {
        public PaginaVideo()
        {
            InitializeComponent();
        }

        async void btnSeleccionar_Clicked(System.Object sender, System.EventArgs e)
        {
            var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions()
            {
                Title = "Elige un video"
            });

            await ObtenerDetallesVideo(video);
        }

        async void btnUsarCamara_Clicked(System.Object sender, System.EventArgs e)
        {
            if (MediaPicker.IsCaptureSupported)
            {
                var video = await MediaPicker.CaptureVideoAsync(new MediaPickerOptions()
                {
                    Title = "Toma el video"
                });

                await ObtenerDetallesVideo(video);
            }
        }

        async Task ObtenerDetallesVideo(FileResult video)
        {
            if (video != null)
            {
                var copia = Path.Combine(FileSystem.CacheDirectory, video.FileName);

                using (var stream = await video.OpenReadAsync())
                {
                    using (var newStream = File.OpenWrite(copia))
                    {
                        await stream.CopyToAsync(newStream);
                    }
                }

                mdeVideo.Source = MediaSource.FromFile(copia);
                lblRutaOriginal.Text = $"Ubicación original: {video.FullPath}{video.FileName}";
                lblRutaCopia.Text = $"Ubicación copia: {copia}";
                lblTipo.Text = $"MIME type: {video.ContentType}";
            }
        }
    }
}
