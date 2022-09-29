--DROP DATABASE GestaoClientes_DB
--GO

CREATE DATABASE GestaoClientes_DB
GO

use GestaoClientes_DB
GO

CREATE TABLE situacao_cliente
(
    id_situacao_cliente  int primary key not null,
    descricao_situacao_cliente varchar(10) not null	
);
GO

CREATE TABLE tipo_cliente
(
    id_tipo_cliente int  primary key not null,
    descricao_tipo_cliente varchar(20) not null	
);
GO

CREATE TABLE cliente
(
    id_cliente int Identity (1,1) primary key not null,
    nome varchar(255) not null,
	cpf varchar(11) not null,
	sexo varchar(10),
	id_tipo_cliente int null,
	id_situacao_cliente int	null
);
GO

create table status_codes
(
	id_status int primary key not null,
	status_code varchar(20) not null
);
GO

alter table cliente add constraint FK_id_tipo_cliente foreign key (id_tipo_cliente)
references tipo_cliente (id_tipo_cliente) 
on delete set null on update cascade
GO

alter table cliente add constraint FK_id_situacao_cliente foreign key (id_situacao_cliente)
references situacao_cliente (id_situacao_cliente)
on delete set null on update cascade
GO

--Tipo Cliente--
insert INTO tipo_cliente VALUES(0, 'Selecione');
insert INTO tipo_cliente VALUES(1, 'Pragmático');
insert INTO tipo_cliente VALUES(2, 'Analítico');
insert INTO tipo_cliente VALUES(3, 'Afável');
insert INTO tipo_cliente VALUES(4, 'Expressivo');
GO

--Situacao Cliente--
insert INTO situacao_cliente VALUES(0, 'Selecione');
insert INTO situacao_cliente VALUES(1, 'Ativo');
insert INTO situacao_cliente VALUES(2, 'Inativo');
GO

--Status Code--
insert INTO status_codes values (200, 'OK');
insert INTO status_codes values (201, 'Created');
insert INTO status_codes values (204, 'No Content');
insert INTO status_codes values (400, 'Bad Request');
insert INTO status_codes values (404, 'Not Found');
GO
