# 2D-isoedit
A program to generate isometric graphics from elevation data.<br>
An example of what is possible with it...<br>
<img src="https://i.imgur.com/lGQsCRJ.gif" /><br>
## Features
Isometric graphics from elevation data<br>
Freely rotatable on Z axis<br>
Generated realistic shadow<br>
## Planned
Real time editor for height map<br>
Advanced export options<br>
## How Use
Download v0.2.1 https://github.com/Grille98/2D-isometricEditor/releases<br>
Run 2D-isoedit.exe in the bin folder<br>
Load any heightmap<br>
## Heightmap format
The green rgb channel is used for the height<br>
G = 100 => height = 100px<br>

The blue rgb channel defines the used texture<br>
List of Textures:<br>
  - 0 = grass<br>
  - 1 = dirt<br>
  - 2 = sand<br>
  - 3 = stone<br>
  - 4 = dark grass<br>
  - 5 = water<br>
  
The red and alpha rgb channels are ignored<br>
