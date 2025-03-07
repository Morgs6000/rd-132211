using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Raycast {
    private Shader shader;
    private Level level;
    private Player player;
    private Tesselator t;

    public Raycast(Level level, Player player) {
        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        this.level = level;
        this.player = player;

        t = new Tesselator(shader);
    }

    public void OnUpdateFrame() {
        HighlightTargetedBlock();
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

    public Vector3? Cast(Player player, Level level, float maxDistance) {
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        float step = 0.1f;
        Vector3 currentPosition = rayOrigin;

        for(float distance = 0; distance < maxDistance; distance += step) {
            currentPosition += rayDirection * step;

            int x = (int)Math.Floor(currentPosition.X);
            int y = (int)Math.Floor(currentPosition.Y);
            int z = (int)Math.Floor(currentPosition.Z);

            //Console.WriteLine($"Raycast: Checking block at ({x}, {y}, {z})");

            if(level.IsTile(x, y, z)) {
                //Console.WriteLine($"Raycast: Block found at ({x}, {y}, {z})");
                return new Vector3(x, y, z);
            }
        }

        //Console.WriteLine("Raycast: No block found within range.");
        return null;
    }

    public void HighlightTargetedBlock() {
        Vector3? highlightedBlock = Cast(player, level, 5.0f);

        if(highlightedBlock.HasValue) {
            int x = (int)highlightedBlock.Value.X;
            int y = (int)highlightedBlock.Value.Y;
            int z = (int)highlightedBlock.Value.Z;

            Highlight(x, y, z);
        }
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
