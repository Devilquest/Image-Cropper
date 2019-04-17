/*
    This program will crop images in half and save both sides.
    Can crop all images in the same directory where the .exe file is executed
    Or crop the images droped on the .exe file...
    If the images width are at least or greater than the width of "minImageWidth" and its extensions are in the "fileExtensions" list.
    If no images dropped the program asks if you want to delete the original files.
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Image_Cropper
{
    class Program
    {
        static void Main(string[] args)
        {
            //Minimum width of the source image.
            int minImageWidth = 3840; //1920, 3840

            //Source images file extension allowed.
            List<string> fileExtensionsList = new List<string> { ".png", ".jpg" };

            //List of cropped images.
            List<string> croppedImagesList = new List<string>();

            //Counts the number of images cropped.
            int imagesCroppedCount = 0;
            string[] imageFiles;
            string cropConfirmation;

            //Welcome and information
            Console.WriteLine("<<< IMAGE CROPPER TOOL >>>\n");
            Console.WriteLine(" - This program will crop images in half and save both sides.");
            Console.WriteLine(" - Crop all images in the same directory where the .exe file is executed.");
            Console.WriteLine(" - Or crop the images droped on the .exe file.");
            Console.WriteLine(" - Will only crop images equal or larger than " + minImageWidth + " pixels width.");
            if (args.Length <= 0)
                Console.WriteLine(" - Then the program asks if you want to delete the original files."); 

            Console.Write("\n\n");


            //If some file is dropped on the .exe file.
            if (args.Length > 0)
            {
                //List of all images dropped with the extensions in "fileExtensions".
                imageFiles = args.Where(s => fileExtensionsList.Contains(Path.GetExtension(s.ToLower()))).ToArray();

                if (imageFiles.Length > 0)
                {
                    //Crop Images
                    cropConfirmation = string.Empty;

                    Console.WriteLine("Images to crop:");
                    foreach (string images in imageFiles)
                        Console.WriteLine(" - " + Path.GetFileName(images));

                    //While the user input is different than "y" or "n" keeps asking for crop confirmation.
                    while (cropConfirmation != "y" && cropConfirmation != "n")
                    {
                        Console.Write("Crop images: [y/n]? ");
                        cropConfirmation = Console.ReadLine().ToLower();
                    }

                    //If the user input is "y" then...
                    if (cropConfirmation == "y")
                    {
                        //Crop Images
                        CropImagesInHalf(imageFiles);

                        //If one or more images were cropped then...
                        if (imagesCroppedCount > 0)
                        {
                            //Console message with the number of images cropped.
                            Console.WriteLine("\n" + imagesCroppedCount + " Images were cropped!\n");
                            Console.WriteLine("Thank you!");
                        }
                        else
                        {
                            //No images where cropped. Show console messages.
                            Console.WriteLine("No images where cropped!");
                            Console.WriteLine("\nExiting...");
                        }
                    }
                    else
                    {
                        //If the user input is "n" do nothing and show console messages.
                        Console.WriteLine("\nImages no cropped!");
                        Console.WriteLine("\nExiting...");
                    }
                }
                else
                {
                    //No images where cropped. Show console messages.
                    Console.WriteLine("No images to crop found!");
                    Console.WriteLine("\nExiting...");
                }
            }
            else
            {
                //If no file was dropped.

                //Directory path where the .exe file is executed.
                string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                //List of all images found with the extensions in "fileExtensions" in the directory "dir". 
                imageFiles = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(s => fileExtensionsList.Contains(Path.GetExtension(s.ToLower()))).ToArray();

                //Stores the user input of the crop confirmation.
                cropConfirmation = string.Empty;

                //While the user input is different than "y" or "n" keeps asking for crop confirmation.
                while (cropConfirmation != "y" && cropConfirmation != "n")
                {
                    Console.Write("Crop all images in \"" + dir + "\" [y/n]? ");
                    cropConfirmation = Console.ReadLine().ToLower();
                }

                //If the user input is "y" then...
                if (cropConfirmation == "y")
                {
                    //Crop Images
                    CropImagesInHalf(imageFiles);

                    //If one or more images were cropped then...
                    if (imagesCroppedCount > 0)
                    {
                        //Console message with the number of images cropped.
                        Console.WriteLine("\n" + imagesCroppedCount + " Images were cropped!\n");

                        //Stores the user input of the delete original images confirmation.
                        string deleteConfirmation = string.Empty;

                        //While the user input is different than "y" or "n" keeps asking for delete original images confirmation.
                        while (deleteConfirmation != "y" && deleteConfirmation != "n")
                        {
                            Console.Write("Delete original files [y/n]? ");
                            deleteConfirmation = Console.ReadLine().ToLower();
                        }

                        //If the user input is "y" then...
                        if (deleteConfirmation == "y")
                        {
                            //Console message.
                            Console.WriteLine("\nDeleting original files...");

                            //For each image cropped...
                            foreach (string imageFile in croppedImagesList)
                            {
                                //Check if image file exists. Then delete the file.
                                if (File.Exists(imageFile))
                                {
                                    Console.WriteLine(Path.GetFileName(imageFile) + " Deleted!");
                                    File.Delete(imageFile);
                                }
                            }

                            //Once done show console messages.
                            Console.WriteLine("\nDeleting Done!");
                            Console.WriteLine("Thank you!");
                        }
                        else
                        {
                            //If the user input is "n" don't delete image files and show console messages.
                            Console.WriteLine("\nOriginal files not deleted!");
                            Console.WriteLine("Thank you!");
                        }
                    }
                    else
                    {
                        //No images where cropped. Show console messages.
                        Console.WriteLine("No images to crop found!");
                        Console.WriteLine("\nExiting...");
                    }
                }
                else
                {
                    //If the user input is "n" do nothing and show console messages.
                    Console.WriteLine("\nImages no cropped!");
                    Console.WriteLine("\nExiting...");
                }
            }
            Console.Read();

            void CropImagesInHalf(string[] images)
            {
                if (images.Length > 0)
                {
                    Console.Write("\n");
                    foreach (string img in images)
                    {
                        //Creates a bitmap image from source image.
                        Bitmap srcImg = Image.FromFile(img) as Bitmap;

                        //If the width of the source image is at least or greater than the "minImageWidth" then crop images...
                        if (srcImg.Size.Width >= minImageWidth)
                        {
                            //Variables for the left side.
                            Rectangle cropRectLeft = new Rectangle(0, 0, srcImg.Size.Width / 2, srcImg.Size.Height);
                            Bitmap targetImgLeft = new Bitmap(cropRectLeft.Width, cropRectLeft.Height);

                            //Variables for the right side.
                            Rectangle cropRectRight = new Rectangle(srcImg.Size.Width / 2, 0, srcImg.Size.Width / 2, srcImg.Size.Height);
                            Bitmap targetImgRight = new Bitmap(cropRectRight.Width, cropRectRight.Height);

                            //Crop left side of the image.
                            using (Graphics g = Graphics.FromImage(targetImgLeft))
                            {
                                g.DrawImage(srcImg, new Rectangle(0, 0, targetImgLeft.Width, targetImgLeft.Height),
                                                    cropRectLeft,
                                                    GraphicsUnit.Pixel);
                            }
                            //Saves the left side of the image as png.
                            targetImgLeft.Save(Path.GetFileNameWithoutExtension(img) + "_1.png", ImageFormat.Png);

                            //Crop right side of the image.
                            using (Graphics g = Graphics.FromImage(targetImgRight))
                            {
                                g.DrawImage(srcImg, new Rectangle(0, 0, targetImgRight.Width, targetImgRight.Height),
                                                    cropRectRight,
                                                    GraphicsUnit.Pixel);
                            }
                            //Saves the right side of the image as png.
                            targetImgRight.Save(Path.GetFileNameWithoutExtension(img) + "_2.png", ImageFormat.Png);

                            //Increment the images cropped.
                            imagesCroppedCount++;

                            //Print the name of the image cropped.
                            Console.WriteLine(imagesCroppedCount + "- Image: " + Path.GetFileName(img) + " Cropped!");

                            //Add cropped image to "croppedImagesList".
                            croppedImagesList.Add(img);
                        }

                        //Release the bitmap image used.
                        srcImg.Dispose();
                    }
                }
            }
        }
    }
}