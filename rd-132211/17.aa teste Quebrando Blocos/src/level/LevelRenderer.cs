namespace RubyDung;

public class LevelRenderer {
    // Referência ao nível (Level) que será renderizado
    private Level level;

    // Array de chunks que compõem o nível
    private Chunk[] chunks;

    // Número de chunks em cada eixo (x, y, z)
    private int xChunks;
    private int yChunks;
    private int zChunks;

    // Construtor da classe LevelRenderer
    public LevelRenderer(Shader shader, Level level) {
        this.level = level;

        // Calcula o número de chunks em cada eixo com base nas dimensões do nível
        // Cada chunk tem tamanho fixo de 16x16x16 blocos
        xChunks = level.width / 16;
        yChunks = level.height / 16;
        zChunks = level.depth / 16;

        // Inicializa o array de chunks com o tamanho apropriado
        chunks = new Chunk[xChunks * yChunks * zChunks];

        // Itera sobre todas as coordenadas de chunks (x, y, z)
        for(int x = 0; x < xChunks; x++) {
            for(int y = 0; y < yChunks; y++) {
                for(int z = 0; z < zChunks; z++) {
                    // Calcula as coordenadas mínimas e máximas do chunk
                    int x0 = x * 16;
                    int y0 = y * 16;
                    int z0 = z * 16;
                    
                    int x1 = (x + 1) * 16;
                    int y1 = (y + 1) * 16;
                    int z1 = (z + 1) * 16;

                    // Cria um novo chunk e o armazena no array de chunks
                    chunks[(x + y * xChunks) * zChunks + z] = new Chunk(shader, level, x0, y0, z0, x1, y1, z1);   
                }
            }
        }   
    }

    // Método chamado para carregar todos os chunks
    public void OnLoad() {
        // Itera sobre todos os chunks e chama o método OnLoad de cada um
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].OnLoad();
        }
    }

    // Método chamado para renderizar todos os chunks
    public void OnRenderFrame() {
        // Itera sobre todos os chunks e chama o método OnRenderFrame de cada um
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].OnRenderFrame();
        }
    }

    public void ChunkReloadNeighbors(int chunkX, int chunkY, int chunkZ) {
        ChunkReload(chunkX, chunkY, chunkZ);

        if(chunkX > 0) {
            ChunkReload(chunkX - 1, chunkY, chunkZ);
        }
        if(chunkX < xChunks - 1) {
            ChunkReload(chunkX + 1, chunkY, chunkZ);
        }
        if(chunkY > 0) {
            ChunkReload(chunkX, chunkY - 1, chunkZ);
        }
        if(chunkY < yChunks - 1) {
            ChunkReload(chunkX, chunkY + 1, chunkZ);
        }
        if(chunkZ > 0) {
            ChunkReload(chunkX, chunkY, chunkZ - 1);
        }
        if(chunkZ < zChunks - 1) {
            ChunkReload(chunkX, chunkY, chunkZ + 1);
        }
    }

    public void ChunkReload(int chunkX, int chunkY, int chunkZ) {
        int index = (chunkX + chunkY * xChunks) * zChunks + chunkZ;

        if(index >= 0 && index < chunks.Length) {
            chunks[index].OnLoad();
        }
    }
}
