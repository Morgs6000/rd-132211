# rd-132211
 
Este projeto tem como objetivo recirar o Minecraft usando a linguagem C# e a biblioteca OpenTK.
Vamos começar do começo, começando pela primeira versão do Minecraft, a rd-132211.

## Ferramentas e Tecnologias
<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/vscode/vscode-original.svg" /></code> VS Code

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-original.svg" /></code> C#

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/opengl/opengl-original.svg" /></code> OpenGL

<code><img height="30" src="https://avatars.githubusercontent.com/u/5914736?s=280&v=4" /></code> OpenTK

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/nuget/nuget-original.svg" /></code> StbImageSharp

## Recursos
### Blocos
<code><img height="30" src="https://github.com/user-attachments/assets/d614ae6c-69ef-41cd-af2e-4e4addff1e2e" /></code> Ar
- Não listado como um bloco no momento.

<code><img height="30" src="https://github.com/user-attachments/assets/23c120f7-7c37-4ff8-b1d6-e8d382cc78ce" /></code> Bloco de Grama

<code><img height="30" src="https://github.com/user-attachments/assets/ea7acd46-3658-4a10-a112-2886606729d1" /></code> Pedra
- Sua textura nessa época seria posteriormente reaproveitada para os Pedregulhos.

### Entidades não-mob
Jogador
- Atualmente não tem modelo.
- Tem uma altura de 1,8 blocos.

### Geração do mundo
Chunks
- Leva cerca de 0,1 segundo para gerar.
- O tamanho de cada blcoo é de 16x16 blocos.
  - Os blocos são de 16x16 blocos em vez de 8x8 porque 8x8 blocos diminuiria o desempenho.
- Os chunks são carregados em ordem de proximidade com o jogador.
- O jogador aparece em um mapa de 256 x 64 x 256 blocos.
  - O jogo leva 20 segundos para gerar um mapa de 256 x 64 x 256 blocos.
- Era possivel cair do mundo, mas não mataria o jogador.
- A geração de niveis era completamente plana, o que era semelhante a um mundo superplano.
- O mundo gerar é um cuboid de 256 x 42 x 256 blocos, em altura máxima de y64.
- O mundo está cheio de Pedra até y41, apenas grama em y42 e nada para parte restante.

### Geral
Luz
- O mecanismo de iluminação em Classic e Pre-Classic era simples, com apenas 2 niveis de luz, claro e escuro.
  - "Luz solar" é emitida pela borda superior do mapa e atinge qualquer bloco que esteja sob ela, independentemente da distância. Ele passa por blocos transparentes para blocos de luz por baixo.
  - Os blocos que não recebem luz estão em uma sombra fraca que permanece no mesmo nível de brilho, não importa o quão longe estejam de uma fonte de luz.
  - Os blocos escurecidos também têm uma camada de névoa preta espessa aplicada a eles, parecendo mais escuros quando vistos de distâncias maiores. Isso causou falhas visuais estranhas.

Modo criativo
- Esta foi uma versão extremamente básica dele. O jogador não podia voar e não havia inventário ou barra de atalho para obtê-los.

World Spawn
- O jogador poderia reaparecer pressionando <code><img height="30" src="https://github.com/user-attachments/assets/e49e3d70-d887-45ef-856c-13bc9d837166" /></code>, teletransportando-os para o mundo em que estavam.

Controles
- Colocação e destruição de blocos.
  - Clicar com o botão esquerdo coloca um bloco. Clicar com o botão direito destrói um bloco.[
  - Os blocos não podem ser colocados além da borda existente.
  - Os jogadores podem colocar blocos no espaço que estão ocupando.
  - Adicionada uma sobreposição branca piscando que é exibida na lateral de um bloco sobre o qual o jogador passa o mouse.
  - Blocos no mesmo nível do bloco de grama se transformam em blocos de grama e blocos no mesmo nível da pedra ou do ar se transformam em pedra.
- O nível pode ser salvo pressionando <code><img height="30" src="https://github.com/user-attachments/assets/c41f8725-0ef0-4386-8dfc-e9e968d18ac8" /></code>.
  - Ele também salva ao fechar o jogo.

## Bugs
### 1 bug reportado
- Os blocos podem ser colocados dentro do espaço em que o jogador está atualmente. Este bug permaneceu no jogo até o Classic 0.0.9a.

## Curiosiades
- O "rd" antes do número da versão old_alpha rd-132211 significa RubyDung, um jogo em que Notch estava trabalhando antes do Minecraft, cuja base de código foi posteriormente reutilizada para o Minecraft.
- Segurar <code><img height="30" src="https://github.com/user-attachments/assets/e49e3d70-d887-45ef-856c-13bc9d837166" /></code> faz com que o jogador percorra rapidamente os locais de reaparecimento acima do nível até que seja liberado.
- Os dados de nível são salvos em um único arquivo **level.dat** dentro da pasta do iniciador, ao contrário das versões futuras que possuem pastas dedicadas.
- O jogador sempre spawna em y74.

## To-Do
- ✅ Gerar uma Janela
- ✅ Gerar um Triangulo
- ✅ Gerando um Shader
- ✅ Gerando um Retangulo com dois triangulos
- ✅ Gerando uma Textura usando um Texture Atlas
- ✅ Recortando um Tile do Texture Atlas
- ✅ Gerando um Icone de Janela
- ✅ Gerando uma Camera
- ✅ Movimentando a camera com as teclada W, A, S, D, Space e LeftShif
- ✅ Rotacioando a camera com o mouse
- ✅ Gerando um Bloco
- ✅ Gerando um Chunk de 16 x 16 x 16 blocos
- ✅ Apagando faces não visiveis entre os blocos do chunk
- ✅ Definindo a posição inicial do jogador e respawn com a tecla R
- ✅ Aplicando cor para gerar uma iluminação/sombra primitiva
- ✅ Gerando camadas de pedra, grama e ar
- ✅ Gerando um Mundo de 256 x 64 x 256 blocos
- ⚠ Gerando um colisor AABB
- ⚠ Gravidade e pulo
- ⚠ Raycast
- ⚠ Highlight
- ⚠ Quebrar blocos
- ❌ Highlight apenas na lateral do bloco que a camera esta apontando
- ❌ Colocar blocos
- ✅ Salvar o jogo

## Progresso
### Gerando uma Janela
![Image](https://github.com/user-attachments/assets/7966c19c-859a-4bce-8f00-e0bf1c0dbe00)

### Gerando um Triangulo
![Image](https://github.com/user-attachments/assets/25c2726f-58b1-4294-a6d2-9634456b92aa)

### Gerando um Retangulo
![Image](https://github.com/user-attachments/assets/291910d8-603b-4fd0-895f-f97ef3c7c428)

### Wireframe
![Image](https://github.com/user-attachments/assets/b2a633a6-334c-411b-ab54-565fab955978)

### Gerando uma Textura
![Image](https://github.com/user-attachments/assets/15e6549f-d3c0-481d-a1e1-9ff0a81047e7)

### Recortando um Tile da Textura
![Image](https://github.com/user-attachments/assets/116071de-dba1-4158-b410-80905653f143)

### Gerando um Bloco
![Image](https://github.com/user-attachments/assets/ec0bbdd3-8940-432f-b8b7-d9676fdf3c65)
![Image](https://github.com/user-attachments/assets/90100421-fd1d-4d8d-a4e4-12e5b259a846)

### Gerando uma Chunk
![Image](https://github.com/user-attachments/assets/1177d147-6aa7-491a-93ac-f25c1edf9fa7)
![Image](https://github.com/user-attachments/assets/b46e172d-f2ae-48bb-b339-3d41d6cb6d0d)
![Image](https://github.com/user-attachments/assets/f4d19992-60ff-4b56-b4b3-df31852fb696)

### Gerando Camadas
![Image](https://github.com/user-attachments/assets/b077e98c-ebbc-4191-93ea-2ab3ce3ca331)

### Gerando um Mundo
![Image](https://github.com/user-attachments/assets/6088ae86-0d36-44f9-b07d-00e1b9570e8c)
