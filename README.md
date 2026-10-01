# ⏱️ StoperDesktop - Aplikacja Okienkowa Windows (.NET 9 WPF)

Natywna, nowoczesna aplikacja desktopowa stopera z interfejsem graficznym w technologii WPF dla systemu Windows.

## 🚀 Funkcjonalności
- **Precyzyjny pomiar czasu**: Płynny licznik milisekundowy z wykorzystaniem `System.Diagnostics.Stopwatch`.
- **Międzyczasy (Okrążenia)**:
  - Zapisywanie poszczególnych okrążeń na liście.
  - Automatyczne oznaczanie najszybszego okrążenia na zielono (`[NAJSZYBSZE]`).
  - Automatyczne oznaczanie najwolniejszego okrążenia na czerwono (`[NAJWOLNIEJSZE]`).
- **Zawsze na wierzchu**: Możliwość przypięcia okna ponad innymi aplikacjami.
- **Skróty klawiszowe**:
  - `Spacja`: Start / Pauza / Wznowienie
  - `L`: Zapisanie międzyczasu (Okrążenie)
  - `R`: Reset stopera

## 🛠️ Wymagania i uruchomienie
- Wymagany zainstalowany zestaw SDK lub środowisko uruchomieniowe **.NET 9.0 Windows Desktop**.
- Uruchomienie z kodu:
  ```powershell
  dotnet run
  ```
- Budowanie wersji produkcyjnej:
  ```powershell
  dotnet build -c Release
  ```
