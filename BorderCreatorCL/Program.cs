using SkiaSharp;

namespace BorderCreatorCL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "--help")
            {
                Console.Write("1st parameter: filename\r\n2nd parameter: line width\r\n3rd parameter: color hex code");
                return;
            }
            switch (args.Length)
            {
                case 0:
                    Console.Write("Please parse at least 1 argument (--help for more info)");
                    break;
                case 1:
                    CheckInputAndRun(args[0], "5", "#000000");
                    break;
                case 2:
                    CheckInputAndRun(args[0], args[1], "#000000");
                    break;
                case 3:
                    CheckInputAndRun(args[0], args[1], args[2]);
                    break;
                default:
                    Console.Write("Too many arguments!");
                    break;
            }
        }

        static void CheckInputAndRun(string first, string second, string third)
        {
            if (File.Exists(first) && int.TryParse(second, out int width) && SKColor.TryParse(third, out SKColor color))
                if (AddBorder(first, width, color))
                    Console.Write("Succes!");
                else
                    Console.Write("Something went wrong");
            else
                Console.Write("Input invalid");
        }

        static bool AddBorder(string path, int width, SKColor c)
        {
            try
            {
                SKBitmap bitmap = SKBitmap.Decode(path);

                // get corners
                SKPoint topleft = new(0, 0);
                SKPoint topright = new(bitmap.Width, 0);
                SKPoint bottomleft = new(0, bitmap.Height);
                SKPoint bottomright = new(bitmap.Width, bitmap.Height);

                // create canvas to draw over the bitmap
                SKCanvas canvas = new(bitmap);

                SKPaint brush = new()
                {
                    Color = c,
                    // double the width bc the center of the "brush" is on the edge so half of it goes outside the image
                    StrokeWidth = width * 2
                };

                canvas.DrawLine(topleft, topright, brush);
                canvas.DrawLine(topleft, bottomleft, brush);
                canvas.DrawLine(bottomright, topright, brush);
                canvas.DrawLine(bottomright, bottomleft, brush);

                bitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(new FileStream(AddPrefix(path), FileMode.Create));

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        static string AddPrefix(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string editedFileName = "edited_" + fileName;
            string editedFilePath = Path.Combine(directory, editedFileName);
            return editedFilePath;
        }
    }
}
