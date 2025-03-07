using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Raycast {
    private Shader shader;
    private Player player;
    private Level level;
    private LevelRenderer levelRenderer;
    private Tesselator t;

    private Vector3 origin;
    private Vector3 direction;
    private Vector3 blockPos;

    public Raycast(Level level, LevelRenderer levelRenderer, Player player) {
        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        t = new Tesselator(shader);

        this.level = level;
        this.levelRenderer = levelRenderer;
        this.player = player;
    }

    // Método para verificar colisões ao longo de um raio
    public void OnUpdateFrame(GameWindow window) {
        origin = player.position;
        direction = player.direction;

        // Verifica se o jogador está olhando para um bloco
        if(Cast()) {
            SetHighlight((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);

            // Se o botão esquerdo do mouse for pressionado, remove o bloco
            if(window.MouseState.IsButtonPressed(MouseButton.Left)) {
                SetBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z, 0);
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

    private void SetHighlight(int x, int y, int z) {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin(GLFW.GetTime() * 10.0) * 0.2f + 0.4f;
        shader.SetColorRGBA("color0", 1.0f, 1.0f, 1.0f, alpha);

        t.Init();
        for(int i = 0; i < 6; i++) {
            Tile.rock.RenderFace(t, x, y, z, i);
        }
        t.OnLoad();
    }

    private void SetBlock(int x, int y, int z, byte id) {
        level.SetTile(x, y, z, id);

        // Calcula a chunk que contém o bloco
        int chunkX = x / 16;
        int chunkY = y / 16;
        int chunkZ = z / 16;

        // Recarrega apenas a chunk afetada
        levelRenderer.ChunkReloadNeighbors(chunkX, chunkY, chunkZ);

        Console.WriteLine($"Bloco removido: ({x}, {y}, {z})");
        Console.WriteLine($"Chunk recarregada: ({chunkX}, {chunkY}, {chunkZ})");
    }

    private bool Cast() {
        // Normaliza a direção do raio
        direction = Vector3.Normalize(direction);

        // Posição atual ao longo do raio
        Vector3 currentPosition = origin;

        // Tamanho do passo (ajuste conforme necessário)
        float stepSize = 0.1f;

        // Distância máxima do raio
        float maxDistance = 10.0f;

        // Itera ao longo do raio
        for(float distance = 0; distance < maxDistance; distance += stepSize) {
            // Atualiza a posição atual
            currentPosition += direction * stepSize;

            // Verifica se a posição atual colide com um bloco sólido
            if(IsSolidBlock(currentPosition)) {
                // Printa a posição do bloco no console
                //Console.WriteLine($"Bloco colidido: ({blockPos})");

                return true; // Sai do método após encontrar uma colisão
            }
        }

        //Console.WriteLine("Nenhum bloco colidido.");
        blockPos = Vector3.Zero;
        return false;
    }

    // Verifica se há um bloco sólido na posição dada
    private bool IsSolidBlock(Vector3 position) {
        blockPos = new Vector3(
            (int)Math.Floor(position.X),
            (int)Math.Floor(position.Y),
            (int)Math.Floor(position.Z)
        );

        return level.IsSolidTile((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
    }
}
