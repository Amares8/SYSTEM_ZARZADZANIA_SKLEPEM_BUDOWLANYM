-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 30 Sty 2023, 10:35
-- Wersja serwera: 10.4.27-MariaDB
-- Wersja PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `obi`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `customers`
--

CREATE TABLE `customers` (
  `customerID` int(11) NOT NULL,
  `firstName` varchar(50) NOT NULL,
  `lastName` varchar(50) NOT NULL,
  `phoneNr` varchar(50) NOT NULL,
  `city` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Zrzut danych tabeli `customers`
--

INSERT INTO `customers` (`customerID`, `firstName`, `lastName`, `phoneNr`, `city`, `email`) VALUES
(6, 'Amadeusz', 'Reszke', '533690219', 'Kazimierz', 'amadi2003@op.pl'),
(7, 'Muhammad', 'ZainDin', '123456789', 'Amman', 'muchomor@wp.pl'),
(8, 'Patrycja', 'Wierkin', '795875449', 'Gdańsk', 'dwier@onet.pl'),
(9, 'Tomasz', 'Wysocki', '221252345', 'Gdynia', 'tomeczek123@mail.com'),
(10, 'Róża', 'Wojtkiewicz', '123456789', 'Pruszcz Gdański', 'roza@email.com');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `employees`
--

CREATE TABLE `employees` (
  `employeeID` int(11) NOT NULL,
  `login` varchar(50) NOT NULL,
  `firstName` varchar(50) NOT NULL,
  `lastName` varchar(50) NOT NULL,
  `jobTitle` varchar(50) NOT NULL,
  `password` varchar(256) NOT NULL,
  `accessLevel` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Zrzut danych tabeli `employees`
--

INSERT INTO `employees` (`employeeID`, `login`, `firstName`, `lastName`, `jobTitle`, `password`, `accessLevel`) VALUES
(1, 'agnswi8', 'Agnieszka', 'Świątek-Brzezińska', 'Accountant', '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', 1),
(5, 'chrrod8', 'Christian', 'Rodolfo', 'Supervisor', '9EF836C161A1A9F5BCC5746795A058CE976190680E51EEA128558EBE4D66BE36', 2),
(6, 'amares8', 'Amadeusz', 'Reszke', 'IT', '9EF836C161A1A9F5BCC5746795A058CE976190680E51EEA128558EBE4D66BE36', 3),
(7, 'tomwys8', 'Tomasz', 'Wysocki', 'HR', '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', 2);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `products`
--

CREATE TABLE `products` (
  `productID` int(11) NOT NULL,
  `productName` varchar(50) NOT NULL,
  `productVendor` varchar(50) NOT NULL,
  `productDescription` varchar(50) NOT NULL,
  `quantityInStock` int(11) NOT NULL,
  `sellingPrice` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Zrzut danych tabeli `products`
--

INSERT INTO `products` (`productID`, `productName`, `productVendor`, `productDescription`, `quantityInStock`, `sellingPrice`) VALUES
(1001, 'Cordless Drill', 'Craftsman', 'Battery Powered 20 Volts', 0, 490),
(1002, 'Blackout Curtain', 'Sanson', 'Tape 140x260 cm navy blue', 642, 70),
(1003, 'Kitchen Faucet', 'WEWE', '‎Heavy-Duty Metal and High Temperature', 820, 80),
(1004, 'Ironing Board', 'Happhom', 'Alloy Steel', 1029, 150),
(1005, 'Quilt Set', 'Permafresh', 'Queen Size 86 x 86 x 1 inches', 255, 340),
(1006, 'Box Fan', 'SONBION', '120V to 5V AC adapter and DC brushless motor', 307, 10000),
(1007, 'Arm Chair', 'Christopher', 'Alisa Mid Century Modern Fabric', 86, 890),
(1008, 'Table Wood Light', 'Safavieh', '13\"D x 17\"W x 23.5\"H', 724, 640),
(1009, 'Thick Area Rug', 'Safavieh', 'Pro Luxe Collection 9 x 12 Blue/Cream Moroccan', 443, 230),
(1010, 'Bath Towels Set', 'Utopia', 'Premium 600 GSM 100% Ring Spun Cotton', 99, 1120),
(1012, 'Steel Wheelbarrow', 'Garden and Friends', '150l steel 16inch wheel', 0, 20000),
(1013, 'Fire Extinguisher', 'Castorama', '20 kg red categoty C 1 year', 0, 10000);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `purchases`
--

CREATE TABLE `purchases` (
  `id` int(11) NOT NULL,
  `customerID` int(11) NOT NULL,
  `employeeID` int(11) NOT NULL,
  `productID` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `transactionID` int(11) NOT NULL,
  `date` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Zrzut danych tabeli `purchases`
--

INSERT INTO `purchases` (`id`, `customerID`, `employeeID`, `productID`, `amount`, `transactionID`, `date`) VALUES
(23, 7, 6, 1006, 10, 7, '2023-01-29'),
(24, 7, 6, 1001, 4, 8, '2023-01-29'),
(25, 7, 6, 1005, 23, 8, '2023-01-29'),
(26, 6, 0, 1010, 50, 9, '2023-01-29'),
(27, 8, 6, 1006, 10, 10, '2023-01-29'),
(28, 8, 6, 1001, 3, 10, '2023-01-29'),
(29, 8, 6, 1012, 0, 10, '2023-01-29'),
(30, 6, 1, 1007, 2, 11, '2023-01-29'),
(31, 9, 5, 1007, 54, 12, '2023-01-29'),
(32, 9, 5, 1012, 10, 12, '2023-01-29'),
(33, 9, 5, 1013, 40, 13, '2023-01-30'),
(34, 9, 6, 1007, 2, 14, '2023-01-30'),
(35, 9, 7, 1006, 100, 15, '2023-01-30');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`customerID`);

--
-- Indeksy dla tabeli `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employeeID`);

--
-- Indeksy dla tabeli `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`productID`);

--
-- Indeksy dla tabeli `purchases`
--
ALTER TABLE `purchases`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT dla zrzuconych tabel
--

--
-- AUTO_INCREMENT dla tabeli `customers`
--
ALTER TABLE `customers`
  MODIFY `customerID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT dla tabeli `employees`
--
ALTER TABLE `employees`
  MODIFY `employeeID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT dla tabeli `products`
--
ALTER TABLE `products`
  MODIFY `productID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1014;

--
-- AUTO_INCREMENT dla tabeli `purchases`
--
ALTER TABLE `purchases`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
