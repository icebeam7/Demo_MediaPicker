using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;

namespace Demo_MediaPicker.Paginas
{
    public partial class PaginaImagen : ContentPage
    {
        public PaginaImagen()
        {
            InitializeComponent();
        }

        async void btnSeleccionar_Clicked(System.Object sender, System.EventArgs e)
        {
            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Elige una imagen"
            });

            await ObtenerDetallesFoto(foto);
        }

        async void btnUsarCamara_Clicked(System.Object sender, System.EventArgs e)
        {
            if (MediaPicker.IsCaptureSupported)
            {
                var foto = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = "Toma la foto"
                });

                await ObtenerDetallesFoto(foto);
            }
        }

        async Task ObtenerDetallesFoto(FileResult foto)
        {
            if (foto != null)
            {
                var copia = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

                using (var stream = await foto.OpenReadAsync())
                {
                    using (var newStream = File.OpenWrite(copia))
                    {
                        await stream.CopyToAsync(newStream);
                    }
                }

                imgFoto.Source = ImageSource.FromFile(copia);
                lblRutaOriginal.Text = $"Ubicación original: {foto.FullPath}{foto.FileName}";
                lblRutaCopia.Text = $"Ubicación copia: {copia}";
                lblTipo.Text = $"MIME type: {foto.ContentType}";
            }
        }
    }
}