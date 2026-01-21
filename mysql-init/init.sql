-- Crear base de datos
CREATE DATABASE IF NOT EXISTS apidb;
USE apidb;

-- Tabla para registrar logs de la API
CREATE TABLE IF NOT EXISTS Logs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Endpoint VARCHAR(100) NOT NULL,
    CalledAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    Message VARCHAR(255) NOT NULL
);

-- Opcional: tabla de usuarios locales (si quieres guardar copia de la API externa)
CREATE TABLE IF NOT EXISTS Users (
    Id INT PRIMARY KEY,
    Name VARCHAR(100),
    Email VARCHAR(100)
);

-- Opcional: tabla de tareas locales
CREATE TABLE IF NOT EXISTS Tasks (
    Id INT PRIMARY KEY,
    UserId INT,
    Title VARCHAR(255),
    Completed BOOLEAN,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
