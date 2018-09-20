using image_02.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

public static class ImageMerger
{
    #region Url Merging

    public static Bitmap MergeImages(List<String> imageUrls, Img img, WebProxy proxy = null)
    {
       List<Bitmap> bitmapList = ConvertUrlsToBitmaps(imageUrls, proxy);
        //List<Bitmap> bitmapList  = ConvertUrlsToBitmapsLocal(imageUrls);
        return Merge(bitmapList, img);
    }

    private static List<Bitmap> ConvertUrlsToBitmaps(List<String> imageUrls, WebProxy proxy = null)
    {
        List<Bitmap> bitmapList = new List<Bitmap>();

        foreach (string imgUrl in imageUrls)
        {
            try
            {
                WebClient wc = new WebClient();
                if (proxy != null)
                    wc.Proxy = proxy;
                byte[] bytes = wc.DownloadData(imgUrl);
                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);
                bitmapList.Add((Bitmap)img);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        return bitmapList;
    }
    private static List<Bitmap> ConvertUrlsToBitmapsLocal(List<String> imageUrls)
    {
        List<Bitmap> bitmapList = new List<Bitmap>();
        foreach (string imgUrl in imageUrls)
        {
            try
            {
                var direct = Directory.GetCurrentDirectory();
                string dir = Path.Combine(Directory.GetCurrentDirectory(), imgUrl);
                Bitmap img = (Bitmap)Image.FromFile(dir);

                bitmapList.Add(img);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        return bitmapList;
    }

    #endregion Url Merging

    #region File System Merging

    public static Bitmap MergeImages(string folderPath, ImageFormat imageFormat, Img img)
    {
        List<Bitmap> bitmapList = ConvertUrlsToBitmaps(folderPath, imageFormat);
        return Merge(bitmapList, img);
    }

    private static List<Bitmap> ConvertUrlsToBitmaps(string folderPath, ImageFormat imageFormat)
    {
        List<Bitmap> bitmapList = new List<Bitmap>();

        List<string> imagesFromFolder = Directory.GetFiles(folderPath, "*." + imageFormat, SearchOption.AllDirectories).ToList();

        foreach (string imgPath in imagesFromFolder)
        {
            try
            {
                var bmp = (Bitmap)Image.FromFile(imgPath);

                bitmapList.Add(bmp);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        return bitmapList;
    }

    #endregion File System Merging

    #region Bitmap Merging

    public static Bitmap MergeImages(List<Bitmap> bitmaps, Img img)
    {
        return Merge(bitmaps, img);
    }

    #endregion Bitmap Merging

    private static Bitmap Merge(IEnumerable<Bitmap> images, Img img)
    {
        var enumerable = images as IList<Bitmap> ?? images.ToList();
        // merge images
        var bitmap = new Bitmap(enumerable.First().Width, enumerable.First().Height);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.DrawImage(enumerable.First(), 0, 0);
            var image = ScaleImage(enumerable.Last(), img.W, img.H);
            var Y = img.Y;
            if (img.T == 1)
            {
                Y = (img.H) / 2 + image.Height / 2;
            }
            g.DrawImage(image, img.X, Y);
        }
        return ScaleImage(bitmap, img.Wr, bitmap.Height);
    }

    static public Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
    {
        var ratioX = (double)maxWidth / image.Width;
        var ratioY = (double)maxHeight / image.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);
        var newImage = new Bitmap(newWidth, newHeight);
        Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
        Bitmap bmp = new Bitmap(newImage);
        return bmp;
    }
}