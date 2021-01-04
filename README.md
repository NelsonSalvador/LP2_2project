# 1º Projeto de Linguagens de Programação II

## Autoria

### Nome dos Autores

Nelson Salvador 21904295 | Vasco Duarte 21905658

### Informação de trabalho

#### Nelson Salvador

- Level1.cs
- Level2.cs
- Player.cs
- Metodo SwitchSprite no CoreGameEngine em ConsoleSprite.cs
- Relatório

#### Vasco Duarte

- Menu.cs
- Help.cs
- Indicator.cs
- CoinConfirmation.cs
- BoxConfirmation.cs
- Score.cs
- Time.cs
- Title.cs
- ReturnMenu.cs
- Diagrama UML

### [Link GitHub](...)

## Arquitetura da Solução

### Descrição da Solução

 Foi utilizado o CoreGameEngin do professor para a realização do nosso projeto,
 este Engin tem um game game loop que têm a thread principal do jogo executada
 pelo game loop e um thread para Inputs do jogador.
 É utilizado o update Method, temos no nosso jogo uma coleção de objetos que
 processam um comportamento por frame e implementamos o component pattern.

 O nosso jogo tem o menu principal, que o `Program` chama. `Menu` que a partir dele é possível jogar o nível 1 ou 2 criadas pelas respetivas classes `Level1` e `Level2`, também é possível chamar o help, `Help` que contém as instruções do jogo, ou sair dojogo. Isto é possivel com o `Indicator`, que permite de seletionar essas opçoes. O `ReturnMenu` é uma classe mais simples identica mas para o `Help`.

 A class `Score` contém o score do jogador e a classe `Player` controla as interações e o movimento do jogador en cada nivel. Os `BoxConfirmation` e `CoinConfirmation` sao iniciados para todos o blocos e moedas no niveis e servem para confirmar que uma moeda fui apanhada ou se um bloco fui usado. O `Time` tambem é iniciado a cada nivel e se chega a 0, entao o jogador volta o menu.

### UML

![Diagrama de classes](UMLProject2.png)

## Referências

 Utilizamos a API do C# muito raramente para tirarmos umas pequenas dúvidas, de
 resto não usamos mais nada para código.
 A Sprite do jogador foi é uma sprite da net de autor desconhecido.
