using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Raycast {
    private Shader shader;
    private Level level;
    private Player player;
    private Tesselator t;
    private Vector3 blockPos;

    public Raycast(Level level, Player player) {
        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        this.level = level;
        this.player = player;

        t = new Tesselator(shader);
    }

    public void OnUpdateFrame() {
        if(Cast()) {
            Highlight((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
        }
        else {
            t.Init();
        }
    }

    public void OnRenderFrame(Vector2i clientSize) {
        shader.OnRenderFrame();

        t.OnRenderFrame();

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= player.LookAt();
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= player.CreatePerspectiveFieldOfView(clientSize);
        shader.SetMatrix4("projection", projection);
    }

    public bool Cast() {
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        float step = 0.1f;
        float maxDistance = 5.0f;
        Vector3 currentPosition = rayOrigin;

        for(float distance = 0; distance < maxDistance; distance += step) {
            currentPosition += rayDirection * step;

            blockPos = new Vector3(
                (int)Math.Floor(currentPosition.X),
                (int)Math.Floor(currentPosition.Y),
                (int)Math.Floor(currentPosition.Z)
            );

            if(level.IsTile((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z)) {
                //Console.WriteLine($"Raycast: Block found at ({x}, {y}, {z})");
                return true;
            }
        }

        //Console.WriteLine("Raycast: No block found within range.");
        return false;
    }

    public void Highlight(int x, int y, int z) {
        shader.OnRenderFrame();

        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        //float alpha = (float)Math.Sin((double)Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        float alpha = (float)Math.Sin(GLFW.GetTime() * 10.0) * 0.2f + 0.4f;
        shader.SetColorRGBA("color0", 1.0f, 1.0f, 1.0f, alpha);
        
        t.Init();
        for(int i = 0; i < 6; i++) {
            Tile.rock.RenderFace(t, x, y, z, i);
        }
        t.OnLoad();
    }
}
