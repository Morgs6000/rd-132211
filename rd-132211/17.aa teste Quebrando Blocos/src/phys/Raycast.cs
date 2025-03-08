using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Raycast {
    private Level level;
    private LevelRenderer levelRenderer;
    private Player player;

    private Shader shader;
    private Tesselator t;
    
    private Vector3i blockPos;

    public Raycast(Level level, LevelRenderer levelRenderer, Player player) {
        this.level = level;
        this.levelRenderer = levelRenderer;
        this.player = player;

        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        t = new Tesselator(shader);
    }

    public void OnUpdateFrame(GameWindow window) {
        MouseState mouseState = window.MouseState;
        
        if (Target()) {
            Highlight(blockPos);

            if(mouseState.IsButtonPressed(MouseButton.Right)) {
                SetBlock(blockPos, 0);
            }
        }
        else {
            t.Init();
        }
    }

    public void OnRenderFrame(Vector2i clientSize) {
        shader.OnRenderFrame();

        t.OnRenderFrame();

        Matrix4 view = Matrix4.Identity;
        view *= player.LookAt();
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= player.CreatePerspectiveFieldOfView(clientSize);
        shader.SetMatrix4("projection", projection);
    }

    private void SetBlock(Vector3i b, byte id) {
        level.SetTile(b.X, b.Y, b.Z, id);

        int chunkX = b.X / 16;
        int chunkY = b.Y / 16;
        int chunkZ = b.Z / 16;

        levelRenderer.ChunkReloadNeighbors(chunkX, chunkY, chunkZ);
    }

    private void Highlight(Vector3i b) {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin((double)Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f, alpha);

        t.Init();
        for(int i = 0; i < 6; i++) {
            Tile.rock.RenderFace(t, b.X, b.Y, b.Z, i);
        }
        t.OnLoad();
    }

    private bool Target() {
        // Posição e direção da câmera
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        // Tamanho do passo para o raycasting
        float step = 0.1f;
        float maxDistance = 5.0f; // Distância máxima para verificar colisões

        Vector3 currentPos = rayOrigin;

        for (float distance = 0; distance < maxDistance; distance += step) {
            currentPos += rayDirection * step;

            // Converte a posição atual para coordenadas de bloco
            blockPos = new Vector3i(
                (int)Math.Floor(currentPos.X),
                (int)Math.Floor(currentPos.Y),
                (int)Math.Floor(currentPos.Z)
            );

            // Verifica se há um bloco sólido nessa posição
            if (level.IsSolidTile(blockPos.X, blockPos.Y, blockPos.Z)) {
                return true; // Bloco encontrado
            }
        }

        return false;
    }
}
