IF DB_ID(N'VetAndesDB') IS NOT NULL
BEGIN
    ALTER DATABASE VetAndesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VetAndesDB;
END
GO

CREATE DATABASE VetAndesDB;
GO

USE VetAndesDB;
GO

CREATE TABLE Propietarios
(
    Id INT IDENTITY(1, 1) NOT NULL,
    Documento NVARCHAR(15) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Correo NVARCHAR(120) NOT NULL,
    CONSTRAINT PK_Propietarios PRIMARY KEY (Id),
    CONSTRAINT UQ_Propietarios_Documento UNIQUE (Documento)
);
GO

CREATE TABLE Pacientes
(
    Id INT IDENTITY(1, 1) NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Especie NVARCHAR(20) NOT NULL,
    Raza NVARCHAR(60) NOT NULL,
    Sexo NVARCHAR(10) NOT NULL,
    PesoKg DECIMAL(5, 2) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    PropietarioId INT NOT NULL,
    CONSTRAINT PK_Pacientes PRIMARY KEY (Id),
    CONSTRAINT FK_Pacientes_Propietarios FOREIGN KEY (PropietarioId)
        REFERENCES Propietarios (Id),
    CONSTRAINT UQ_Pacientes_Propietario_Nombre UNIQUE (PropietarioId, Nombre),
    CONSTRAINT CK_Pacientes_PesoKg CHECK (PesoKg > 0 AND PesoKg <= 200)
);
GO

INSERT INTO Propietarios (Documento, Nombre, Telefono, Correo)
VALUES
    (N'45678912', N'Lucía Fernández', N'987654321', N'lucia.fernandez@correo.com'),
    (N'12345678', N'Marco Quispe', N'912345678', N'marco.quispe@correo.com'),
    (N'87654321', N'Elena Vargas', N'998877665', N'elena.vargas@correo.com');
GO

INSERT INTO Pacientes (Nombre, Especie, Raza, Sexo, PesoKg, FechaNacimiento, PropietarioId)
VALUES
    (N'Luna', N'Perro', N'Labrador', N'Hembra', 18.50, '2021-03-14', 1),
    (N'Tom', N'Gato', N'Mestizo', N'Macho', 4.20, '2022-11-02', 1),
    (N'Kiwi', N'Ave', N'Periquito', N'Hembra', 0.08, '2024-01-20', 2),
    (N'Rocky', N'Perro', N'Pastor alemán', N'Macho', 32.00, '2019-07-08', 3),
    (N'Misha', N'Gato', N'Siamés', N'Hembra', 3.60, '2023-05-19', 3);
GO
