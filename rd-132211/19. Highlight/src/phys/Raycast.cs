using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RubyDung;

public class Raycast {
    private Level level;
    private Player player;

    private Shader shader;
    private Tesselator t;
    private HitResult hitResult = null;

    public Raycast(Level level, Player player) {
        this.level = level;
        this.player = player;

        // Cria uma instância do shader, carregando os arquivos de vertex e fragment shader
        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        t = new Tesselator(shader);

        hitResult = new HitResult(0, 0, 0);
    }

    public void OnUpdateFrame() {
        if(Target()) {
            RenderHit(hitResult);
        }
        else {
            t.Init();
        }
    }

    public void OnRenderFrame(Vector2i clientSize) {
        shader.OnRenderFrame();

        t.OnRenderFrame();

        // Cria a matriz de visualização (view) a partir da posição e orientação do jogador
        Matrix4 view = Matrix4.Identity;
        view *= player.LookAt();
        shader.SetMatrix4("view", view); // Passa a matriz de visualização para o shader

        // Cria a matriz de projeção em perspectiva a partir do tamanho da janela
        Matrix4 projection = Matrix4.Identity;
        projection *= player.CreatePerspectiveFieldOfView(clientSize);
        shader.SetMatrix4("projection", projection); // Passa a matriz de projeção para o shader
    }

    private void RenderHit(HitResult h) {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin((double)Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f, alpha);

        t.Init();
        Tile.rock.RenderFace(t, h.x, h.y, h.z);
        t.OnLoad();

        //GL.Disable(EnableCap.Blend);
    }

    private bool Target() {
        // Posição e direção da camera
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        // Tamanho do passo para o raycasting
        float step = 0.1f;
        float maxDistance = 5.0f; // Distancia maxima para verificar colisões

        Vector3 currentPos = rayOrigin;

        for(float distante = 0; distante < maxDistance; distante += step) {
            currentPos += rayDirection * step;

            hitResult.x = (int)Math.Floor(currentPos.X);
            hitResult.y = (int)Math.Floor(currentPos.Y);
            hitResult.z = (int)Math.Floor(currentPos.Z);

            // Verifica se há um bloco solido nessa posição
            if(level.IsSolidTile(hitResult.x, hitResult.y, hitResult.z)) {
                return true;
            }
        } 

        return false;
    }
}
