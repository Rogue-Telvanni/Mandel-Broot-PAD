using static System.Console;
using System.Numerics;

namespace MandleBroot_PAD;

public sealed class Mandelbrot
{
    private (int width, int height) _sise;

    private static double NormComplex(Complex complex) => 
        (complex.Real * complex.Real) + (complex.Imaginary * complex.Imaginary);
    
    
    public void Run(string fileName)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        const int width = 1200;
        const int height = 1200;
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/width;
        double pixelHeight = (cyMax - cyMin)/height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[width * height]; // image = dynamic array of colors
	
        for (int j = 0; j < height; ++j)
        {
            //double y = (height/2 - (j + 0.5)) / (height/2) * plane_radius;
            double y =  cyMin + j*pixelHeight; /* mapping from screen to world; reverse Y  axis */
            for (int i = 0; i < width; ++i)
            {
                // double x = plane_center + (i + 0.5 - width/2) / (height/2) * plane_radius;
                double x = cxMin + i*pixelWidth;
                Complex c = new(x,y); // parameter c of  fc(z) = z^2 + c
                Complex z = 0; // z = z0 = critical point 
                int k; // number of iterations
                for (k = 0; k < kMax; ++k)
                {
                    z = z * z + c; // forward iteration of complex quadratic polynomial
                    if (NormComplex(z) > escapeRadiusEnd) // if abs(z) > ER
                    {break;}
                }
                img[j * width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
            }
        }
        
        // create pgm file using command redirection https://en.wikipedia.org/wiki/Redirection_(computing)
        using var fileStream = new FileStream(fileName, FileMode.Create);
        byte[] header = System.Text.Encoding.ASCII.GetBytes($"P5\n{width} {height}\n255\n"); // header of P5 = binary pgm 
        fileStream.Write(header, 0, header.Length);
        fileStream.Write(img, 0, img.Length);
        WriteLine($"Imagem salva com sucesso em: {fileName}");
    }
}