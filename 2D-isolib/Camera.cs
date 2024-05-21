using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric;

public class Camera
{
    private Vector2 _screenSize;
    private Vector2 _screenSizeHalf;

    public Vector2 Origin { get; } = Vector2.Zero;

    public Vector2 Position { get; set; }

    float _angle;
    public float Angle
    {
        set
        {
            if (_angle == value) return;
            _angle = value;
            if (_angle <= 0) _angle += 360;
            else if (_angle >= 360) _angle -= 360;
        }
        get
        {
            return _angle;
        }
    }

    float _tilt = 0.5f;
    public float Tilt
    {
        get => _tilt; set
        {
            _tilt = value;
        }
    }

    public float Scale { get; set; } = 1;

    public float MaxScale { get; set; } = 16;

    public float MinScale { get; set; } = 0.25f;

    public RectangleF Bounds { get; set; } = new RectangleF(-5000, -5000, 10000, 10000);

    public Vector2 LastLocation { get; private set; }

    public Vector2 ScreenSize
    {
        get => _screenSize;
        set
        {
            _screenSize = value;
            _screenSizeHalf = value / 2;
        }
    }

    public void MouseScroll(bool zoomIn, float scrollFactor)
    {
        var oldWorldPos = ScreenToWorldSpace(LastLocation, false);
        if (zoomIn)
            Scale *= scrollFactor;
        else
            Scale /= scrollFactor;

        Scale = Math.Clamp(Scale, MinScale, MaxScale);

        var newWorldPos = ScreenToWorldSpace(LastLocation, false);
        Position = Position + oldWorldPos - newWorldPos;

        ClampToBounds();
    }

    public void MouseMove(Vector2 position, bool move)
    {
        if (move)
        {
            var oldWorldPos = ScreenToWorldSpace(LastLocation, false);
            var newWorldPos = ScreenToWorldSpace(position, false);
            Position = Position + oldWorldPos - newWorldPos;

            ClampToBounds();
        }
        LastLocation = position;
    }

    void ClampToBounds()
    {
        float x = Math.Clamp(Position.X, Bounds.Left, Bounds.Right);
        float y = Math.Clamp(Position.Y, Bounds.Top, Bounds.Bottom);
        Position = new Vector2(x, y);
    }

    public Vector2 ScreenToWorldSpace(Vector2 screenPos, bool applyAngle = true)
    {
        var pos = screenPos;

        pos -= _screenSizeHalf;
        if (Tilt > 0)
        {
            pos.Y /= Tilt;
        }
        pos /= Scale;
        pos += Position;
        if (applyAngle)
        {
            pos = Rotate(pos, -Angle);
        }

        return pos;
    }

    public Vector2 WorldToScreenSpace(Vector2 worldPos)
    {
        var pos = worldPos;

        pos = Rotate(pos, Angle);
        pos -= Position;
        pos *= Scale;
        pos.Y *= Tilt;
        pos += _screenSizeHalf;

        return pos;
    }

    public static Vector2 Rotate(Vector2 value, float angle)
    {
        float rad = angle * MathF.PI / 180f;

        float x = value.X;
        float y = value.Y;

        float cos = MathF.Cos(rad);
        float sin = MathF.Sin(rad);

        float xNew = x * cos - y * sin;
        float yNew = x * sin + y * cos;

        var tpos = new Vector2(xNew, yNew);

        return tpos;
    }

    public Vector2 WorldToScreenSpace(Vector3 worldPos)
    {
        var pos = WorldToScreenSpace(new Vector2(worldPos.X, worldPos.Y));
        pos.Y -= worldPos.Z * Scale;
        return pos;
    }

    public float AngleFromScreenPosition(Vector2 screenPos)
    {
        var worldPos = ScreenToWorldSpace(screenPos, false);
        return (float)(Math.Atan2(worldPos.Y, worldPos.X) * (180 / Math.PI));
    }

    public float MouseMoveAngleAroundOrigin(Vector2 position)
    {
        float curAngle = AngleFromScreenPosition(position);
        float lastAngle = AngleFromScreenPosition(LastLocation);

        return curAngle - lastAngle;
    }
}