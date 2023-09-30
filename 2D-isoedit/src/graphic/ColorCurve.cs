using Program.src.graphic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

public class ColorCurve
{
    record struct ColorPoint(float Position, ARGBColor Color);

    private List<ColorPoint> colorPoints = new List<ColorPoint>();
    private ColorPoint[] array;

    bool changed = false;

    public void Add(float position, ARGBColor color)
    {
        if (position < 0 || position > 1)
        {
            throw new ArgumentException("Position must be between 0 and 1.");
        }

        colorPoints.Add(new(position, color));
        colorPoints.Sort((a, b) => a.Position.CompareTo(b.Position));
        array = colorPoints.ToArray();
    }

    public ARGBColor[] Bake(int length)
    {
        var array = new ARGBColor[length];
        for (int i = 0; i < length; i++)
        {
            float position = i / (float)length;
            array[i] = Sample(position);
        }
        return array;
    }

    public ARGBColor Sample(float position)
    {
        position = Math.Clamp(position, 0, 1);

        ColorPoint prevPoint = array[0];
        ColorPoint nextPoint = array[array.Length - 1];



        for (int i = 0; i < array.Length;i++)
        {
            var point = array[i];

            if (point.Position == position)
            {
                return point.Color;
            }
            if (point.Position <= position)
            {
                prevPoint = point;
            }
            else
            {
                nextPoint = point;
                break;
            }
        }

        // Interpolate between the two nearest color points
        float tInterpolated = (position - prevPoint.Position) / (nextPoint.Position - prevPoint.Position);
        return ARGBColor.Mix(prevPoint.Color, nextPoint.Color, tInterpolated);
    }
}
