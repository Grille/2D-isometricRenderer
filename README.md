# 2D-isoedit
A program to generate isometric graphics from elevation data.

## Features
- Isometric graphics from elevation data.
- Freely rotatable on Z axis
- Generate normal-map form height-map
- Shading based on normals
- Realtime Rendering

## How it works
The renderer is completely CPU based but makes heavy use of `Taks`'s.

It takes an input buffer that contain height, color and normals, normals can be generated from height if needed.

The render also needs an instance of the `Swapchain` class, its job is to provide the `IsometricRenderer` with a `ARGBColor` pointer to the data of the next image, and the finished result to the outside, while making sure that both never conflict with each other.

The renderer goes through each pixel in the area that contains the baseplate.
That loop goes front to back then left to right.

A sample method is used to get the input at the location after applying the rotation.

Based on the height, a line is drawn top to bottom.
For each drawn pixel in that line, an “shader” method is invoked where custom logic can be inserted.\
If the line reaches an already drawn pixel the loop is exits.

Since the image is rendered front to back, this is a relatively effective optimization that ensures that no pixel is drawn twice.

## Links
- [Github Repository](https://github.com/Grille/2D-isometricRenderer)
- [Nuget Package](https://www.nuget.org/packages/Grille.Graphics.Isometric) / [Windows](https://www.nuget.org/packages/Grille.Graphics.Isometric.WinForms)
- [Youtube Video](https://www.youtube.com/watch?v=cMj5tAFPiHg)

## Example
![Animation16](https://raw.githubusercontent.com/Grille/grille.github.io/images/IsometricAnimation16.gif)

## HowTo Use Demo
Download the latest demo https://github.com/Grille/2D-isometricRenderer/releases and run it.\
It should be fairly self-explanatory

The program has problems with non-square images, if you try to load these the program will likely crash.

## Use Packages
If you like to use the renderer in, you own projects you can use the [Windows](https://www.nuget.org/packages/Grille.Graphics.Isometric.WinForms) package.
It contains a `Grille.Graphics.Isometric.WinForms.RenderSurface` WinForms Control and the `BitmapInputData` static class for easy use.

It is also possible to use the [Base](https://www.nuget.org/packages/Grille.Graphics.Isometric) Package which is platform independent.\
But it has no way of rendering or loading data setup, and with the lack of documentation it is probably rather difficult to use.
