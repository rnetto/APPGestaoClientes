
# APPGestaoClientes
<h1>Bem vindo ao APP- Gestão de Clientes</h1>

<h3>Instruções para Build do App</h3>

  <h4><b>--></b> Criação do Banco de dados <i>GestaoClientes_DB</i> no <b>SQL SERVER</b></h4>

<div id="div1">
    - Na Camada de serviço <i>GestaoClientes.Services</i> a pasta <i>Scripts_gestao_cliente_DB</i> estão todos os SCRIPTs de criação do DB.<br/>
    - <i>DB_clientes.sql</i> - script de criação do DB. <br/>
    - <i>INSERT 1900 - clientes.sql</i> - script para gerar registros de cliente automáticos no DB. <br/>
    - <i>stp_Delete_Clientes</i> - script para criar a PROCEDURE de apagar cliente. <br/>
    - <i>stp_Get_Clientes</i> - script para criar a PROCEDURE de busca/listagem. <br/>
    - <i>stp_Post_Clientes</i> - script para criar a PROCEDURE de inserir clientes. <br/>
    - <i>stp_Put_Clientes</i> - script para criar a PROCEDURE de Editar clientes. <br/><br/>
    <img src="https://user-images.githubusercontent.com/81520077/193058095-664e7b59-ffc8-4f1f-a8a8-e03096631639.jpg"/>   
<br/>
</div>

  <h4><b>--></b> Configuração do BUILD do app <i>APPGestaoClientes</i> no <b>Visual Studio</b></h4>
  
 <div id="div1">
    - Selecionar a Solução 'AppGestaoClientes'. <br/>
    - Acessar as propriedades da solução (Botão direito > Propriedades ou Alt+Enter). <br/>
    - Selecione a opção <i>Vários projetos de inicialização:</i><br/>
    - Selecione o <i>Projeto - APIGestaoClientes</i> - altere a ação para - <i>Ação - Iniciar</i>. <br/>
    - Selecione o <i>Projeto - GestaoClientesUi</i> - altere a ação para - <i>Ação - Iniciar</i>. <br/>
    - Selecione o botão <i>Aplicar</i><br/>
    - Selecione o botão <i>Ok</i><br/><br/>
     <img src="https://user-images.githubusercontent.com/81520077/193060353-a307e709-0269-46ea-b625-d15b0f6518fe.jpg"/>

 </div>
