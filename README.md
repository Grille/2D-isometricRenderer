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
![Animation16](https://private-user-images.githubusercontent.com/26384012/333878765-b4541b00-b4d4-4e0e-a9a6-983a1ff05d5e.gif?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MTY3NTM1ODYsIm5iZiI6MTcxNjc1MzI4NiwicGF0aCI6Ii8yNjM4NDAxMi8zMzM4Nzg3NjUtYjQ1NDFiMDAtYjRkNC00ZTBlLWE5YTYtOTgzYTFmZjA1ZDVlLmdpZj9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNDA1MjYlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjQwNTI2VDE5NTQ0NlomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTJhZTVkYmJjMzRhY2I0MThkMjc3ZmVkODRhODExMmJjNDVjYTYyNmVhOWRiZDQ0OGQ2NDU2MmUzZDVmM2RkMjAmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.6TTV1MOWrIICc3vn3q0J5cJTZu0JLAkRLjQKmmC4p48)

## HowTo Use Demo
Download the latest demo https://github.com/Grille/2D-isometricRenderer/releases and run it.\
It should be fairly self-explanatory

The program has problems with non-square images, if you try to load these the program will likely crash.

## Use Packages
If you like to use the renderer in, you own projects you can use the [Windows](https://www.nuget.org/packages/Grille.Graphics.Isometric.WinForms) package.
It contains a `Grille.Graphics.Isometric.WinForms.RenderSurface` WinForms Control and the `BitmapInputData` static class for easy use.

It is also possible to use the [Base](https://www.nuget.org/packages/Grille.Graphics.Isometric) Package which is platform independent.\
But it has no way of rendering or loading data setup, and with the lack of documentation it is probably rather difficult to use.
