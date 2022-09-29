use GestaoClientes_DB
GO

IF OBJECT_ID('stp_Post_Cliente') IS NOT NULL
   DROP PROCEDURE stp_Post_Cliente
GO

CREATE PROCEDURE stp_Post_Cliente 

 @nome varchar(255) = null,                        
 @cpf varchar(11) = null,                        
 @sexo varchar(10) = NULL,
 @id_tipo_cliente int = null,
 @id_situacao_cliente int = null
 
AS

if(@nome is null or @cpf is null)
	begin
		select 0 as id_cliente, 'Nome/CPF inválidos.' as msg ,* from status_codes where id_status = 400
	RETURN
END

if exists(SELECT 1 from cliente where cpf = @cpf)
	BEGIN
		select 0 as id_cliente, 'CPF já cadastrado.' as msg ,* from status_codes where id_status = 400
	RETURN
END

if(@id_tipo_cliente is not NULL AND not EXISTS(SELECT * from tipo_cliente where id_tipo_cliente = @id_tipo_cliente))
	begin
		select 0 as id_cliente, 'Id tipo cliente não existe.' as msg, * from status_codes where id_status = 400
	RETURN
END

if(@id_situacao_cliente is not NULL AND not EXISTS(SELECT * from situacao_cliente where id_situacao_cliente = @id_situacao_cliente))
	begin
		select 0 as id_cliente, 'Id situação cliente não existe.' as msg, * from status_codes where id_status = 400
	RETURN
END

insert INTO cliente values 
(@nome,                        
 @cpf,                        
 @sexo,
 @id_tipo_cliente,
 @id_situacao_cliente
 );

 select (SELECT id_cliente from cliente where cpf = @cpf) as id_cliente, 'Cliente inserido com sucesso' as msg, * from status_codes where id_status = 201

GO
GRANT EXECUTE ON stp_Post_Cliente TO PUBLIC
GO