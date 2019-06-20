using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MazeRetreat.Api.Logic
{
    public class RenderingLogic
    {
        private readonly String _border = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAIAAAAlC+aJAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyhpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTMyIDc5LjE1OTI4NCwgMjAxNi8wNC8xOS0xMzoxMzo0MCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUuNSAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NUFBRDRFNjA1RURDMTFFNjkxQThEQUIzRDFGRjQyOTIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NUFBRDRFNjE1RURDMTFFNjkxQThEQUIzRDFGRjQyOTIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo1QUFENEU1RTVFREMxMUU2OTFBOERBQjNEMUZGNDI5MiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo1QUFENEU1RjVFREMxMUU2OTFBOERBQjNEMUZGNDI5MiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pn8auUcAAADPSURBVHja7JlLCoAwDAX9Igiew2t5Qi8nCCKKO+mmUGmiCU5WrooD81KeluM8FZ6nKpwPAF9P4/Glj2VDIQBMZ6Dq2/v5XHfx8+uhQyEA1DMQ7trQuZTR8D6WKxQCQCsDMe9DF7W9TzkfhQCw3gdy7hMUAsBiBp7uew3vUQiAtzOg3XFRCADrGbDmPQoBQB9AIQD8ZIA+AMBfMvB030t1DBQCQDIDOV5K/UcLs5RyPgoBIJkBy90XhQDw2ge0+wMKAeApAxrfiFAIgMy5BBgAWcs64FTfoyoAAAAASUVORK5CYII=";
        private readonly String _road = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAIAAAAlC+aJAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyhpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTMyIDc5LjE1OTI4NCwgMjAxNi8wNC8xOS0xMzoxMzo0MCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUuNSAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NUFCMjMxOEU1RURDMTFFNjkxQThEQUIzRDFGRjQyOTIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NUFCMjMxOEY1RURDMTFFNjkxQThEQUIzRDFGRjQyOTIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo1QUFENEU2NjVFREMxMUU2OTFBOERBQjNEMUZGNDI5MiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo1QUFENEU2NzVFREMxMUU2OTFBOERBQjNEMUZGNDI5MiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PhLx5cMAAADPSURBVHja7Jk7CoBADAVVRPzd/ywexiNY+sPKTrYRVjbRBCeV1eLAvCxP82kcMs9TZM4HgK+n9PjS87KiEACmM9DU1fW87Yf4+X3XohAA6hkId23oXMxoeH+XKxQCQCsDd96HLmp7H3M+CgFgvQ+k3CcoBIDFDDzd9xreoxAAb2dAu+OiEADWM2DNexQCgD6AQgD4yQB9AIC/ZODpvpfqGCgEgGQGUryU+o8WZinmfBQCQDIDlrsvCgHgtQ9o9wcUAsBTBjS+EaEQAIlzCjAAETE8OS0fHPEAAAAASUVORK5CYII=";
        private readonly String _start = "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";
        private readonly String _finish = "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";

        public byte[] RenderMaze(String mazeData, String solutionData)
        {
            var borderImage = PngFromBase64(_border);
            var roadImage = PngFromBase64(_road);
            var startImage = PngFromBase64(_start);
            var finishImage = PngFromBase64(_finish);

            var mazeRows = mazeData.Split(Environment.NewLine);

            var maze = new char[mazeRows[0].Length, mazeRows.Length];
            int mY = 0;
            foreach (var mazeRow in mazeRows)
            {
                int mX = 0;
                foreach (var mazeChar in mazeRow)
                {
                    maze[mX, mY] = mazeChar;
                    mX++;
                }
                mY++;
            }
            using (Bitmap bitmap = new Bitmap(maze.GetLength(1) * 64, maze.GetLength(0) * 64))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int y = 0; y < maze.GetLength(0); y++)
                    {
                        for (int x = 0; x < maze.GetLength(1); x++)
                        {
                            if (maze[x, y] == '#')
                            {
                                g.DrawImageUnscaled(borderImage, 64 * x, 64 * y);
                            }
                            if (maze[x, y] == '.')
                            {
                                g.DrawImageUnscaled(roadImage, 64 * x, 64 * y);
                            }
                            if (maze[x, y] == 'S')
                            {
                                g.DrawImageUnscaled(roadImage, 64 * x, 64 * y);
                                g.DrawImageUnscaled(startImage, 64 * x, 64 * y);
                            }
                            if (maze[x, y] == 'F')
                            {
                                g.DrawImageUnscaled(roadImage, 64 * x, 64 * y);
                                g.DrawImageUnscaled(finishImage, 64 * x, 64 * y);
                            }
                        }
                    }

                    if (solutionData != null)
                    {
                        var steps = solutionData.Split(";", StringSplitOptions.RemoveEmptyEntries);
                        foreach (var step in steps)
                        {
                            var parts = step.Split(":", StringSplitOptions.RemoveEmptyEntries);
                            var coords = parts[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
                            var x = Convert.ToInt32(coords[0]);
                            var y = Convert.ToInt32(coords[1]);
                            using (var font = new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                using (var format = new StringFormat())
                                {
                                    format.Alignment = StringAlignment.Center;
                                    format.LineAlignment = StringAlignment.Center;
                                    g.DrawString(parts[0], font, Brushes.Red, new RectangleF(64 * x, 64 * y, 64, 64), format);
                                }
                            }
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.GetBuffer();
                }
            }
        }

        private Image PngFromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}