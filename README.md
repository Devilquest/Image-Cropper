# Image Cropper Tool
### This program will crop images in half and save both sides.
* Can crop all images in the same directory where the .exe file is executed...
* Or crop the images droped on the .exe file...
* Will only crop the images if its width is equal or larger than 3840 pixels (or  width specified in `minImageWidth` variable).
* Will only crop images if its extension is *.jpg or *.png (or  extensions specified in `fileExtensionsList` list).
* The result images will be saved in the same directory of the original image. With the same name ended in `_1.png` for the left side and  `_2.png` for the right side.
