create database Obligatorio
go
use Obligatorio
go
create table Countries
(
	Id int PRIMARY KEY IDENTITY(1,1),
	Name varchar(10) not null
)

create table Persona
(
	Id int PRIMARY KEY,
	Nombre varchar(30) not null,
	Apellido varchar(30) not null,
	PaisId int not null,
	NombreArt varchar(30),
	Tipo varchar(1) not null,
	MinPantalla int,
	CHECK (Tipo = 'A' or Tipo = 'D'),
	FOREIGN KEY (PaisId) REFERENCES Pais(PaisId)
)

create table Genero
(
	Nombre varchar(10) PRIMARY KEY,
	Descripcion varchar(50) not null
)

create table Material
(
	CodISAN varchar(12) PRIMARY KEY,
	Titulo varchar(10) not null,
	FechaEstreno date not null,
	DirectorId int not null,
	Genero varchar(10) not null,
	PaisId int not null,
	Descripcion varchar(50),
	Imagen varchar(50),
	Tipo char(1) not null,
	FOREIGN KEY (Genero) REFERENCES Genero(Nombre),
	FOREIGN KEY (PaisId) REFERENCES Pais(PaisId),
	CHECK Tipo in ('P' or 'S')
)

create table Elenco
(
	ElencoId int,
	CodISAN varchar(12),
	PersonaId int,
	PRIMARY KEY (ElencoId, CodISAN, PersonaId),
	FOREIGN KEY (CodISAN) REFERENCES Material(CodISAN),
	FOREIGN KEY (PersonaId) REFERENCES Persona(Id)
)

create table Pelicula
(
	CodISAN varchar(12) PRIMARY KEY,
	CantEntradas int not null,
	MontoRecaudado int not null,
	Duracion int not null,
	FOREIGN KEY (CodISAN) REFERENCES Material(CodISAN)
)

create table Serie
(
	CodISAN varchar(12) PRIMARY KEY,
	FOREIGN KEY (CodISAN) REFERENCES Material(CodISAN)
)

create table Temporada
(
	CodISAN varchar(12),
	Titulo varchar(10) not null,
	NroTemporada int not null,
	FechaEstreno date not null,
	Imagen varchar(50) not null,
	FOREIGN KEY (CodISAN) REFERENCES Serie(CodISAN),
	PRIMARY KEY (CodISAN, NroTemporada)
)

create table Episodio
(
	CodISAN varchar(12),
	NroTemporada int not null,
	Ordial int not null,
	Titulo varchar(30) not null,
	Descripcion varchar(30),
	Duracion int,
	Tipo char(1) not null,
	PRIMARY KEY (CodISAN, NroTemporada, Ordial),
	FOREIGN KEY (CodISAN, NroTemporada) REFERENCES Temporada(CodISAN, NroTemporada)
)
