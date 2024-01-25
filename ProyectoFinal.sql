use master;
go

if exists (select * from Sysdatabases where name='ProyectoFinal')
begin
	drop database ProyectoFinal
end

----------------------------------------------------------------------------------
create database ProyectoFinal
go
----------------------------------------------------------------------------------

use ProyectoFinal

CREATE TABLE Pais(
	  CodPais varchar(3) PRIMARY KEY  NOT NULL,
	  Nombre VARCHAR(30) NOT NULL
	)
go

CREATE TABLE Ciudad(
	  CodCiudad VARCHAR(3) NOT NULL,
	  Nombre VARCHAR(30) NOT NULL,
	  CodPais VARCHAR(3) foreign key references Pais (CodPais) NOT NULL,
	  PRIMARY KEY (CodCiudad, CodPais)
	)
go

CREATE TABLE Usuario(
	  Usuario varchar(30) PRIMARY KEY NOT NULL,
	  Passw VARCHAR(30) NOT NULL,
	  Nombre VARCHAR (30)
	)
go

CREATE TABLE Pronostico(
	  Interno INT PRIMARY KEY IDENTITY  NOT NULL,
	  FechaYHora DATETIME,
	  TempMax INT,
	  TempMin INT,
	  Viento INT,
	  ProbLluvia INT,
	  TipoCielo VARCHAR(30),
	  Usuario VARCHAR(30) NOT NULL,
	  CodCiudad VARCHAR(3) NOT NULL,
	  CodPais VARCHAR (3) NOT NULL
	  FOREIGN KEY (Usuario) REFERENCES Usuario (Usuario),
	  FOREIGN KEY (CodCiudad, CodPais) REFERENCES Ciudad (CodCiudad, CodPais)
	)
go

insert into Pais values ('URU', 'Uruguay')
insert into Pais values ('ARG', 'Argentina')
insert into Pais values ('BRA', 'Brasil')
go

insert into Ciudad values ('MVO','Montevideo', 'URU')
insert into Ciudad values ('DUR','Durazno', 'URU')
insert into Ciudad values ('BSA','Buenos Aires', 'ARG')
insert into Ciudad values ('RSO','Rosario', 'ARG')
insert into Ciudad values ('RIO','Rio de Janeiro', 'BRA')
go

insert into Usuario values ('juanro', '123456789', 'Juan Rodriguez')
insert into Usuario values ('mariago', '987654321', 'Maria Gonzalez')
insert into Usuario values ('carlosgo', '01234567', 'Carlos Gonzalez')
go

insert into Pronostico values ('20210815 22:00', 23, 15, 25, 30, 'Despejado', 'juanro', 'MVO', 'URU')
insert into Pronostico values ('20220110 15:00', 30, 21, 16, 00, 'Parcialmente nuboso', 'mariago', 'BSA', 'ARG')
insert into Pronostico values ('20220115 08:00', 35, 26, 34, 15, 'Nuboso', 'carlosgo', 'RIO', 'BRA')
go






---------------------------PROCEDIMIENTOS PAISES

create PROC BuscarPais
@CodPais varchar(3)
AS
BEGIN
	Select CodPais, Nombre
	From Pais
	where CodPais=@CodPais
END
GO

--exec BuscarPais 'URU'
--GO



create PROC AgregarPais
@CodPais varchar(3) ,
@Nombre varchar(30)  
AS
BEGIN 
	
	if EXISTS (select * from Pais where CodPais=@CodPais) --si ya existe un pais con ese codigo
	BEGIN
		return -2
	END

	if LEN(@codPais)<>3  --si el cod pais no es de 3 letras
		BEGIN
			return -3
		END

	DECLARE @Error int

	INSERT Pais( CodPais ,Nombre) VALUES( @CodPais ,@Nombre );
	SET @Error=@@ERROR;

	IF(@Error=0) --se agrego correctamente
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN 0;
		END	
END
go

--declare @resul varchar(20)
--exec @resul=AgregarPais 'PAR', 'Paraguay'
--select @resul
--GO


create PROC ModificarPais
@codPais varchar (3),
@nombre varchar (30)
AS
BEGIN

	if NOT EXISTS (select * from Pais where CodPais=@codPais) --si no existe el pais a modificar
	BEGIN
		return -2
	END

	UPDATE Pais set Nombre=@nombre
	where CodPais=@codPais
	if @@ERROR <> 0
		BEGIN
			return -1
		END
	else
		BEGIN
			return 1
		END
END
go

--declare @resul varchar(20)
--exec @resul=ModificarPais 'PAR', 'Bolivia'
--select @resul
--GO


CREATE PROC EliminarPais
@codPais varchar (3)
AS
BEGIN

	if NOT EXISTS (select * from Pais where CodPais=@codPais) --si no existe el pais a eliminar
	BEGIN
		return -2
	END

	if EXISTS (select * from Pronostico where CodPais=@codPais) --si tiene pronosticos asociados no se elimina
	BEGIN
		return -3
	END

	declare @Error int

	BEGIN TRAN

		DELETE Pronostico
		from Pronostico
		WHERE CodPais =@codPais
		SET @Error=@@ERROR;

		DELETE Ciudad
		from Ciudad
		where CodPais=@codPais
		SET @Error=@@ERROR+@Error;

		DELETE Pais
		from Pais
		where CodPais=@codPais
		SET @Error=@@ERROR+@Error

		IF(@Error=0) --se elimino correctamente
			BEGIN
				COMMIT TRAN;
				RETURN 1;
			END
		ELSE
			BEGIN
				ROLLBACK TRAN;
				RETURN -1;
			END	
END
GO

--declare @resul varchar(20)
--exec @resul=EliminarPais 'BOL'
--select @resul
--GO

create PROC ListarPaises
AS
BEGIN
	Select *
	FROM Pais
END
go

--exec ListarPaises
--GO


-------------------------------PROCEDMIENTOS CIUDADES

create PROC BuscarCiudad
@CodPais varchar(3),
@CodCiudad varchar(3)
AS
BEGIN
	Select CodCiudad, CodPais, Nombre
	From Ciudad
	where CodPais=@CodPais AND CodCiudad=@CodCiudad
END
GO

--exec BuscarCiudad 'URU', 'MVO'
--GO


create PROC AgregarCiudad
@CodCiudad varchar(3),
@CodPais varchar(3),
@Nombre varchar(30)  
AS
BEGIN 
	DECLARE @Error int
	
	if NOT EXISTS (select * from Pais where CodPais=@CodPais) --si no existe el pais al que se quiere agregar la ciudad
		BEGIN
			return -1
		END

	if EXISTS (select * from Ciudad where CodPais=@CodPais and CodCiudad=@CodCiudad) --si ya existe una ciudad con esos identificadores
		BEGIN
			return -2
		END

	if LEN(@CodPais)<>3 --si cod pais no es de 3 caracteres
		BEGIN
			return -3
		END

	if LEN(@CodCiudad)<>3 --si cod ciudad no es de 3 caracteres
		BEGIN
			return -4
		END

	INSERT Ciudad(CodCiudad, CodPais ,Nombre) VALUES(@CodCiudad, @CodPais ,@Nombre );
	SET @Error=@@ERROR;

	IF(@Error=0) --se agrego correctamente
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN 0;
		END	
END
go

--declare @resul varchar(20)
--exec @resul=AgregarCiudad 'MAL', 'URU', 'Maldonado'
--select @resul
--GO


create PROC ModificarCiudad
@codPais varchar (3),
@codCiudad varchar(3),
@nombre varchar (30)
AS
BEGIN

	if NOT EXISTS (select * from Ciudad where CodPais=@CodPais and CodCiudad=@codCiudad)--si no existe la ciudad a modificar
		BEGIN
			return -2
		END

	UPDATE Ciudad set Nombre=@nombre
	where CodPais=@codPais and CodCiudad=@codCiudad

	if @@ERROR <> 0
		BEGIN
			return -1
		END
	else
		BEGIN
			return 1
		END
END
go

--declare @resul varchar(20)
--exec @resul=ModificarCiudad 'URU', 'MAL', 'PAY', 'Paysandu'
--select @resul
--GO

create PROC EliminarCiudad
@codCiudad varchar(3),
@codPais varchar (3)
AS
BEGIN
	
	if NOT EXISTS (select * from Ciudad where CodPais=@CodPais and CodCiudad=@codCiudad)--si no existe la ciudad a eliminar
		BEGIN
			return -2
		END

	declare @Error int

	BEGIN TRAN
		
		DELETE Pronostico
		WHERE CodPais =@codPais and CodCiudad=@codCiudad
		SET @Error=@@ERROR;

		DELETE Ciudad
		where CodPais=@codPais and CodCiudad=@codCiudad
		SET @Error=@@ERROR+@Error;

		IF(@Error=0) --se elimino correctamente
			BEGIN
				COMMIT TRAN;
				RETURN 1;
			END
		ELSE
			BEGIN
				ROLLBACK TRAN;
				RETURN -1;
			END	
END
GO

--declare @resul varchar(20)
--exec @resul=EliminarCiudad 'MAL', 'URU'
--select @resul
--GO

create PROC ListarCiudadesPorPais
@codPais varchar(3)
AS
BEGIN
	Select *
	FROM Ciudad
	where CodPais=@codPais
END
go

--exec ListarCiudadesPorPais 'URU'
--GO

-------------------------------PROCEDIMIENTOS USUARIOS

create PROC BuscarUsuario
@Usuario varchar(30)
AS
BEGIN
	Select Usuario, Passw, Nombre
	From Usuario
	where Usuario=@Usuario
END
GO

--exec BuscarUsuario 'juanro'
--GO

CREATE PROC AgregarUsuario
@Usuario varchar(30),
@Password varchar(30),
@Nombre varchar(30)
AS
BEGIN 

	if EXISTS (select * from Usuario where Usuario=@Usuario)--si ya existe ese usuario
		BEGIN
			return -2
		END

	DECLARE @Error int

	INSERT Usuario(Usuario, Passw, Nombre) VALUES(@Usuario, @Password ,@Nombre );
	SET @Error=@@ERROR;

	IF(@Error=0) --se agrego correctamente
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN 0;
		END	
END
go

--declare @resul varchar(20)
--exec @resul=AgregarUsuario 'pepelop', '123456', 'Jose Lopez'
--select @resul
--GO


create PROC ModificarUsuario
@usuario varchar(30),
@password varchar(30),
@nombre varchar (30)
AS
BEGIN

	if NOT EXISTS (select * from Usuario where Usuario=@usuario) --si no existe el usuario a modificar
		BEGIN
			return -2
		END

	UPDATE Usuario set Passw=@password, Nombre=@nombre
	where Usuario=@usuario
	if @@ERROR <> 0
		BEGIN
			return -1
		END
	else
		BEGIN
			return 1
		END
END
go

--declare @resul varchar(20)
--exec @resul= ModificarUsuario 'pepelop', '123456', 'Facundo Gonzalez'
--select @resul
--GO



CREATE PROCEDURE BorrarUsuario
@usuario varchar (30)
AS
BEGIN

	IF EXISTS (SELECT Interno FROM Pronostico WHERE Usuario=@usuario)--si existe un pronostico asociado al usuario
		BEGIN
			RETURN -2;
		END

	if NOT EXISTS (select * from Usuario where Usuario=@usuario) --si no existe el usuario a modificar
		BEGIN
			return -3
		END

	DECLARE @Error int

	DELETE Usuario WHERE Usuario=@usuario
	SET @Error=@@ERROR

		IF(@Error=0) --se borro correctamente
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN -1;
		END	
END
GO

--declare @resul varchar(20)
--exec @resul=BorrarUsuario 'facugon'
--select @resul
--GO


----------------------------------PROCEDIMIENTOS PRONOSTICOS

create PROC AgregarPronostico
@fechaYhora DATETIME,
@TempMax INT,
@tempMin INT,
@viento INT,
@probLluvia INT,
@tipoCielo VARCHAR(30),
@usuario VARCHAR(30),
@CodCiudad varchar(3),
@CodPais varchar(3) 
AS
BEGIN 
	DECLARE @Error int
	
	if exists (select * from Pronostico where FechaYHora=@fechaYhora and CodCiudad=@CodCiudad and CodPais=@CodPais)
		BEGIN
			return -4
		END

	if NOT EXISTS (select * from Ciudad where CodCiudad=@CodCiudad AND CodPais=@CodPais)--si no existe la ciudad para el pronostico
		BEGIN
			return -2
		END

	if NOT EXISTS (select * from Usuario where Usuario=@usuario) --si no existe el usuario
		BEGIN
			return -3
		END

	INSERT Pronostico(FechaYHora, TempMax, TempMin, Viento, ProbLluvia, TipoCielo, Usuario, CodCiudad, CodPais) 
	VALUES(@fechaYhora, @TempMax, @tempMin, @viento, @probLluvia, @tipoCielo, @usuario, @CodCiudad, @CodPais);

	SET @Error=@@ERROR;

	IF(@Error=0) --se agrego correctamente
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN -1;
		END	
END
go

--declare @resul varchar(20)
--exec @resul=AgregarPronostico '20210819 22:00', 20, 14, 34, 50, 'despejado', 'juanro', 'MVO', 'URU' 
--select @resul
--GO




create  PROCEDURE ListarPronosticosPorDia
@fecha DateTIME
AS
BEGIN 
	SELECT Interno, FechaYHora, TempMax, TempMin, Viento, ProbLluvia, TipoCielo, Usuario, CodCiudad, CodPais
	FROM Pronostico
	WHERE DAY(FechaYHora)=DAY(@fecha) AND MONTH(FechaYHora)=MONTH(@fecha) AND YEAR(FechaYHora)=YEAR(@fecha)
END
go

--exec ListarPronosticosPorDia '2021-08-15'
--GO


CREATE  PROCEDURE ListarPronosticosCiudad
@codPais varchar (3),
@codCiudad varchar (3)
AS
BEGIN 
	SELECT Interno, FechaYHora, TempMax, TempMin, Viento, ProbLluvia, TipoCielo, Usuario, CodCiudad, CodPais
	FROM Pronostico
	WHERE CodCiudad=@codCiudad AND CodPais=@codPais
END
go

--exec ListarPronosticosCiudad 'URU', 'MVO'
--GO


CREATE PROC ListarPronosticos
AS
BEGIN
	Select *
	FROM Pronostico
END
go

--exec ListarPronosticos
--GO