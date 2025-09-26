-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-09-2025 a las 21:57:36
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `IdContrato` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `Monto` decimal(12,2) NOT NULL,
  `UsuarioCreadorId` int(11) DEFAULT NULL,
  `UsuarioTerminadorId` int(11) DEFAULT NULL,
  `FechaCreacion` datetime DEFAULT NULL,
  `FechaTerminacion` datetime DEFAULT NULL,
  `Terminado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `InquilinoId`, `InmuebleId`, `FechaInicio`, `FechaFin`, `Monto`, `UsuarioCreadorId`, `UsuarioTerminadorId`, `FechaCreacion`, `FechaTerminacion`, `Terminado`) VALUES
(5, 1, 2, '2025-09-11', '2026-09-10', 550000.00, NULL, NULL, NULL, NULL, 0),
(7, 2, 2, '2014-02-12', '2015-03-12', 123123.00, NULL, NULL, NULL, NULL, 0),
(9, 2, 9, '2028-01-01', '2029-01-01', 12312.00, NULL, NULL, NULL, NULL, 0),
(10, 2, 10, '2025-03-12', '2026-04-12', 15000.00, 3, NULL, '2025-09-26 01:07:26', NULL, 0),
(11, 2, 8, '2025-03-22', '2027-02-23', 1232.00, 3, 3, '2025-09-26 01:23:19', '2025-09-26 01:23:27', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `Direccion` varchar(150) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Superficie` decimal(10,2) NOT NULL,
  `Latitud` decimal(10,6) DEFAULT NULL,
  `Longitud` decimal(10,6) DEFAULT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Disponible` tinyint(1) DEFAULT 1,
  `TipoInmuebleId` int(11) DEFAULT NULL,
  `Uso` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`IdInmueble`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Disponible`, `TipoInmuebleId`, `Uso`) VALUES
(1, 'Avenida del Puerto 45, 2ºB', 3, 85.50, 39.470000, 56.000000, 1, 1, 1, 1),
(2, 'Calle Colón 28, 4ºD', 2, 65.75, 39.462056, -0.376402, 1, 1, 3, 2),
(3, 'Gran Vía Marqués del Turia 67, 1ºA', 4, 120.30, 39.470000, -0.380000, 2, 1, 6, 2),
(8, 'Direccion Modificada', 3, 45.00, 45.000000, 56.000000, 2, 1, 4, 1),
(9, 'Cambio Depuracion', 3, 45.00, 45.000000, 76.000000, 2, 1, 2, 2),
(10, 'Rivadavia 1091', 3, 85.00, 85.000000, 76.000000, 5, 0, 5, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(150) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `creado_en` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `dni`, `nombre`, `telefono`, `email`, `direccion`, `creado_en`) VALUES
(1, '345345', 'María González Pérez', '+34 612345677', 'kjasndajksnd@mail.com', 'Calle Mayor 123, 4ºB, Madrid', '2025-08-28 06:46:14'),
(2, '87654321B', 'Carlitos Rodríguez López', '911876543', 'juasdbnasd@mail.com', 'Avenida de la Constitución 45, 2ºA, Sevilla', '2025-08-28 06:46:42');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Observacion` text DEFAULT NULL,
  `UsuarioCreadorId` int(11) DEFAULT NULL,
  `UsuarioAnuladorId` int(11) DEFAULT NULL,
  `FechaCreacion` datetime DEFAULT NULL,
  `FechaAnulacion` datetime DEFAULT NULL,
  `Anulado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`IdPago`, `ContratoId`, `FechaPago`, `Monto`, `Observacion`, `UsuarioCreadorId`, `UsuarioAnuladorId`, `FechaCreacion`, `FechaAnulacion`, `Anulado`) VALUES
(2, 7, '2025-09-25', 24000.00, 'pago', NULL, NULL, NULL, NULL, 0),
(3, 9, '2025-09-26', 222.00, NULL, 3, 3, '2025-09-26 01:30:08', '2025-09-26 01:48:39', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `creado_en` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `dni`, `nombre`, `telefono`, `email`, `direccion`, `creado_en`) VALUES
(1, '453453345', 'Ana Martínez Sánchez', '+34 623456789', 'ana.martinez@propietario.com', 'Calle Sol 67, 3ºD, Valencia', '2025-08-28 07:05:43'),
(2, '40987654D', 'Javier Fernández Gómez', '345678', 'javier.fernandez@inmobiliaria.es', 'Paseo de Gracia 125, 1ºA, Barcelona', '2025-08-28 07:05:51'),
(5, '47322064', 'Huilen Olindo', '2664825042', 'huilen@gmail.com', 'Vallejos 3147', '2025-09-26 03:53:56');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tiposinmueble`
--

CREATE TABLE `tiposinmueble` (
  `IdTipoInmueble` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tiposinmueble`
--

INSERT INTO `tiposinmueble` (`IdTipoInmueble`, `Nombre`, `Descripcion`) VALUES
(1, 'Casa', 'Vivienda unifamiliar'),
(2, 'Departamento', 'Unidad dentro de un edificio'),
(3, 'Local', 'Espacio comercial'),
(4, 'Depósito', 'area de almacenamiento'),
(5, 'Oficina', 'Espacio de trabajo profesional'),
(6, 'PH', 'Propiedad horizontal'),
(7, 'Terreno', 'Lote vacio'),
(8, 'Galpón', 'Estructura industrial');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `IdUsuario` int(11) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Clave` varchar(255) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Rol` int(11) NOT NULL DEFAULT 2,
  `Avatar` varchar(255) DEFAULT NULL,
  `CreadoEn` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`IdUsuario`, `Email`, `Clave`, `Nombre`, `Rol`, `Avatar`, `CreadoEn`) VALUES
(3, 'bene@gmail.com', 'OnjOKvi1SSWrwdYwiP+eginWhBfjzRWTdKlJMgzkHrE=', 'Benenatti Santiago', 1, '/uploads/avatars/avatar_3_3860800a-ed9b-49e3-a0fc-bf34b88c3ce1.jpg', '2025-09-25 18:37:17'),
(4, 'pepe@gmail.com', 'TB1w08KbRG1PxQ7Fk0ZUCq6cPDva4dla1cx6nA1ZfjA=', 'empleado pepe', 2, NULL, '2025-09-25 18:42:21'),
(6, '2@gmail.com', 'tK/k719KBt6GxGROaxApAGCoQWXy5Hu8ekB9wPwbJak=', 'empleado 3', 2, NULL, '2025-09-26 13:11:53');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `InquilinoId` (`InquilinoId`),
  ADD KEY `InmuebleId` (`InmuebleId`),
  ADD KEY `UsuarioCreadorId` (`UsuarioCreadorId`),
  ADD KEY `UsuarioTerminadorId` (`UsuarioTerminadorId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `PropietarioId` (`PropietarioId`),
  ADD KEY `TipoInmuebleId` (`TipoInmuebleId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `ContratoId` (`ContratoId`),
  ADD KEY `UsuarioCreadorId` (`UsuarioCreadorId`),
  ADD KEY `UsuarioAnuladorId` (`UsuarioAnuladorId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tiposinmueble`
--
ALTER TABLE `tiposinmueble`
  ADD PRIMARY KEY (`IdTipoInmueble`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`IdUsuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `tiposinmueble`
--
ALTER TABLE `tiposinmueble`
  MODIFY `IdTipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`IdInmueble`),
  ADD CONSTRAINT `contratos_ibfk_3` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuarios` (`IdUsuario`),
  ADD CONSTRAINT `contratos_ibfk_4` FOREIGN KEY (`UsuarioTerminadorId`) REFERENCES `usuarios` (`IdUsuario`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`id`),
  ADD CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`TipoInmuebleId`) REFERENCES `tiposinmueble` (`IdTipoInmueble`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`IdContrato`),
  ADD CONSTRAINT `pagos_ibfk_2` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuarios` (`IdUsuario`),
  ADD CONSTRAINT `pagos_ibfk_3` FOREIGN KEY (`UsuarioAnuladorId`) REFERENCES `usuarios` (`IdUsuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
