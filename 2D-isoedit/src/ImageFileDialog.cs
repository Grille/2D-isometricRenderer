using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Program;

internal class ImageFileDialog
{
    public const string ImageFilter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF)|*.bmp;*.jpg;*.gif;*.png;*.tiff|All files (*.*)|*.*";

    public static string DirectoryExport;
    public static string DirectoryImport;

    public static bool TryOpenImage(IWin32Window owner, out string path)
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(DirectoryImport);
        dialog.Filter = ImageFilter;

        if (dialog.ShowDialog(owner) == DialogResult.OK)
        {
            path = dialog.FileName;
            DirectoryImport = Path.GetDirectoryName(path);

            return true;
        }

        path = null!;
        return false;
    }

    public static bool TrySaveImage(IWin32Window owner, Bitmap bitmap)
    {
        var dialog = new SaveFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(DirectoryExport);
        dialog.AddExtension = true;
        dialog.Filter = ImageFilter;
        dialog.DefaultExt = ".png";

        if (dialog.ShowDialog(owner) == DialogResult.OK)
        {
            var path = dialog.FileName;
            var format = GetImageFormat(path);

            bitmap.Save(dialog.FileName, format);
            DirectoryExport = Path.GetDirectoryName(path);

            return true;
        }
        return false;
    }

    public static void SaveImage(IWin32Window owner, Bitmap bitmap)
    {
        try
        {
            TrySaveImage(owner, bitmap);
        }
        catch (Exception e)
        {
            ExceptionDialog.Show(owner, e);
        }
    }

    public static ImageFormat GetImageFormat(string path)
    {
        var extension = Path.GetExtension(path).ToLower();
        return extension switch
        {
            ".bmp" => ImageFormat.Bmp,
            ".jpg" => ImageFormat.Jpeg,
            ".jpeg" => ImageFormat.Jpeg,
            ".gif" => ImageFormat.Gif,
            ".png" => ImageFormat.Png,
            ".tiff" => ImageFormat.Tiff,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

}
