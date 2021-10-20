using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Laborator_EGC
{
    class ProgramWindow : GameWindow
    {
        bool showObject = false;
        bool TurnLeft = false;
        bool TurnRight = false;
        bool StartMove = false;
        bool MoveLeft = false;
        bool MoveRight = false;
        KeyboardState lastKeyPress;
        const float rotation_speed = 240.0f;
        float angle;

        public ProgramWindow() : base(640, 480)
        {
            VSync = VSyncMode.On;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.AliceBlue);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(-10.0, 10.0, -10.0, 10.0, 0.0, 4.0);
            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
                return;
            }

            if (keyboard[Key.F11])
            {
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
            }

            else if (keyboard[Key.Space] && !keyboard.Equals(lastKeyPress))
            {
                if (!showObject)
                    showObject = true;
            }
            else if (keyboard[Key.ControlLeft] && !keyboard.Equals(lastKeyPress))
            {
                if (showObject)
                    showObject = false;
            }
            else if (keyboard[Key.Left] && !keyboard.Equals(lastKeyPress))
            {
                if (TurnLeft)
                    TurnLeft = false;
                else
                {
                    TurnLeft = true;
                    TurnRight = false;
                }
            }
            else if (keyboard[Key.Right] && !keyboard.Equals(lastKeyPress))
            {
                if (TurnRight)
                    TurnRight = false;
                else
                {
                    TurnRight = true;
                    TurnLeft = false;
                }
            }

            if (mouse[MouseButton.Left])
            {
                if (StartMove)
                    StartMove = false;
                else
                    StartMove = true;
            }

            MoveLeft = false;
            MoveRight = false;

            if (mouse.X < -30) MoveLeft = true;
            else if (mouse.X > 50) MoveRight = true;

            lastKeyPress = keyboard;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(15, 15, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            angle += rotation_speed * (float)e.Time;

            if (showObject)
            {
                if (TurnRight)
                    GL.Rotate(angle, 0.0f, -1.0f, 0.0f);
                else if (TurnLeft)
                    GL.Rotate(angle, 0.0f, 1.0f, 0.0f);

                if (StartMove)
                {
                    if (MoveLeft)
                        GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
                    else if (MoveRight)
                        GL.Rotate(angle, 0.0f, -1.0f, 0.0f);
                }
                DrawCube();
                DrawTriangle();
            }
            SwapBuffers();
        }

        private void DrawTriangle()
        {
            GL.Translate(0, 5, 0);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex3(0, 0, 0);
            GL.Color3(Color.Crimson);
            GL.Vertex3(2, 0, 0);
            GL.Color3(Color.DarkKhaki);
            GL.Vertex3(0, 2, 0);

            GL.End();
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Red);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Yellow);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }

        static void Main()
        {
            using (ProgramWindow project = new ProgramWindow())
            {
                project.Run(30.0, 0.0);
            }
        }
    }
}