using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics;
using Color = Raylib_cs.Color;

namespace MandleBroot_PAD
{
    internal enum RenderMethod : byte
    {
        BaseLine,
        TreadsSimples,
        ThreadsSimplesOtimizado,
        ParallelFor,
        ParallelForLowLevel,
        ParallelForLowLevelAVX2,
        AVX2
    }

    internal sealed class Renderer
    {
        Color[] img;
        static (int width, int height) size = (1200, 1200);
        Image rayImage;

        // controle de fluxo
        static CancellationTokenSource cts;
        static Task SelectedTask;
        static string algoritmoAtual = "1: Base Line";
        static Stopwatch stopwatch = new();

        public Renderer()
        {
            img = new Color[size.width * size.height];
            Raylib.InitWindow(size.width, size.height, "Mandelbrot - Raylib");
            Raylib.SetTargetFPS(120);

            rayImage = new Image
            {
                Data = null,
                Width = size.width,
                Height = size.height,
                Format = PixelFormat.UncompressedR8G8B8A8,
                Mipmaps = 1
            };

        }

        public void Start()
        {
            Texture2D texture = Raylib.LoadTextureFromImage(rayImage);
            for (int i = 0; i < img.Length; i++)
            {
                img[i] = Color.Black;
            }

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.One)) AlterarMetodo(RenderMethod.BaseLine);
                if (Raylib.IsKeyPressed(KeyboardKey.Two)) AlterarMetodo(RenderMethod.TreadsSimples);
                if (Raylib.IsKeyPressed(KeyboardKey.Three)) AlterarMetodo(RenderMethod.ThreadsSimplesOtimizado);
                if (Raylib.IsKeyPressed(KeyboardKey.Four)) AlterarMetodo(RenderMethod.ParallelFor);
                if (Raylib.IsKeyPressed(KeyboardKey.Five)) AlterarMetodo(RenderMethod.ParallelForLowLevel);
                if (Raylib.IsKeyPressed(KeyboardKey.Six)) AlterarMetodo(RenderMethod.ParallelForLowLevelAVX2);
                if (Raylib.IsKeyPressed(KeyboardKey.Seven)) AlterarMetodo(RenderMethod.AVX2);


                unsafe
                {
                    fixed (Color* p = img)
                    {
                        rayImage.Data = p;
                        Raylib.UpdateTexture(texture, rayImage.Data);
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Purple);

                Raylib.DrawTexture(texture, 0, 0, Color.White);

                if (SelectedTask is not null)
                {
                    if (!SelectedTask.IsCompleted)
                    {
                        Raylib.DrawText("Calculando... - " + algoritmoAtual, 12, 12, 20, Color.Red);
                    }
                    else
                    {
                        Raylib.DrawText("Terminado - " + algoritmoAtual + " - Tempo De Execução: " + stopwatch.Elapsed.TotalSeconds, 12, 12, 20, Color.Green);
                    }
                }

                Raylib.EndDrawing();
            }

            Raylib.UnloadTexture(texture);
            Raylib.CloseWindow();
        }

        private void AlterarMetodo(RenderMethod renderMethod)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
            Array.Clear(img, 0, img.Length);
            cts = new CancellationTokenSource();
            var token = cts.Token;
            stopwatch.Reset();

            algoritmoAtual = renderMethod switch
            {
                RenderMethod.BaseLine => "Base Line",
                RenderMethod.TreadsSimples => "Threads Simples",
                RenderMethod.ThreadsSimplesOtimizado => "Threads Simples Otimizado",
                RenderMethod.ParallelFor => "Parallel For",
                RenderMethod.ParallelForLowLevel => "Parallel For Low-Level",
                RenderMethod.ParallelForLowLevelAVX2 => "Parallel For Low-Level AVX2",
                RenderMethod.AVX2 => "AVX2",
                _ => "Método desconhecido"
            };

            SelectedTask = renderMethod switch
            {
                RenderMethod.BaseLine => Task.Run(() => { stopwatch.Start();  RenderBaseLine(size, token); stopwatch.Stop(); }),
                RenderMethod.TreadsSimples => Task.Run(() => { stopwatch.Start(); RunThreadsSimples(size, Environment.ProcessorCount, token); stopwatch.Stop(); }),
                RenderMethod.ThreadsSimplesOtimizado => Task.Run(() => { stopwatch.Start(); RunThreadsSimplesOtimizado(size, Environment.ProcessorCount, token); stopwatch.Stop(); }),
                RenderMethod.ParallelFor => Task.Run(() => { stopwatch.Start(); RunParallelFor(size, Environment.ProcessorCount, token); stopwatch.Stop();}),
                RenderMethod.ParallelForLowLevel => Task.Run(() => { stopwatch.Start(); RunParallelForPrimitives(size, token); stopwatch.Stop(); }),
                RenderMethod.ParallelForLowLevelAVX2 => Task.Run(() => { stopwatch.Start(); RunParallelForLowLevelAVX2(size, token); stopwatch.Stop(); }),
                RenderMethod.AVX2 => Task.Run(() => { stopwatch.Start(); RunAVX2(size, token); stopwatch.Stop(); }),
            };
        }

        private void RenderBaseLine((int width, int height) size, CancellationToken token)
        {
            const double aspectRatio = 1.0; 
                                            
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


            for (int j = 0; j < size.height; ++j)
            {
                if (token.IsCancellationRequested)
                    return;

                double y = cyMin + j * pixelHeight; 
                for (int i = 0; i < size.width; ++i)
                {
                    double x = cxMin + i * pixelWidth;
                    Complex c = new(x, y); 
                    Complex z = 0; 
                    int k; 
                    for (k = 0; k < kMax; ++k)
                    {
                        z = z * z + c; 
                        if (NormComplex(z) > escapeRadiusEnd) 
                        { break; }
                    }
                    
                    byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                    img[j * size.width + i] = pixelColor;
                }
            }
        }

        private static double NormComplex(Complex complex) =>
            (complex.Real * complex.Real) + (complex.Imaginary * complex.Imaginary);

        public void RunThreadsSimples((int width, int height) size, int numberOfThreds, CancellationToken token)
        {
            const double aspectRatio = 1.0;

            const double planeRadius = 2;
            Complex planeCenter = new(-0.5, 0.0);

            double cxMin = planeCenter.Real - (planeRadius * aspectRatio);
            double cxMax = planeCenter.Real + (planeRadius * aspectRatio);
            double cyMin = planeCenter.Imaginary - planeRadius;
            double cyMax = planeCenter.Imaginary + planeRadius;
            double pixelWidth = (cxMax - cxMin) / size.width;
            double pixelHeight = (cyMax - cyMin) / size.height;


            const int kMax = 1024; 
            const double escapeRadius = 2;
            const double escapeRadiusEnd = escapeRadius * escapeRadius;

            var tasks = new Task[numberOfThreds];

            int offset = size.height / tasks.Length;
            for (int part = 0; part < tasks.Length; part++)
            {
                var part1 = part;
                tasks[part] = Task.Run(() =>
                {
                    int limit = (part1 + 1) * offset;
                    for (int j = part1 * offset; j < limit; ++j)
                    {
                        double y = cyMin + j * pixelHeight; 
                        for (int i = 0; i < size.width; ++i)
                        {
                            double x = cxMin + i * pixelWidth;
                            Complex c = new(x, y); 
                            Complex z = 0;
                            int k;
                            for (k = 0; k < kMax; ++k)
                            {
                                z = z * z + c; 
                                if (NormComplex(z) > escapeRadiusEnd)
                                { break; }
                            }

                            byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                            Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                            img[j * size.width + i] = pixelColor;
                        }
                    }
                });
            }

            Task.WaitAll(tasks, token);
        }

        public void RunThreadsSimplesOtimizado((int width, int height) size, int numberOfThreds, CancellationToken token)
        {
            const double aspectRatio = 1.0;

            const double planeRadius = 2;
            Complex planeCenter = new(-0.5, 0.0);

            double cxMin = planeCenter.Real - (planeRadius * aspectRatio);
            double cxMax = planeCenter.Real + (planeRadius * aspectRatio);
            double cyMin = planeCenter.Imaginary - planeRadius;
            double cyMax = planeCenter.Imaginary + planeRadius;
            double pixelWidth = (cxMax - cxMin) / size.width;
            double pixelHeight = (cyMax - cyMin) / size.height;


            const int kMax = 1024; 
            const double escapeRadius = 2;
            const double escapeRadiusEnd = escapeRadius * escapeRadius;

            var tasks = new Task[numberOfThreds];

            int offset = size.height / tasks.Length;
            for (int part = 0; part < tasks.Length; part++)
            {
                var part1 = part;
                tasks[part] = Task.Run(() =>
                {
                    for (int j = 0; j < offset; ++j)
                    {
                        int line = part1 + tasks.Length * j;
                        double y = cyMin + line * pixelHeight;
                        for (int i = 0; i < size.width; ++i)
                        {
                            double x = cxMin + i * pixelWidth;
                            Complex c = new(x, y); 
                            Complex z = 0; 
                            int k;
                            for (k = 0; k < kMax; ++k)
                            {
                                z = z * z + c; 
                                if (NormComplex(z) > escapeRadiusEnd)
                                {
                                    break;
                                }
                            }

                            byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                            Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                            img[line * size.width + i] = pixelColor;
                        }
                    }
                });
            }

            Task.WaitAll(tasks, token);
        }

        public void RunParallelFor((int width, int height) size, int numberOfThreds, CancellationToken token)
        {
            const double aspectRatio = 1.0;

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


            /*
             * parallel por padrão faz um balanceamento melhor devido a cada linha vai executar em uma thread da thread pool
             * com isso ele faz com que a parte central que precisa validar todas as condicoes de escape seja dividida em
             * várias threads diminuindo assim o custo de uma unica thread no multi thread simples melhorando o desempenho
             * de forma significativa
            */
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = numberOfThreds,
                CancellationToken = token
            };

            Parallel.For(0, size.height, options, (int j) =>
            {
                double y = cyMin + j * pixelHeight; 
                for (int i = 0; i < size.width; ++i)
                {
                    double x = cxMin + i * pixelWidth;
                    Complex c = new(x, y);
                    Complex z = 0;
                    int k;
                    for (k = 0; k < kMax; ++k)
                    {
                        z = z * z + c;
                        if (NormComplex(z) > escapeRadiusEnd) 
                        {
                            break;
                        }
                    }

                    byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                    img[j * size.width + i] = pixelColor;
                }
            });

        }

        public void RunParallelForPrimitives((int width, int height) size, CancellationToken token)
        {
            const double aspectRatio = 1.0;

            const double planeRadius = 2;
            Complex planeCenter = new(-0.5, 0.0);

            double cxMin = planeCenter.Real - (planeRadius * aspectRatio);
            double cxMax = planeCenter.Real + (planeRadius * aspectRatio);
            double cyMin = planeCenter.Imaginary - planeRadius;
            double cyMax = planeCenter.Imaginary + planeRadius;
            double pixelWidth = (cxMax - cxMin) / size.width;
            double pixelHeight = (cyMax - cyMin) / size.height;


            const int kMax = 1024;
            const double escapeRadius = 2;
            const double escapeRadiusEnd = escapeRadius * escapeRadius;

            Parallel.For(0, size.height, (int j) =>
            {
                double y = cyMin + j * pixelHeight;
                for (int i = 0; i < size.width; ++i)
                {
                    double x = cxMin + i * pixelWidth;

                    double cr = x;
                    double ci = y;

                    double zr = 0, zi = 0;
                    int k; 
                    for (k = 0; k < kMax; ++k)
                    {

                        /*
                         * Z^2 um numero complexo ao quadrado é similar a
                         * Z^2 = (z_r + z_i i) * (z_r + z_i i)
                         * Z^2 = z_r^2 + z_r z_i i + z_r z_i i + z_i^2 i^2
                         * ONDE z_r^2 - z_i^2 é a parte real e 2 z_r z_i é a parte imaginaria
                         * removemos a struct e chamadas e overloads de metodos do Complex e usamos objetos com menos
                         * overhead de performance.
                         */

                        double zr2 = zr * zr;
                        double zi2 = zi * zi;

                        if (zr2 + zi2 > escapeRadiusEnd)
                        {
                            break;
                        }

                        zi = 2.0 * zr * zi + ci;
                        zr = zr2 - zi2 + cr;
                    }


                    byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                    img[j * size.width + i] = pixelColor;
                }
            });
        }

        public void RunParallelForLowLevelAVX2((int width, int height) size, CancellationToken token)
        {
            const double aspectRatio = 1.0;

            const double planeRadius = 2;
            Complex planeCenter = new(-0.5, 0.0);

            double cxMin = planeCenter.Real - (planeRadius * aspectRatio);
            double cxMax = planeCenter.Real + (planeRadius * aspectRatio);
            double cyMin = planeCenter.Imaginary - planeRadius;
            double cyMax = planeCenter.Imaginary + planeRadius;
            double pixelWidth = (cxMax - cxMin) / size.width;
            double pixelHeight = (cyMax - cyMin) / size.height;


            const int kMax = 1024;
            const double escapeRadius = 2;
            const double escapeRadiusEnd = escapeRadius * escapeRadius;

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = token
            };

            Parallel.For(0, size.height, options, (int j) =>
            {
                Vector256<double> maxRadiusVector = Vector256.Create(escapeRadiusEnd);
                Vector256<double> doubleValueVector256 = Vector256.Create(2.0);

                double y = cyMin + j * pixelHeight;
                int simdLimit = size.width - (size.width % 4);
                for (int i = 0; i < simdLimit; i += 4)
                {
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
                        Vector256<double> zr2 = zrVector * zrVector;
                        Vector256<double> zi2 = ziVector * ziVector;

                        Vector256<double> radius = zr2 + zi2;

                        Vector256<double> maskDouble = Vector256.LessThanOrEqual(radius, maxRadiusVector);
                        Vector256<long> mask = maskDouble.AsInt64();

                        if (mask == Vector256<long>.Zero)
                        {
                            break;
                        }

                        iterations = iterations - mask;

                        ziVector = (doubleValueVector256 * zrVector * ziVector) + ciVector;
                        zrVector = zr2 - zi2 + crVector;
                    }

                    byte intensity0 = (iterations[0] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor0 = new(intensity0, intensity0, intensity0, (byte)255);
                    img[j * size.width + i] = pixelColor0;

                    byte intensity1 = (iterations[1] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor1 = new(intensity1, intensity1, intensity1, (byte)255);
                    img[j * size.width + i + 1] = pixelColor1;

                    byte intensity2 = (iterations[2] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor2 = new(intensity2, intensity2, intensity2, (byte)255);
                    img[j * size.width + i + 2] = pixelColor2;

                    byte intensity3 = (iterations[3] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor3 = new(intensity3, intensity3, intensity3, (byte)255);
                    img[j * size.width + i + 3] = pixelColor3;
                }

                for (int i = simdLimit; i < size.width; ++i)
                {
                    double x = cxMin + i * pixelWidth;

                    double cr = x;
                    double ci = y;

                    double zr = 0, zi = 0;
                    int k;
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

                        if (zr2 + zi2 > escapeRadiusEnd)
                        {
                            break;
                        }

                        zi = 2.0 * zr * zi + ci;
                        zr = zr2 - zi2 + cr;
                    }

                    byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                    img[j * size.width + i] = pixelColor;
                }
            });

        }

        public void RunAVX2((int width, int height) size, CancellationToken token)
        {
            const double aspectRatio = 1.0;

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


            Vector256<double> maxRadiusVector = Vector256.Create(escapeRadiusEnd);
            Vector256<double> doubleValueVector256 = Vector256.Create(2.0);

            for (int j = 0; j < size.height; ++j)
            {
                if(token.IsCancellationRequested)
                    return;

                double y = cyMin + j * pixelHeight;
                int simdLimit = size.width - (size.width % 4);
                for (int i = 0; i < simdLimit; i += 4)
                {
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

                    int k;
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

                        Vector256<double> maskDouble = Vector256.LessThanOrEqual(radius, maxRadiusVector);
                        Vector256<long> mask = maskDouble.AsInt64();

                        if (mask == Vector256<long>.Zero)
                        {
                            break;
                        }

                        iterations = iterations - mask;

                        ziVector = (doubleValueVector256 * zrVector * ziVector) + ciVector;
                        zrVector = zr2 - zi2 + crVector;
                    }

                    byte intensity0 = (iterations[0] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor0 = new(intensity0, intensity0, intensity0, (byte)255);
                    img[j * size.width + i] = pixelColor0;

                    byte intensity1 = (iterations[1] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor1 = new(intensity1, intensity1, intensity1, (byte)255);
                    img[j * size.width + i + 1] = pixelColor1;

                    byte intensity2 = (iterations[2] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor2 = new(intensity2, intensity2, intensity2, (byte)255);
                    img[j * size.width + i + 2] = pixelColor2;

                    byte intensity3 = (iterations[3] == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor3 = new(intensity3, intensity3, intensity3, (byte)255);
                    img[j * size.width + i + 3] = pixelColor3;
                }

                for (int i = simdLimit; i < size.width; ++i)
                {
                    double x = cxMin + i * pixelWidth;

                    double cr = x;
                    double ci = y;

                    double zr = 0, zi = 0;
                    int k;
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

                        if (zr2 + zi2 > escapeRadiusEnd)
                        {
                            break;
                        }

                        zi = 2.0 * zr * zi + ci;
                        zr = zr2 - zi2 + cr;
                    }

                    byte intensity = (k == kMax) ? (byte)0 : (byte)255;
                    Color pixelColor = new(intensity, intensity, intensity, (byte)255);
                    img[j * size.width + i] = pixelColor;
                }
            }

        }
    }
}
