# rd-132211
 
Este projeto tem como objetivo recriar o Minecraft usando a linguagem C# e a biblioteca OpenTK.

Este é uma continuação da rd-131655 (Cave game tech test):
- https://github.com/Morgs6000/rd-131655/tree/main

## Ferramentas e Tecnologias
<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/vscode/vscode-original.svg" /></code> VS Code

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-original.svg" /></code> C#

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/opengl/opengl-original.svg" /></code> OpenGL

<code><img height="30" src="https://avatars.githubusercontent.com/u/5914736?s=280&v=4" /></code> OpenTK

<code><img height="30" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/nuget/nuget-original.svg" /></code> StbImageSharp

## Adições
### Geral
Controles
- Colocação e destruição de blocos.
  - Clicar com o botão esquerdo coloca um bloco. Clicar com o botão direito do mouse destrói um bloco.
  - Os blocos não podem ser colocados além da borda existente.
  - Os jogadores podem colocar blocos no espaço que estão ocupando.
  - Adicionada uma sobreposição branca piscando que é exibida na lateral de um bloco sobre o qual o jogador passa o mouse.
  - Blocos no mesmo nível do bloco de grama se transformam em blocos de grama e blocos no mesmo nível da pedra ou do ar se transformam em pedra.
- O nível pode ser salvo pressionando <code><img height="30" src="https://github.com/user-attachments/assets/2a2af294-5b89-4b3c-9c0d-d7885ca8fb98" /></code>.
  - Ele também salva ao fechar o jogo.

## Bugs
### 1 bug reportado
- Os blocos podem ser colocados dentro do espaço em que o jogador está atualmente. Este bug permaneceu no jogo até o Classic 0.0.9a.

## Curiosiades
- O "rd" antes do número da versão old_alpha rd-132211 significa RubyDung, um jogo em que Notch estava trabalhando antes do Minecraft, cuja base de código foi posteriormente reutilizada para o Minecraft.
- Segurar <code><img height="30" src="https://github.com/user-attachments/assets/e49e3d70-d887-45ef-856c-13bc9d837166" /></code> faz com que o jogador percorra rapidamente os locais de reaparecimento acima do nível até que seja liberado.
- Os dados de nível são salvos em um único arquivo **level.dat** dentro da pasta do iniciador, ao contrário das versões futuras que possuem pastas dedicadas.
- O jogador sempre spawna em y74.

## Referencias
- https://minecraft.wiki/w/Java_Edition_pre-Classic_rd-132211

## To-Do
- ⚠ Raycast
- ⚠ Highlight
- ⚠ Quebrar blocos
- ❌ Highlight apenas na lateral do bloco que a camera esta apontando
- ❌ Colocar blocos
- ✅ Salvar o jogo

## Progresso
### Sobreposição branca piscante em todos os lados do bloco
![Screenshot_463](https://github.com/user-attachments/assets/d8364637-1cce-4d95-b174-935f5b17f7a7)
![Screenshot_465](https://github.com/user-attachments/assets/7163710d-a104-4724-9045-645f21e086d6)
![Screenshot_464](https://github.com/user-attachments/assets/97aa7a89-d669-4bc5-b3fd-dec5f9ebad02)
![Screenshot_466](https://github.com/user-attachments/assets/d2282681-0674-48de-8e7a-62fb38ae2038)
