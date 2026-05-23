using static System.Console;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MandleBroot_PAD;

public sealed class Mandelbrot
{
   
    
    public static void Run(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/size.width;
        double pixelHeight = (cyMax - cyMin)/size.height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[size.width * size.height]; // image = dynamic array of colors
	
        for (int j = 0; j < size.height; ++j)
        {
            //double y = (size.height/2 - (j + 0.5)) / (size.height/2) * plane_radius;
            double y =  cyMin + j*pixelHeight; /* mapping from screen to world; reverse Y  axis */
            for (int i = 0; i < size.width; ++i)
            {
                // double x = plane_center + (i + 0.5 - size.width/2) / (size.height/2) * plane_radius;
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
                img[j * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
            }
        }
        
        WriteFile(fileName, size, img);
    }

    public static void RunThreadsSimples(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/size.width;
        double pixelHeight = (cyMax - cyMin)/size.height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[size.width * size.height]; // image = dynamic array of colors
        var tasks = new Task[12]; // numero de processadores fisicos Ryzen 3600
        
        // subdivide o problema em tasks.size pedaços de forma mais simples e ineficiente para testes
        // dividindo o problema em tasks.size blocos e na altura
        int offset = size.height / tasks.Length;
        for (int part = 0; part < tasks.Length; part++)
        {
            var part1 = part;
            tasks[part] = Task.Run(() =>
            {
                int limit = (part1 + 1) * offset;
                for (int j = part1 * offset; j < limit; ++j)
                {
                    double y =  cyMin + j*pixelHeight; /* mapping from screen to world; reverse Y  axis */
                    for (int i = 0; i < size.width; ++i)
                    {
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
                        img[j * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
                    }
                }
            });
        }
        
        Task.WaitAll(tasks);
        WriteFile(fileName, size, img);
    }

    public static void RunThreadsSimplesOtimizado(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);

        double cxMin = planeCenter.Real - (planeRadius * aspectRatio);
        double cxMax = planeCenter.Real + (planeRadius * aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin) / size.width;
        double pixelHeight = (cyMax - cyMin) / size.height;


        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;

        var img = new byte[size.width * size.height]; // image = dynamic array of colors
        var tasks = new Task[12]; // numero de processadores fisicos 6 virtuais sao 12  Ryzen 3600

        // subdivide o problema em tasks.size pedaços de forma mais simples e ineficiente para testes
        // dividindo o problema em tasks.size blocos e na altura
        int offset = size.height / tasks.Length;
        for (int part = 0; part < tasks.Length; part++)
        {
            var part1 = part;
            tasks[part] = Task.Run(() =>
            {
                for (int j = 0; j < offset; ++j)
                {
                    int line = part1 + tasks.Length * j;
                    double y = cyMin + line * pixelHeight; /* mapping from screen to world; reverse Y  axis */
                    for (int i = 0; i < size.width; ++i)
                    {
                        double x = cxMin + i * pixelWidth;
                        Complex c = new(x, y); // parameter c of  fc(z) = z^2 + c
                        Complex z = 0; // z = z0 = critical point 
                        int k; // number of iterations
                        for (k = 0; k < kMax; ++k)
                        {
                            z = z * z + c; // forward iteration of complex quadratic polynomial
                            if (NormComplex(z) > escapeRadiusEnd) // if abs(z) > ER
                            {
                                break;
                            }
                        }

                        img[line * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
                    }
                }
            });
        }

        Task.WaitAll(tasks);
        WriteFile(fileName, size, img);
    }
    
    public static void RunParallelFor(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/size.width;
        double pixelHeight = (cyMax - cyMin)/size.height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[size.width * size.height]; // image = dynamic array of colors
        
        
        /*
         * parallel por padrão faz um balanceamento melhor devido a cada linha vai executar em uma thread da thread pool
         * com isso ele faz com que a parte central que precisa validar todas as condicoes de escape seja dividida em
         * várias threads diminuindo assim o custo de uma unica thread no multi thread simples melhorando o desempenho
         * de forma significativa
        */
        Parallel.For(0, size.height, (int j) =>
        {
            //double y = (size.height/2 - (j + 0.5)) / (size.height/2) * plane_radius;
            double y = cyMin + j * pixelHeight; /* mapping from screen to world; reverse Y  axis */
            for (int i = 0; i < size.width; ++i)
            {
                // double x = plane_center + (i + 0.5 - size.width/2) / (size.height/2) * plane_radius;
                double x = cxMin + i * pixelWidth;
                Complex c = new(x, y); // parameter c of  fc(z) = z^2 + c
                Complex z = 0; // z = z0 = critical point 
                int k; // number of iterations
                for (k = 0; k < kMax; ++k)
                {
                    z = z * z + c; // forward iteration of complex quadratic polynomial
                    if (NormComplex(z) > escapeRadiusEnd) // if abs(z) > ER
                    {
                        break;
                    }
                }

                img[j * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
            }
        });
        
        WriteFile(fileName, size, img);
    }
    
    public static void RunParallelForLowLevel(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/size.width;
        double pixelHeight = (cyMax - cyMin)/size.height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[size.width * size.height]; // image = dynamic array of colors
        
        Parallel.For(0, size.height, (int j) =>
        {
            //double y = (size.height/2 - (j + 0.5)) / (size.height/2) * plane_radius;
            double y = cyMin + j * pixelHeight; /* mapping from screen to world; reverse Y  axis */
            for (int i = 0; i < size.width; ++i)
            {
                // double x = plane_center + (i + 0.5 - size.width/2) / (size.height/2) * plane_radius;
                double x = cxMin + i * pixelWidth;
                
                // parameter c of  fc(z) = z^2 + c
                double cr = x;
                double ci = y; 
                
                double zr = 0, zi = 0;
                int k; // number of iterations
                for (k = 0; k < kMax; ++k)
                {
                    
                    /*
                     * Z^2 um numero complexo ao quadrado é similar a
                     * Z^2 = (z_r + z_i i) \times (z_r + z_i i)
                     * Z^2 = z_r^2 + z_r z_i i + z_r z_i i + z_i^2 i^2
                     * ONDE z_r^2 - z_i^2 é a parte real e 2 z_r z_i é a parte imaginaria
                     * removemos a struct e chamadas e overloads de metodos do Complex e usamos objetos com menos
                     * overhead de performance.
                     */
                    
                    double zr2 = zr * zr;
                    double zi2 = zi * zi;
                    
                    //z = z * z + c; // forward iteration of complex quadratic polynomial
                    if (zr2 + zi2 > escapeRadiusEnd) // if abs(z) > ER
                    {
                        break;
                    }
                    
                    zi = 2.0 * zr * zi + ci;
                    zr = zr2 - zi2 + cr;
                }

                img[j * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
            }
        });
        
        WriteFile(fileName, size, img);
    }
    
    public static void RunParallelForLowLevelAVX2(string fileName, (int width, int height) size)
    {
        const double aspectRatio = 1.0; // https://en.wikipedia.org/wiki/Aspect_ratio_(image)
        // integer coordinate ( pixel or screen or image coordinate)
        // double coordinate ( real world)
        const double planeRadius = 2;
        Complex planeCenter = new(-0.5, 0.0);
	
        double cxMin = planeCenter.Real - (planeRadius*aspectRatio);	
        double cxMax = planeCenter.Real + (planeRadius*aspectRatio);
        double cyMin = planeCenter.Imaginary - planeRadius;
        double cyMax = planeCenter.Imaginary + planeRadius;
        double pixelWidth = (cxMax - cxMin)/size.width;
        double pixelHeight = (cyMax - cyMin)/size.height;
	
		
        const int kMax = 1024; // maximal number of iterations
        const double escapeRadius = 2;
        const double escapeRadiusEnd = escapeRadius * escapeRadius;
  
        var img = new byte[size.width * size.height]; // image = dynamic array of colors
        
        Parallel.For(0, size.height, (int j) =>
        {
            //double y = (size.height/2 - (j + 0.5)) / (size.height/2) * plane_radius;
            Vector256<double> maxRadiusVector = Vector256.Create(escapeRadiusEnd);
            Vector256<double> doubleValueVector256 = Vector256.Create(2.0);
            
            double y = cyMin + j * pixelHeight; /* mapping from screen to world; reverse Y  axis */
            int simdLimit = size.width - (size.width % 4);
            for (int i = 0; i < simdLimit; i += 4)
            {
                // double x = plane_center + (i + 0.5 - size.width/2) / (size.height/2) * plane_radius;
                
                Vector256<double> crVector = Vector256.Create(
                    cxMin + i * pixelWidth,
                    cxMin + (i + 1) * pixelWidth,
                    cxMin + (i + 2) * pixelWidth,
                    cxMin + (i + 3) * pixelWidth
                );
                
                Vector256<double> ciVector = Vector256.Create(y);
                
                Vector256<double> zrVector = Vector256<double>.Zero;
                Vector256<double> ziVector = Vector256<double>.Zero;
                
                
                Vector256<long> iterations = Vector256<long>.Zero;
                
                int k; // number of iterations
                for (k = 0; k < kMax; ++k)
                {
                    /*
                     * Z^2 um numero complexo ao quadrado é similar a
                     * Z^2 = (z_r + z_i i) \times (z_r + z_i i)
                     * Z^2 = z_r^2 + z_r z_i i + z_r z_i i + z_i^2 i^2
                     * ONDE z_r^2 - z_i^2 é a parte real e 2 z_r z_i é a parte imaginaria
                     * removemos a struct e chamadas e overloads de metodos do Complex e usamos objetos com menos
                     * overhead de performance.
                     */
                    Vector256<double> zr2 = zrVector * zrVector;
                    Vector256<double> zi2 = ziVector * ziVector;
                    
                    Vector256<double> radius = zr2 + zi2;
                    
                    // Compare magnitude squared against the limit.
                    // True = pixel is still inside. False = pixel escaped.
                    Vector256<double> maskDouble = Vector256.LessThanOrEqual(radius, maxRadiusVector);
                    // Reinterpret the mask as 64-bit integers.
                    // Active lanes become -1. Escaped lanes become 0.
                    Vector256<long> mask = maskDouble.AsInt64();

                    // If all 4 lanes have escaped (the mask is entirely zeros), we can break early!
                    if (mask == Vector256<long>.Zero)
                    {
                        break;
                    }
                    
                    iterations = iterations - mask;
                    
                    ziVector = (doubleValueVector256 * zrVector * ziVector) + ciVector;
                    zrVector = zr2 - zi2 + crVector;
                }

                img[j * size.width + i]     = iterations[0] == kMax ? (byte)0 : (byte)255;
                img[j * size.width + i + 1] = iterations[1] == kMax ? (byte)0 : (byte)255;
                img[j * size.width + i + 2] = iterations[2] == kMax ? (byte)0 : (byte)255;
                img[j * size.width + i + 3] = iterations[3] == kMax ? (byte)0 : (byte)255;
            }
            
            // para arrays que não são perfeitamente divisiveis por 4 
            for (int i = simdLimit; i < size.width; ++i)
            {
                // double x = plane_center + (i + 0.5 - size.width/2) / (size.height/2) * plane_radius;
                double x = cxMin + i * pixelWidth;
                
                // parameter c of  fc(z) = z^2 + c
                double cr = x;
                double ci = y; 
                
                double zr = 0, zi = 0;
                int k; // number of iterations
                for (k = 0; k < kMax; ++k)
                {
                    
                    /*
                     * Z^2 um numero complexo ao quadrado é similar a
                     * Z^2 = (z_r + z_i i) \times (z_r + z_i i)
                     * Z^2 = z_r^2 + z_r z_i i + z_r z_i i + z_i^2 i^2
                     * ONDE z_r^2 - z_i^2 é a parte real e 2 z_r z_i é a parte imaginaria
                     * removemos a struct e chamadas e overloads de metodos do Complex e usamos objetos com menos
                     * overhead de performance.
                     */
                    
                    double zr2 = zr * zr;
                    double zi2 = zi * zi;
                    
                    //z = z * z + c; // forward iteration of complex quadratic polynomial
                    if (zr2 + zi2 > escapeRadiusEnd) // if abs(z) > ER
                    {
                        break;
                    }
                    
                    zi = 2.0 * zr * zi + ci;
                    zr = zr2 - zi2 + cr;
                }

                img[j * size.width + i] = (k == kMax) ? (byte)0 : (byte)255; // compute and save color to array
            }
        });
        
        WriteFile(fileName, size, img);
    }

    private static double NormComplex(double real, double imaginary) => 
        (real * real) + (imaginary * imaginary);
    
    
    private static double NormComplex(Complex complex) => 
        (complex.Real * complex.Real) + (complex.Imaginary * complex.Imaginary);
    
    private static void WriteFile(string fileName, (int width, int height) size, byte[] img)
    {
        // create pgm file using command redirection https://en.wikipedia.org/wiki/Redirection_(computing)
        using var fileStream = new FileStream(fileName, FileMode.Create);
        // header of P5 = binary pgm 
        byte[] header = System.Text.Encoding.ASCII.GetBytes($"P5\n{size.width} {size.height}\n255\n"); 
        fileStream.Write(header, 0, header.Length);
        fileStream.Write(img, 0, img.Length);
        WriteLine($"Imagem salva com sucesso em: {fileName}");
    }
}