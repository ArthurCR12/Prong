using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Prong2
{
    class Retangulo
    {
        public int x;
        public int y;

        public int largura;
        public int altura;
    }

    class Program : GameWindow
    {
        Retangulo bola;
        int velocidadeDaBolaEmX = 3;
        int velocidadeDaBolaEmY = 3;

        int velocidadeDaMaquina = 5;

        int contraMaquina;

        float tempoDecorrido = 0;
        float intervaloAumentoVelocidade = 1.0f;        
        bool velocidadeAumentada = false;

        Retangulo jogador1;
        Retangulo jogador2;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            tempoDecorrido += (float)e.Time;

            if (tempoDecorrido >= intervaloAumentoVelocidade && velocidadeDaBolaEmX < 12 && velocidadeDaBolaEmX > -12)
            {
                if (velocidadeDaBolaEmX > 0)
                {
                    velocidadeDaBolaEmX += 1;
                }
                else
                {
                    velocidadeDaBolaEmX -= 1;
                }
                tempoDecorrido = 0; // Redefinir o tempoDecorrido
            }

            bola.x = bola.x + velocidadeDaBolaEmX;
            bola.y = bola.y + velocidadeDaBolaEmY;

            if (bola.x + bola.largura / 2 > jogador2.x - jogador2.largura / 2
                && bola.y + bola.altura / 2 > jogador2.y - jogador2.altura / 2
                && bola.x - bola.largura / 2 < jogador2.x + jogador2.largura / 2
                && bola.y - bola.altura / 2 < jogador2.y + jogador2.altura / 2)
            {
                velocidadeDaBolaEmX = -velocidadeDaBolaEmX;
            }

            if (bola.x - bola.largura / 2 < jogador1.x + jogador1.largura / 2
                && bola.y + bola.altura / 2 > jogador1.y - jogador1.altura / 2
                && bola.x + bola.largura / 2 > jogador1.x - jogador1.largura / 2
                && bola.y - bola.altura / 2 < jogador1.y + jogador1.altura / 2)
            {
                velocidadeDaBolaEmX = -velocidadeDaBolaEmX;
            }


            if (bola.y + bola.altura / 2 > ClientSize.Height / 2)
            {
                velocidadeDaBolaEmY = -velocidadeDaBolaEmY;
            }

            if (bola.y - bola.altura / 2 < -ClientSize.Height / 2)
            {
                velocidadeDaBolaEmY = -velocidadeDaBolaEmY;
            }

            if (bola.x < -ClientSize.Width / 2 || bola.x > ClientSize.Width / 2)
            {
                bola.x = 0;
                bola.y = 0;
                tempoDecorrido = 0;
                velocidadeDaBolaEmX = 3;
                velocidadeAumentada = false;
            }

            if (contraMaquina == 0)
            {
                if (bola.x > 0) // Certifique-se de que a bola está na metade do jogador 2
                {
                    if (bola.y > jogador2.y && jogador2.altura + jogador2.altura / 2 < ClientSize.Height / 2)
                    {
                        jogador2.y += velocidadeDaMaquina;
                    }
                    else if (bola.y < jogador2.y)
                    {
                        jogador2.y -= velocidadeDaMaquina;
                    }
                }
            }            

            if (Keyboard.GetState().IsKeyDown(Key.W) && jogador1.y + jogador1.altura / 2 < ClientSize.Height / 2)
            {
                jogador1.y += 5;
            }
            if (Keyboard.GetState().IsKeyDown(Key.S) && jogador1.y - jogador1.altura / 2 > -ClientSize.Height / 2)
            {
                jogador1.y -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Key.Up) && jogador2.y + jogador2.altura / 2 < ClientSize.Height / 2)
            {
                jogador2.y += 5;
            }
            if (Keyboard.GetState().IsKeyDown(Key.Down) && jogador2.y - jogador2.altura / 2 > -ClientSize.Height / 2)
            {
                jogador2.y -= 5;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            Matrix4 projection = Matrix4.CreateOrthographic(ClientSize.Width, ClientSize.Height, 0.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            DesenharRetangulo(bola.x, bola.y, bola.largura, bola.altura, 1.0f, 1.0f, 0.0f);
            DesenharRetangulo(jogador1.x, jogador1.y, jogador1.largura, jogador1.altura, 1.0f, 0.0f, 0.0f);
            DesenharRetangulo(jogador2.x, jogador2.y, jogador2.largura, jogador2.altura, 0.0f, 0.0f, 1.0f);

            SwapBuffers();
            Console.WriteLine(velocidadeDaBolaEmX);
        }

        void DesenharRetangulo(int x, int y, int largura, int altura, float r, float g, float b)
        {
            GL.Color3(r, g, b);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f * largura + x, -0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x, -0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x, 0.5f * altura + y);
            GL.Vertex2(-0.5f * largura + x, 0.5f * altura + y);
            GL.End();
        }

        static Retangulo CriarRetangulo(int x, int y, int largura, int altura)
        {
            Retangulo r = new Retangulo();
            r.x = x;
            r.y = y;
            r.largura = largura;
            r.altura = altura;

            return r;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Atualiza a posição dos jogadores com base no novo tamanho da tela
            int larguraDosJogadores = bola.largura;
            int alturaDosJogadores = 3 * bola.altura;
            jogador1 = CriarRetangulo(-ClientSize.Width / 2 + larguraDosJogadores / 2, 0, larguraDosJogadores, alturaDosJogadores);
            jogador2 = CriarRetangulo(ClientSize.Width / 2 - larguraDosJogadores / 2, 0, larguraDosJogadores, alturaDosJogadores);
        }


        static void Main()
        {
            Program p = new Program();

            Console.WriteLine("Qual o tamanho da bola que você quer jogar ?\n" +
                "Escolha uma das opções:\n" +
                "10, 15, 20, 25\n");

            int inputDimesão = int.Parse(Console.ReadLine());
            p.bola = CriarRetangulo(0, 0, inputDimesão, inputDimesão);
            //p.bola = CriarRetangulo(0, 0, 20, 20);

            int larguraDosJogadores = p.bola.largura;
            int alturaDosJogadores = 3 * p.bola.altura;
            p.jogador1 = CriarRetangulo(-p.ClientSize.Width / 2 + larguraDosJogadores / 2, 0, larguraDosJogadores, alturaDosJogadores);
            p.jogador2 = CriarRetangulo(p.ClientSize.Width / 2 - larguraDosJogadores / 2, 0, larguraDosJogadores, alturaDosJogadores);

            Console.WriteLine("\nPara Jogar contra a maquina digite: 0.\nContra outro jogador digite: 1");
            int inputTipoJogador = int.Parse(Console.ReadLine());
            if (inputTipoJogador != 0)
            {
                p.contraMaquina = inputTipoJogador; 
            }            

            p.Run();
        }
    }
}